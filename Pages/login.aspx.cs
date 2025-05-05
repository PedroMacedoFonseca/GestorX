using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;
using Projeto1.DAL;
using Projeto1.Models;

namespace Projeto1
{
    public partial class login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                txtCPF.Text = RemoveCPFMask(txtCPF.Text);
            }
        }

        protected void ValidateCPF(object source, ServerValidateEventArgs args)
        {
            args.IsValid = IsValidCPF(args.Value);
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {
                string cpf = RemoveCPFMask(txtCPF.Text.Trim());
                string senha = txtSenha.Text;

                UsuarioDAL usuarioDAL = new UsuarioDAL();
                Usuario usuario = usuarioDAL.ObterPorCPF(cpf);

                if (usuario != null)
                {
                    bool senhaCorreta = Seguranca.VerificarSenha(senha, usuario.SenhaHash);
                    bool primeiroAcesso = Seguranca.VerificarSenha(usuario.Matricula, usuario.SenhaHash);

                    if (senhaCorreta)
                    {
                        if (primeiroAcesso)
                        {
                            Session["UsuarioTrocaSenha"] = usuario;
                            Response.Redirect("~/Pages/TrocaSenha.aspx");
                        }
                        else
                        {
                            Session["UsuarioLogado"] = usuario;
                            Response.Redirect("~/Pages/Inicio.aspx");
                        }
                    }
                    else
                    {
                        lblMensagemErro.Text = "CPF ou senha incorretos.";
                    }
                }
                else
                {
                    lblMensagemErro.Text = "CPF não cadastrado no sistema.";
                }
            }
        }

        private string RemoveCPFMask(string cpf)
        {
            return Regex.Replace(cpf, @"[^\d]", "");
        }

        private bool IsValidCPF(string cpf)
        {
            cpf = RemoveCPFMask(cpf);

            if (cpf.Length != 11)
                return false;

            // Verifica dígitos repetidos
            bool todosDigitosIguais = true;
            for (int i = 1; i < cpf.Length; i++)
            {
                if (cpf[i] != cpf[0])
                {
                    todosDigitosIguais = false;
                    break;
                }
            }
            if (todosDigitosIguais)
                return false;

            // Validação do CPF
            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            string tempCpf = cpf.Substring(0, 9);
            int soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

            int resto = soma % 11;
            resto = resto < 2 ? 0 : 11 - resto;

            string digito = resto.ToString();
            tempCpf = tempCpf + digito;
            soma = 0;

            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

            resto = soma % 11;
            resto = resto < 2 ? 0 : 11 - resto;

            digito = digito + resto.ToString();

            return cpf.EndsWith(digito);
        }
    }
}