using Npgsql;
using System;

namespace DatabaseAccess
{
    public class DBConnection
    {
        private DBConnection(){    }

        private static NpgsqlConnection conn;

        public static NpgsqlConnection GetConnection()
        {
            try
            {
                if (conn == null)
                {
                    conn = new NpgsqlConnection("Server=127.0.0.1; User Id = postgres; Password = talavip123; Database = movie rental;");
                    conn.Open();
                }
                Console.WriteLine("Access success");
                return conn;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.WriteLine("Access failed");
            return conn;
        }
    }
}