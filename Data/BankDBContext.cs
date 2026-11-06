using EFCore04.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EFCore04.Data
{
    public class BankDBContext : DbContext
    {
        public DbSet<Branch> Branches { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<CustomerAccount> CustomerAccounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnConfiguring(
            DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                "Server=.;Database=NationalBankDB;Trusted_Connection=True;TrustServerCertificate=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(BankDBContext).Assembly);

            modelBuilder.Entity<Manager>().HasData(
                new Manager
                {
                    Id = 1,
                    FullName = "Ahmed Mohamed",
                    Email = "ahmed@bank.com",
                    PhoneNumber = "01000000000",
                    HireDate = new DateTime(2020, 1, 1)
                },
                new Manager
                {
                    Id = 2,
                    FullName = "Mohamed Ali",
                    Email = "mohamed@bank.com",
                    PhoneNumber = "01100000000",
                    HireDate = new DateTime(2021, 1, 1)
                }
            );

            modelBuilder.Entity<Branch>().HasData(
                new Branch
                {
                    Code = 100,
                    Name = "Cairo Branch",
                    Address = "Cairo",
                    PhoneNumber = "0220000000",
                    ManagerId = 1
                },
                new Branch
                {
                    Code = 200,
                    Name = "Giza Branch",
                    Address = "Giza",
                    PhoneNumber = "0230000000",
                    ManagerId = 2
                }
            );

            base.OnModelCreating(modelBuilder);
        }
    }
}
