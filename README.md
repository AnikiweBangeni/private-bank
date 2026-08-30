# A Private Bank — Spending Dashboard

A private, local-first ASP.NET Core Razor Pages dashboard for understanding personal spending from CSV bank statements. Statement rows are parsed entirely in the browser and stored in `sessionStorage`: no transaction is posted to the ASP.NET server or saved in a database.

## What it shows

- Total spent in the current calendar month
- Previous-month spending and percentage comparison
- Projected current-month spend based on month-to-date pace
- Typical weekly cash need based on the average of completed months
- Latest available account balance
- Six-month spending trend and top categories
- Searchable recent transactions
- Multiple-statement import with duplicate detection

## Run in Visual Studio

1. Clone `https://github.com/AnikiweBangeni/private-bank.git`.
2. Open `a-private-bank-main.sln` in Visual Studio.
3. Ensure the **ASP.NET and web development** workload and .NET 9 SDK are installed.
4. Select the `https` profile and press **F5**.

Or run from a terminal:

```powershell
dotnet restore a-private-bank-main.sln
dotnet run --project a-private-bank-api
```

## Statement format

Export a CSV statement from your bank. The parser accepts commas or semicolons, quoted fields, South African `dd/MM/yyyy` dates, ISO dates, and common header variations.

For a safe first run, upload the fictional [`samples/sample-bank-statement.csv`](samples/sample-bank-statement.csv) file included in this repository.

Required:

- `Transaction Date`, `Posting Date`, or `Date`
- `Money Out`, `Debit`, or a signed `Amount` column

For a signed `Amount` column, negative values are treated as spending and positive values as money in.

Recommended:

- `Description`
- `Category` / `Parent Category`
- `Money In` / `Credit`
- `Fee`
- `Balance`
- `Nr`, transaction ID, or reference

The existing project format (`Nr, Account, Posting Date, Transaction Date, Description, Original Description, Parent Category, Category, Money In, Money Out, Fee, Balance`) is supported.

## Privacy model

- The file input is read with the browser File API; there is no upload endpoint.
- Parsed rows are kept in the current tab's `sessionStorage` so they survive a refresh.
- Closing the tab/window clears that tab's session data according to normal browser behavior.
- **Clear private data** removes the stored rows immediately.
- No database, account, telemetry, third-party script, or external API is used.

`sessionStorage` is convenient privacy, not encryption. Do not use the app on a shared or untrusted computer, and keep the original statement file protected.

## Calculation notes

- Spending is `Money Out + Fee` for each transaction.
- Typical weekly need is the average spend of completed calendar months divided by `4.345` weeks per month.
- When no completed month exists, the weekly estimate uses the current month-to-date daily pace.
- Projected month spend extrapolates current spend by elapsed days across the number of days in the current month.

## Project structure

```text
a-private-bank-api/       Active Razor Pages dashboard
a-private-bank-main/      Earlier database prototype retained for history/reference
.github/workflows/ci.yml  Restore, formatting, and Release build checks
```

The earlier SQL Server prototype is no longer part of the active solution or runtime. It remains in the repository so the project's evolution is reviewable.
