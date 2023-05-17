using System;
using System.Collections.Generic;
using System.IO;
using System.Transactions;

namespace ExpenseTracker
{
    class Program
    {
        static void Main(string[] args)
        {
            TransactionList transactionList = new TransactionList();
            //ReportGenerator reportGenerator = new WordDocumentReportGenerator();

            while (true)
            {
                Console.WriteLine("Expense Tracker");
                Console.WriteLine("----------------");

                Console.WriteLine("1. Add expense");
                Console.WriteLine("2. Add income");
                Console.WriteLine("3. View transactions");
                Console.WriteLine("4. Export report to console");
                Console.WriteLine("5. Export report to text file");
                Console.WriteLine("6. Clear all transactions");
                Console.WriteLine("7. Exit");

                Console.Write("\nEnter your choice: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        transactionList.AddTransaction(TransactionType.Expense);
                        break;

                    case "2":
                        transactionList.AddTransaction(TransactionType.Income);
                        break;

                    case "3":
                        transactionList.ViewTransactions();
                        break;

                    case "4":
                        transactionList.DisplayReport(new ConsoleReportGenerator());
                        break;

                    case "5":
                        transactionList.DisplayReport(new TextFileReportGenerator());
                        break;

                    case "6":
                        transactionList.ClearTransactions();
                        break;

                    case "7":
                        Console.WriteLine("Thank you for using Expense Tracker!");
                        return;

                    default:
                        Console.WriteLine("Invalid input. Please try again.");
                        break;
                }
            }
        }
    }

    abstract class ReportGenerator
    {
        public void GenerateReport(List<Transaction> transactions)
        {
            Console.WriteLine("Expense Tracker Report");
            Console.WriteLine("-----------------------");
            Console.WriteLine("Transactions:");
            GenerateReportImpl(transactions);
        }

        protected abstract void GenerateReportImpl(List<Transaction> transactions);
    }

    class ConsoleReportGenerator : ReportGenerator
    {
        protected override void GenerateReportImpl(List<Transaction> transactions)
        {
            decimal totalExpenses = 0;
            decimal totalIncome = 0;

            foreach (var transaction in transactions)
            {
                if (transaction.Type == TransactionType.Expense)
                {
                    Console.WriteLine($"Expense: {transaction.Category} - {transaction.Amount:C}");
                    totalExpenses += transaction.Amount;
                }
                else
                {
                    Console.WriteLine($"Income: {transaction.Category} - {transaction.Amount:C}");
                    totalIncome += transaction.Amount;
                }
            }

            Console.WriteLine("");
            Console.WriteLine($"Total Expenses: {totalExpenses:C}");
            Console.WriteLine($"Total Income: {totalIncome:C}");
            Console.WriteLine($"Net Balance: {(totalIncome - totalExpenses):C}");
        }
    }

    class TextFileReportGenerator : ReportGenerator
    {
        protected override void GenerateReportImpl(List<Transaction> transactions)
        {
            string fileName = "report.txt";

            using (StreamWriter file = new StreamWriter(fileName))
            {
                decimal totalExpenses = 0;
                decimal totalIncome = 0;

                file.WriteLine("Expense Tracker Report");
                file.WriteLine("-----------------------");
                file.WriteLine("Transactions:");

                foreach (var transaction in transactions)
                {
                    if (transaction.Type == TransactionType.Expense)
                    {
                        file.WriteLine($"Expense: {transaction.Category} - {transaction.Amount:C}");
                        totalExpenses += transaction.Amount;
                    }
                    else
                    {
                        file.WriteLine($"Income: {transaction.Category} - {transaction.Amount:C}");
                        totalIncome += transaction.Amount;
                    }
                }

                file.WriteLine("");
                file.WriteLine($"Total Expenses: {totalExpenses:C}");
                file.WriteLine($"Total Income: {totalIncome:C}");
                file.WriteLine($"Net Balance: {(totalIncome - totalExpenses):C}");
            }
            Console.WriteLine($"Report exported to {fileName}.");
        }
    }


    // Define the Transaction class
    class Transaction
    {
        public decimal Amount { get; }
        public string Category { get; }
        public TransactionType Type { get; }

        public Transaction(decimal amount, string category, TransactionType type)
        {
            Amount = amount;
            Category = category;
            Type = type;
        }

        public override string ToString()
        {
            return $"{Category}: {Amount}$ ({Type})";
        }
    }

    class TransactionList
    {
        public List<Transaction> transactions { get; set; }

        public TransactionList()
        {
            transactions = new List<Transaction>();
        }

        public void AddTransaction(TransactionType type)
        {
            Console.Write("Enter amount: ");
            var expenseAmount = decimal.Parse(Console.ReadLine());
            Console.Write("Enter category: ");
            var expenseCategory = Console.ReadLine();

            transactions.Add(new Transaction(expenseAmount, expenseCategory, type));

            Console.WriteLine("Transaction added successfully.");
        }

        public void ClearTransactions()
        {
            transactions.Clear();

            Console.WriteLine("All transactions cleared.");
        }

        public void ViewTransactions()
        {
            Console.WriteLine("Transactions:");
            foreach (var transaction in transactions)
            {
                Console.WriteLine(transaction);
            }
        }

        public void DisplayReport(ReportGenerator reportGenerator)
        {
            reportGenerator.GenerateReport(transactions);
        }


    }
    // Define the TransactionType enum
    enum TransactionType
    {
        Expense,
        Income
    }
}

    
