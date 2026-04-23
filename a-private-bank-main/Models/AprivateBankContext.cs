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

    public virtual DbSet<Person> People { get; set; }

    public virtual DbSet<TransactionsStatementEntityModel> TransactionsStatements { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__Accounts__349DA5A602A51DF9");

            entity.Property(e => e.BankName).HasMaxLength(255);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
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

        modelBuilder.Entity<TransactionsStatementEntityModel>(entity =>
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

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
