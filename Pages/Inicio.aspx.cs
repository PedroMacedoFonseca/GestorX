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
        protected void Page_Init(object sender, EventArgs e)
        {
            EnsureCadastroUsuarioControlIsLoaded();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UsuarioLogado"] == null && !IsPostBack)
            {
                Response.Redirect("~/Pages/login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                Session["IsCadastroUsuarioLoaded"] = false;
                Usuario usuario = (Usuario)Session["UsuarioLogado"];

                if (usuario.Perfil == Perfis.Administrador)
                {
                    pnlAdmin.Visible = true;
                    pnlColaborador.Visible = false;
                    CarregarUsuarios();
                }
                else
                {
                    pnlAdmin.Visible = false;
                    pnlColaborador.Visible = true;
                    CarregarDadosUsuario(usuario);
                }
            }

            if (IsPostBack && Request.Form["__EVENTTARGET"] == updCadastro.UniqueID)
            {
                string argument = Request.Form["__EVENTARGUMENT"];
                if (!string.IsNullOrEmpty(argument) && argument.StartsWith("Editar_"))
                {
                    EnsureCadastroUsuarioControlIsLoaded(); 
                    int id = int.Parse(argument.Split('_')[1]);
                    cadUsuarioCtrl.CarregarUsuarioParaEdicao(id);
                    litModalTitle.Text = "Editar Usuário";
                    updCadastro.Update();
                    ScriptManager.RegisterStartupScript(
                        updCadastro,
                        updCadastro.GetType(),
                        "AbrirModalEdicao",
                        "$('#modalCadastro').modal('show');",
                        true
                    );
                }
            }

            var userCtrl = phCadastroUsuario.FindControl("cadUsuario") as Projeto1.Controls.CadastroUsuario;
            if (userCtrl != null)
            {
                var btnSalvar = userCtrl.FindControl("btnSalvar");
                if (btnSalvar != null)
                {
                    ScriptManager.GetCurrent(this).RegisterAsyncPostBackControl(btnSalvar);
                }
            }
        }

        private void EnsureCadastroUsuarioControlIsLoaded()
        {
            if (phCadastroUsuario.FindControl("cadUsuario") == null)
            {
                cadUsuarioCtrl = (CadastroUsuario)LoadControl("~/Controls/CadastroUsuario.ascx");
                cadUsuarioCtrl.ID = "cadUsuario";
                cadUsuarioCtrl.SalvoComSucesso += cadUsuario_SalvoComSucesso;
                cadUsuarioCtrl.ErroOcorreu += cadUsuario_ErroOcorreu;
                phCadastroUsuario.Controls.Add(cadUsuarioCtrl);
            }
        }

        protected void btnAbrirModalNovoUsuario_Click(object sender, EventArgs e)
        {
            EnsureCadastroUsuarioControlIsLoaded();
            cadUsuarioCtrl.InicializarParaNovoUsuario();
            litModalTitle.Text = "Novo Usuário";
            updCadastro.Update();

            ScriptManager.RegisterStartupScript(
                updCadastro,
                updCadastro.GetType(),
                "AbrirModalNovo",
                "var myModal = new bootstrap.Modal(document.getElementById('modalCadastro')); myModal.show();",
                true
            );
        }

        protected void cadUsuario_SalvoComSucesso(object sender, EventArgs e)
        {
            CarregarUsuarios();
            ScriptManager.RegisterStartupScript(this, GetType(), "SalvoSucessoEFecharModal", "alert('Usuário salvo com sucesso!'); $('#modalCadastro').modal('hide');", true);

            if (phCadastroUsuario != null)
            {
                phCadastroUsuario.Controls.Clear(); 
            }
            Session["IsCadastroUsuarioLoaded"] = false;
        }

        protected void cadUsuario_ErroOcorreu(object sender, Exception ex)
        {
            updCadastro.Update();

            ScriptManager.RegisterStartupScript(
                updCadastro,
                updCadastro.GetType(),
                "ShowModalComMasks",
                "$('#modalCadastro').modal('show');",
                true);
        }

        private void CarregarUsuarios()
        {
            UsuarioDAL dal = new UsuarioDAL();
            gvUsuarios.DataSource = dal.ObterTodos();
            gvUsuarios.DataBind();
        }

        private void CarregarDadosUsuario(Usuario usuario)
        {
            lblNome.Text = usuario.NomeCompleto;
            lblCPF.Text = FormatCPF(usuario.CPF);
            lblMatricula.Text = usuario.Matricula;
            lblUnidade.Text = usuario.Unidade;
            lblTelefone.Text = usuario.Telefone;
            lblPerfil.Text = usuario.Perfil;
            lblDataCadastro.Text = usuario.DataCadastro.ToString("dd/MM/yyyy HH:mm");
        }

        protected void gvUsuarios_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Excluir")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                UsuarioDAL dal = new UsuarioDAL();
                dal.Excluir(id);
                CarregarUsuarios();
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