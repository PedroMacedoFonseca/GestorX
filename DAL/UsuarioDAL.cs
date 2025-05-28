using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Projeto1.Models;

namespace Projeto1.DAL
{
    public class UsuarioDAL
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["MinhaConexao"].ConnectionString;

        public List<Usuario> ObterTodos()
        {
            var usuarios = new List<Usuario>();
            using (var conn = new SqlConnection(connectionString))
            {
                var cmdText = @"
                SELECT u.ID, u.NomeCompleto, u.CPF, u.Matricula, u.Telefone,
                       u.UnidadeID, un.Nome AS NomeUnidade, u.Perfil, u.DataCadastro, u.SenhaHash
                FROM Usuarios u 
                LEFT JOIN Unidades un ON u.UnidadeID = un.UnidadeID
                ORDER BY u.NomeCompleto";

                var cmd = new SqlCommand(cmdText, conn);
                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        usuarios.Add(new Usuario
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            NomeCompleto = reader["NomeCompleto"].ToString(),
                            CPF = reader["CPF"].ToString(),
                            Matricula = reader["Matricula"].ToString(),
                            Telefone = reader["Telefone"] == DBNull.Value ? null : reader["Telefone"].ToString(),
                            UnidadeID = reader["UnidadeID"] == DBNull.Value ? 0 : Convert.ToInt32(reader["UnidadeID"]),
                            NomeUnidade = reader["NomeUnidade"] == DBNull.Value ? "N/A" : reader["NomeUnidade"].ToString(),
                            Perfil = reader["Perfil"].ToString(),
                            DataCadastro = Convert.ToDateTime(reader["DataCadastro"]),
                            SenhaHash = reader["SenhaHash"].ToString()
                        });
                    }
                }
            }
            return usuarios;
        }

        public Usuario ObterPorId(int id)
        {
            Usuario usuario = null;
            using (var conn = new SqlConnection(connectionString))
            {
                var cmdText = @"
                SELECT u.ID, u.NomeCompleto, u.CPF, u.Matricula, u.Telefone,
                       u.UnidadeID, un.Nome AS NomeUnidade, u.Perfil, u.DataCadastro, u.SenhaHash
                FROM Usuarios u
                LEFT JOIN Unidades un ON u.UnidadeID = un.UnidadeID
                WHERE u.ID = @ID";
                var cmd = new SqlCommand(cmdText, conn);
                cmd.Parameters.AddWithValue("@ID", id);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        usuario = new Usuario
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            NomeCompleto = reader["NomeCompleto"].ToString(),
                            CPF = reader["CPF"].ToString(),
                            Matricula = reader["Matricula"].ToString(),
                            Telefone = reader["Telefone"] == DBNull.Value ? null : reader["Telefone"].ToString(),
                            UnidadeID = reader["UnidadeID"] == DBNull.Value ? 0 : Convert.ToInt32(reader["UnidadeID"]),
                            NomeUnidade = reader["NomeUnidade"] == DBNull.Value ? "N/A" : reader["NomeUnidade"].ToString(),
                            Perfil = reader["Perfil"].ToString(),
                            DataCadastro = Convert.ToDateTime(reader["DataCadastro"]),
                            SenhaHash = reader["SenhaHash"].ToString()
                        };
                    }
                }
            }
            return usuario;
        }

        public Usuario ObterPorCPF(string cpf)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = @"
                SELECT u.*, un.Nome AS NomeUnidade 
                FROM Usuarios u
                LEFT JOIN Unidades un ON u.UnidadeID = un.UnidadeID
                WHERE u.CPF = @CPF";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add("@CPF", SqlDbType.VarChar).Value = cpf;
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Usuario
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            NomeCompleto = reader["NomeCompleto"].ToString(),
                            CPF = reader["CPF"].ToString(),
                            Matricula = reader["Matricula"].ToString(),
                            Telefone = reader["Telefone"] == DBNull.Value ? null : reader["Telefone"].ToString(),
                            UnidadeID = reader["UnidadeID"] == DBNull.Value ? 0 : Convert.ToInt32(reader["UnidadeID"]),
                            NomeUnidade = reader["NomeUnidade"] == DBNull.Value ? "N/A" : reader["NomeUnidade"].ToString(),
                            Perfil = reader["Perfil"].ToString(),
                            DataCadastro = Convert.ToDateTime(reader["DataCadastro"]),
                            SenhaHash = reader["SenhaHash"].ToString()
                        };
                    }
                }
            }
            return null;
        }

        public void Inserir(Usuario usuario)
        {
            if (ExisteCPF(usuario.CPF))
            {
                throw new Exception("CPF já cadastrado");
            }

            using (var conn = new SqlConnection(connectionString))
            {
                var cmdText = @"INSERT INTO Usuarios (NomeCompleto, CPF, Matricula, Telefone, UnidadeID, Perfil, SenhaHash, DataCadastro)
                            VALUES (@NomeCompleto, @CPF, @Matricula, @Telefone, @UnidadeID, @Perfil, @SenhaHash, @DataCadastro)"; 
                var cmd = new SqlCommand(cmdText, conn);
                cmd.Parameters.AddWithValue("@NomeCompleto", usuario.NomeCompleto);
                cmd.Parameters.AddWithValue("@CPF", usuario.CPF);
                cmd.Parameters.AddWithValue("@Matricula", usuario.Matricula);
                cmd.Parameters.AddWithValue("@Telefone", (object)usuario.Telefone ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@UnidadeID", usuario.UnidadeID > 0 ? (object)usuario.UnidadeID : DBNull.Value); 
                cmd.Parameters.AddWithValue("@Perfil", usuario.Perfil);
                cmd.Parameters.AddWithValue("@SenhaHash", usuario.SenhaHash);
                cmd.Parameters.AddWithValue("@DataCadastro", usuario.DataCadastro);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Atualizar(Usuario usuario)
        {
            if (ExisteCPF(usuario.CPF, usuario.ID))
            {
                throw new Exception("CPF já pertence a outro usuário");
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                var cmdText = @"UPDATE Usuarios SET NomeCompleto = @NomeCompleto, CPF = @CPF,
                                Matricula = @Matricula, Telefone = @Telefone, UnidadeID = @UnidadeID,
                                Perfil = @Perfil
                            WHERE ID = @ID";
                var cmd = new SqlCommand(cmdText, conn);
                cmd.Parameters.AddWithValue("@NomeCompleto", usuario.NomeCompleto);
                cmd.Parameters.AddWithValue("@CPF", usuario.CPF); 
                cmd.Parameters.AddWithValue("@Matricula", usuario.Matricula);
                cmd.Parameters.AddWithValue("@Telefone", (object)usuario.Telefone ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@UnidadeID", usuario.UnidadeID > 0 ? (object)usuario.UnidadeID : DBNull.Value);
                cmd.Parameters.AddWithValue("@Perfil", usuario.Perfil);
                cmd.Parameters.AddWithValue("@ID", usuario.ID);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void AtualizarSenha(int idUsuario, string novaSenha)
        {
            string novaHash = Seguranca.GerarHashSenha(novaSenha);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = @"UPDATE Usuarios SET 
                            SenhaHash = @SenhaHash
                            WHERE ID = @ID";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add("@SenhaHash", SqlDbType.VarChar, 64).Value = novaHash;
                cmd.Parameters.Add("@ID", SqlDbType.Int).Value = idUsuario;

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Excluir(int id)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = "DELETE FROM Usuarios WHERE ID = @ID";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add("@ID", SqlDbType.Int).Value = id;

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public bool VerificarSenha(int usuarioId, string senhaInformada)
        {
            string hashArmazenado = ObterSenhaHash(usuarioId);

            if (string.IsNullOrEmpty(hashArmazenado))
                return false;

            return Seguranca.VerificarSenha(senhaInformada, hashArmazenado);
        }

        private string ObterSenhaHash(int usuarioId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = "SELECT SenhaHash FROM Usuarios WHERE ID = @ID";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add("@ID", SqlDbType.Int).Value = usuarioId;

                conn.Open();
                return cmd.ExecuteScalar()?.ToString();
            }
        }

        public bool ExisteCPF(string cpf, int idAtual = 0)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = @"SELECT COUNT(1) 
                     FROM Usuarios 
                     WHERE CPF = @CPF 
                     AND ID != @ID";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add("@CPF", SqlDbType.VarChar, 14).Value = cpf;
                cmd.Parameters.Add("@ID", SqlDbType.Int).Value = idAtual;

                conn.Open();
                return (int)cmd.ExecuteScalar() > 0;
            }
        }
    }
}