using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace DatabaseAccess
{
    public class DBConnection
    {
        private DBConnection() { }

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
