using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using Projeto1.DAL;
using Projeto1.Models;

namespace Projeto1
{
    public partial class Inicio : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UsuarioLogado"] == null && !IsPostBack)
            {
                Response.Redirect("~/Pages/login.aspx");
                return;
            }

            if (!IsPostBack)
            {
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

            else if (Request["__EVENTARGUMENT"] == "RefreshGrid")
            {
                Usuario usuario = (Usuario)Session["UsuarioLogado"];
                if (usuario != null && usuario.Perfil == Perfis.Administrador)
                {
                    CarregarUsuarios(); 
                }
            }
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
            else if (e.CommandName == "Editar")
            {
                // O modal será aberto via JavaScript com o conteúdo carregado via AJAX
            }
        }

        protected void btnAlterarSenha_Click(object sender, EventArgs e)
        {
            Usuario usuario = (Usuario)Session["UsuarioLogado"];
            UsuarioDAL dal = new UsuarioDAL();

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

            if (txtNovaSenha.Text.Length < 6)
            {
                lblMensagemSenha.Text = "A senha deve ter pelo menos 6 caracteres.";
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
            return Convert.ToUInt64(cpf).ToString(@"000\.000\.000\-00");
        }
    }
}