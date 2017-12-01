using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Globalization;

namespace StepCoin_v1
{
    public class Transaction
    {
        static int count = 0;//статический счетчик, на котором основывается присвоение Id
        public int Id { get; set; }
        public string Sender { get; set; }//адрес отправителя
        public string Recipient { get; set; } //адрес получателя
        public decimal Amount { get; set; } //сумма
        public string Timestamp { get; set; } //время создания объекта в виде строки с точностью до милисекунд

        /// <summary>
        /// Хэш данных данного экземпляра. Во-первых, будет использоваться в качестве ключа
        /// в hashtable, которая будет хранится в блокчейне. Во-вторых, может использоваться
        /// при формировании блока (вместо передачи в него всех данных транзакции в виде строки)
        /// - в этом случае необходимо калькулировать его перед запуском метода создания блока
        /// </summary>
        public string TransacationHash { get; set; }

        public Transaction(string s, string r, decimal a)
        {
            Id = ++count;
            Sender = s;
            Recipient = r;
            Amount = a;
            Timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);             
        }

        public string CalculateTransactionHash()
        {
            StringBuilder thisTransactionHash = new StringBuilder();
            string toEncrypt = Id + Sender + Recipient + Amount.ToString() + Timestamp;
            using (MD5 hash = MD5.Create())
            {
                Encoding enc = Encoding.UTF8;
                Byte[] result = hash.ComputeHash(enc.GetBytes(toEncrypt));
                foreach (Byte b in result)
                {
                    thisTransactionHash.Append(b.ToString("x2"));
                }
            }
            return thisTransactionHash.ToString();
        }

        public override string ToString()
        {
            return Id + " | " + Timestamp + " | "+ Sender + "=>" + Recipient + " : " + Amount.ToString() + "\n";
        }
    }
}
