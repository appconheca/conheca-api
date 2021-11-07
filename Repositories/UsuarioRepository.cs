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
    public class UsuarioRepository : ConnectionRepository, IRepository<Usuario>
    {
        public Usuario Create(Usuario domainObject)
        {
            Usuario usuario = domainObject;
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.Connection = base.GetConnection();
            cmd.CommandText = @"INSERT INTO usuarios (nome, sobrenome, senha, email, data_nasc, status) 
                                VALUES (@nome, @sobrenome, @senha, @email, @data_nasc, @status) RETURNING id;";
            cmd.Parameters.AddWithValue("@nome", usuario.Nome);
            cmd.Parameters.AddWithValue("@sobrenome", usuario.Sobrenome);
            cmd.Parameters.AddWithValue("@senha", usuario.Senha);
            cmd.Parameters.AddWithValue("@email", usuario.Email);
            cmd.Parameters.AddWithValue("@data_nasc", usuario.DataNascimento);
            cmd.Parameters.AddWithValue("@status", usuario.Status);
            int id = Convert.ToInt32(cmd.ExecuteScalar());
            usuario.Id = id;

            return usuario;
        }

        
        public void Update(object pk, Usuario domainObject)
        {
            int id = (int)pk;
            Usuario usuario = domainObject;
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.Connection = base.GetConnection();
            cmd.CommandText = @"UPDATE usuarios SET nome = @nome, sobrenome = @sobrenome, senha = @senha,   
                                email = @email, data_nasc = @data_nasc, status = @status WHERE id = @id;";
            cmd.Parameters.AddWithValue("@id", id);            
            cmd.Parameters.AddWithValue("@nome", usuario.Nome);
            cmd.Parameters.AddWithValue("@sobrenome", usuario.Sobrenome);
            cmd.Parameters.AddWithValue("@senha", usuario.Senha);
            cmd.Parameters.AddWithValue("@email", usuario.Email);
            cmd.Parameters.AddWithValue("@data_nasc", usuario.DataNascimento);
            cmd.Parameters.AddWithValue("@status", usuario.Status);
            cmd.ExecuteNonQuery();
        }
        
        
        public void Delete(object pk)
        {
            int id = (int) pk;
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.Connection = base.GetConnection();
            cmd.CommandText = @"DELETE FROM usuarios WHERE id = @id";
            cmd.Parameters.AddWithValue("@id", id);

            cmd.ExecuteNonQuery();
        }


        public List<Usuario> Read()
        {
            List<Usuario> lista = new List<Usuario>();
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.Connection = base.GetConnection();
            cmd.CommandText = "SELECT * FROM usuarios ORDER BY nome;";
            NpgsqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Usuario usuario = new Usuario()
                {
                    Id = (int)reader["Id"],
                    Nome = (string)reader["Nome"],
                    Sobrenome = (string)reader["Sobrenome"],
                    Senha = (string)reader["Senha"],
                    Email = (string)reader["Email"],
                    DataNascimento = (DateTime)reader["data_nasc"],
                    Status = (int)reader["Status"] 
                };
                lista.Add(usuario);
            }
            reader.Close();

            return lista;
        }

        public Usuario Read(object pk)
        {
            int id = (int) pk;
            Usuario usuario = null;
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.Connection = base.GetConnection();
            cmd.CommandText = @"SELECT * FROM usuarios WHERE id = @id;";
            cmd.Parameters.AddWithValue("@id", id);
            NpgsqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                usuario = new Usuario()
                {
                    Id = (int)reader["Id"],
                    Nome = (string)reader["Nome"],
                    Sobrenome = (string)reader["Sobrenome"],
                    Senha = (string)reader["Senha"],
                    Email = (string)reader["Email"],
                    DataNascimento = (DateTime)reader["data_nasc"],
                    Status = (int)reader["Status"]
                };
            }
            reader.Close();

            return usuario;
        }
    }
}

