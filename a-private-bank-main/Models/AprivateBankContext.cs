using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace a_private_bank_main.Models;

public partial class AprivateBankContext : DbContext
{
    public AprivateBankContext()
    {
    }

    public AprivateBankContext(DbContextOptions<AprivateBankContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<BudgetPeriod> BudgetPeriods { get; set; }

    public virtual DbSet<BudgetSummary> BudgetSummaries { get; set; }

    public virtual DbSet<FixedExpense> FixedExpenses { get; set; }

    public virtual DbSet<Income> Incomes { get; set; }

    public virtual DbSet<Person> People { get; set; }

    public virtual DbSet<TransactionsStatement> TransactionsStatements { get; set; }

    public virtual DbSet<VariableExpense> VariableExpenses { get; set; }

    public virtual DbSet<WishList> WishLists { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS04;Database=APrivateBank;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__Accounts__349DA5A602A51DF9");

            entity.Property(e => e.BankName).HasMaxLength(255);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<BudgetPeriod>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__budget_p__3213E83FD47D36FE");

            entity.ToTable("budget_periods");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.OpeningBalance)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("opening_balance");
            entity.Property(e => e.PeriodEnd).HasColumnName("period_end");
            entity.Property(e => e.PeriodStart).HasColumnName("period_start");
        });

        modelBuilder.Entity<BudgetSummary>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("budget_summary");

            entity.Property(e => e.BalanceAfterApprovedWishes)
                .HasColumnType("decimal(38, 2)")
                .HasColumnName("balance_after_approved_wishes");
            entity.Property(e => e.BalanceIfAllWishesBought)
                .HasColumnType("decimal(38, 2)")
                .HasColumnName("balance_if_all_wishes_bought");
            entity.Property(e => e.OpeningBalance)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("opening_balance");
            entity.Property(e => e.PeriodEnd).HasColumnName("period_end");
            entity.Property(e => e.PeriodId)
                .ValueGeneratedOnAdd()
                .HasColumnName("period_id");
            entity.Property(e => e.PeriodStart).HasColumnName("period_start");
            entity.Property(e => e.TotalFixed)
                .HasColumnType("decimal(38, 2)")
                .HasColumnName("total_fixed");
            entity.Property(e => e.TotalIncomeExpected)
                .HasColumnType("decimal(38, 2)")
                .HasColumnName("total_income_expected");
            entity.Property(e => e.TotalIncomeReceived)
                .HasColumnType("decimal(38, 2)")
                .HasColumnName("total_income_received");
            entity.Property(e => e.TotalVariableActual)
                .HasColumnType("decimal(38, 2)")
                .HasColumnName("total_variable_actual");
            entity.Property(e => e.TotalVariableBudgeted)
                .HasColumnType("decimal(38, 2)")
                .HasColumnName("total_variable_budgeted");
            entity.Property(e => e.WishListApproved)
                .HasColumnType("decimal(38, 2)")
                .HasColumnName("wish_list_approved");
            entity.Property(e => e.WishListTotalPending)
                .HasColumnType("decimal(38, 2)")
                .HasColumnName("wish_list_total_pending");
        });

        modelBuilder.Entity<FixedExpense>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__fixed_ex__3213E83F66F4CAE6");

            entity.ToTable("fixed_expenses");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Amount)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("amount");
            entity.Property(e => e.Category)
                .HasMaxLength(50)
                .HasColumnName("category");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DueDate).HasColumnName("due_date");
            entity.Property(e => e.IsPaid)
                .HasDefaultValue(false)
                .HasColumnName("is_paid");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.PaymentMethod)
                .HasMaxLength(50)
                .HasColumnName("payment_method");
            entity.Property(e => e.PeriodId).HasColumnName("period_id");

            entity.HasOne(d => d.Period).WithMany(p => p.FixedExpenses)
                .HasForeignKey(d => d.PeriodId)
                .HasConstraintName("FK_fixed_period");
        });

        modelBuilder.Entity<Income>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__income__3213E83F7E24DF48");

            entity.ToTable("income");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ActualAmount)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("actual_amount");
            entity.Property(e => e.Category)
                .HasMaxLength(50)
                .HasColumnName("category");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.ExpectedAmount)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("expected_amount");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.PeriodId).HasColumnName("period_id");
            entity.Property(e => e.ReceivedDate).HasColumnName("received_date");
            entity.Property(e => e.Source)
                .HasMaxLength(100)
                .HasColumnName("source");

            entity.HasOne(d => d.Period).WithMany(p => p.Incomes)
                .HasForeignKey(d => d.PeriodId)
                .HasConstraintName("FK_income_period");
        });

        modelBuilder.Entity<Person>(entity =>
        {
            entity.HasKey(e => e.PersonId).HasName("PK__People__AA2FFBE57AF49289");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.FullName).HasMaxLength(255);
        });

        modelBuilder.Entity<TransactionsStatement>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Transact__3214EC0754964832");

            entity.ToTable("TransactionsStatement");

            entity.HasIndex(e => new { e.Nr, e.Account }, "UQ_Nr_Account").IsUnique();

            entity.Property(e => e.Balance).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Category).HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.Fee).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.MoneyIn).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.MoneyOut).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.OriginalDescription).HasMaxLength(255);
            entity.Property(e => e.ParentCategory).HasMaxLength(100);
            entity.Property(e => e.PostingDate).HasColumnType("datetime");
            entity.Property(e => e.TransactionDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<VariableExpense>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__variable__3213E83FB63B1A0C");

            entity.ToTable("variable_expenses");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ActualAmount)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("actual_amount");
            entity.Property(e => e.BudgetedAmount)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("budgeted_amount");
            entity.Property(e => e.Category)
                .HasMaxLength(50)
                .HasColumnName("category");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.PaymentMethod)
                .HasMaxLength(50)
                .HasColumnName("payment_method");
            entity.Property(e => e.PeriodId).HasColumnName("period_id");
            entity.Property(e => e.SpentDate).HasColumnName("spent_date");

            entity.HasOne(d => d.Period).WithMany(p => p.VariableExpenses)
                .HasForeignKey(d => d.PeriodId)
                .HasConstraintName("FK_variable_period");
        });

        modelBuilder.Entity<WishList>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__wish_lis__3213E83FEB08951A");

            entity.ToTable("wish_list");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Category)
                .HasMaxLength(50)
                .HasColumnName("category");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DeferToPeriod).HasColumnName("defer_to_period");
            entity.Property(e => e.EstimatedCost)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("estimated_cost");
            entity.Property(e => e.IsNecessity)
                .HasDefaultValue(false)
                .HasColumnName("is_necessity");
            entity.Property(e => e.ItemName)
                .HasMaxLength(150)
                .HasColumnName("item_name");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.PeriodId).HasColumnName("period_id");
            entity.Property(e => e.Priority)
                .HasDefaultValue((short)3)
                .HasColumnName("priority");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("pending")
                .HasColumnName("status");

            entity.HasOne(d => d.DeferToPeriodNavigation).WithMany(p => p.WishListDeferToPeriodNavigations)
                .HasForeignKey(d => d.DeferToPeriod)
                .HasConstraintName("FK_wishlist_defer");

            entity.HasOne(d => d.Period).WithMany(p => p.WishListPeriods)
                .HasForeignKey(d => d.PeriodId)
                .HasConstraintName("FK_wishlist_period");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
