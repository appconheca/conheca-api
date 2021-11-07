using ConhecaApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ConhecaApi;
using Npgsql;

namespace ConhecaApi.Repositories
{
    public class CategoriaRepository : ConnectionRepository, IRepository<Categoria>
    {
        public Categoria Create(Categoria domainObject)
        {
            Categoria categoria = domainObject;
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.Connection = base.GetConnection();
            cmd.CommandText = @"INSERT INTO categorias (tipo) VALUES (@nome) RETURNING id;";
            cmd.Parameters.AddWithValue("@nome", categoria.Tipo);            
            int id = Convert.ToInt32(cmd.ExecuteScalar());
            categoria.Id = id;

            return categoria;
        }

        
        public void Update(object pk, Categoria domainObject)
        {
            int id = (int)pk;
            Categoria categoria = domainObject;
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.Connection = base.GetConnection();
            cmd.CommandText = @"UPDATE categorias SET tipo = @tipo WHERE id = @id;";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@tipo", categoria.Tipo);
            cmd.ExecuteNonQuery();
        }
        
        
        public void Delete(object pk)
        {
            int id = (int) pk;
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.Connection = base.GetConnection();
            cmd.CommandText = @"DELETE FROM categorias WHERE id = @id";
            cmd.Parameters.AddWithValue("@id", id);

            cmd.ExecuteNonQuery();
        }

        public List<Categoria> Read()
        {
            List<Categoria> lista = new List<Categoria>();
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.Connection = base.GetConnection();
            cmd.CommandText = "SELECT c.id AS \"Id\", c.tipo AS \"Tipo\" FROM categorias c ORDER BY c.tipo;";
            NpgsqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Categoria categoria = new Categoria()
                {
                    Id = (int)reader["Id"],
                    Tipo = (string)reader["Tipo"]
                };
                lista.Add(categoria);
            }
            reader.Close();

            return lista;
        }

        public Categoria Read(object pk)
        {
            int id = (int) pk;
            Categoria categoria = null;
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.Connection = base.GetConnection();
            cmd.CommandText = @"SELECT c.id AS ""Id"", c.tipo AS ""Tipo"" FROM categorias c WHERE id = @id;";
            cmd.Parameters.AddWithValue("@id", id);
            NpgsqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                categoria = new Categoria
                {   
                    Id = (int) reader["Id"],
                    Tipo = (string) reader["Tipo"]
                    
                };
            }
            reader.Close();

            return categoria;
        }
    }
}
