using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Projeto1.Controls;
using Projeto1.DAL;
using Projeto1.Models;

namespace Projeto1
{
    public partial class Inicio : System.Web.UI.Page
    {
        private CadastroUsuario cadUsuarioCtrl;
        private CadastroUnidade cadUnidadeCtrl;
        private readonly UnidadeDAL _unidadeDAL = new UnidadeDAL();

        protected void Page_Init(object sender, EventArgs e)
        {
            if (Request.Form["__EVENTTARGET"] == updCadastro.UniqueID ||
                phCadastroUsuario.FindControl("cadUsuario") != null ||
                Session["AbrirModalUsuario"] != null && (bool)Session["AbrirModalUsuario"])
            {
                EnsureCadastroUsuarioControlIsLoaded();
            }

            if (Request.Form["__EVENTTARGET"] == updUnidade.UniqueID ||
                phCadastroUnidade.FindControl("cadUnidade") != null ||
                Session["AbrirModalUnidade"] != null && (bool)Session["AbrirModalUnidade"])
            {
                EnsureCadastroUnidadeControlIsLoaded();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UsuarioLogado"] == null)
            {
                Response.Redirect("~/Pages/login.aspx");
                return;
            }

            Usuario usuarioLogado = (Usuario)Session["UsuarioLogado"];

            if (!IsPostBack)
            {
                if (usuarioLogado.Perfil == "Administrador")
                {
                    pnlAdmin.Visible = true;
                    pnlColaborador.Visible = false;
                    CarregarUsuarios();
                }
                else
                {
                    pnlAdmin.Visible = false;
                    pnlColaborador.Visible = true;
                    CarregarDadosUsuario(usuarioLogado);
                }
            }

            if (Session["AbrirModalUsuario"] != null && (bool)Session["AbrirModalUsuario"])
            {
                EnsureCadastroUsuarioControlIsLoaded();
                ScriptManager.RegisterStartupScript(
                    updCadastro,
                    updCadastro.GetType(),
                    "ReabrirModalUsuario",
                    "window.shouldOpenModalCadastro = true;",
                    addScriptTags: true
                );
                Session["AbrirModalUsuario"] = null;
            }

            if (Session["AbrirModalUnidade"] != null && (bool)Session["AbrirModalUnidade"])
            {
                EnsureCadastroUnidadeControlIsLoaded();
                ScriptManager.RegisterStartupScript(
                    updUnidade,
                    updUnidade.GetType(),
                    "ReabrirModalUnidade",
                    "window.shouldOpenModalUnidade = true;",
                    addScriptTags: true
                );
                Session["AbrirModalUnidade"] = null;
            }

            if (IsPostBack && Request.Form["__EVENTTARGET"] == updCadastro.UniqueID)
            {
                string argument = Request.Form["__EVENTARGUMENT"];
                if (!string.IsNullOrEmpty(argument) && argument.StartsWith("Editar_"))
                {
                    EnsureCadastroUsuarioControlIsLoaded();
                    if (cadUsuarioCtrl != null)
                    {
                        int id = int.Parse(argument.Split('_')[1]);
                        cadUsuarioCtrl.CarregarUsuarioParaEdicao(id);
                        litModalTitle.Text = "Editar Usuário";
                        lblModalErrorMessage.Visible = false;
                        updCadastro.Update();
                        ScriptManager.RegisterStartupScript(
                            updCadastro,
                            updCadastro.GetType(),
                            "AbrirModalEdicaoUsuario",
                            "window.shouldOpenModalCadastro = true;",
                            addScriptTags: true
                        );
                    }
                }
            }
        }

        private void CarregarUsuarios()
        {
            UsuarioDAL dal = new UsuarioDAL();
            gvUsuarios.DataSource = dal.ObterTodos();
            gvUsuarios.DataBind();
        }

        public void AtualizarGridUsuarios()
        {
            CarregarUsuarios();
            gvUsuarios.DataBind();
            updCadastro.Update(); 
        }

        private void CarregarDadosUsuario(Usuario usuario)
        {
            UsuarioDAL dal = new UsuarioDAL();
            var usuarioCompleto = dal.ObterPorId(usuario.ID);

            if (usuarioCompleto != null)
            {
                lblNome.Text = usuarioCompleto.NomeCompleto;
                lblCPF.Text = FormatCPF(usuarioCompleto.CPF);
                lblMatricula.Text = usuarioCompleto.Matricula;
                lblUnidade.Text = usuarioCompleto.NomeUnidade;
                lblTelefone.Text = usuarioCompleto.Telefone;
                lblPerfil.Text = usuarioCompleto.Perfil;
                lblDataCadastro.Text = usuarioCompleto.DataCadastro.ToString("dd/MM/yyyy HH:mm");
            }
            else
            {
                lblNome.Text = "Erro ao carregar dados.";
            }
        }

        private void EnsureCadastroUsuarioControlIsLoaded()
        {
            if (phCadastroUsuario.FindControl("cadUsuario") == null)
            {
                cadUsuarioCtrl = (CadastroUsuario)LoadControl("~/Controls/CadastroUsuario.ascx");
                cadUsuarioCtrl.ID = "cadUsuario";
                cadUsuarioCtrl.SalvoComSucesso += CadUsuario_SalvoComSucesso;
                cadUsuarioCtrl.ErroOcorreu += CadUsuario_ErroOcorreu;
                phCadastroUsuario.Controls.Add(cadUsuarioCtrl);
                RegistrarControleAsyncPostBackUsuario();
            }
            else
            {
                cadUsuarioCtrl = (CadastroUsuario)phCadastroUsuario.FindControl("cadUsuario");
                cadUsuarioCtrl.SalvoComSucesso -= CadUsuario_SalvoComSucesso;
                cadUsuarioCtrl.SalvoComSucesso += CadUsuario_SalvoComSucesso;
                cadUsuarioCtrl.ErroOcorreu -= CadUsuario_ErroOcorreu;
                cadUsuarioCtrl.ErroOcorreu += CadUsuario_ErroOcorreu;
                RegistrarControleAsyncPostBackUsuario();
            }
        }

        private void EnsureCadastroUnidadeControlIsLoaded()
        {
            if (phCadastroUnidade.FindControl("cadUnidade") == null)
            {
                cadUnidadeCtrl = (CadastroUnidade)LoadControl("~/Controls/CadastroUnidade.ascx");
                cadUnidadeCtrl.ID = "cadUnidade";
                cadUnidadeCtrl.UnidadeSalvaComSucesso += CadUnidade_SalvaComSucesso;
                cadUnidadeCtrl.ErroAoSalvarUnidade += CadUnidade_ErroAoSalvar;
                phCadastroUnidade.Controls.Add(cadUnidadeCtrl);
                RegistrarControleAsyncPostBackUnidade();
            }
            else
            {
                cadUnidadeCtrl = (CadastroUnidade)phCadastroUnidade.FindControl("cadUnidade");
                cadUnidadeCtrl.UnidadeSalvaComSucesso -= CadUnidade_SalvaComSucesso;
                cadUnidadeCtrl.UnidadeSalvaComSucesso += CadUnidade_SalvaComSucesso;
                cadUnidadeCtrl.ErroAoSalvarUnidade -= CadUnidade_ErroAoSalvar;
                cadUnidadeCtrl.ErroAoSalvarUnidade += CadUnidade_ErroAoSalvar;
                RegistrarControleAsyncPostBackUnidade();
            }
        }

        private void RegistrarControleAsyncPostBackUsuario()
        {
            if (cadUsuarioCtrl != null)
            {
                if (cadUsuarioCtrl.FindControl("btnSalvar") is Button btnSalvarUsuario)
                {
                    ScriptManager.GetCurrent(this).RegisterAsyncPostBackControl(btnSalvarUsuario);
                }
            }
        }
        private void RegistrarControleAsyncPostBackUnidade()
        {
            if (cadUnidadeCtrl != null)
            {
                if (cadUnidadeCtrl.FindControl("btnSalvarUnidade") is Button btnSalvarUnidade)
                {
                    ScriptManager.GetCurrent(this).RegisterAsyncPostBackControl(btnSalvarUnidade);
                }
            }
        }

        protected void btnAbrirModalNovoUsuario_Click(object sender, EventArgs e)
        {
            EnsureCadastroUsuarioControlIsLoaded();
            if (cadUsuarioCtrl != null)
            {
                cadUsuarioCtrl.InicializarParaNovoUsuario();
                litModalTitle.Text = "Novo Usuário";
                lblModalErrorMessage.Visible = false;
                updCadastro.Update();
                ScriptManager.RegisterStartupScript(
                    updCadastro,
                    updCadastro.GetType(),
                    "AbrirModalNovoUsuario",
                    "window.shouldOpenModalCadastro = true;",
                    addScriptTags: true
                );
            }
        }

        protected void CadUsuario_SalvoComSucesso(object sender, EventArgs e)
        {
            CarregarUsuarios();
            ScriptManager.RegisterStartupScript(
                this,
                GetType(),
                "UsuarioSalvoEFecharModal",
                "alert('Usuário salvo com sucesso!'); window.shouldCloseModalCadastro = true;",
                addScriptTags: true
            );
            updCadastro.Update();
        }

        protected void CadUsuario_ErroOcorreu(object sender, Exception ex)
        {
            EnsureCadastroUsuarioControlIsLoaded();
            lblModalErrorMessage.Text = "Erro: " + ex.Message;
            lblModalErrorMessage.Visible = true;

            Session["AbrirModalUsuario"] = true;
            updCadastro.Update();
        }

        protected void gvUsuarios_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int id = Convert.ToInt32(e.CommandArgument);
            if (e.CommandName == "Excluir")
            {
                UsuarioDAL dal = new UsuarioDAL();
                dal.Excluir(id);
                CarregarUsuarios();
            }
            else if (e.CommandName == "Editar")
            {
                EnsureCadastroUsuarioControlIsLoaded();
                cadUsuarioCtrl.CarregarUsuarioParaEdicao(id);
                litModalTitle.Text = "Editar Usuário";
                lblModalErrorMessage.Visible = false;
                updCadastro.Update();
                ScriptManager.RegisterStartupScript(
                    updCadastro,
                    updCadastro.GetType(),
                    "AbrirModalEdicaoUsuario",
                    "window.shouldOpenModalCadastro = true;",
                    addScriptTags: true
                );
            }
        }

        protected void btnAlterarSenha_Click(object sender, EventArgs e)
        {
            Usuario usuario = (Usuario)Session["UsuarioLogado"];
            UsuarioDAL dal = new UsuarioDAL();

            if (usuario == null)
            {
                lblMensagemSenha.Text = "Sua sessão expirou. Por favor, faça login novamente.";
                return;
            }

            if (!dal.VerificarSenha(usuario.ID, txtSenhaAtual.Text))
            {
                lblMensagemSenha.Text = "Senha atual incorreta.";
                return;
            }
            if (txtNovaSenha.Text != txtConfirmarSenha.Text)
            {
                lblMensagemSenha.Text = "As senhas não coincidem.";
                return;
            }
            if (string.IsNullOrWhiteSpace(txtNovaSenha.Text) || txtNovaSenha.Text.Length < 6)
            {
                lblMensagemSenha.Text = "A nova senha deve ter pelo menos 6 caracteres.";
                return;
            }
            dal.AtualizarSenha(usuario.ID, txtNovaSenha.Text);
            lblMensagemSenha.Text = "Senha alterada com sucesso!";
            txtSenhaAtual.Text = "";
            txtNovaSenha.Text = "";
            txtConfirmarSenha.Text = "";
        }

        protected void btnAbrirModalNovaUnidade_Click(object sender, EventArgs e)
        {
            EnsureCadastroUnidadeControlIsLoaded();
            if (cadUnidadeCtrl != null)
            {
                cadUnidadeCtrl.InicializarParaNovaUnidade();
                litModalUnidadeTitle.Text = "Nova Unidade";
                updUnidade.Update();
                ScriptManager.RegisterStartupScript(
                    updUnidade,
                    updUnidade.GetType(),
                    "AbrirModalNovaUnidade",
                    "window.shouldOpenModalUnidade = true;",
                    addScriptTags: true
                );
            }
        }

        protected void btnGerenciarUnidades_Click(object sender, EventArgs e)
        {
            CarregarUnidadesGrid();
            updGerenciarUnidades.Update();
            ScriptManager.RegisterStartupScript(
                this,
                this.GetType(),
                "AbrirModalGerenciarUnidades",
                "window.shouldOpenModalGerenciarUnidades = true;",
                addScriptTags: true
            );
        }

        private void CarregarUnidadesGrid()
        {
            gvUnidades.DataSource = _unidadeDAL.ObterTodas();
            gvUnidades.DataBind();
        }

        protected void gvUnidades_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int unidadeId = Convert.ToInt32(e.CommandArgument);
            if (e.CommandName == "EditarUnidade")
            {
                EnsureCadastroUnidadeControlIsLoaded();
                cadUnidadeCtrl.CarregarUnidadeParaEdicao(unidadeId);
                litModalUnidadeTitle.Text = "Editar Unidade";
                updUnidade.Update();
                ScriptManager.RegisterStartupScript(
                    this,
                    this.GetType(),
                    "AbrirModalEdicaoUnidade",
                    "$('#modalGerenciarUnidades').modal('hide'); " + 
                    "$('#modalUnidade').modal('show');",             
                    true
                );
            }
            else if (e.CommandName == "ExcluirUnidade")
            {
                try
                {
                    _unidadeDAL.Excluir(unidadeId);
                    CarregarUnidadesGrid();
                    updGerenciarUnidades.Update();

                    if (phCadastroUsuario.FindControl("cadUsuario") != null)
                    {
                        cadUsuarioCtrl = (CadastroUsuario)phCadastroUsuario.FindControl("cadUsuario");
                        cadUsuarioCtrl.AtualizarListaDeUnidades();
                        updCadastro.Update();
                    }
                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterClientScriptBlock(
                        this,
                        this.GetType(),
                        "alert",
                        $"alert('Erro ao excluir unidade: {ex.Message.Replace("'", "\\'")}');",
                        true
                    );
                }
            }
        }

        protected void CadUnidade_SalvaComSucesso(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(
                this,
                GetType(),
                "FecharModalSucesso",
                "fecharModalUnidade();",
                true
            );
        }

        protected void CadUnidade_ErroAoSalvar(object sender, string errorMessage)
        {
            ScriptManager.RegisterStartupScript(
                this,
                GetType(),
                "ManterModalAberto",
                "manterModalUnidadeAberto();",
                true
            );
        }

        private string FormatCPF(string cpf)
        {
            if (string.IsNullOrEmpty(cpf)) return "";
            string cpfNumerico = new string(cpf.Where(char.IsDigit).ToArray());
            if (cpfNumerico.Length == 11)
                return Convert.ToUInt64(cpfNumerico).ToString(@"000\.000\.000\-00");
            return cpf;
        }

        [System.Web.Services.WebMethod]
        public static bool VerificarCPFExistente(string cpf, int idAtual)
        {
            cpf = Projeto1.Helpers.CpfHelper.RemoverFormatacao(cpf);
            var service = new Projeto1.Services.UsuarioService();
            return service.CpfExiste(cpf, idAtual);
        }
    }
}
