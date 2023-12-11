using System;
using System.Data;
using Microsoft.Data.SqlClient;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DataBasHunters.Shared
{
	public class TransactionMethods
	{
		public TransactionMethods()
		{
		}
		public List<Transaction> GetTransactions(int id, out string errormsg)
		{
            SqlConnection dbConnection = new SqlConnection();
            dbConnection.ConnectionString = @"Server=tcp:basehunters.database.windows.net,1433;Initial Catalog=databasprojekt;Persist Security Info=False;User ID=hunters;Password=COOLkille15;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            string sqlstring = "SELECT * FROM [Transactions] WHERE Id = @id";
            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);

            dbCommand.Parameters.Add("@id", SqlDbType.Int).Value = id;

            List<Transaction> transactions = new List<Transaction>();

            try
            {
                dbConnection.Open();
                SqlDataReader reader = dbCommand.ExecuteReader();

                while (reader.Read())
                {
                    Transaction transaction = new Transaction()
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Sum = Convert.ToInt32(reader["Sum"]),
                        Date = Convert.ToDateTime(reader["Date"])
                    };
                    transactions.Add(transaction);
                }
                errormsg = "";
            }
            catch (Exception e)
            {
                errormsg = e.Message;
                return new List<Transaction>();
            }
            finally
            {
                dbConnection.Close();
            }
            return transactions;
        }
    }
}

