using System;
using DT = System.Data;
using QC = System.Data.SqlClient;

namespace DataBase
{
    public class Program
    {
        static public void Main()
        {
            using (var connection = new QC.SqlConnection(
            "Server=tcp:walletkeeper.database.windows.net,1433;Database=WalletKeeperDB;User ID=Arkadiy;Password=Musly1997mu;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
            ))
            {
                connection.Open();
                Console.WriteLine("Connected successfully.");
                //Экспорт; Вводится User_id, выводится время и сумма 
                //SelectRows(connection); 
                //Импорт; 1)Вводится Имя и User_id, вводится в базу инфа о юзере; 2)Вводится User_id, Сумма, вводится в базу инфа о сумме и времени, связывается с таблицей User 
                //InsertUser(connection); 
                //InsertAmount(connection); 
                //Удаление; Вводится User_id, удаляет всю инфу о нем 
                //DeleteRows(connection); 
                Console.WriteLine("Press any key to finish...");
                Console.ReadKey(true);
            }
        }

        static public void SelectRows(QC.SqlConnection connection)
        {
            using (var command = new QC.SqlCommand())
            {
                int User_id = 12345678;
                command.Connection = connection;
                command.CommandType = DT.CommandType.Text;
                command.CommandText =
                $@"SELECT dt, Amount FROM [dbo].[Query] WHERE User_id = {User_id} 
                    ORDER BY dt DESC;";

                QC.SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Console.WriteLine("{0}\t{1} Р", reader.GetDateTime(0), reader.GetDouble(1));
                }

            }

        }
        static public void InsertUser(QC.SqlConnection connection)
        {

            string Name = "Васяс";
            int User_id = 12345679;
            QC.SqlParameter parameter;
            using (var command = new QC.SqlCommand())
            {
                command.Connection = connection;
                command.CommandType = DT.CommandType.Text;
                command.CommandText = $@"INSERT INTO [dbo].[User] (Name, User_id) VALUES(@Name, @User_id);";

                parameter = new QC.SqlParameter("@Name", DT.SqlDbType.NChar, 50);
                parameter.Value = Name;
                command.Parameters.Add(parameter);

                parameter = new QC.SqlParameter("@User_id", DT.SqlDbType.Int);
                parameter.Value = User_id;
                command.Parameters.Add(parameter);

                QC.SqlDataReader reader = command.ExecuteReader();



            }



        }
        static public void InsertAmount(QC.SqlConnection connection)
        {

            int User_id = 12345679;
            double Amount = 12.30;
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
        static public void DeleteRows(QC.SqlConnection connection)
        {
            using (var command = new QC.SqlCommand())
            {
                int User_id = 12345678;
                command.Connection = connection;
                command.CommandType = DT.CommandType.Text;
                command.CommandText =
                $@"DELETE FROM [dbo].[Query] WHERE User_id = {User_id} 
                    DELETE FROM [dbo].[User] WHERE User_id = {User_id};";

                QC.SqlDataReader reader = command.ExecuteReader();


            }

        }
    }
}
