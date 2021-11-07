using ConhecaApi.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConhecaApi.Repositories
{
    public class PontoTuristicoRepository : ConnectionRepository, IRepository<PontoTuristico>
    {
        PontoTuristico IRepository<PontoTuristico>.Create(PontoTuristico domainObject)
        {
            PontoTuristico ponto = domainObject;
            NpgsqlTransaction transaction = base.GetConnection().BeginTransaction();
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand()
                {
                    Connection = base.GetConnection(),
                    Transaction = transaction,
                    CommandText = @"INSERT INTO pontos_turisticos (nome, descricao, data, latitude, longitude, usuario_id) 
                                    VALUES (@nome, @descricao, @data, @latitude, @longitude, @usuario_id) RETURNING id;"

                };
                cmd.Parameters.AddWithValue("@nome", ponto.Nome);
                cmd.Parameters.AddWithValue("@descricao", ponto.Descricao);
                cmd.Parameters.AddWithValue("@data", ponto.Data);
                cmd.Parameters.AddWithValue("@latitude", ponto.Latitude);
                cmd.Parameters.AddWithValue("@longitude", ponto.Longitude);
                cmd.Parameters.AddWithValue("@usuario_id", ponto.Usuario.Id);
                int id = Convert.ToInt32(cmd.ExecuteScalar());
                ponto.Id = id;


                if (ponto.Endereco != null)
                {
                    NpgsqlCommand enderecoCommand = new NpgsqlCommand
                    {
                        Connection = base.GetConnection(),
                        Transaction = transaction,
                        CommandText = @"INSERT INTO enderecos (ponto_id, numero, logradouro, cep, complemento, referencia, cidade_id)
	                                VALUES (@ponto_id, @numero, @logradouro, @cep, @complemento, @referencia, @cidade_id)"
                    };
                    enderecoCommand.Parameters.AddWithValue("@ponto_id", ponto.Id);
                    enderecoCommand.Parameters.AddWithValue("@numero", ponto.Endereco.Numero);
                    enderecoCommand.Parameters.AddWithValue("@logradouro", ponto.Endereco.Logradouro);
                    enderecoCommand.Parameters.AddWithValue("@cep", ponto.Endereco.CEP);
                    enderecoCommand.Parameters.AddWithValue("@complemento", ponto.Endereco.Complemento);
                    enderecoCommand.Parameters.AddWithValue("@referencia", ponto.Endereco.Referencia);
                    enderecoCommand.Parameters.AddWithValue("@cidade_id", ponto.Endereco.Cidade.Id);
                    enderecoCommand.ExecuteNonQuery();
                }

                // PontoTuristico__Categoria - Relacionamento N--N
                foreach (PontoTuristicoCategoria pc in ponto.Categorias)
                {
                    NpgsqlCommand nnCommand = new NpgsqlCommand
                    {
                        Connection = base.GetConnection(),
                        Transaction = transaction,
                        CommandText = @"INSERT INTO pontos__categorias (ponto_id, categoria_id)
                                        VALUES (@ponto_id, @categoria_id)"
                    };
                    nnCommand.Parameters.AddWithValue("@ponto_id", ponto.Id);
                    nnCommand.Parameters.AddWithValue("@categoria_id", pc.Categoria.Id);

                    nnCommand.ExecuteNonQuery();
                }
                // Tudo certo
                transaction.Commit();

                return ponto;
            }
            catch (Exception exp)
            {
                // Algo deu ruim, desfazer tudo
                transaction.Rollback();

                // relançar exceção para ser tratada pela camada de serviço
                throw exp;
            }            
        }


        void IRepository<PontoTuristico>.Delete(object pk)
        {
            int id = (int)pk;
            NpgsqlTransaction transaction = base.GetConnection().BeginTransaction();
            try
            {
                // Apagando endereço, se houver
                NpgsqlCommand enderecoCommand = new NpgsqlCommand
                {
                    Connection = base.GetConnection(),
                    Transaction = transaction,
                    CommandText = @"DELETE FROM enderecos WHERE ponto_id = @id;"
                };
                enderecoCommand.Parameters.AddWithValue("@id", id);
                enderecoCommand.ExecuteNonQuery();


                // Relacionamento N--N - PontoTuristico__Categoria
                NpgsqlCommand nnCommand = new NpgsqlCommand
                {
                    Connection = base.GetConnection(),
                    Transaction = transaction,
                    CommandText = @"DELETE FROM pontos__categorias WHERE ponto_id = @id;"
                };
                nnCommand.Parameters.AddWithValue("@id", id);
                nnCommand.ExecuteNonQuery();


                // PontoTurísitco
                NpgsqlCommand cmd = new NpgsqlCommand
                {
                    Connection = base.GetConnection(),
                    Transaction = transaction,
                    CommandText = @"DELETE FROM pontos_turisticos WHERE id = @id;"
                };
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();

                // Tudo certo
                transaction.Commit();
            }
            catch (Exception exp)
            {
                // Algo deu ruim, desfazer tudo
                transaction.Rollback();
                // relançar exceção para ser tratada pela camada de serviço
                throw exp;
            }
        }

        List<PontoTuristico> IRepository<PontoTuristico>.Read()
        {
            List<int> listaIds = new List<int>();
            List<PontoTuristico> listaPontos = new List<PontoTuristico>();
            NpgsqlCommand cmd = new NpgsqlCommand
            {
                Connection = base.GetConnection(),
                CommandText = @"SELECT id FROM pontos_turisticos"
            };

            NpgsqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int id = (int)reader["id"];                
                listaIds.Add(id);
            }
            reader.Close();

            foreach (int id in listaIds) 
            {
                PontoTuristico ponto = Read(id);
                listaPontos.Add(ponto);
            }

            return listaPontos;
        }


        PontoTuristico IRepository<PontoTuristico>.Read(object pk)
        {
            return this.Read(pk);
        }
        PontoTuristico Read(object pk)
        {
            int id = (int)pk;
            PontoTuristico ponto = null;
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.Connection = base.GetConnection();
            cmd.CommandText = @"SELECT * FROM pontos_turisticos WHERE id = @id";
            cmd.Parameters.AddWithValue("@id", id);
            NpgsqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                ponto = new PontoTuristico
                {
                    Id = (int)reader["id"],
                    Nome = (string)reader["nome"],
                    Descricao = (string)reader["descricao"],
                    Data = (DateTime)reader["data"],
                    Latitude = (double)reader["latitude"],
                    Longitude = (double)reader["longitude"],
                    Usuario = new Usuario()
                    {
                        Id = (int)reader["usuario_id"]
                    }
                };
            }
            reader.Close();

            if (ponto == null) return null;

            NpgsqlCommand enderecoCommand = new NpgsqlCommand
            {
                Connection = base.GetConnection(),
                CommandText = @"SELECT e.id, e.numero, e.logradouro, e.cep, e.complemento, e.ponto_id, e.referencia, 
		                 c.id AS ""cidade_id"", c.nome AS ""cidade_nome"", uf.uf AS ""estado_uf"", uf.nome AS ""estado_nome""
                    FROM enderecos e INNER JOIN cidades c  on c.id = e.cidade_id INNER JOIN estados uf on uf.uf = c.estado_uf
                    WHERE ponto_id = @ponto_id;"
            };
            enderecoCommand.Parameters.AddWithValue("@ponto_id", ponto.Id);
            reader = enderecoCommand.ExecuteReader();
            if (reader.Read())
            {
                ponto.Endereco = new Endereco
                {
                    Id = (int)reader["id"],
                    Numero = (string)reader["numero"],
                    Logradouro = (string)reader["logradouro"],
                    CEP = (string)reader["cep"],
                    Complemento = (string)reader["complemento"],
                    Referencia = (string)reader["referencia"],
                    PontoTuristicoId = (int)reader["ponto_Id"],
                    Cidade = new Cidade
                    {
                        Id = (int)reader["cidade_id"],
                        Nome = (string)reader["cidade_nome"],
                        Estado = new Estado
                        {
                            Uf = (string)reader["estado_uf"],
                            Nome = (string)reader["estado_nome"]
                        }
                    }
                };
            }
            reader.Close();

            NpgsqlCommand usuarioCommand = new NpgsqlCommand
            {
                Connection = base.GetConnection(),
                CommandText = @"SELECT * FROM usuarios WHERE id = @usuario_id;"
            };
            usuarioCommand.Parameters.AddWithValue("@usuario_id", ponto.Usuario.Id);
            reader = usuarioCommand.ExecuteReader();
            if (reader.Read())
            {
                ponto.Usuario.Nome = (string)reader["nome"];
                ponto.Usuario.Sobrenome = (string)reader["sobrenome"];
                ponto.Usuario.Senha = (string)reader["senha"];
                ponto.Usuario.Email = (string)reader["email"];
                ponto.Usuario.Status = (int)reader["status"];
                ponto.Usuario.DataNascimento = (DateTime)reader["data_nasc"];
            }
            reader.Close();

            // Buscando as categorias no relacionamento N--N
            NpgsqlCommand nnCommand = new NpgsqlCommand
            {
                Connection = base.GetConnection(),
                CommandText = @"SELECT pc.ponto_id, pc.categoria_id, c.tipo 
                                FROM pontos__categorias pc
                                INNER JOIN pontos_turisticos p on p.id = pc.ponto_id
                                INNER JOIN categorias c on c.id = pc.categoria_id
                                AND p.id = @id ORDER BY c.tipo;"
            };
            nnCommand.Parameters.AddWithValue("@id", ponto.Id);

            NpgsqlDataReader nnReader = nnCommand.ExecuteReader();
            while (nnReader.Read())
            {
                Categoria categoria = new Categoria
                {
                    Id = (int)nnReader["categoria_id"],
                    Tipo = (string)nnReader["tipo"]                    
                };
                ponto.Categorias.Add(new PontoTuristicoCategoria(ponto, categoria));
            }
            nnReader.Close();


            return ponto;
        }



        void IRepository<PontoTuristico>.Update(object pk, PontoTuristico domainObject)
        {
            int id = (int)pk;
            PontoTuristico ponto = domainObject;
            PontoTuristico pontoExistente = null;
            // Buscar o objeto persistido no banco
            pontoExistente = this.Read(id);

            NpgsqlTransaction transaction = base.GetConnection().BeginTransaction();
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand()
                {
                    Connection = base.GetConnection(),
                    Transaction = transaction,
                    CommandText = @"UPDATE pontos_turisticos SET nome = @nome, descricao = @descricao, 
                                        latitude = @latitude, longitude = @longitude 
                                    WHERE id = @id;"

                };
                cmd.Parameters.AddWithValue("@id", ponto.Id);
                cmd.Parameters.AddWithValue("@nome", ponto.Nome);
                cmd.Parameters.AddWithValue("@descricao", ponto.Descricao);
                cmd.Parameters.AddWithValue("@latitude", ponto.Latitude);
                cmd.Parameters.AddWithValue("@longitude", ponto.Longitude);                
                cmd.ExecuteNonQuery();
                
                if (ponto.Endereco != null)
                {
                    NpgsqlCommand enderecoCommand = new NpgsqlCommand();
                    enderecoCommand.Connection = base.GetConnection();
                    enderecoCommand.Transaction = transaction;

                    // A instrução SQL vai depender se é uma atualização de endereço pré-existente ou
                    // se trata de um ponto turístico que não tinha endereço antes e agora passará a ter

                    // sem endereço prévio
                    if (pontoExistente.Endereco == null)    
                    {
                        enderecoCommand.CommandText = @"INSERT INTO enderecos (ponto_id, numero, logradouro, cep, complemento, referencia, cidade_id)
	                                VALUES (@ponto_id, @numero, @logradouro, @cep, @complemento, @referencia, @cidade_id);";
                        enderecoCommand.Parameters.AddWithValue("@ponto_id", ponto.Id);
                        enderecoCommand.Parameters.AddWithValue("@numero", ponto.Endereco.Numero);
                        enderecoCommand.Parameters.AddWithValue("@logradouro", ponto.Endereco.Logradouro);
                        enderecoCommand.Parameters.AddWithValue("@cep", ponto.Endereco.CEP);
                        enderecoCommand.Parameters.AddWithValue("@complemento", ponto.Endereco.Complemento);
                        enderecoCommand.Parameters.AddWithValue("@referencia", ponto.Endereco.Referencia);
                        enderecoCommand.Parameters.AddWithValue("@cidade_id", ponto.Endereco.Cidade.Id);
                    }
                    // já havia endereço, vamos atualizar
                    else
                    {
                        enderecoCommand.CommandText = @"UPDATE enderecos SET numero = @numero, logradouro = @logradouro, cep = @cep, 
                                            complemento = @complemento, referencia = @referencia, cidade_id = @cidade_id
                                            WHERE ponto_id = @ponto_id"; ;                    
                        enderecoCommand.Parameters.AddWithValue("@ponto_id", ponto.Id);
                        enderecoCommand.Parameters.AddWithValue("@numero", ponto.Endereco.Numero);
                        enderecoCommand.Parameters.AddWithValue("@logradouro", ponto.Endereco.Logradouro);
                        enderecoCommand.Parameters.AddWithValue("@cep", ponto.Endereco.CEP);
                        enderecoCommand.Parameters.AddWithValue("@complemento", ponto.Endereco.Complemento);
                        enderecoCommand.Parameters.AddWithValue("@referencia", ponto.Endereco.Referencia);
                        enderecoCommand.Parameters.AddWithValue("@cidade_id", ponto.Endereco.Cidade.Id);
                    }
                    enderecoCommand.ExecuteNonQuery();
                }


                // PontoTuristico__Categoria - Relacionamento N--N

                // Verificar se as categorias foram alteradas

                bool alterouCategoria = false;
                // Se mudou a quantidade, teve alteração
                if (ponto.Categorias.Count != pontoExistente.Categorias.Count)
                {
                    alterouCategoria = true;
                }
                // Se não foi na quantidade, ainda pode ser alteração no conteúdo, no tipo
                else
                {
                    foreach (PontoTuristicoCategoria pc in ponto.Categorias)
                    {
                        bool achei = false;
                        for (int i = 0; i < pontoExistente.Categorias.Count; i++)
                        {
                            Categoria categoriaExistente = pontoExistente
                                                                        .Categorias
                                                                        .ElementAt<PontoTuristicoCategoria>(i)
                                                                        .Categoria;
                            if ( pc.Categoria.Id == categoriaExistente.Id )
                            {
                                achei = true;
                            }
                        }
                        if ( ! achei )
                        {
                            alterouCategoria = true;
                            break;
                        }
                    }
                }
                if (alterouCategoria)
                {
                    // Remove tudo
                    foreach (PontoTuristicoCategoria pc in ponto.Categorias)
                    {
                        NpgsqlCommand nnCommand = new NpgsqlCommand
                        {
                            Connection = base.GetConnection(),
                            Transaction = transaction,
                            CommandText = @"DELETE FROM pontos__categorias WHERE ponto_id = @ponto_id;"
                        };
                        nnCommand.Parameters.AddWithValue("@ponto_id", ponto.Id);

                        nnCommand.ExecuteNonQuery();
                    }
                    
                    // Insere as categorias
                    foreach (PontoTuristicoCategoria pc in ponto.Categorias)
                    {
                        NpgsqlCommand nnCommand = new NpgsqlCommand
                        {
                            Connection = base.GetConnection(),
                            Transaction = transaction,
                            CommandText = @"INSERT INTO pontos__categorias (ponto_id, categoria_id)
                                            VALUES (@ponto_id, @categoria_id);"
                        };
                        nnCommand.Parameters.AddWithValue("@ponto_id", ponto.Id);
                        nnCommand.Parameters.AddWithValue("@categoria_id", pc.Categoria.Id);

                        nnCommand.ExecuteNonQuery();
                    }
                }

                // Tudo certo
                transaction.Commit();

                
            }
            catch (Exception exp)
            {
                // Algo deu ruim, desfazer tudo
                transaction.Rollback();

                // relançar exceção para ser tratada pela camada de serviço
                throw exp;
            }
        }
    }
}
