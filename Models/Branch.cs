using System;
using System.Collections.Generic;
using System.Text;

namespace EFCore04.Models
{
    public class Branch
    {
        public int Code { get; set; }
        public string Name { get; set; }
        public string Address{ get; set; }
        public string PhoneNumber { get; set; }
        public int ManagerId { get; set; }
        public Manager Manager { get; set; }
        public ICollection<Account> Accounts { get; set; } = new List<Account>();
    }
}
