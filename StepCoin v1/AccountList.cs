using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StepCoin_v1
{
    public class AccountList
    {
        public List<Account> ListOfAllAccounts { get; set; } = new List<Account>();

        public void DepositeWithdraw(Transaction someTransaction)
        {
            foreach(Account instance in ListOfAllAccounts)
            {
                if(someTransaction.Sender == instance.Address)
                {
                    instance.OutcomingTransactions.Add(someTransaction);
                }
                if(someTransaction.Recipient == instance.Address)
                {
                    instance.IncomingTransactions.Add(someTransaction);
                }
            }
        }
    }
}