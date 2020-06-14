using Npgsql;
using System;
namespace DatabaseAccess
{
    public class DBQuery
    {
        private NpgsqlConnection _dbConnection;  

        public DBQuery(NpgsqlConnection dbConnection)
        {            
            _dbConnection = dbConnection;
            
        }
        
        public void DisplayMovieDetail(int movieId)
        {
            NpgsqlCommand cmd = new NpgsqlCommand($"SELECT * FROM movies WHERE movie_id = {movieId}",_dbConnection);
            NpgsqlDataReader dataReader = cmd.ExecuteReader();
            Console.WriteLine();
            while (dataReader.Read())
            {
                Console.WriteLine($"title: {dataReader["title"]}\n" +
                                  $"year: {dataReader["year"]}\n" +
                                  $"age restriction: {dataReader["age_restriction"]}\n" +
                                  $"price: {dataReader["price"]}\n");
            }
            dataReader.Close();
        }

        public void DisplayActors(int movieId)
        {
            NpgsqlCommand cmd = new NpgsqlCommand($"SELECT actors.first_name, actors.last_name, movies.title FROM actors " +
                                                  $"JOIN starring ON actors.actor_id = starring.actor_id " +
                                                  $"JOIN movies ON starring.movie_id = movies.movie_id "+
                                                  $"WHERE movies.movie_id = {movieId}",_dbConnection);
            NpgsqlDataReader dataReader = cmd.ExecuteReader();
            Console.WriteLine($"List of actors:\n");
            
            while(dataReader.Read())
            {
                Console.WriteLine($"{dataReader["first_name"]} {dataReader["last_name"]}", _dbConnection);
            }
        }

        public void DisplayCopiesStatus()
        {
            NpgsqlCommand cmd = new NpgsqlCommand($"SELECT * FROM copies JOIN movies ON copies.movie_id = movies.movie_id", _dbConnection);
        }

        //3. see list of movies
        public void DisplayListMovie()
        {
            NpgsqlCommand cmd = new NpgsqlCommand($"SELECT movies.title, count(copies.movie_id) AS copy_number FROM copies " +
                                                  $"JOIN movies ON copies.movie_id = movies.movie_id " +
                                                  $"GROUP BY movies.title", _dbConnection);
            NpgsqlDataReader dataReader = cmd.ExecuteReader();
            Console.WriteLine("List of movies : number of copies\n");

            while (dataReader.Read())
            {
                Console.WriteLine($"{dataReader["title"]}: {dataReader["copy_number"]}");
            }
            dataReader.Close();
        }

        //4. See User detail

        public void DisplayUserRentalDetail(int clientId)
        {
            NpgsqlCommand cmd = new NpgsqlCommand($"SELECT first_name ||' '|| last_name as full_name, movies.title FROM clients " +
                                                  $"JOIN rentals ON clients.client_id = rentals.client_id " +
                                                  $"JOIN copies ON rentals.copy_id = copies.copy_id " +
                                                  $"JOIN movies ON copies.movie_id = movies.movie_id " +
                                                  $"WHERE clients.client_id = {clientId};", _dbConnection);
            NpgsqlDataReader dataReader = cmd.ExecuteReader();
            dataReader.Read();
            Console.WriteLine($"{dataReader["full_name"]}'s rental history: ");
            while (dataReader.Read())
            {
                Console.WriteLine($"{dataReader["title"]}");
            }
            dataReader.Close();
        }


    }
}