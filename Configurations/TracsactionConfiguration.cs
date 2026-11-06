using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EFCore04.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EFCore04.Configurations
{
    public class TracsactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.HasKey(t => t.TransactionNumber);

            builder.Property(t => t.Amount)
                .HasColumnType("decimal(18,2)");

            builder.Property(t => t.Note)
                .HasMaxLength(300);

            builder.Property(t => t.TransactionType)
                .HasConversion<string>();

            builder.HasOne(t => t.Account)
                .WithMany(a => a.Transactions)
                .HasForeignKey(t => t.AccountNumber);
        }
    }
}
