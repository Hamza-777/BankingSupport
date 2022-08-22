using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSupport
{
    internal interface IBankRepository
    {
        bool RegisterNewUser(string username, string password, int accno);
        bool LoginUser(string username, string password);
        bool LogoutUser();
        bool NewAccount(SBAccount acc);
        SBAccount? GetAccountDetails(int accno);
        List<SBAccount>? GetAllAccounts();
        bool DepositAmount(int accno, float amt);
        bool WithdrawAmount(int accno, float amt);
        bool TransferAmount(int fromAcc, int toAcc, float amt);
        List<SBTransaction>? GetTransactions(int accno);
        bool DeleteAccount(int accno);
    }
}
