using DatabaseAccess;
using Npgsql;
using System;

namespace ConsoleUI
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            NpgsqlConnection newConnect = DBConnection.GetConnection();
            DBQuery newQuery = new DBQuery(newConnect);

            bool continueProgram = true;            
            while (continueProgram)
            {
                DisplayMenu();
                int userInput = int.Parse(Console.ReadLine());
                switch (userInput)
                {
                    case 1:
                        Console.Clear();                        
                        newQuery.DisplayListMovie();
                        break;
                    case 2:
                        Console.Clear();
                        Console.Write("Please enter user id: ");                        
                        newQuery.DisplayUserRentalDetail(int.Parse(Console.ReadLine()));

                        break;
                    case 3:
                        break;
                    case 4:
                        break;
                    case 5:
                        break;
                    case 6:
                        break;
                    case 7:
                        break;
                    case 8:
                        break;
                    default:
                        break;
                }
                Console.Write("\nDo you want to go back(y = yes/ anykey = no): ");
                if (isContinue(Console.ReadLine()))
                {
                    Console.Clear();
                    continue;
                }
                else
                {
                    break;
                }
            }
        }

        static public void DisplayMenu()
        {            
            Console.WriteLine("---------------DVD RENTAL STORE ADMINSTRATION---------------");
            Console.WriteLine("                 1.List of avaiable movie");
            Console.WriteLine("                 2.Customer rental information");
            Console.WriteLine("                 3.Create a new rental");
            Console.WriteLine("                 4.Registering for returning anew rental");
            Console.WriteLine("                 5.Create a new rental");
            Console.WriteLine("                 6.Create a new user");
            Console.WriteLine("                 7.Number of rental");
            Console.WriteLine("                 8.List of overdue rental");
            Console.Write("\nEnter your selection: ");
        }

        public static bool isContinue(string ans)
        {
            return (ans.ToLower() == "y") ? true : false;

        }
    }
}