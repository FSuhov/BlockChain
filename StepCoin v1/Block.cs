using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace StepCoin_v1
{
    public class Block
    {
        static int counter = 0;
        public int Index { get; set; }
        public string Author { get; set; } //адрес эккаунта майнера, на который переведется вознаграждение
        public string Timestamp { get; set; }

        public List<Transaction> ThisBlockTransactions { get; set; } = new List<Transaction>();
        public string PreviousHash { get; set; }
        public string ThisHash { get; set; } = "";
        public int Nonce { get; set; } = 0;
        public Block()
        {
            Index = ++counter;
        }

        public string CalculateThisHash()
        {
            //currentHash - строка, которую вернет этот метод. 
            StringBuilder currentHash = new StringBuilder();
            //thisBlockData - строка, в которую добвятся хэши всех транзакций из списка
            StringBuilder thisBlockData = new StringBuilder(Index +
                PreviousHash + Nonce);
            foreach (Transaction instance in ThisBlockTransactions)
            {
                thisBlockData.Append(instance.CalculateTransactionHash());
            }
            string toEncrypt = thisBlockData.ToString();
            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                Byte[] result = hash.ComputeHash(enc.GetBytes(toEncrypt));
                foreach (Byte b in result)
                {
                    currentHash.Append(b.ToString("x2"));
                }
            }
            return currentHash.ToString();
        }

        public override string ToString()
        {
            string toShow = "Id = " + Index + " | " + "TimeStamp: " + Timestamp + "\n";
            foreach (Transaction item in ThisBlockTransactions)
            {
                toShow += item.ToString();
            }
            toShow += "Previous hash: " + PreviousHash + "\n";
            toShow += "This Block hash: " + ThisHash + "\n";
            toShow += "Nonce - " + Nonce + "\n";
            return toShow;
        }
    }


}