using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;

namespace EFCore04.Models
{
    public class Account
    {
        public string AccountNumber { get; set; }
        public decimal CurrentBalance { get; set; }
        public AccountType AccountType { get; set; }
        public DateTime OpeningDate { get; set; }
        public int BranchCode { get; set; }
        public Branch Branch { get; set; }
        public ICollection<CustomerAccount> CustomerAccounts { get; set; } = new List<CustomerAccount>();
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

    }
}
