using System;
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
    }
}
