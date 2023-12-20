using System;
using System.Data;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
                    Cointoss game = new Cointoss
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

        public List<Cointoss> GetMyGames(int id, out string errormsg)
        {
            SqlConnection dbConnection = new SqlConnection();
            dbConnection.ConnectionString = @"Server=tcp:basehunters.database.windows.net,1433;Initial Catalog=databasprojekt;Persist Security Info=False;User ID=hunters;Password=COOLkille15;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            string sqlstring = "SELECT * FROM Cointoss c, MadeGame mg WHERE c.Id = mg.CointossId AND ACTIVE = 1 AND mg.UserId = @id";
            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);
            dbCommand.Parameters.Add("@id", System.Data.SqlDbType.Int).Value = id;


            List<Cointoss> games = new List<Cointoss>();

            try
            {
                dbConnection.Open();
                SqlDataReader reader = dbCommand.ExecuteReader();

                while (reader.Read())
                {
                    Cointoss game = new Cointoss
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
            errormsg = "";
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

        public int AddGame(Cointoss ct, int CreatorId, out string errormsg)
        {
            SqlConnection dbConnection = new SqlConnection();
            dbConnection.ConnectionString = @"Server=tcp:basehunters.database.windows.net,1433;Initial Catalog=databasprojekt;Persist Security Info=False;User ID=hunters;Password=COOLkille15;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            string sqlstring = "EXECUTE [dbo].[CreateGame] @CreatorId, @Sum, @Heads, @CreationDate";

            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);

            dbCommand.Parameters.Add("@CreatorId", SqlDbType.Int).Value = CreatorId;
            dbCommand.Parameters.Add("@CreationDate", SqlDbType.DateTime).Value = DateTime.Now;
            dbCommand.Parameters.Add("@Sum", SqlDbType.Int).Value = ct.Sum;
            dbCommand.Parameters.Add("@Heads", SqlDbType.Bit).Value = ct.Heads;



            try
            {
                dbConnection.Open();
                dbCommand.ExecuteScalar();
                errormsg = "";

                return 1;
            }
            catch (Exception e)
            {
                errormsg = $"Error: {e.Message}\nStackTrace: {e.StackTrace}";
                return 0;
            }
            finally
            {
                dbConnection.Close();
            }
        }
        public int DeleteGame(int userId, int gameId, out string errormsg)
        {
            SqlConnection dbConnection = new SqlConnection();
            dbConnection.ConnectionString = @"Server=tcp:basehunters.database.windows.net,1433;Initial Catalog=databasprojekt;Persist Security Info=False;User ID=hunters;Password=COOLkille15;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            string sqlstring = "EXECUTE [dbo].[DropGame] @UserId, @GameId";

            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);

            dbCommand.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;
            dbCommand.Parameters.Add("@GameId", SqlDbType.Int).Value = gameId;


            try
            {
                dbConnection.Open();
                dbCommand.ExecuteScalar();
                errormsg = "";

                return 1;
            }
            catch (Exception e)
            {
                errormsg = $"Error: {e.Message}\nStackTrace: {e.StackTrace}";
                return 0;
            }
            finally
            {
                dbConnection.Close();
            }
        }


        public Cointoss GetGameById(int id, out string errorMsg)
        {
            SqlConnection dbConnection = new SqlConnection();
            dbConnection.ConnectionString = @"Server=tcp:basehunters.database.windows.net,1433;Initial Catalog=databasprojekt;Persist Security Info=False;User ID=hunters;Password=COOLkille15;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            string sqlstring = "SELECT Id, Date, Sum, Heads, Active FROM [Cointoss] WHERE Id = @Id";
            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);

            dbCommand.Parameters.Add("@Id", SqlDbType.Int).Value = id;

            try
            {
                dbConnection.Open();

                SqlDataReader reader = dbCommand.ExecuteReader();

                if (reader.Read())
                {
                    Cointoss game = new Cointoss();
                    game.Id = Convert.ToInt32(reader["Id"]);
                    game.Date = Convert.ToDateTime(reader["Date"]);
                    game.Sum = Convert.ToInt32(reader["Sum"]);
                    game.Heads = Convert.ToBoolean(reader["Heads"]);
                    game.Heads = Convert.ToBoolean(reader["Active"]);

                    errorMsg = "";
                    return game;
                }
                else
                {
                    errorMsg = "Game not found";
                    return null;
                }
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
                return null;
            }
            finally
            {
                dbConnection.Close();
            }
        }
        public int JoinGame(Cointoss ct, User joinperson, out string errormsg)
        {
            SqlConnection dbConnection = new SqlConnection();
            dbConnection.ConnectionString = @"Server=tcp:basehunters.database.windows.net,1433;Initial Catalog=databasprojekt;Persist Security Info=False;User ID=hunters;Password=COOLkille15;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            string sqlstring = "UPDATE [USER] SET [FUNDS] = [FUNDS] - (@Sum) WHERE [Id] = @joinperson";

            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);

            dbCommand.Parameters.Add("@joinperson", SqlDbType.Int).Value = joinperson.Id;
            dbCommand.Parameters.Add("@Sum", SqlDbType.Int).Value = ct.Sum;




            try
            {
                dbConnection.Open();
                dbCommand.ExecuteScalar();
                errormsg = "";

                return 1;
            }
            catch (Exception e)
            {
                errormsg = $"Error: {e.Message}\nStackTrace: {e.StackTrace}";
                return 0;
            }
            finally
            {
                dbConnection.Close();
            }
        }
        public int FinishGame(Cointoss ct, int winnerId, int loserId, out string errormsg)
        {
            SqlConnection dbConnection = new SqlConnection();
            dbConnection.ConnectionString = @"Server=tcp:basehunters.database.windows.net,1433;Initial Catalog=databasprojekt;Persist Security Info=False;User ID=hunters;Password=COOLkille15;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            string sqlstring = "EXECUTE [dbo].[WinnerWinner] @WinnerId, @LoserId, @GameId, @Date";

            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);

            dbCommand.Parameters.Add("@WinnerId", SqlDbType.Int).Value = winnerId;
            dbCommand.Parameters.Add("@LoserId", SqlDbType.Int).Value = loserId;
            dbCommand.Parameters.Add("@GameId", SqlDbType.Int).Value = ct.Id;
            dbCommand.Parameters.Add("@Date", SqlDbType.DateTime).Value = DateTime.Now;



            try
            {
                dbConnection.Open();
                dbCommand.ExecuteScalar();
                errormsg = "";

                return 1;
            }
            catch (Exception e)
            {
                errormsg = $"Error: {e.Message}\nStackTrace: {e.StackTrace}";
                return 0;
            }
            finally
            {
                dbConnection.Close();
            }
        }
    }
}
