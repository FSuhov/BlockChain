using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StepCoin_v1
{
    /// <summary>
    /// Класс Account, предназначен для создания счета пользователем
    /// Хранит адрес (строка, по которой данный Account ищется в списке всех счетов, 
    /// которая служит адресом отправителя или получателя в транзакции.
    /// SecretKey - строка, которая служит паролем при совершении транзакции.
    /// Класс имеет список всех исходящих и входящих транзакций и метод, который вычисляет баланс
    /// по истории всех транзакций
    /// </summary>
    public class Account
    {
        public string Address { get; set; }
        public string SecretKey { get; set; }
        public List<Transaction> IncomingTransactions { get; set; } = new List<Transaction>();
        public List<Transaction> OutcomingTransactions { get; set; } = new List<Transaction>();

        public decimal Balance()
        {
            decimal sent = 0;
            decimal received = 0;
            foreach (Transaction instance in OutcomingTransactions)
            {
                sent += instance.Amount;
            }
            foreach (Transaction instance in IncomingTransactions)
            {
                received += instance.Amount;
            }
            return (received - sent);
        }

    }
}