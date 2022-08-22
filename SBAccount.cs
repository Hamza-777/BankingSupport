using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSupport
{
    public class SBAccount
    {
        public int AccountNumber { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public float CustomerBalance { get; set; }

        public SBAccount() { }
        public SBAccount(int accountNumber, string customerName, string customerAddress, float customerBalance)
        {
            AccountNumber = accountNumber;
            CustomerName = customerName;
            CustomerAddress = customerAddress;
            CustomerBalance = customerBalance;
        }

        public override string ToString()
        {
            return String.Format($"Account Details:\n Account Number: {AccountNumber} \n Customer Name: {CustomerName} \n Customer Address: {CustomerAddress} \n Account Balance: {CustomerBalance} \n");
        }
    }
}
