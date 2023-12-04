using System;
using System.Data.Common;
using Microsoft.Data.SqlClient;

namespace DataBasHunters.Shared
{
    public class GameMethods
    {
        public GameMethods()
        {
        }

        public List<Cointoss> GetGames(out string errormsg)
        {
            SqlConnection dbConnection = new SqlConnection();
            dbConnection.ConnectionString = @"Server=tcp:basehunters.database.windows.net,1433;Initial Catalog=databasprojekt;Persist Security Info=False;User ID=hunters;Password=COOLkille15;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            string sqlstring = "SELECT * FROM [Cointoss] WHERE Active = 1";
            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);

            List<Cointoss> games = new List<Cointoss>();

            try
            {
                dbConnection.Open();
                SqlDataReader reader = dbCommand.ExecuteReader();

                while (reader.Read())
                {
                    Cointoss game= new Cointoss
                    {
                        Id = Convert.ToInt16(reader["Id"]),
                        Date = Convert.ToDateTime(reader["Date"]),
                        Active = Convert.ToInt16(reader["Active"]),
                        Sum = Convert.ToInt16(reader["Sum"]),
                        Heads = Convert.ToBoolean(reader["Heads"])
                    };
                    games.Add(game);
                }
                errormsg = "";
            }
            catch (Exception e)
            {
                errormsg = e.Message;
                return new List<Cointoss>();
            }
            finally
            {
                dbConnection.Close();
            }

            return games;
        }


        public int CreateGame(Cointoss ct, out string errormsg)
        {
            errormsg = ""; // Initialize with a default value
            SqlConnection dbConnection = new SqlConnection();
            dbConnection.ConnectionString = @"Server=tcp:basehunters.database.windows.net,1433;Initial Catalog=databasprojekt;Persist Security Info=False;User ID=hunters;Password=COOLkille15;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            string sqlstring = "INSERT INTO [Cointoss] (Date, Sum, Heads) VALUES (@Date, @Sum, @Heads)";
            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);
            dbCommand.Parameters.Add("@Date", System.Data.SqlDbType.DateTime).Value = ct.Date;
            dbCommand.Parameters.Add("@Sum", System.Data.SqlDbType.Int).Value = ct.Sum;
            dbCommand.Parameters.Add("@Heads", System.Data.SqlDbType.Bit).Value = ct.Heads;


            try
            {
                dbConnection.Open();
                int i = 0;
                i = dbCommand.ExecuteNonQuery();

                if (i == 1)
                {
                    errormsg = "";
                }
                else
                {
                    errormsg = "De gåå int";
                    return 0;
                }
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

    }
}
