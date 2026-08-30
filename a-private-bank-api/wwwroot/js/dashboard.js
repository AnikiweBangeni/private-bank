(() => {
    "use strict";

    const STORAGE_KEY = "a-private-bank.transactions.v1";
    const currency = new Intl.NumberFormat("en-ZA", { style: "currency", currency: "ZAR" });
    const monthName = new Intl.DateTimeFormat("en-ZA", { month: "short", year: "numeric" });
    const fullMonthName = new Intl.DateTimeFormat("en-ZA", { month: "long", year: "numeric" });
    const shortDate = new Intl.DateTimeFormat("en-ZA", { day: "2-digit", month: "short", year: "numeric" });

    const ui = Object.fromEntries([
        "statementFile", "dropZone", "demoButton", "clearButton", "statusMessage", "emptyState", "dashboard",
        "dashboardTitle", "dataSummary", "currentSpend", "currentComparison", "previousSpend", "previousLabel",
        "weeklyNeed", "latestBalance", "projectedSpend", "monthlyChart", "categoryTitle", "categoryList",
        "transactionRows", "transactionSearch"
    ].map(id => [id, document.getElementById(id)]));

    let transactions = loadSession();

    ui.statementFile.addEventListener("change", event => importFiles(event.target.files));
    ui.demoButton.addEventListener("click", loadDemoData);
    ui.clearButton.addEventListener("click", clearData);
    ui.transactionSearch.addEventListener("input", () => renderTransactions(transactions));
    ["dragenter", "dragover"].forEach(type => ui.dropZone.addEventListener(type, event => {
        event.preventDefault(); ui.dropZone.classList.add("dragover");
    }));
    ["dragleave", "drop"].forEach(type => ui.dropZone.addEventListener(type, event => {
        event.preventDefault(); ui.dropZone.classList.remove("dragover");
    }));
    ui.dropZone.addEventListener("drop", event => importFiles(event.dataTransfer.files));

    render();

    async function importFiles(fileList) {
        const files = [...fileList].filter(file => file.name.toLowerCase().endsWith(".csv") || file.type === "text/csv");
        if (!files.length) return showStatus("Choose at least one CSV file.", true);

        try {
            const imported = [];
            const rejected = [];
            for (const file of files) {
                const parsed = parseStatement(await file.text());
                if (!parsed.length) rejected.push(file.name); else imported.push(...parsed);
            }
            if (!imported.length) throw new Error("No usable transactions were found. Check the CSV column names and date format.");
            const before = transactions.length;
            transactions = deduplicate([...transactions, ...imported]);
            saveSession();
            render();
            const added = transactions.length - before;
            showStatus(`${added} transaction${added === 1 ? "" : "s"} added privately in this tab.${rejected.length ? ` Could not read: ${rejected.join(", ")}.` : ""}`);
            ui.statementFile.value = "";
        } catch (error) {
            showStatus(error instanceof Error ? error.message : "The statement could not be read.", true);
        }
    }

    function parseStatement(text) {
        const delimiter = detectDelimiter(text);
        const rows = parseCsv(text.replace(/^\uFEFF/, ""), delimiter);
        if (rows.length < 2) return [];
        const headers = rows[0].map(normalizeHeader);
        const aliases = {
            date: ["transactiondate", "postingdate", "date", "valuedate"],
            description: ["description", "originaldescription", "details", "transactiondescription", "narrative", "merchant"],
            category: ["category", "parentcategory", "transactioncategory"],
            parentCategory: ["parentcategory", "categorygroup"],
            moneyOut: ["moneyout", "debit", "debitamount", "amountout", "withdrawal"],
            moneyIn: ["moneyin", "credit", "creditamount", "amountin", "deposit"],
            amount: ["amount", "transactionamount"],
            fee: ["fee", "fees", "bankfee"],
            balance: ["balance", "runningbalance", "availablebalance"],
            account: ["account", "accountnumber"],
            number: ["nr", "number", "transactionid", "reference"]
        };
        const index = Object.fromEntries(Object.entries(aliases).map(([key, names]) => [key, findColumn(headers, names)]));
        if (index.date < 0 || (index.moneyOut < 0 && index.amount < 0)) {
            throw new Error("CSV needs a transaction date and a Money Out, Debit, or Amount column.");
        }

        return rows.slice(1).map((row, rowNumber) => {
            if (!row.some(value => value.trim())) return null;
            const date = parseDate(valueAt(row, index.date));
            if (!date) return null;
            const signedAmount = parseMoney(valueAt(row, index.amount));
            let moneyOut = Math.abs(parseMoney(valueAt(row, index.moneyOut)) || 0);
            let moneyIn = Math.abs(parseMoney(valueAt(row, index.moneyIn)) || 0);
            if (index.moneyOut < 0 && signedAmount < 0) moneyOut = Math.abs(signedAmount);
            if (index.moneyIn < 0 && signedAmount > 0) moneyIn = signedAmount;
            const fee = Math.abs(parseMoney(valueAt(row, index.fee)) || 0);
            return {
                id: valueAt(row, index.number) || `${date.toISOString()}-${rowNumber}`,
                date: date.toISOString(),
                description: valueAt(row, index.description) || "Bank transaction",
                category: valueAt(row, index.category) || valueAt(row, index.parentCategory) || "Uncategorised",
                parentCategory: valueAt(row, index.parentCategory),
                moneyOut, moneyIn, fee,
                balance: parseNullableMoney(valueAt(row, index.balance)),
                account: valueAt(row, index.account)
            };
        }).filter(Boolean);
    }

    function detectDelimiter(text) {
        const firstLine = text.split(/\r?\n/, 1)[0] || "";
        return (firstLine.match(/;/g) || []).length > (firstLine.match(/,/g) || []).length ? ";" : ",";
    }

    function parseCsv(text, delimiter) {
        const rows = []; let row = []; let field = ""; let quoted = false;
        for (let i = 0; i < text.length; i++) {
            const char = text[i];
            if (quoted) {
                if (char === '"' && text[i + 1] === '"') { field += '"'; i++; }
                else if (char === '"') quoted = false;
                else field += char;
            } else if (char === '"') quoted = true;
            else if (char === delimiter) { row.push(field); field = ""; }
            else if (char === "\n") { row.push(field.replace(/\r$/, "")); rows.push(row); row = []; field = ""; }
            else field += char;
        }
        if (field.length || row.length) { row.push(field.replace(/\r$/, "")); rows.push(row); }
        return rows;
    }

    function normalizeHeader(value) { return value.trim().toLowerCase().replace(/[^a-z0-9]/g, ""); }
    function findColumn(headers, names) { return headers.findIndex(header => names.includes(header)); }
    function valueAt(row, index) { return index >= 0 ? (row[index] || "").trim() : ""; }

    function parseMoney(value) {
        if (!value) return 0;
        const negative = /^\s*\(.*\)\s*$/.test(value) || /^\s*-/.test(value);
        let cleaned = value.replace(/[R$€£\s()]/g, "");
        if (/^-?\d{1,3}(\.\d{3})+,\d{1,2}$/.test(cleaned)) cleaned = cleaned.replace(/\./g, "").replace(",", ".");
        else cleaned = cleaned.replace(/,/g, "");
        const number = Number.parseFloat(cleaned.replace(/^-/, ""));
        return Number.isFinite(number) ? (negative ? -number : number) : 0;
    }
    function parseNullableMoney(value) { return value ? parseMoney(value) : null; }

    function parseDate(value) {
        if (!value) return null;
        const iso = value.match(/^(\d{4})[-/]([01]?\d)[-/]([0-3]?\d)/);
        if (iso) return validDate(Number(iso[1]), Number(iso[2]), Number(iso[3]));
        const local = value.match(/^([0-3]?\d)[-/\s]([01]?\d)[-/\s](\d{2,4})/);
        if (local) {
            const year = Number(local[3]) < 100 ? 2000 + Number(local[3]) : Number(local[3]);
            return validDate(year, Number(local[2]), Number(local[1]));
        }
        const parsed = new Date(value);
        return Number.isNaN(parsed.getTime()) ? null : parsed;
    }
    function validDate(year, month, day) {
        const date = new Date(year, month - 1, day);
        return date.getFullYear() === year && date.getMonth() === month - 1 && date.getDate() === day ? date : null;
    }

    function deduplicate(items) {
        const seen = new Set();
        return items.filter(item => {
            const key = [item.date, item.description, item.moneyOut, item.moneyIn, item.fee, item.balance, item.account].join("|");
            if (seen.has(key)) return false; seen.add(key); return true;
        }).sort((a, b) => new Date(b.date) - new Date(a.date));
    }

    function render() {
        const hasData = transactions.length > 0;
        ui.emptyState.hidden = hasData; ui.dashboard.hidden = !hasData;
        if (!hasData) return;

        const now = new Date();
        const currentKey = monthKey(now);
        const previous = new Date(now.getFullYear(), now.getMonth() - 1, 1);
        const previousKey = monthKey(previous);
        const monthly = groupSpendingByMonth(transactions);
        const currentSpend = monthly.get(currentKey) || 0;
        const previousSpend = monthly.get(previousKey) || 0;
        const completed = [...monthly.entries()].filter(([key]) => key < currentKey && key !== currentKey);
        const historicalMonthlyAverage = completed.length ? completed.reduce((sum, [, amount]) => sum + amount, 0) / completed.length : 0;
        const daysInMonth = new Date(now.getFullYear(), now.getMonth() + 1, 0).getDate();
        const projected = now.getDate() ? currentSpend / now.getDate() * daysInMonth : currentSpend;
        const weekly = historicalMonthlyAverage ? historicalMonthlyAverage / 4.345 : currentSpend / Math.max(now.getDate(), 1) * 7;
        const latestWithBalance = transactions.find(item => item.balance !== null);

        ui.dashboardTitle.textContent = fullMonthName.format(now);
        ui.dataSummary.textContent = `${transactions.length} private transaction${transactions.length === 1 ? "" : "s"} in this tab`;
        ui.currentSpend.textContent = currency.format(currentSpend);
        ui.previousSpend.textContent = currency.format(previousSpend);
        ui.previousLabel.textContent = fullMonthName.format(previous);
        ui.weeklyNeed.textContent = currency.format(weekly);
        ui.latestBalance.textContent = latestWithBalance ? currency.format(latestWithBalance.balance) : "Not provided";
        ui.projectedSpend.textContent = `Projected month: ${currency.format(projected)}`;
        renderComparison(currentSpend, previousSpend);
        renderMonthlyChart(monthly, currentKey);
        renderCategories(currentKey);
        renderTransactions(transactions);
    }

    function expense(item) { return Math.max(0, Number(item.moneyOut) || 0) + Math.max(0, Number(item.fee) || 0); }
    function monthKey(date) { return `${date.getFullYear()}-${String(date.getMonth() + 1).padStart(2, "0")}`; }
    function groupSpendingByMonth(items) {
        const result = new Map();
        for (const item of items) {
            const key = monthKey(new Date(item.date)); result.set(key, (result.get(key) || 0) + expense(item));
        }
        return result;
    }
    function dateFromMonthKey(key) { const [year, month] = key.split("-").map(Number); return new Date(year, month - 1, 1); }

    function renderComparison(current, previous) {
        ui.currentComparison.className = "metric-detail";
        if (!previous) { ui.currentComparison.textContent = "No previous-month spend"; return; }
        const change = (current - previous) / previous * 100;
        ui.currentComparison.textContent = `${Math.abs(change).toFixed(0)}% ${change <= 0 ? "less" : "more"} than last month`;
        ui.currentComparison.classList.add(change <= 0 ? "positive" : "negative");
    }

    function renderMonthlyChart(monthly, currentKey) {
        ui.monthlyChart.replaceChildren();
        const entries = [...monthly.entries()].sort(([a], [b]) => a.localeCompare(b)).slice(-6);
        const max = Math.max(...entries.map(([, value]) => value), 1);
        ui.monthlyChart.setAttribute("aria-label", entries.map(([key, value]) => `${monthName.format(dateFromMonthKey(key))}: ${currency.format(value)}`).join("; "));
        for (const [key, value] of entries) {
            const column = element("div", "bar-column");
            column.append(element("span", "bar-value", compactMoney(value)));
            const bar = element("div", `bar${key === currentKey ? " current" : ""}`); bar.style.height = `${Math.max(2, value / max * 82)}%`; column.append(bar);
            column.append(element("span", "bar-label", monthName.format(dateFromMonthKey(key)).replace(" ", " ’")));
            ui.monthlyChart.append(column);
        }
    }

    function renderCategories(currentKey) {
        const currentItems = transactions.filter(item => monthKey(new Date(item.date)) === currentKey && expense(item) > 0);
        const fallbackKey = transactions.length ? monthKey(new Date(transactions[0].date)) : currentKey;
        const items = currentItems.length ? currentItems : transactions.filter(item => monthKey(new Date(item.date)) === fallbackKey && expense(item) > 0);
        const groups = new Map();
        for (const item of items) groups.set(item.category || "Uncategorised", (groups.get(item.category || "Uncategorised") || 0) + expense(item));
        const sorted = [...groups.entries()].sort((a, b) => b[1] - a[1]).slice(0, 6);
        const max = sorted[0]?.[1] || 1;
        ui.categoryTitle.textContent = currentItems.length ? "Top categories this month" : `Top categories · ${monthName.format(dateFromMonthKey(fallbackKey))}`;
        ui.categoryList.replaceChildren();
        if (!sorted.length) ui.categoryList.append(element("p", "muted", "No spending categories available."));
        for (const [name, amount] of sorted) {
            const row = element("div", "category-row"); row.append(element("span", "category-name", name), element("span", "category-amount", currency.format(amount)));
            const track = element("div", "category-track"); const fill = element("div", "category-fill"); fill.style.width = `${amount / max * 100}%`; track.append(fill); row.append(track); ui.categoryList.append(row);
        }
    }

    function renderTransactions(items) {
        const term = ui.transactionSearch.value.trim().toLowerCase();
        const filtered = items.filter(item => !term || `${item.description} ${item.category}`.toLowerCase().includes(term)).slice(0, 25);
        ui.transactionRows.replaceChildren();
        if (!filtered.length) {
            const row = document.createElement("tr"); const cell = element("td", "muted", "No matching transactions."); cell.colSpan = 6; row.append(cell); ui.transactionRows.append(row); return;
        }
        for (const item of filtered) {
            const row = document.createElement("tr");
            row.append(cell(shortDate.format(new Date(item.date))), cell(item.description), taggedCell(item.category), moneyCell(item.moneyIn, "money-in"), moneyCell(expense(item), "money-out"), moneyCell(item.balance, ""));
            ui.transactionRows.append(row);
        }
    }

    function loadDemoData() {
        const now = new Date(); const descriptions = [
            ["Woolworths Food", "Groceries", 845], ["Fuel station", "Fuel", 920], ["Home internet", "Internet", 699],
            ["Restaurant", "Restaurants", 540], ["Medical aid", "Medical", 1300], ["Digital subscription", "Digital Subscriptions", 149]
        ];
        const demo = [];
        for (let offset = 0; offset < 6; offset++) {
            const month = new Date(now.getFullYear(), now.getMonth() - offset, 1);
            descriptions.forEach(([description, category, base], index) => {
                const day = Math.min(3 + index * 4, new Date(month.getFullYear(), month.getMonth() + 1, 0).getDate());
                if (offset === 0 && day > now.getDate()) return;
                demo.push({ id: `demo-${offset}-${index}`, date: new Date(month.getFullYear(), month.getMonth(), day).toISOString(), description, category, parentCategory: "", moneyOut: base * (1 + offset * .035), moneyIn: 0, fee: index === 1 ? 5 : 0, balance: 32500 - offset * 2100 - index * 600, account: "Demo" });
            });
        }
        transactions = deduplicate(demo); saveSession(); render(); showStatus("Sample data loaded. Clear it before importing your own statement.");
    }

    function clearData() {
        if (!window.confirm("Clear all statement data stored in this browser tab?")) return;
        transactions = []; sessionStorage.removeItem(STORAGE_KEY); ui.transactionSearch.value = ""; render(); showStatus("Private statement data cleared from this tab.");
    }
    function loadSession() {
        try { const value = JSON.parse(sessionStorage.getItem(STORAGE_KEY) || "[]"); return Array.isArray(value) ? deduplicate(value) : []; }
        catch { sessionStorage.removeItem(STORAGE_KEY); return []; }
    }
    function saveSession() {
        try { sessionStorage.setItem(STORAGE_KEY, JSON.stringify(transactions)); }
        catch { throw new Error("This statement is too large for session storage. Try fewer months or smaller CSV files."); }
    }
    function showStatus(message, isError = false) {
        ui.statusMessage.textContent = message; ui.statusMessage.className = `notice${isError ? " error" : ""}`; ui.statusMessage.hidden = false;
        ui.statusMessage.scrollIntoView({ behavior: "smooth", block: "nearest" });
    }
    function compactMoney(value) { return value >= 1000 ? `R${(value / 1000).toFixed(value >= 10000 ? 0 : 1)}k` : `R${value.toFixed(0)}`; }
    function element(tag, className = "", text = "") { const node = document.createElement(tag); if (className) node.className = className; if (text) node.textContent = text; return node; }
    function cell(text) { const node = document.createElement("td"); node.textContent = text; return node; }
    function taggedCell(text) { const node = document.createElement("td"); node.append(element("span", "category-tag", text || "Uncategorised")); return node; }
    function moneyCell(value, className) { const node = element("td", `number ${className}`.trim(), value === null || value === undefined ? "—" : currency.format(value)); return node; }
})();
