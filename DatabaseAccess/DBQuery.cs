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
                                                  $"WHERE clients.client_id = @clientId;", _dbConnection);
            cmd.Parameters.AddWithValue("@clientId", clientId);
            NpgsqlDataReader dataReader = cmd.ExecuteReader();
            dataReader.Read();
            Console.WriteLine($"{dataReader["full_name"]}'s rental history: ");
            while (dataReader.Read())
            {
                Console.WriteLine($"{dataReader["title"]}");                
            }
            dataReader.Close();
        }

        //5. Create a new rental
        public void CreateNewRental(int clientId, int copyId)
        {
            string dateTime = DateTime.Now.ToString();
            NpgsqlCommand cmd = DBConnection.GetConnection().CreateCommand();
            cmd.Connection = DBConnection.GetConnection();
            try
            {
                cmd.CommandText = $"INSERT INTO rentals(copy_id, client_id, date_of_rental) VALUES(@copyId, @clientId, '{dateTime}')";
                cmd.Parameters.AddWithValue("@clientId", clientId);
                cmd.Parameters.AddWithValue("@copyId", copyId);
                cmd.Parameters.AddWithValue("@dateTime", dateTime);
                cmd.ExecuteNonQuery();
               
                Console.WriteLine("record is written to database");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.WriteLine("Error occured");               
            };

        }

        //6  registering a return
        public void RegistryReturn(int clientId, int coppyId, string returnDate)
        {
            string dateTime = DateTime.Now.ToString();
            NpgsqlCommand cmd = DBConnection.GetConnection().CreateCommand();
            NpgsqlTransaction transaction = DBConnection.GetConnection().BeginTransaction();
            cmd.Connection = DBConnection.GetConnection();
            cmd.Transaction = transaction;

            try
            {
                cmd.CommandText = $"UPDATE rentals SET date_of_return = '{returnDate}' WHERE copy_id = {coppyId} AND client_id = {clientId}";
                cmd.ExecuteNonQuery();
                transaction.Commit();
                Console.WriteLine("record is updated");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.WriteLine("Error occured");
                transaction.Rollback();
            };
        }

        //7 Create a new user
        public void CreateNewUser(int clientId)
        {
            Console.Write("Enter first name: ");
            string firstName = Console.ReadLine();
            Console.Write("Enter last name: ");
            string lastName = Console.ReadLine();
            Console.Write("Enter date of birth: ");
            string birthday = Console.ReadLine();           
            NpgsqlCommand cmd = DBConnection.GetConnection().CreateCommand();           

            try
            {
                cmd.CommandText = $"INSERT INTO clients(client_id, first_name, last_name, birthday) VALUES({clientId}, '{firstName}', '{lastName}','{birthday}')";
                cmd.ExecuteNonQuery();                
                Console.WriteLine("record is updated");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.WriteLine("Error occured");                
            };
        }

        public void CreateNewMovie(int movieId, int copyId)
        {
            Console.Write("Enter the title: ");
            string title = Console.ReadLine();

            Console.Write("Year: ");
            string year = Console.ReadLine();

            Console.Write("Enter Age restriction: ");
            int ageRestriction = int.Parse(Console.ReadLine());

            Console.Write("Enter Price: ");
            int price = int.Parse(Console.ReadLine());

            NpgsqlCommand cmd = DBConnection.GetConnection().CreateCommand();
            NpgsqlTransaction transaction = DBConnection.GetConnection().BeginTransaction();
            cmd.Connection = DBConnection.GetConnection();
            cmd.Transaction = transaction;

            try
            {
                cmd.CommandText = $"INSERT INTO movies(title, year, age_restriction, movie_id, price) VALUES('{title}', {year}, {ageRestriction}, {movieId}, {price})";
                cmd.ExecuteNonQuery();
                Console.WriteLine("How many copy you want to have: ");
                int nCopy = int.Parse(Console.ReadLine());                
                cmd.CommandText = $"INSERT INTO copies(copy_id, available, movie_id) VALUES('{copyId}', {true}, {movieId})";
                cmd.ExecuteNonQuery();                             
                transaction.Commit();
                Console.WriteLine("Both records are updated to database");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.WriteLine("Error occured");
                transaction.Rollback();
            };
            
        }

        public void ClientStatistic(int clientID)
        {
            NpgsqlCommand cmd = new NpgsqlCommand($"SELECT rentals.client_id, COUNT(client_id) FROM rentals " +
                                                 $"WHERE client_id = {clientID} " +
                                                 $"GROUP BY rentals.client_id " , _dbConnection);
            NpgsqlDataReader dataReader = cmd.ExecuteReader();
            Console.WriteLine($"List of actors:\n");

            while (dataReader.Read())
            {
                Console.WriteLine($"{dataReader["first_name"]} {dataReader["last_name"]}", _dbConnection);
            }
        }
    }
}