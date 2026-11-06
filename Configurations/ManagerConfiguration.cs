using EFCore04.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
namespace EFCore04.Configurations
{
    public class ManagerConfiguration : IEntityTypeConfiguration<Manager>
    {
        public void Configure(EntityTypeBuilder<Manager> builder)
        {
            builder.HasKey(m => m.Id);

            builder.Property(m => m.FullName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(m => m.Email)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(m => m.PhoneNumber)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(m => m.HireDate)
                .IsRequired();
        }
    }
}

