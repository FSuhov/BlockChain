using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StepCoin_v1
{
    public class Validator
    {
        public BlockChain CopyOfBlockChain { get; set; }
        public Block NewlyCreatedBlock { get; set; }
        public TransactionsValidator CopyOfTransactionValidator {get;set;}
        public AccountList CopyOfAccountList { get; set; }

        /// <summary>
        /// Метод проверки корректности нового блока по двум параметрам:
        /// - Равен ли хэш предыдущего блока предыдущему хэшу нового блока;
        /// - Соотвтествует ли хэш нового блока установленной сложности.
        /// ??? Неплохо бы еше реализовать проверку, включает ли новый блок нужный список транзакций, но 
        /// ??? это будет сделано после того, как решим, как производить отсечку pending transactions
        /// </summary>
        /// <returns></returns>
        public bool IsBlockValid()
        {            
            if (CopyOfBlockChain.GetLastBlock().ThisHash != NewlyCreatedBlock.PreviousHash) return false;
            string NewBlockHashBeginning = NewlyCreatedBlock.ThisHash.Substring(0, CopyOfBlockChain.Difficulty);
            StringBuilder sample = new StringBuilder();
            for (int i = 0; i < CopyOfBlockChain.Difficulty; i++)
            {
                sample.Append("0");
            }
            if (NewBlockHashBeginning != sample.ToString()) return false;
            return true;
        }

        /// <summary>
        /// Проверка корректности блок-чейна:
        /// Пересчитываем для каждого блока хэш и сверяем, равен ли он тому, 
        /// который указан в его свойстве.
        /// Сверяем, равен ли PreviousHash каждого блока хэшу предыдущего.
        /// </summary>
        /// <returns></returns>
        public bool IsChainValid()
        {
            LinkedListNode<Block> previousBlock = CopyOfBlockChain.MyChain.First;
            for (int i = 0; i < CopyOfBlockChain.MyChain.Count - 1; i++)
            {
                LinkedListNode<Block> currentBlock = previousBlock.Next;
                if (currentBlock.Value.ThisHash != currentBlock.Value.CalculateThisHash()) return false;
                if (currentBlock.Value.PreviousHash != previousBlock.Value.ThisHash) return false;
                previousBlock = currentBlock;
            }
            return true;
        }

        /// <summary>
        /// Добавление нового блока с валидацией его.
        /// Нужно еще добавить здесь формирование транзакции с премией автору блока
        /// и добавлением этой транзакции в список pending transactions. 
        /// Будет реализовано, когда определимся как и когда опустошается список pending transactions
        /// </summary>
        public void AddNewBlock()
        {
            if(IsBlockValid()&&IsChainValid())
            {
                CopyOfBlockChain.AddBlock(NewlyCreatedBlock);
                foreach(Transaction instance in NewlyCreatedBlock.ThisBlockTransactions)
                {
                    DistrubuteMoney(instance);
                }
            }
        }

        public void DistrubuteMoney(Transaction oneOfThisBlockTransaction)
        {
            CopyOfAccountList.DepositeWithdraw(oneOfThisBlockTransaction);
        }
    }
}