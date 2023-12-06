﻿using System;
using System.Data;
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

        public int AddGame(Cointoss ct, out string errormsg)
        {
            SqlConnection dbConnection = new SqlConnection();
            dbConnection.ConnectionString = @"Server=tcp:basehunters.database.windows.net,1433;Initial Catalog=databasprojekt;Persist Security Info=False;User ID=hunters;Password=COOLkille15;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            string sqlstring = "INSERT INTO [Cointoss] (Date, Sum, Heads) VALUES (@Date, @Sum, @Heads)";
            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);

            dbCommand.Parameters.Add("@Date", SqlDbType.NVarChar, 50).Value = DateTime.Now;
            dbCommand.Parameters.Add("@Sum", SqlDbType.NVarChar, 50).Value = ct.Sum;
            dbCommand.Parameters.Add("@Heads", SqlDbType.NVarChar, 50).Value = ct.Heads;


            try
            {
                dbConnection.Open();
                int i = dbCommand.ExecuteNonQuery();
                if (i == 1) { errormsg = ""; }
                else { errormsg = "Failed to create game, try again"; };
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

        public Cointoss GetGameById(int id, out string errorMsg)
        {
            SqlConnection dbConnection = new SqlConnection();
            dbConnection.ConnectionString = @"Data Source=localhost,1433; DataBase=YourDatabaseName; User Id=yourUsername; Password=yourPassword;";
            String sqlstring = "SELECT Id, Date, Sum FROM [Cointoss] WHERE Id = @Id";
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
    }
}
