using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSupport
{
    public class SBTransaction
    {
        public int TransactionId { get; set; }
        public DateTime TransactionDate { get; set; }
        public int AccountNumber { get; set; }
        public float Amount { get; set; }
        public string TransactionType { get; set; }

        public SBTransaction() { }
        public SBTransaction(int transactionId, DateTime transactionDate, int accountNumber, float amount, string transactionType)
        {
            TransactionId = transactionId;
            TransactionDate = transactionDate;
            AccountNumber = accountNumber;
            Amount = amount;
            TransactionType = transactionType;
        }
        public override string ToString()
        {
            return String.Format($"Transaction Details:\n Transaction ID: {TransactionId} \n Transaction Date: {TransactionDate} \n Account Number: {AccountNumber} \n Amount Transferred: {Amount} \n Transaction Type: {TransactionType} \n");
        }
    }
}
