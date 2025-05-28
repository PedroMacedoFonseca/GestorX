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
        private readonly UnidadeDAL _unidadeDAL = new UnidadeDAL(); 

        public event EventHandler SalvoComSucesso;
        public event EventHandler<Exception> ErroOcorreu;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
            }
        }
        public void AtualizarListaDeUnidades()
        {
            string selectedValue = ddlUnidade.SelectedValue;
            CarregarUnidadesDropDown();
            if (ddlUnidade.Items.FindByValue(selectedValue) != null)
            {
                ddlUnidade.SelectedValue = selectedValue;
            }
        }


        private void CarregarUnidadesDropDown()
        {
            try
            {
                var unidadesAtivas = _unidadeDAL.ObterAtivas();
                ddlUnidade.DataSource = unidadesAtivas;
                ddlUnidade.DataTextField = "Nome";
                ddlUnidade.DataValueField = "UnidadeID";
                ddlUnidade.DataBind();
                ddlUnidade.Items.Insert(0, new ListItem("Selecione...", ""));
            }
            catch (Exception ex)
            {
                lblMensagemErroControle.Text = "Erro ao carregar unidades: " + ex.Message;
            }
        }

        public void InicializarParaNovoUsuario()
        {
            hfID.Value = "0";
            txtNomeCompleto.Text = string.Empty;
            txtCPF.Text = string.Empty;
            txtMatricula.Text = string.Empty;
            txtTelefone.Text = string.Empty;
            ddlPerfil.ClearSelection();
            if (ddlPerfil.Items.FindByValue("") != null)
            {
                ddlPerfil.SelectedValue = "";
            }
            txtSenha.Text = string.Empty;
            lblMensagemErroControle.Text = string.Empty;
            btnSalvar.Enabled = true;
            btnSalvar.Text = "Salvar";


            CarregarUnidadesDropDown(); 
            ddlUnidade.ClearSelection();
            if (ddlUnidade.Items.FindByValue("") != null)
            {
                ddlUnidade.SelectedValue = "";
            }

            ConfigurarModoNovoUsuario();
        }

        public void CarregarUsuarioParaEdicao(int id)
        {
            lblMensagemErroControle.Text = string.Empty;
            btnSalvar.Enabled = true;
            btnSalvar.Text = "Salvar";

            CarregarUnidadesDropDown(); 
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

            ddlUnidade.ClearSelection();
            ListItem itemUnidade = ddlUnidade.Items.FindByValue(usuario.UnidadeID.ToString());
            if (itemUnidade != null)
            {
                itemUnidade.Selected = true;
            }
            else if (ddlUnidade.Items.FindByValue("") != null) 
            {
                ddlUnidade.SelectedValue = ""; 
            }


            ddlPerfil.ClearSelection();
            if (ddlPerfil.Items.FindByValue(usuario.Perfil) != null) 
            {
                ddlPerfil.SelectedValue = usuario.Perfil;
            }
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                lblMensagemErroControle.Text = string.Empty;
                Page.Validate("vgCadastro"); 
                if (!Page.IsValid)
                {
                    ErroOcorreu?.Invoke(this, new Exception("Dados inválidos. Verifique os campos."));
                    btnSalvar.Enabled = true; 
                    btnSalvar.Text = "Salvar";
                    return;
                }

                if (string.IsNullOrEmpty(ddlUnidade.SelectedValue))
                {
                    lblMensagemErroControle.Text = "O campo Unidade é obrigatório.";
                    ErroOcorreu?.Invoke(this, new Exception("O campo Unidade é obrigatório."));
                    btnSalvar.Enabled = true;
                    btnSalvar.Text = "Salvar";
                    return;
                }

                var usuario = new Usuario
                {
                    ID = Convert.ToInt32(string.IsNullOrEmpty(hfID.Value) ? "0" : hfID.Value),
                    NomeCompleto = txtNomeCompleto.Text.Trim(),
                    CPF = CpfHelper.RemoverFormatacao(txtCPF.Text),
                    Matricula = txtMatricula.Text.Trim(),
                    Telefone = CpfHelper.RemoverFormatacao(txtTelefone.Text.Trim()),
                    UnidadeID = Convert.ToInt32(ddlUnidade.SelectedValue), 
                    Perfil = ddlPerfil.SelectedValue
                };

                if (usuario.ID == 0) 
                {
                    if (string.IsNullOrWhiteSpace(txtSenha.Text) || txtSenha.Text.Length < 6)
                    {
                        lblMensagemErroControle.Text = "A senha inicial é obrigatória e deve ter pelo menos 6 caracteres.";
                        ErroOcorreu?.Invoke(this, new Exception(lblMensagemErroControle.Text));
                        btnSalvar.Enabled = true;
                        btnSalvar.Text = "Salvar";
                        return;
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
                if (this.Page is Inicio inicioPage)
                {
                    inicioPage.AtualizarGridUsuarios();
                }

            }
            catch (Exception ex)
            {
                lblMensagemErroControle.Text = "Erro ao salvar usuário: " + ex.Message;
                ErroOcorreu?.Invoke(this, new Exception("Erro ao salvar: " + ex.Message, ex));
                btnSalvar.Enabled = true; 
                btnSalvar.Text = "Salvar";
            }
        }
        protected void ValidateCPF(object source, ServerValidateEventArgs args)
        {
            args.IsValid = CpfHelper.Validar(args.Value);
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
                if (idAtual > 0)
                {
                    var usuarioAtual = _usuarioService.ObterPorId(idAtual);
                    if (usuarioAtual != null && usuarioAtual.CPF == cpf)
                    {
                        args.IsValid = true;
                        return;
                    }
                }
                args.IsValid = !_usuarioService.CpfExiste(cpf, idAtual);
            }
            catch
            {
                args.IsValid = false;
            }
        }
    }
}