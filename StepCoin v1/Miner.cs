using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace StepCoin_v1
{
    public class Miner
    {
        public string Address { get; set; }
        public BlockChain CopyOfBlockChain { get; set; }
        public TransactionsValidator CopyOfTransactionValidator { get; set; }

        public Block CreateNewBlock()
        {
            Block newBlock = new Block();
            string hash = CalculateThisBlock(newBlock);
            string prevHash = CopyOfBlockChain.MyChain.Last.Value.ThisHash;
            newBlock.Timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
            newBlock.ThisBlockTransactions = CopyOfTransactionValidator.PendingTransactions;
            newBlock.Author = Address;
            return newBlock;
        }

        public string CalculateThisBlock(Block newBlock)
        {
            //sample - строка, с которой должен начинаться хеш
            StringBuilder sample = new StringBuilder();
            for (int i = 0; i < CopyOfBlockChain.Difficulty; i++)
            {
                sample.Append("0");
            }

            string currentHash = string.Empty;
            do
            {
                newBlock.Nonce++;
                currentHash = CalculateThisHash(newBlock);
            }
            while (currentHash.Substring(0, CopyOfBlockChain.Difficulty) != sample.ToString());

            Console.WriteLine("Block mined: " + currentHash);
            return currentHash.ToString();
        }


        public string CalculateThisHash(Block newBlock)
        {
            //currentHash - строка, которую вернет этот метод. 
            StringBuilder currentHash = new StringBuilder();
            //thisBlockData - строка, в которую добвятся хэши всех транзакций из списка
            StringBuilder thisBlockData = new StringBuilder(newBlock.Index + 
                CopyOfBlockChain.MyChain.Last.Value.ThisHash + newBlock.Nonce);
            foreach (Transaction instance in CopyOfTransactionValidator.PendingTransactions)
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

    }
}