using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseAccess;
using Npgsql;


namespace ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            NpgsqlConnection newConnect = DBConnection.GetConnection();

            Console.Write("Enter the year: ");
            string year = Console.ReadLine();
            NpgsqlCommand cmd = new NpgsqlCommand($"SELECT title, year FROM movies WHERE year ={year}", newConnect);
            NpgsqlDataReader dataReader = cmd.ExecuteReader();
            Console.WriteLine($"Movie produced in {year}:");
            while (dataReader.Read())
            {
                Console.WriteLine($"Title: {dataReader["title"]}\n");
            }
            newConnect.Close();
        }
    }
}
