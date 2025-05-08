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
            if (!IsPostBack)
            {
                string idParam = Request.QueryString["edit"];
                int parsedId; 

                if (!string.IsNullOrEmpty(idParam) && int.TryParse(idParam, out parsedId))
                {

                    CarregarUsuarioParaEdicao(parsedId);                                       
                    divSenha.Visible = false;
                    rfvSenha.Enabled = false;
                }
                else
                {
                    hfID.Value = "0";
                    divSenha.Visible = true;
                    rfvSenha.Enabled = true;
                }
            }
        }

        private void CarregarUsuarioParaEdicao(int id)
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
                    if (ddlPerfil.Items.FindByValue(usuario.Perfil) != null)
                    {
                        ddlPerfil.SelectedValue = usuario.Perfil;
                    }
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "UserNotFound", "alert('Usuário não encontrado.'); window.parent.$('#modalCadastro').modal('hide');", true);
                }
        }
    
        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                Page.Validate("vgCadastro");
                if (!Page.IsValid)
                {
                    return;
                }

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

                bool isNovoUsuario = (usuario.ID == 0);

                if (isNovoUsuario)
                {
                    if (txtSenha.Text.Length < 6 && rfvSenha.Enabled)
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "SenhaInvalida", "alert('A senha deve ter pelo menos 6 caracteres.');", true);
                        return;
                    }
                    usuario.SenhaHash = Seguranca.GerarHashSenha(txtSenha.Text); 
                    dal.Inserir(usuario);
                }
                else
                {
                    dal.Atualizar(usuario);
                }

                ScriptManager.RegisterStartupScript(this, GetType(), "redirect", "window.location = '/Pages/Inicio.aspx';", true);
            }
            catch (Exception ex)
            {
                string errorMsg = "Ocorreu um erro ao salvar o usuário: " + ex.Message.Replace("'", "\\'").Replace("\r", " ").Replace("\n", " ");
                ClientScript.RegisterStartupScript(this.GetType(), "ErroSalvar", $"alert('{errorMsg}');", true);
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
            lblDebug.Text = "ValidarCPF: " + args.IsValid;
        }

        private bool ValidarCPF(string cpf)
        {
            cpf = cpf.Replace(".", "").Replace("-", "");

            if (cpf.Length != 11)
                return false;

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
