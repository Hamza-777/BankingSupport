namespace BankingSupport
{
    public class BankRepository : IBankRepository
    {
        static List<SBAccount> accounts = Utilities.SelectAllAccounts();
        static List<SBTransaction> transactions = Utilities.SelectAllTransactions();
        static List<SBUser> users = Utilities.SelectAllUsers();
        public static SBUser? loggedInUser = null;
        static int transactionId = Utilities.SelectMaxTransactionId();

        public bool RegisterNewUser(string username, string password, int accno)
        {
            try
            {
                SBAccount? requiredAccount = Utilities.FindAccount(accno, accounts);
                SBUser? requiredUserByUserName = Utilities.FindUserWithUserName(username, users);
                SBUser? requiredUserByAccountNumber = Utilities.FindUserWithAccountNumber(accno, users);

                if (accounts.Count == 0)
                {
                    throw new Exception("No Accounts are registered in the banking system yet!");
                } else if(requiredUserByUserName != null && requiredUserByUserName.UserName == username)
                {
                    throw new Exception("User with given username already exists!");
                } else if (requiredUserByAccountNumber != null && requiredUserByAccountNumber.AccountNumber == accno)
                {
                    throw new Exception("User with given account number already exists!");
                }

                if (requiredAccount != null)
                {
                    users.Add(new SBUser(username, password, accno));
                    Utilities.InsertDataIntoSBUser(users[users.Count - 1]);
                    return true;
                }
                else
                {
                    throw new Exception("Entered Account Number Does Not Exist!");
                }
            } catch(Exception e)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine(e.Message);
                Console.ResetColor();
                return false;
            }
        }

        public bool LoginUser(string username, string password)
        {
            try
            {
                SBUser? requiredUser = Utilities.FindUserWithUserNameAndPassword(username, password, users);

                if (requiredUser != null)
                {
                    requiredUser.Password = "**********";
                    loggedInUser = requiredUser;
                    return true;
                }
                else
                {
                    throw new Exception("Entered User Does Not Exist!");
                }
            } catch(Exception e)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine(e.Message);
                Console.ResetColor();
                return false;
            }
        }

        public bool LogoutUser()
        {
            loggedInUser = null;
            return true;
        }

        public bool DepositAmount(int accno, float amt)
        {
            try
            {
                if (accounts.Count == 0)
                {
                    throw new Exception("No Accounts are registered in the banking system yet!");
                } else if (amt <= 0)
                {
                    throw new Exception("Enter a valid amount to deposit!");
                }

                SBAccount? requiredAccount = Utilities.FindAccount(accno, accounts);

                if (requiredAccount != null)
                {
                    requiredAccount.CustomerBalance += amt;
                    Utilities.UpdateAccountBalance(requiredAccount);
                    Utilities.MakeChanges(requiredAccount, accounts);
                    transactions.Add(new SBTransaction(transactionId, DateTime.Now, accno, amt, "Deposit"));
                    Utilities.InsertDataIntoSBTransaction(transactions[transactionId]);
                    Utilities.updatePassbook(String.Format(requiredAccount.CustomerName + requiredAccount.AccountNumber), transactions[transactionId]);
                    transactionId++;
                    return true;
                }
                else
                {
                    throw new Exception("Entered Account Number Does Not Exist!");
                }
            } catch(Exception e)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine(e.Message);
                Console.ResetColor();
                return false;
            }
        }

        public bool TransferAmount(int fromAcc, int toAcc, float amt)
        {
            try
            {
                if (accounts.Count == 0)
                {
                    throw new Exception("No Accounts are registered in the banking system yet!");
                }
                else if (amt <= 0)
                {
                    throw new Exception("Enter a valid amount to transfer!");
                }

                SBAccount? fromAccount = Utilities.FindAccount(fromAcc, accounts);
                SBAccount? toAccount = Utilities.FindAccount(toAcc, accounts);

                if(fromAccount == null)
                {
                    throw new Exception("Entered From Account does not exist!");
                } else if (toAccount == null)
                {
                    throw new Exception("Entered To Account does not exist!");
                }
                else
                {
                    if (amt > fromAccount.CustomerBalance)
                    {
                        throw new Exception("Not enough balance to transfer the entered amount!");
                    } else
                    {
                        fromAccount.CustomerBalance -= amt;
                        Utilities.UpdateAccountBalance(fromAccount);
                        transactions.Add(new SBTransaction(transactionId, DateTime.Now, fromAcc, amt, $"Transaction To Account Number {toAcc}"));
                        Utilities.InsertDataIntoSBTransaction(transactions[transactionId]);
                        Utilities.updatePassbook(String.Format(fromAccount.CustomerName + fromAccount.AccountNumber), transactions[transactionId]);
                        transactionId++;
                        toAccount.CustomerBalance += amt;
                        Utilities.UpdateAccountBalance(toAccount);
                        transactions.Add(new SBTransaction(transactionId, DateTime.Now, toAcc, amt, $"Transaction From Account Number {fromAcc}"));
                        Utilities.InsertDataIntoSBTransaction(transactions[transactionId]);
                        Utilities.updatePassbook(String.Format(toAccount.CustomerName + toAccount.AccountNumber), transactions[transactionId]);
                        transactionId++;
                        Utilities.MakeChanges(fromAccount, accounts);
                        Utilities.MakeChanges(toAccount, accounts);
                        return true;
                    }
                }
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine(e.Message);
                Console.ResetColor();
                return false;
            }
        }

        public SBAccount? GetAccountDetails(int accno)
        {
            try
            {
                if (accounts.Count == 0)
                {
                    throw new Exception("No Accounts are registered in the banking system yet!");
                }
                
                SBAccount? requiredAccount = Utilities.FindAccount(accno, accounts);

                if(requiredAccount != null)
                {
                    return requiredAccount;
                }
                else
                {
                    throw new Exception("Entered Account Number Does Not Exist!");
                }
            } catch(Exception e)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine(e.Message);
                Console.ResetColor();
                return null;
            }
        }

        public List<SBAccount>? GetAllAccounts()
        {
            try
            {
                if(accounts.Count == 0)
                {
                    throw new Exception("No Accounts are registered in the banking system yet!");
                }
                return accounts;
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine(e.Message);
                Console.ResetColor();
                return null;
            }
        }

        public List<SBTransaction>? GetTransactions(int accno)
        {
            try
            {
                if (transactions.Count == 0)
                {
                    throw new Exception("There are no transactions to show!");
                } else if(Utilities.FindAccount(accno, accounts) == null)
                {
                    throw new Exception("Entered Account Number Does Not Exist!");
                } else
                {
                    List<SBTransaction>? requiredTransactions = Utilities.FindTransactions(accno, transactions);

                    if(requiredTransactions != null)
                    {
                        return requiredTransactions;
                    }
                    else
                    {
                        throw new Exception("Entered account hasn't made any transactions yet!");
                    }
                }
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine(e.Message);
                Console.ResetColor();
                return null;
            }
        }

        public bool NewAccount(SBAccount acc)
        {
            try
            {
                foreach (SBAccount account in accounts)
                {
                    if(account.AccountNumber == acc.AccountNumber)
                    {
                        throw new Exception("Entered Account Number Already Exists!");
                    }
                }
                accounts.Add(acc);
                Utilities.InsertDataIntoSBAccount(acc);
                return true;
            } catch(Exception e)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine(e.Message);
                Console.ResetColor();
                return false;
            }
        }

        public bool WithdrawAmount(int accno, float amt)
        {
            try
            {
                if (accounts.Count == 0)
                {
                    throw new Exception("No Accounts are registered in the banking system yet!");
                } else if (amt <= 0)
                {
                    throw new Exception("Enter a valid amount to withdraw!");
                }

                SBAccount? requiredAccount = Utilities.FindAccount(accno, accounts);

                if (requiredAccount != null)
                {
                    if (amt > requiredAccount.CustomerBalance)
                    {
                        throw new Exception("Not enough balance to withdraw the entered amount!");
                    }
                    else
                    {
                        requiredAccount.CustomerBalance -= amt;
                        Utilities.UpdateAccountBalance(requiredAccount);
                        Utilities.MakeChanges(requiredAccount, accounts);
                        transactions.Add(new SBTransaction(transactionId, DateTime.Now, accno, amt, "Withdrawl"));
                        Utilities.InsertDataIntoSBTransaction(transactions[transactionId]);
                        Utilities.updatePassbook(String.Format(requiredAccount.CustomerName + requiredAccount.AccountNumber), transactions[transactionId]);
                        transactionId++;
                        return true;
                    }
                }
                else
                {
                    throw new Exception("Entered Account Number Does Not Exist!");
                }
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine(e.Message);
                Console.ResetColor();
                return false;
            }
        }

        public bool DeleteAccount(int accno)
        {
            try
            {
                if (accounts.Count == 0)
                {
                    throw new Exception("No Accounts are registered in the banking system yet!");
                }

                SBAccount? requiredAccount = Utilities.FindAccount(accno, accounts);
                SBUser? requiredUser = Utilities.FindUserWithAccountNumber(accno, users);

                if (requiredAccount != null)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Warning: Are you sure you want to go on to delete your account? This process is not reversible!!");
                    Console.Write("Warning: Want to continue? (y/n)");
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    string? confirm = Console.ReadLine();
                    while(confirm == "")
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.Write("Enter a valid option... Choose from (y/n): ");
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        confirm = Console.ReadLine();
                    }
                    if(confirm == "y")
                    {
                        accounts.Remove(requiredAccount);
                        Utilities.DeleteAccount(accno);
                        if(requiredUser != null)
                        {
                            users.Remove(requiredUser);
                            Utilities.DeleteUser(accno);
                        }
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.WriteLine("Thank you for choosing to stay with us!!");
                        return false;
                    }
                    return true;
                }
                else
                {
                    throw new Exception("Entered Account Number Does Not Exist!");
                }
            }
            catch(Exception e)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine(e.Message);
                Console.ResetColor();
                return false;
            }
        }
    }
}