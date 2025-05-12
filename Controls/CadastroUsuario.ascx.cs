using System;
using System.Linq; 
using System.Web.UI;
using System.Web.UI.WebControls;
using Projeto1.DAL;
using Projeto1.Helpers; 
using Projeto1.Models;
using Projeto1.Services;

namespace Projeto1.Controls
{
    public partial class CadastroUsuario : UserControl
    {
        private readonly UsuarioService _usuarioService = new UsuarioService(); 

        public event EventHandler SalvoComSucesso;
        public event EventHandler<Exception> ErroOcorreu;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Page.Validate("vgCadastro");
            }
        }

        public void InicializarParaNovoUsuario()
        {
            hfID.Value = "0"; 
            txtNomeCompleto.Text = string.Empty;
            txtCPF.Text = string.Empty;
            txtMatricula.Text = string.Empty;
            txtTelefone.Text = string.Empty;
            txtUnidade.Text = string.Empty;
            ddlPerfil.ClearSelection();
            if (ddlPerfil.Items.FindByValue("") != null)
            {
                ddlPerfil.SelectedValue = "";
            }
            txtSenha.Text = string.Empty;

            ConfigurarModoNovoUsuario();
        }

        public void CarregarUsuarioParaEdicao(int id)
        {
            var usuario = _usuarioService.ObterPorId(id);
            if (usuario != null)
            {
                PreencherCampos(usuario);
                ConfigurarModoEdicao();
            }
            else
            {
                ErroOcorreu?.Invoke(this, new Exception("Usuário não encontrado."));
            }
        }

        private void ConfigurarModoNovoUsuario()
        {
            divSenha.Visible = true; 
            rfvSenha.Enabled = true;
            txtCPF.ReadOnly = false; 
        }

        private void ConfigurarModoEdicao()
        {
            divSenha.Visible = false; 
            rfvSenha.Enabled = false;
            txtSenha.Text = string.Empty; 
            txtCPF.ReadOnly = true; 
        }

        private void PreencherCampos(Usuario usuario)
        {
            hfID.Value = usuario.ID.ToString();
            txtNomeCompleto.Text = usuario.NomeCompleto;
            txtCPF.Text = usuario.CPF; 
            txtMatricula.Text = usuario.Matricula;
            txtTelefone.Text = usuario.Telefone;
            txtUnidade.Text = usuario.Unidade;

            ddlPerfil.ClearSelection();
            if (ddlPerfil.Items.FindByValue(usuario.Perfil.ToString()) != null) 
            {
                ddlPerfil.SelectedValue = usuario.Perfil.ToString();
            }
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                Page.Validate("vgCadastro");
                if (!Page.IsValid)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowModal",
                        "$('#modalCadastro').modal('show');", true);
                    return;

                }

                var usuario = new Usuario
                {
                    ID = Convert.ToInt32(string.IsNullOrEmpty(hfID.Value) ? "0" : hfID.Value),
                    NomeCompleto = txtNomeCompleto.Text.Trim(),
                    CPF = CpfHelper.RemoverFormatacao(txtCPF.Text), 
                    Matricula = txtMatricula.Text.Trim(),
                    Telefone = CpfHelper.RemoverFormatacao(txtTelefone.Text.Trim()), 
                    Unidade = txtUnidade.Text.Trim(),
                    Perfil = ddlPerfil.SelectedValue 
                };

                if (usuario.ID == 0) 
                {
                    if (string.IsNullOrWhiteSpace(txtSenha.Text) || txtSenha.Text.Length < 6)
                    {
                        throw new Exception("A senha inicial é obrigatória e deve ter pelo menos 6 caracteres.");
                    }
                    usuario.SenhaHash = Seguranca.GerarHashSenha(txtSenha.Text); 
                    usuario.DataCadastro = DateTime.Now; 
                    _usuarioService.Inserir(usuario);
                }
                else 
                {
                    _usuarioService.Atualizar(usuario); 
                }

                SalvoComSucesso?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                ErroOcorreu?.Invoke(this, ex);
            }
        }

        protected void ValidarCPFExistente(object source, ServerValidateEventArgs args)
        {
            try
            {
                var cpf = CpfHelper.RemoverFormatacao(args.Value);
                int idAtual = 0;
                if (!string.IsNullOrEmpty(hfID.Value))
                {
                    int.TryParse(hfID.Value, out idAtual);
                }
                args.IsValid = !_usuarioService.CpfExiste(cpf, idAtual);
            }
            catch
            {
                args.IsValid = false; 
            }
        }

        protected void ValidateCPF(object source, ServerValidateEventArgs args)
        {
            args.IsValid = CpfHelper.Validar(args.Value);
        }
    }
}