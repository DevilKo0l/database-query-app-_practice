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
                int clientId;
                int coppyId;
                int movieId;
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
                        Console.Clear();
                        Console.Write("Enter client id: ");
                        clientId = int.Parse(Console.ReadLine());
                        Console.Write("Enter movie id: ");
                        movieId = int.Parse(Console.ReadLine());
                        newQuery.CreateNewRental(clientId, movieId);
                        break;

                    case 4:
                        Console.Clear();
                        Console.WriteLine("Enter client id: ");
                        clientId = int.Parse(Console.ReadLine());
                        Console.WriteLine("Enter coppy id: ");
                        coppyId = int.Parse(Console.ReadLine());
                        Console.WriteLine("Enter return date: ");
                        string returnDate = Console.ReadLine();
                        newQuery.RegistryReturn(clientId, coppyId, returnDate);
                        break;

                    case 5:
                        Console.Clear();
                        Console.Write("Enter client id: ");
                        clientId = int.Parse(Console.ReadLine());                        
                        newQuery.CreateNewUser(clientId);
                        break;                        

                    case 6:
                        Console.Clear();
                        Console.Write("Enter movie id: ");
                        movieId = int.Parse(Console.ReadLine());
                        Console.Write("Enter copy id: ");
                        coppyId = int.Parse(Console.ReadLine());
                        newQuery.CreateNewMovie(movieId, coppyId);
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
            Console.WriteLine("                 4.Registering for returning");
            Console.WriteLine("                 5.Create a new user");
            Console.WriteLine("                 6.Create a new movie");
            Console.WriteLine("                 7.Rental statistic");
            Console.WriteLine("                 8.List of overdue rental");
            Console.Write("\nEnter your selection: ");
        }

        public static bool isContinue(string ans)
        {
            return (ans.ToLower() == "y") ? true : false;
        }
    }
}