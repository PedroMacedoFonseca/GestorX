using System;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;
using Projeto1.DAL;
using Projeto1.Models;

namespace Projeto1
{
    public partial class CadastroUsuario : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack && IsAjaxRequest())
                {
                    Response.Clear();
                    Response.ContentType = "text/html";

                    string id = Request.QueryString["edit"];
                    bool isEdicao = !string.IsNullOrEmpty(id);

                    Response.Write(
                        $"<script>window.parent.$('#modalTitle').text('{(isEdicao ? "Editar Usuário" : "Novo Usuário")}');</script>"
                    );

                    if (isEdicao)
                    {
                        CarregarUsuarioParaEdicao(Convert.ToInt32(id));
                    }

                    Response.Flush();
                    return;
                }
            }
            catch (Exception ex)
            {
                Response.Write($"<script>console.error('Erro: {ex.Message}');</script>");
                Response.Flush();
                return;
            }
        }

        private bool IsAjaxRequest()
        {
            return Request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }

        private void CarregarUsuarioParaEdicao(int id)
        {
            try
            {
                UsuarioDAL dal = new UsuarioDAL();
                Usuario usuario = dal.ObterPorId(id);

                if (usuario != null)
                {
                    hfID.Value = usuario.ID.ToString();
                    txtNomeCompleto.Text = usuario.NomeCompleto;
                    txtCPF.Text = usuario.CPF;
                    txtMatricula.Text = usuario.Matricula;
                    txtTelefone.Text = usuario.Telefone;
                    txtUnidade.Text = usuario.Unidade;
                    ddlPerfil.SelectedValue = usuario.Perfil;
                }
            }
            catch (Exception ex)
            {
                Response.Clear();
                Response.Write($"<div class='alert alert-danger'>Erro ao carregar usuário: {ex.Message}</div>");
                Response.Flush();
                return;
            }
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                Page.Validate();
                if (!Page.IsValid) return;

                if (IsValid)
                {
                    UsuarioDAL dal = new UsuarioDAL();
                    Usuario usuario = new Usuario
                    {
                        ID = Convert.ToInt32(hfID.Value),
                        NomeCompleto = txtNomeCompleto.Text.Trim(),
                        CPF = txtCPF.Text.Replace(".", "").Replace("-", ""),
                        Matricula = txtMatricula.Text.Trim(),
                        Telefone = txtTelefone.Text.Trim(),
                        Unidade = txtUnidade.Text.Trim(),
                        Perfil = ddlPerfil.SelectedValue
                    };

                    if (usuario.ID == 0)
                    {
                        usuario.SenhaHash = Seguranca.GerarHashSenha(txtSenha.Text);
                        dal.Inserir(usuario);
                    }
                    else
                    {
                        dal.Atualizar(usuario);
                    }

                    string script = @"
                        <script type='text/javascript'>
                            if (window.parent) {
                                window.parent.$('#modalCadastro').modal('hide');
                                window.parent.location.reload();
                            }
                        </script>";

                    Response.Clear();
                    Response.Write(script);
                    Response.Flush();
                    Context.ApplicationInstance.CompleteRequest();
                    return;
                }
            }
            catch (ThreadAbortException)
            {
                // Ignora exceção de thread abortada
            }
            catch (Exception ex)
            {
                Response.Clear();
                Response.Write($"<div class='alert alert-danger'>Erro ao salvar: {ex.Message}</div>");
                Response.Flush();
                return;
            }
        }

        protected void ValidarCPFExistente(object source, ServerValidateEventArgs args)
        {
            try
            {
                string cpf = args.Value.Replace(".", "").Replace("-", "");
                int idAtual = int.Parse(hfID.Value);

                args.IsValid = !UsuarioDAL.ExisteCPF(cpf, idAtual);
            }
            catch
            {
                args.IsValid = false;
            }
        }

        protected void ValidateCPF(object source, ServerValidateEventArgs args)
        {
            args.IsValid = ValidarCPF(args.Value);
        }

        private bool ValidarCPF(string cpf)
        {
            cpf = cpf.Replace(".", "").Replace("-", "");

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