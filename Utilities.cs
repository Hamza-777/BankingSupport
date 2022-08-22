using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace BankingSupport
{
    public class Utilities
    {
        private static SqlConnection? connection;
        private static SqlCommand? command;

        private static SqlConnection setConnection()
        {
            var builder = new ConfigurationBuilder().AddJsonFile($"appSettings.json", true, true);
            var config = builder.Build();
            string connectionString = config["ConnectionString"];
            connection = new SqlConnection(connectionString);
            connection.Open();
            return connection;
        }

        public static int SelectMaxAccNumber()
        {
            connection = setConnection();
            command = new SqlCommand("select max(accountnumber) from SBAccount");
            command.Connection = connection;
            SqlDataReader dr = command.ExecuteReader();
            int maxAcc = 1123;
            while (dr.Read())
            {
                if (dr.IsDBNull(0))
                {
                    return maxAcc;
                }
                else
                {
                    maxAcc = Convert.ToInt32(dr[0]) + 1;
                }
            }
            dr.Close();
            
            return maxAcc;
        }

        public static int SelectMaxTransactionId()
        {
            connection = setConnection();
            command = new SqlCommand("select max(transactionid) from SBTransaction");
            command.Connection = connection;
            SqlDataReader dr = command.ExecuteReader();
            int maxId = 0;
            while (dr.Read())
            {
                if (dr.IsDBNull(0))
                {
                    return maxId;
                }
                else
                {
                    maxId = dr.GetInt32(0) + 1;
                }
            }
            dr.Close();

            return maxId;
        }

        public static List<SBAccount> SelectAllAccounts()
        {
            List<SBAccount> accountsList = new List<SBAccount>();
            connection = setConnection();
            command = new SqlCommand("select * from SBAccount");
            command.Connection = connection;
            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                if (dr.IsDBNull(0))
                {
                    return accountsList;
                }
                accountsList.Add(new SBAccount(dr.GetInt32(0), dr.GetFieldValue<string>(1), dr.GetFieldValue<string>(2), Convert.ToSingle(dr.GetValue(3))));
            }
            dr.Close();

            return accountsList;
        }

        public static List<SBTransaction> SelectAllTransactions()
        {
            List<SBTransaction> transactionsList = new List<SBTransaction>();
            connection = setConnection();
            command = new SqlCommand("select * from SBTransaction");
            command.Connection = connection;
            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                if (dr.IsDBNull(0))
                {
                    return transactionsList;
                }
                transactionsList.Add(new SBTransaction(dr.GetInt32(0), dr.GetDateTime(1), dr.GetInt32(2), Convert.ToSingle(dr.GetValue(3)), dr.GetFieldValue<string>(4)));
            }
            dr.Close();

            return transactionsList;
        }

        public static List<SBUser> SelectAllUsers()
        {
            List<SBUser> usersList = new List<SBUser>();
            connection = setConnection();
            command = new SqlCommand("select * from SBUser");
            command.Connection = connection;
            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                if (dr.IsDBNull(0))
                {
                    return usersList;
                }
                usersList.Add(new SBUser(dr.GetFieldValue<string>(0), dr.GetFieldValue<string>(1), dr.GetInt32(2)));
            }
            dr.Close();

            return usersList;
        }

        public static void InsertDataIntoSBUser(SBUser user)
        {
            connection = setConnection();
            command = new SqlCommand("insert into SBUser values(@username, @password, @accountnumber)");
            command.Connection = connection;
            command.Parameters.AddWithValue("@username", user.UserName);
            command.Parameters.AddWithValue("@password", user.Password);
            command.Parameters.AddWithValue("@accountnumber", user.AccountNumber);
            command.ExecuteNonQuery();
        }

        public static void InsertDataIntoSBAccount(SBAccount acc)
        {
            connection = setConnection();
            command = new SqlCommand("insert into SBAccount values(@accountnumber, @customername, @customeraddress, @customerbalance)");
            command.Connection = connection;
            command.Parameters.AddWithValue("@accountnumber", acc.AccountNumber);
            command.Parameters.AddWithValue("@customername", acc.CustomerName);
            command.Parameters.AddWithValue("@customeraddress", acc.CustomerAddress);
            command.Parameters.AddWithValue("@customerbalance", acc.CustomerBalance);
            command.ExecuteNonQuery();
        }

        public static void InsertDataIntoSBTransaction(SBTransaction transaction)
        {
            connection = setConnection();
            command = new SqlCommand("insert into SBTransaction values(@transactionid, @transactiondate, @accountnumber, @amount, @transactiontype)");
            command.Connection = connection;
            command.Parameters.AddWithValue("@transactionid", transaction.TransactionId);
            command.Parameters.AddWithValue("@transactiondate", transaction.TransactionDate);
            command.Parameters.AddWithValue("@accountnumber", transaction.AccountNumber);
            command.Parameters.AddWithValue("@amount", transaction.Amount);
            command.Parameters.AddWithValue("@transactiontype", transaction.TransactionType);
            command.ExecuteNonQuery();
        }

        public static void UpdateAccountBalance(SBAccount account)
        {
            connection = setConnection();
            command = new SqlCommand($"update SBAccount set customerbalance = {account.CustomerBalance} where accountnumber = {account.AccountNumber}");
            command.Connection = connection;
            command.ExecuteNonQuery();
        }

        public static void DeleteUser(int accno)
        {
            connection = setConnection();
            command = new SqlCommand($"delete from SBUser where accountnumber = {accno}");
            command.Connection = connection;
            command.ExecuteNonQuery();
        }

        public static void DeleteAccount(int accno)
        {
            connection = setConnection();
            command = new SqlCommand($"delete from SBAccount where accountnumber = {accno}");
            command.Connection = connection;
            command.ExecuteNonQuery();
        }

        public static SBUser? FindUserWithUserName(string username, List<SBUser> users)
        {
            SBUser? requiredUser = null;

            foreach (SBUser user in users)
            {
                if (user.UserName == username)
                {
                    requiredUser = user;
                }
            }

            return requiredUser;
        }

        public static SBUser? FindUserWithAccountNumber(int accno, List<SBUser> users)
        {
            SBUser? requiredUser = null;

            foreach (SBUser user in users)
            {
                if (user.AccountNumber == accno)
                {
                    requiredUser = user;
                }
            }

            return requiredUser;
        }

        public static SBUser? FindUserWithUserNameAndPassword(string username, string password, List<SBUser> users)
        {
            SBUser? requiredUser = null;

            foreach (SBUser user in users)
            {
                if (user.UserName == username && user.Password == password)
                {
                    requiredUser = user;
                }
            }

            return requiredUser;
        }

        public static SBAccount? FindAccount(int accno, List<SBAccount> accounts)
        {
            SBAccount? requiredAccount = null;

            foreach (SBAccount account in accounts)
            {
                if (account.AccountNumber == accno)
                {
                    requiredAccount = account;
                }
            }

            return requiredAccount;
        }

        public static void MakeChanges(SBAccount acc, List<SBAccount> accounts)
        {
            for (int i = 0; i < accounts.Count; i++)
            {
                if (accounts[i].AccountNumber == acc.AccountNumber)
                {
                    accounts[i] = acc;
                }
            }
        }

        public static List<SBTransaction>? FindTransactions(int accno, List<SBTransaction> transactions)
        {
            List<SBTransaction> requiredTransactions = new List<SBTransaction>();

            foreach (SBTransaction transaction in transactions)
            {
                if (transaction.AccountNumber == accno)
                {
                    requiredTransactions.Add(transaction);
                }
            }

            return requiredTransactions.Count > 0 ? requiredTransactions : null;
        }

        public static void updatePassbook(string username, SBTransaction transaction)
        {
            FileStream fs = new FileStream($"{username}.txt", FileMode.Append, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine(transaction.ToString());
            sw.Flush();
            fs.Close();
        }
    }
}
