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
        private string connectionString = ConfigurationManager.ConnectionStrings["MinhaConexao"].ConnectionString;

        public List<Usuario> ObterTodos()
        {
            List<Usuario> lista = new List<Usuario>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = "SELECT ID, NomeCompleto, CPF, Unidade, Perfil, DataCadastro, Telefone, Matricula FROM Usuarios";

                SqlCommand cmd = new SqlCommand(sql, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    lista.Add(new Usuario
                    {
                        ID = Convert.ToInt32(reader["ID"]),
                        NomeCompleto = reader["NomeCompleto"].ToString(),
                        CPF = reader["CPF"].ToString(),
                        Unidade = reader["Unidade"].ToString(),
                        Perfil = reader["Perfil"].ToString(),
                        Telefone = reader["Telefone"].ToString(),
                        Matricula = reader["Matricula"].ToString(),
                        DataCadastro = Convert.ToDateTime(reader["DataCadastro"])
                    });
                }
            }
            return lista;
        }

        public Usuario ObterPorId(int id)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = "SELECT * FROM Usuarios WHERE ID = @ID";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add("@ID", SqlDbType.Int).Value = id;
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
                            Unidade = reader["Unidade"].ToString(),
                            Perfil = reader["Perfil"].ToString(),
                            SenhaHash = reader["SenhaHash"].ToString(),
                            Telefone = reader["Telefone"].ToString(),
                            Matricula = reader["Matricula"].ToString(),
                            DataCadastro = Convert.ToDateTime(reader["DataCadastro"]),
                        };
                    }
                }
            }
            return null;
        }

        public Usuario ObterPorCPF(string cpf)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = "SELECT * FROM Usuarios WHERE CPF = @CPF";
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
                            Unidade = reader["Unidade"].ToString(),
                            Perfil = reader["Perfil"].ToString(),
                            SenhaHash = reader["SenhaHash"].ToString(),
                            Telefone = reader["Telefone"].ToString(),
                            Matricula = reader["Matricula"].ToString(),
                            DataCadastro = Convert.ToDateTime(reader["DataCadastro"]),
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

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = @"INSERT INTO Usuarios 
                            (NomeCompleto, CPF, Unidade, Perfil, SenhaHash, DataCadastro, Telefone, Matricula)
                            VALUES 
                            (@NomeCompleto, @CPF, @Unidade, @Perfil, @SenhaHash, @DataCadastro, @Telefone, @Matricula)";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add("@NomeCompleto", SqlDbType.VarChar, 100).Value = usuario.NomeCompleto;
                cmd.Parameters.Add("@CPF", SqlDbType.VarChar, 14).Value = usuario.CPF;
                cmd.Parameters.Add("@Unidade", SqlDbType.VarChar, 100).Value = usuario.Unidade;
                cmd.Parameters.Add("@Perfil", SqlDbType.VarChar, 50).Value = usuario.Perfil;
                cmd.Parameters.Add("@SenhaHash", SqlDbType.VarChar, 64).Value = usuario.SenhaHash;
                cmd.Parameters.Add("@DataCadastro", SqlDbType.DateTime).Value = DateTime.Now;
                cmd.Parameters.Add("@Telefone", SqlDbType.VarChar, 20).Value = usuario.Telefone;
                cmd.Parameters.Add("@Matricula", SqlDbType.VarChar, 50).Value = usuario.Matricula;

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
                string sql = @"UPDATE Usuarios SET 
                            NomeCompleto = @NomeCompleto, 
                            CPF = @CPF, 
                            Unidade = @Unidade, 
                            Perfil = @Perfil,
                            Telefone = @Telefone,
                            Matricula = @Matricula
                            WHERE ID = @ID";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add("@ID", SqlDbType.Int).Value = usuario.ID;
                cmd.Parameters.Add("@NomeCompleto", SqlDbType.VarChar, 100).Value = usuario.NomeCompleto;
                cmd.Parameters.Add("@CPF", SqlDbType.VarChar, 14).Value = usuario.CPF;
                cmd.Parameters.Add("@Unidade", SqlDbType.VarChar, 100).Value = usuario.Unidade;
                cmd.Parameters.Add("@Perfil", SqlDbType.VarChar, 50).Value = usuario.Perfil;
                cmd.Parameters.Add("@Telefone", SqlDbType.VarChar, 20).Value = usuario.Telefone;
                cmd.Parameters.Add("@Matricula", SqlDbType.VarChar, 50).Value = usuario.Matricula;

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
            // Obter o hash da senha armazenada no banco
            string hashArmazenado = ObterSenhaHash(usuarioId);

            if (string.IsNullOrEmpty(hashArmazenado))
                return false;

            // Usar o método existente da classe Seguranca para verificar
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
            string connectionString = ConfigurationManager.ConnectionStrings["MinhaConexao"].ConnectionString;

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