using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace DataBasHunters.Shared
{
    public class UserMethods
    {
        public UserMethods()
        {
        }

        public List<User> GetUsers(out string errormsg)
        {
            SqlConnection dbConnection = new SqlConnection();
            dbConnection.ConnectionString = @"Server=tcp:basehunters.database.windows.net,1433;Initial Catalog=databasprojekt;Persist Security Info=False;User ID=hunters;Password=COOLkille15;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            string sqlstring = "SELECT * FROM [User]";
            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);

            List<User> userList = new List<User>();

            try
            {
                dbConnection.Open();
                SqlDataReader reader = dbCommand.ExecuteReader();

                while (reader.Read())
                {
                    User user = new User
                    {
                        Id = Convert.ToInt16(reader["Id"]),
                        Username = reader["Username"].ToString(),
                        Password = Cryptography.Decrypt(reader["Password"].ToString()),
                        Funds = Convert.ToInt32(reader["Funds"])
                    };
                    userList.Add(user);
                }
                errormsg = "";
            }
            catch (Exception e)
            {
                errormsg = e.Message;
                return new List<User>();
            }
            finally
            {
                dbConnection.Close();
            }

            return userList;
        }

        public int AddUser(User user, out string errormsg)
        {
            SqlConnection dbConnection = new SqlConnection();
            dbConnection.ConnectionString = @"Server=tcp:basehunters.database.windows.net,1433;Initial Catalog=databasprojekt;Persist Security Info=False;User ID=hunters;Password=COOLkille15;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            string sqlstring = "INSERT INTO [User] (Username, Password) VALUES (@Username, @Password)";
            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);

            dbCommand.Parameters.Add("@Username", SqlDbType.NVarChar, 50).Value = user.Username;
            dbCommand.Parameters.Add("@Password", SqlDbType.NVarChar, 50).Value = Cryptography.Encrypt(user.Password);

            try
            {
                dbConnection.Open();
                int i = dbCommand.ExecuteNonQuery();
                if (i == 1) { errormsg = ""; }
                else { errormsg = "Failed to create user, try again"; };
                return i;
            }
            catch (Exception e)
            {
                errormsg = e.Message;
                return 0;
            }
            finally
            {
                dbConnection.Close();
            }
        }

        public int LoginUser(User user, out string errormsg)
        {
            SqlConnection dbConnection = new SqlConnection();
            dbConnection.ConnectionString = @"Server=tcp:basehunters.database.windows.net,1433;Initial Catalog=databasprojekt;Persist Security Info=False;User ID=hunters;Password=COOLkille15;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            string sqlstring = "SELECT [Id] FROM [User] WHERE Username = @Username AND Password = @Password";
            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);

            dbCommand.Parameters.Add("@Username", SqlDbType.NVarChar, 50).Value = user.Username;
            dbCommand.Parameters.Add("@Password", SqlDbType.NVarChar, 50).Value = Cryptography.Encrypt(user.Password);

            try
            {
                dbConnection.Open();
                object result = dbCommand.ExecuteScalar();
                int i = (result != null) ? 1 : 0;
                if (i == 1) { errormsg = ""; }
                else { errormsg = "Failed to login in user, try again"; };
                return i;
            }
            catch (Exception e)
            {
                errormsg = e.Message;
                return 0;
            }
            finally
            {
                dbConnection.Close();
            }
        }
        public int GetUserId(string Username, out string errormsg)
        {
            SqlConnection dbConnection = new SqlConnection();
            dbConnection.ConnectionString = @"Server=tcp:basehunters.database.windows.net,1433;Initial Catalog=databasprojekt;Persist Security Info=False;User ID=hunters;Password=COOLkille15;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            string sqlstring = "SELECT [Id] FROM [User] WHERE Username = @Username";
            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);

            dbCommand.Parameters.Add("@Username", SqlDbType.NVarChar, 50).Value = Username;

            try
            {
                dbConnection.Open();

                // Använd ExecuteScalar för att hämta användar-ID från databasen
                object result = dbCommand.ExecuteScalar();

                if (result != null)
                {
                    // Om användar-ID hittades, returnera det
                    errormsg = "";
                    return Convert.ToInt32(result);
                }
                else
                {
                    // Om ingen användare hittades, sätt felmeddelande och returnera 0
                    errormsg = "User not found";
                    return 0;
                }
            }
            catch (Exception e)
            {
                // Om ett undantag uppstår, sätt felmeddelande och returnera 0
                errormsg = e.Message;
                return 0;
            }
            finally
            {
                dbConnection.Close();
            }
        }


        // DONT USE
        public void EncryptUsers(out string errormsg)
        {
            SqlConnection dbConnection = new SqlConnection();
            dbConnection.ConnectionString = @"Server=tcp:basehunters.database.windows.net,1433;Initial Catalog=databasprojekt;Persist Security Info=False;User ID=hunters;Password=COOLkille15;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            string selectSql = "SELECT [Id], [Username], [Password], [Funds] FROM [User]";
            string updateSql = "UPDATE [dbo].[User] SET [Password] = @NewPassword WHERE [Id] = @UserId";

            List<User> userList = new List<User>();

            try
            {
                dbConnection.Open();
                using (SqlCommand selectCommand = new SqlCommand(selectSql, dbConnection))
                {
                    SqlDataReader reader = selectCommand.ExecuteReader();

                    while (reader.Read())
                    {
                        User user = new User
                        {
                            Id = Convert.ToInt16(reader["Id"]),
                            Username = reader["Username"].ToString(),
                            Password = reader["Password"].ToString(),
                            Funds = Convert.ToInt32(reader["Funds"])
                        };
                        userList.Add(user);
                    }
                }
                errormsg = "";
            }
            catch (Exception e)
            {
                errormsg = e.Message;
                return;
            }
            finally
            {
                dbConnection.Close();
            }

            try
            {
                dbConnection.Open();
                using (SqlCommand updateCommand = new SqlCommand(updateSql, dbConnection))
                {
                    foreach (User user in userList)
                    {
                        // Kryptera lösenordet för varje användare
                        string encryptedPassword = Cryptography.Encrypt(user.Password);

                        // Återanvänd samma updateCommand, rensa parametrar innan varje ny användning
                        updateCommand.Parameters.Clear();
                        updateCommand.Parameters.AddWithValue("@NewPassword", encryptedPassword);
                        updateCommand.Parameters.AddWithValue("@UserId", user.Id);

                        updateCommand.ExecuteNonQuery();
                    }
                }
                errormsg = "";
            }
            catch (Exception e)
            {
                errormsg = e.Message;
            }
            finally
            {
                dbConnection.Close();
            }
        }

    }

}
