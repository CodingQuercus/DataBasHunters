using System;
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
                        Password = reader["Password"].ToString(),
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
    }
}
