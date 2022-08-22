using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSupport
{
    public class SBUser
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public int AccountNumber { get; set; }

        public SBUser() { }

        public SBUser(string userName, string password, int accountNumber)
        {
            UserName = userName;
            Password = password;
            AccountNumber = accountNumber;
        }
    }
}
