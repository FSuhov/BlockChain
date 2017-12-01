using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StepCoin_v1
{
    public class TransactionsValidator
    {
        public List<Transaction> PendingTransactions { get; set; } = new List<Transaction>();
        public List<Transaction> RejectedTransactions { get; set; } = new List<Transaction>();
        
        
        public void AddTransaction(Transaction someTransaction, Hashtable transactions, List<Account> ListOfAllAccounts)
        {
            if (IsValidTransaction(someTransaction, transactions, ListOfAllAccounts))
            {
                PendingTransactions.Add(someTransaction);
            }
            else RejectedTransactions.Add(someTransaction);
        } 

        public bool IsValidTransaction(Transaction someTransaction, Hashtable transactions, List<Account> ListOfAllAccounts)
        {
            if (!IsValidAddresses(someTransaction, ListOfAllAccounts)) return false;
            decimal amountRecieved = 0;
            foreach(Transaction instance in TransactionsAsRecipent(someTransaction, transactions))
            {
                amountRecieved += instance.Amount;
            }
            decimal amountSent = 0;
            foreach (Transaction instance in TransactionsAsSender(someTransaction, transactions))
            {
                amountSent += instance.Amount;
            }
            return ((amountRecieved - amountSent - someTransaction.Amount) >= 0);
        }

        public bool IsValidAddresses(Transaction someTransaction, List<Account> ListOfAllAccounts)
        {
            if (someTransaction.Sender == someTransaction.Recipient) return false;
            bool isValidSender = false;
            bool isValidRecipient = false;
            foreach(Account account in ListOfAllAccounts)
            {
                if (someTransaction.Sender == account.Address) isValidSender = true;
                if (someTransaction.Recipient == account.Address) isValidRecipient = true;
            }
            if (isValidRecipient && isValidSender) return true;
            else return false;
        }

        public List<Transaction> TransactionsAsSender(Transaction someTransaction, Hashtable transactions)
        {
            List<Transaction> outcomingTransactions = new List<Transaction>();
            foreach(Transaction instance in transactions)
            {
                if (someTransaction.Sender == instance.Sender) outcomingTransactions.Add(instance);
            }
            return outcomingTransactions;
        }

        public List<Transaction> TransactionsAsRecipent(Transaction someTransaction, Hashtable transactions)
        {
            List<Transaction> incomingTransactions = new List<Transaction>();
            foreach (Transaction instance in transactions)
            {
                if (someTransaction.Sender == instance.Recipient) incomingTransactions.Add(instance);
            }
            return incomingTransactions;
        }
    }
}