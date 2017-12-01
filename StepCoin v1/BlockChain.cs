using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StepCoin_v1
{
    public class BlockChain
    {
        public LinkedList<Block> MyChain { get; } = new LinkedList<Block>();
        public Hashtable Transactions { get; set; } = new Hashtable();
        public int Difficulty = 4;
        public BlockChain()
        {
            MyChain.AddLast(CreateBlockZero());
        }
        public Block CreateBlockZero()
        {
            List<Transaction> list = new List<Transaction>();
            return new Block
            {
                Timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", 
                System.Globalization.CultureInfo.InvariantCulture),
                ThisBlockTransactions = list,
                PreviousHash = "00",
                ThisHash = "0000"
            };
        }

        /// <summary>
        /// Метод добавляющий и блок и транзакции из этого блока
        /// </summary>
        /// <param name="newBlock"></param>
        public void AddBlock (Block newBlock)
        {          
            MyChain.AddLast(newBlock);
            foreach (Transaction instance in newBlock.ThisBlockTransactions)
            {
                string key = instance.CalculateTransactionHash();
                Transactions.Add(key, instance);
            }               
        }

        public Block GetLastBlock()
        {
            return MyChain.Last();
        }
    }
}