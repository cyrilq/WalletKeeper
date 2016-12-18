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
                //SelectRows(connection);    //Экспорт (Кирилл)
                InsertRows(connection);    //Импорт (Полина)
                                           //НЕ ТРОГАЙТЕ DELETE ПОКА, ПЛЗ // DeleteRows(connection);
                Console.WriteLine("Press any key to finish...");
                Console.ReadKey(true);
            }
        }

        static public void SelectRows(QC.SqlConnection connection)
        {
            using (var command = new QC.SqlCommand())
            {
                int a = 12455224;
                command.Connection = connection;
                command.CommandType = DT.CommandType.Text;
                command.CommandText =
                    $@"SELECT dt, Amount FROM [dbo].[Query] WHERE Q_User_id = {a}
                    ORDER BY dt DESC;";

                QC.SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Console.WriteLine("{0}\t{1}", reader.GetDateTime(0), reader.GetDecimal(1));
                }

            }

        }
        static public void InsertRows(QC.SqlConnection connection)
        {
            //Это прост пример; нужно перенести запарсенное имя, новый id(!!!), ну и запарсенные сумму; ID должен совпадать с новым id, прилепленным к новому юзеру
            string a = "00000";
            int b = 00000;
            int q = b;
            Decimal c = 101;
            DateTime d = DateTime.Now;
            QC.SqlParameter parameter;
            using (var command = new QC.SqlCommand())
            {
                command.Connection = connection;
                command.CommandType = DT.CommandType.Text;
                command.CommandText = $@"IF EXISTS (SELECT * FROM [dbo].[User] WHERE User_id = '{b}')
                                        BEGIN
                                        INSERT INTO [dbo].[Query] (Q_User_id, Amount, dt) VALUES(@Q_User_id, @Amount, @dt);
                                        END
                                        ELSE
                                        BEGIN
                                        INSERT INTO [dbo].[User] (Name, User_id) VALUES(@Name, @User_id)
                                        INSERT INTO [dbo].[Query] (Q_User_id, Amount, dt) VALUES(@Q_User_id, @Amount, @dt);
                                        END";
                parameter = new QC.SqlParameter("@Name", DT.SqlDbType.NChar, 50);
                parameter.Value = a;
                command.Parameters.Add(parameter);


                parameter = new QC.SqlParameter("@User_id", DT.SqlDbType.Int);
                parameter.Value = b;
                command.Parameters.Add(parameter);



                parameter = new QC.SqlParameter("@Q_User_id", DT.SqlDbType.Int);
                parameter.Value = q;
                command.Parameters.Add(parameter);

                parameter = new QC.SqlParameter("@Amount", DT.SqlDbType.Decimal);
                parameter.Value = c;
                command.Parameters.Add(parameter);

                parameter = new QC.SqlParameter("@dt", DT.SqlDbType.DateTime);
                parameter.Value = d;
                command.Parameters.Add(parameter);
                QC.SqlDataReader reader = command.ExecuteReader();

                // while (reader.Read())
                // {
                //     Console.WriteLine("{0}\t{1}", reader.GetInt32(0), reader.GetString(1));
                // }



            }



        }
        static public void DeleteRows(QC.SqlConnection connection)
        {
            using (var command = new QC.SqlCommand())
            {

                command.Connection = connection;
                command.CommandType = DT.CommandType.Text;
                command.CommandText =
                    @"DELETE FROM [dbo].[Query];
                      DELETE FROM [dbo].[User];
                      ";

                QC.SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Console.WriteLine("{0}\t{1}", reader.GetDateTime(0), reader.GetDecimal(1));
                }

            }

        }
    }
}
