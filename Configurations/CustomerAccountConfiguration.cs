using EFCore04.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;


namespace EFCore04.Configurations
{
    public class CustomerAccountConfiguration : IEntityTypeConfiguration<CustomerAccount>
    {
        public void Configure(EntityTypeBuilder<CustomerAccount> builder)
        {
            builder.HasKey(ca => new
            {
                ca.CustomerId,
                ca.AccountNumber
            });

            builder.Property(ca => ca.OwnershipType)
                .HasConversion<string>();

            builder.Property(ca => ca.AccountStatus)
                .HasConversion<string>();

            builder.HasOne(ca => ca.Customer)
                .WithMany(c => c.CustomerAccounts)
                .HasForeignKey(ca => ca.CustomerId);

            builder.HasOne(ca => ca.Account)
                .WithMany(a => a.CustomerAccounts)
                .HasForeignKey(ca => ca.AccountNumber);
        }
    }
}

