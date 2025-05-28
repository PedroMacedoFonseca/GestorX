using System;
using System.Web.UI;
using Projeto1.DAL;
using Projeto1.Models;

namespace Projeto1
{
    public partial class TrocaSenha : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UsuarioTrocaSenha"] == null)
            {
                Response.Redirect("~/Pages/login.aspx");
            }
        }

        protected void btnSalvarSenha_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {
                if (txtNovaSenha.Text.Length < 6)
                {
                    lblMensagem.Text = "A senha deve ter pelo menos 6 caracteres.";
                    return;
                }

                Usuario usuario = (Usuario)Session["UsuarioTrocaSenha"];
                UsuarioDAL dal = new UsuarioDAL();

                dal.AtualizarSenha(usuario.ID, txtNovaSenha.Text);
                Session["UsuarioLogado"] = usuario;
                Session.Remove("UsuarioTrocaSenha");
                Response.Redirect("~/Pages/Inicio.aspx");
            }
        }
    }
}