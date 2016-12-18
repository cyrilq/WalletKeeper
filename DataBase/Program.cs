using System;
using DT = System.Data;
using QC = System.Data.SqlClient;

namespace DataBase
{
    public class Program
    {
        static public void Main()
        {

        }


        static public string SelectRows(int User_id)
        {
            using (var command = new QC.SqlCommand())
            {
                using (var connection = new QC.SqlConnection(
              "Server=tcp:walletkeeper.database.windows.net,1433;Database=WalletKeeperDB;User ID=Arkadiy;Password=Musly1997mu;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
              ))
                {
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    connection.Open();
                    //int User_id = 12345678;
                    command.Connection = connection;
                    command.CommandType = DT.CommandType.Text;
                    command.CommandText =
                    $@"SELECT dt, Amount FROM [dbo].[Query] WHERE User_id = {User_id} 
                    ORDER BY dt DESC;";


                    QC.SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        sb.Append("").Append(String.Format("\n{0}\t{1} Р", reader.GetDateTime(0), reader.GetDouble(1)));
                    }
                    string str = sb.ToString();
                    return str;

                }
            }

        }

        static public void InsertUser(int User_id, string Name)
        {
            using (var connection = new QC.SqlConnection(
                     "Server=tcp:walletkeeper.database.windows.net,1433;Database=WalletKeeperDB;User ID=Arkadiy;Password=Musly1997mu;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
                     ))
            {
                connection.Open();
                //string Name = "Васяс";
                //int User_id = 12345679;
                QC.SqlParameter parameter;
                using (var command = new QC.SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = DT.CommandType.Text;
                    command.CommandText = $@"IF NOT EXISTS (SELECT name FROM [dbo].[User] WHERE User_id = '{User_id}') 
                                            INSERT INTO [dbo].[User] (Name, User_id) VALUES(@Name, @User_id);";

                    parameter = new QC.SqlParameter("@Name", DT.SqlDbType.NChar, 50);
                    parameter.Value = Name;
                    command.Parameters.Add(parameter);

                    parameter = new QC.SqlParameter("@User_id", DT.SqlDbType.Int);
                    parameter.Value = (int)User_id;
                    command.Parameters.Add(parameter);
                    QC.SqlDataReader reader = command.ExecuteReader();
                    Console.WriteLine("Insert - OK");

                }
            }
        }
        static public void InsertAmount(int User_id, double Amount)
        {
            using (var connection = new QC.SqlConnection(
                     "Server=tcp:walletkeeper.database.windows.net,1433;Database=WalletKeeperDB;User ID=Arkadiy;Password=Musly1997mu;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
                     ))
            {
                connection.Open();
                //int User_id = 12345679;
                //double Amount = 12.30;
                DateTime dt = DateTime.Now;
                QC.SqlParameter parameter;
                using (var command = new QC.SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = DT.CommandType.Text;
                    command.CommandText = $@"INSERT INTO [dbo].[Query] (User_id, Amount, dt) VALUES(@User_id, @Amount, @dt);";

                    parameter = new QC.SqlParameter("@User_id", DT.SqlDbType.Int);
                    parameter.Value = User_id;
                    command.Parameters.Add(parameter);

                    parameter = new QC.SqlParameter("@Amount", DT.SqlDbType.Float);
                    parameter.Value = Amount;
                    command.Parameters.Add(parameter);

                    parameter = new QC.SqlParameter("@dt", DT.SqlDbType.DateTime);
                    parameter.Value = dt;
                    command.Parameters.Add(parameter);
                    QC.SqlDataReader reader = command.ExecuteReader();
                }
            }
        }

        static public void DeleteRows(int User_id)
        {
            using (var command = new QC.SqlCommand())
            {
                using (var connection = new QC.SqlConnection(
                            "Server=tcp:walletkeeper.database.windows.net,1433;Database=WalletKeeperDB;User ID=Arkadiy;Password=Musly1997mu;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
                            ))
                {
                    connection.Open();
                    //int User_id = 12345678;
                    command.Connection = connection;
                    command.CommandType = DT.CommandType.Text;
                    command.CommandText =
                    $@"DELETE FROM [dbo].[Query] WHERE User_id = {User_id} 
                    DELETE FROM [dbo].[User] WHERE User_id = {User_id};";
                    QC.SqlDataReader reader = command.ExecuteReader();
                    Console.WriteLine("Delete - OK");

                }
            }
        }
    }
}
