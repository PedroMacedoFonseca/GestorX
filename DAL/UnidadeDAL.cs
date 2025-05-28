using Projeto1.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Projeto1.DAL
{
    public class UnidadeDAL
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["MinhaConexao"].ConnectionString;

        public List<Unidade> ObterTodas()
        {
            var unidades = new List<Unidade>();
            using (var conn = new SqlConnection(connectionString))
            {
                var cmd = new SqlCommand("SELECT UnidadeID, Nome, Ativo, DataCriacao FROM Unidades ORDER BY Nome", conn);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        unidades.Add(new Unidade
                        {
                            UnidadeID = Convert.ToInt32(reader["UnidadeID"]),
                            Nome = reader["Nome"].ToString(), 
                            Ativo = Convert.ToBoolean(reader["Ativo"]),
                            DataCriacao = Convert.ToDateTime(reader["DataCriacao"]) 
                        });
                    }
                }
            }
            return unidades;
        }

        public List<Unidade> ObterAtivas()
        {
            var unidades = new List<Unidade>();
            using (var conn = new SqlConnection(connectionString))
            {
                var cmd = new SqlCommand("SELECT UnidadeID, Nome FROM Unidades WHERE Ativo = 1 ORDER BY Nome", conn);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        unidades.Add(new Unidade
                        {
                            UnidadeID = Convert.ToInt32(reader["UnidadeID"]),
                            Nome = reader["Nome"].ToString(), 
                            Ativo = true 
                        });
                    }
                }
            }
            return unidades;
        }

        public Unidade ObterPorId(int unidadeId)
        {
            Unidade unidade = null;
            using (var conn = new SqlConnection(connectionString))
            {
                var cmd = new SqlCommand("SELECT UnidadeID, Nome, Ativo, DataCriacao FROM Unidades WHERE UnidadeID = @UnidadeID", conn);
                cmd.Parameters.AddWithValue("@UnidadeID", unidadeId);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        unidade = new Unidade
                        {
                            UnidadeID = Convert.ToInt32(reader["UnidadeID"]),
                            Nome = reader["Nome"].ToString(), 
                            Ativo = Convert.ToBoolean(reader["Ativo"]),
                            DataCriacao = Convert.ToDateTime(reader["DataCriacao"]) 
                        };
                    }
                }
            }
            return unidade;
        }

        public void Inserir(Unidade unidade)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                var cmd = new SqlCommand("INSERT INTO Unidades (Nome, Ativo) OUTPUT INSERTED.UnidadeID VALUES (@Nome, @Ativo)", conn);

                cmd.Parameters.AddWithValue("@Nome", unidade.Nome);
                cmd.Parameters.AddWithValue("@Ativo", unidade.Ativo);
                conn.Open();
                unidade.UnidadeID = Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        public void Atualizar(Unidade unidade)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                var cmd = new SqlCommand("UPDATE Unidades SET Nome = @Nome, Ativo = @Ativo WHERE UnidadeID = @UnidadeID", conn);
                cmd.Parameters.AddWithValue("@Nome", unidade.Nome); 
                cmd.Parameters.AddWithValue("@Ativo", unidade.Ativo);
                cmd.Parameters.AddWithValue("@UnidadeID", unidade.UnidadeID);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public void Excluir(int unidadeId) 
        {
            using (var conn = new SqlConnection(connectionString))
            {
                var cmd = new SqlCommand("UPDATE Unidades SET Ativo = 0 WHERE UnidadeID = @UnidadeID", conn);
                cmd.Parameters.AddWithValue("@UnidadeID", unidadeId);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}