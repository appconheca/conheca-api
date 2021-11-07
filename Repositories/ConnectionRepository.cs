using System;
using System.Data;
using Npgsql;

namespace ConhecaApi.Repositories
{
    public abstract class ConnectionRepository : IDisposable
    {        
        private static string Server = "ec2-54-87-92-21.compute-1.amazonaws.com";
        private static string UserId = "jgidmtuduonzdo";
        private static string DatabaseName = "d11idmhanku4qg";
        private static string Password = "1d4115913b8dafdee0403f0ff6ab2b678fe079c42e036e62a260bb48a05bc33d";
        private static string Port = "5432";

        string connString =
            String.Format("Server={0}; Port={1}; User Id={2}; Password={3}; Database={4}; sslmode = Require; Trust Server Certificate = true",
                Server,
                Port,
                UserId,
                Password,
                DatabaseName);

        private NpgsqlConnection connectionDB;

        protected ConnectionRepository() { }

        protected NpgsqlConnection GetConnection()
        {
            if (connectionDB == null)
            {
                try
                {
                    connectionDB = new NpgsqlConnection(connString);
                    connectionDB.Open();
                }
                catch (Exception exp)
                {
                    Console.WriteLine("Erro ao instanciar a conexão.\nErro=>\n" + exp);
                }
                
            }
            else if (connectionDB.State == ConnectionState.Open)
            {
                return connectionDB;
            }
            else if (connectionDB.State == ConnectionState.Broken)
            {
                connectionDB.Close();
                try
                {                    
                    connectionDB.Open();
                }
                catch (Exception exp)
                {
                    Console.WriteLine("Erro ao instanciar a conexão.\nErro=>\n" + exp);
                }
                
            }
            return this.connectionDB;
        }

        public void Dispose()
        {
            connectionDB?.Close();
        }
        /*
using (NpgsqlConnection conn = new NpgsqlConnection(connString))
{
   Console.Out.WriteLine("Opening connection");
   conn.Open();

   using (var command = new NpgsqlCommand("DROP TABLE IF EXISTS inventory", conn))
   {
       command.ExecuteNonQuery();
       Console.Out.WriteLine("Finished dropping table (if existed)");

   }

   using (NpgsqlCommand command = new NpgsqlCommand("CREATE TABLE inventory(id serial PRIMARY KEY, name VARCHAR(50), quantity INTEGER)", conn))
   {
       command.ExecuteNonQuery();
       Console.Out.WriteLine("Finished creating table");
   }

   using (NpgsqlCommand command = new NpgsqlCommand("INSERT INTO inventory (name, quantity) VALUES (@n1, @q1), (@n2, @q2), (@n3, @q3)", conn))
   {
       command.Parameters.AddWithValue("n1", "banana");
       command.Parameters.AddWithValue("q1", 150);
       command.Parameters.AddWithValue("n2", "orange");
       command.Parameters.AddWithValue("q2", 154);
       command.Parameters.AddWithValue("n3", "apple");
       command.Parameters.AddWithValue("q3", 100);

       int nRows = command.ExecuteNonQuery();
       Console.Out.WriteLine(String.Format("Number of rows inserted={0}", nRows));
   }
}

Console.WriteLine("Press RETURN to exit");
Console.ReadLine();
*/
    }
}
