using EFCore04.Data;
using EFCore04.Models;
using Microsoft.EntityFrameworkCore;
namespace EFCore04
{
    internal class Program
    {
        static void Main(string[] args)
        {

            using var db = new BankDBContext();

            while (true)
            {
                Console.Clear();

                Console.WriteLine("==================================");
                Console.WriteLine("    NATIONAL BANK MANAGEMENT");
                Console.WriteLine("==================================");
                Console.WriteLine("1. Add New Customer");
                Console.WriteLine("2. Open New Account");
                Console.WriteLine("3. Update Account Status");
                Console.WriteLine("4. Remove Account from Customer");
                Console.WriteLine("5. List All Customers");
                Console.WriteLine("0. Exit");
                Console.WriteLine("==================================");

                Console.Write("Enter your choice: ");

                string choice = Console.ReadLine();

                try
                {
                    switch (choice)
                    {
                        case "1":
                            AddCustomer(db);
                            break;

                        case "2":
                            OpenAccount(db);
                            break;

                        case "3":
                            UpdateAccountStatus(db);
                            break;

                        case "4":
                            RemoveAccount(db);
                            break;

                        case "5":
                            ListCustomers(db);
                            break;

                        case "0":
                            Console.WriteLine("Goodbye!");
                            return;

                        default:
                            Console.WriteLine("Invalid choice!");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                }

                Console.WriteLine();
                Console.WriteLine(
                    "Press any key to return to the menu...");

                Console.ReadKey();
            }
        }

        #region Add Customer
        static void AddCustomer(BankDBContext db)
        {
            Console.WriteLine("\n--- Add New Customer ---");

            Console.Write("Full Name: ");
            string name = Console.ReadLine();

            Console.Write("National ID: ");
            string nationalId = Console.ReadLine();

            Console.Write("Date of Birth (yyyy-MM-dd): ");

            if (!DateTime.TryParse(
                Console.ReadLine(), out DateTime dob))
            {
                Console.WriteLine("Invalid date!");
                return;
            }

            Console.Write("Email: ");
            string email = Console.ReadLine();

            Console.Write("Phone Number: ");
            string phone = Console.ReadLine();

            Console.Write("Address: ");
            string address = Console.ReadLine();

            Console.WriteLine("Customer Type:");
            Console.WriteLine("1. Individual");
            Console.WriteLine("2. Business");

            string type = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(name) ||
                string.IsNullOrWhiteSpace(nationalId) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(phone) ||
                string.IsNullOrWhiteSpace(address))
            {
                Console.WriteLine("All fields are required!");
                return;
            }

            if (type != "1" && type != "2")
            {
                Console.WriteLine("Invalid customer type!");
                return;
            }

            if (db.Customers.Any(c => c.NationalId == nationalId))
            {
                Console.WriteLine("National ID already exists!");
                return;
            }

            Customer customer = new Customer
            {
                FullName = name,
                NationalId = nationalId,
                DateOfBirth = dob,
                Email = email,
                PhoneNumber = phone,
                Address = address,

                CustomerType = type == "1"
                    ? CustomerType.Individual
                    : CustomerType.Business
            };

            db.Customers.Add(customer);
            db.SaveChanges();

            Console.WriteLine("Customer added successfully!");
            Console.WriteLine("Customer ID: " + customer.Id);
        }
        #endregion

        #region Open New Account
        static void OpenAccount(BankDBContext db)
        {
            Console.WriteLine("\n--- Open New Account ---");

            Console.Write("Account Number: ");
            string accountNumber = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(accountNumber) ||
                db.Accounts.Any(a => a.AccountNumber == accountNumber))
            {
                Console.WriteLine("Invalid or existing account number!");
                return;
            }

            Console.Write("Account Type (1 Saving / 2 Current): ");
            string accountType = Console.ReadLine();

            if (accountType != "1" && accountType != "2")
            {
                Console.WriteLine("Invalid account type!");
                return;
            }

            Console.Write("Branch Code: ");

            if (!int.TryParse(Console.ReadLine(), out int branchCode) ||
                !db.Branches.Any(b => b.Code == branchCode))
            {
                Console.WriteLine("Branch does not exist!");
                return;
            }

            Console.Write("Customer ID: ");

            if (!int.TryParse(Console.ReadLine(), out int customerId) ||
                !db.Customers.Any(c => c.Id == customerId))
            {
                Console.WriteLine("Customer does not exist!");
                return;
            }

            Console.Write("Ownership Role (1 Primary / 2 CoHolder): ");
            string role = Console.ReadLine();

            if (role != "1" && role != "2")
            {
                Console.WriteLine("Invalid ownership role!");
                return;
            }

            var account = new Account
            {
                AccountNumber = accountNumber,
                AccountType = accountType == "1"
                    ? AccountType.Savings
                    : AccountType.Current,
                CurrentBalance = 0,
                OpeningDate = DateTime.Now,
                BranchCode = branchCode
            };

            var customerAccount = new CustomerAccount
            {
                AccountNumber = accountNumber,
                CustomerId = customerId,
                OwnershipType = role == "1"
                    ? OwnershipType.Primary
                    : OwnershipType.CoHolder,
                OwnershipStartDate = DateTime.Now,
                AccountStatus = AccountStatus.Active
            };

            using var transaction = db.Database.BeginTransaction();

            try
            {
                db.Accounts.Add(account);
                db.CustomerAccounts.Add(customerAccount);

                db.SaveChanges();
                transaction.Commit();

                Console.WriteLine("Account opened successfully!");
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
        #endregion

        #region Update Account Status
        static void UpdateAccountStatus(BankDBContext db)
        {
            Console.WriteLine("\n--- Update Account Status ---");

            Console.Write("Account Number: ");
            string accountNumber = Console.ReadLine();

            Console.Write("Customer ID: ");

            if (!int.TryParse(Console.ReadLine(), out int customerId))
            {
                Console.WriteLine("Invalid Customer ID!");
                return;
            }

            var customerAccount = db.CustomerAccounts
                .FirstOrDefault(ca =>
                    ca.AccountNumber == accountNumber &&
                    ca.CustomerId == customerId);

            if (customerAccount == null)
            {
                Console.WriteLine("Customer Account not found!");
                return;
            }

            customerAccount.AccountStatus =
                customerAccount.AccountStatus == AccountStatus.Active
                    ? AccountStatus.Inactive
                    : AccountStatus.Active;

            db.SaveChanges();

            Console.WriteLine("Account status updated successfully!");
            Console.WriteLine("New Status: " +
                customerAccount.AccountStatus);
        }
        #endregion

        #region Remove Account From Customer
        static void RemoveAccount(BankDBContext db)
        {
            Console.WriteLine("\n--- Remove Account ---");

            Console.Write("Account Number: ");
            string accountNumber = Console.ReadLine();

            Console.Write("Customer ID: ");

            if (!int.TryParse(Console.ReadLine(), out int customerId))
            {
                Console.WriteLine("Invalid Customer ID!");
                return;
            }

            var customerAccount = db.CustomerAccounts
                .FirstOrDefault(ca =>
                    ca.AccountNumber == accountNumber &&
                    ca.CustomerId == customerId);

            if (customerAccount == null)
            {
                Console.WriteLine("Customer Account not found!");
                return;
            }

            db.CustomerAccounts.Remove(customerAccount);

            db.SaveChanges();

            Console.WriteLine("Customer Account removed successfully!");
        }

        #endregion

        #region List All Customers With Accounts _ Eager Loading
        static void ListCustomers(BankDBContext db)
        {
            Console.WriteLine("\n--- All Customers ---");

            var customers = db.Customers
                .Include(c => c.CustomerAccounts)
                .ThenInclude(ca => ca.Account)
                .ToList();

            foreach (var customer in customers)
            {
                Console.WriteLine("\n================================");

                Console.WriteLine("Customer ID: " + customer.Id);
                Console.WriteLine("Name: " + customer.FullName);
                Console.WriteLine("National ID: " + customer.NationalId);
                Console.WriteLine("Email: " + customer.Email);
                Console.WriteLine("Phone: " + customer.PhoneNumber);
                Console.WriteLine("Type: " + customer.CustomerType);

                Console.WriteLine("\nAccounts:");

                if (!customer.CustomerAccounts.Any())
                {
                    Console.WriteLine("No accounts found.");
                }

                foreach (var ca in customer.CustomerAccounts)
                {
                    Console.WriteLine("----------------------------");

                    Console.WriteLine(
                        "Account Number: " + ca.AccountNumber);

                    Console.WriteLine(
                        "Account Type: " + ca.Account.AccountType);

                    Console.WriteLine(
                        "Balance: " + ca.Account.CurrentBalance);

                    Console.WriteLine(
                        "Ownership: " + ca.OwnershipType);

                    Console.WriteLine(
                        "Status: " + ca.AccountStatus);
                }
            }
        }
        #endregion
    }

}
 

