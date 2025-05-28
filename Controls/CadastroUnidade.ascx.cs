using System;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using Projeto1.DAL;
using Projeto1.Models;

namespace Projeto1.Controls
{
    public partial class CadastroUnidade : System.Web.UI.UserControl
    {
        private readonly UnidadeDAL _unidadeDAL = new UnidadeDAL();

        public event EventHandler UnidadeSalvaComSucesso;
        public event EventHandler<string> ErroAoSalvarUnidade;


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
            }
        }

        public void InicializarParaNovaUnidade()
        {
            hfUnidadeID.Value = "0";
            txtNomeUnidade.Text = string.Empty;
            chkUnidadeAtiva.Checked = true;
            lblMensagemUnidade.Text = string.Empty;
            btnSalvarUnidade.Enabled = true;
        }

        public void CarregarUnidadeParaEdicao(int unidadeId)
        {
            var unidade = _unidadeDAL.ObterPorId(unidadeId);
            if (unidade != null)
            {
                hfUnidadeID.Value = unidade.UnidadeID.ToString();
                txtNomeUnidade.Text = unidade.Nome;
                chkUnidadeAtiva.Checked = unidade.Ativo;
                lblMensagemUnidade.Text = string.Empty;
                btnSalvarUnidade.Enabled = true;
            }
            else
            {
                lblMensagemUnidade.Text = "Unidade não encontrada.";
                btnSalvarUnidade.Enabled = false; 
            }
        }

        protected void btnSalvarUnidade_Click(object sender, EventArgs e)
        {
            pnlValidation.Visible = false;
            lblMensagemUnidade.Text = "";

            if (string.IsNullOrWhiteSpace(txtNomeUnidade.Text))
            {
                MostrarErro("Nome da unidade é obrigatório.");
                return;
            }

            try
            {
                var unidade = new Unidade
                {
                    UnidadeID = Convert.ToInt32(hfUnidadeID.Value),
                    Nome = txtNomeUnidade.Text.Trim(),
                    Ativo = chkUnidadeAtiva.Checked
                };

                if (unidade.UnidadeID == 0)
                {
                    _unidadeDAL.Inserir(unidade);
                    hfUnidadeID.Value = unidade.UnidadeID.ToString();
                }
                else
                {
                    _unidadeDAL.Atualizar(unidade);
                }

                UnidadeSalvaComSucesso?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                MostrarErro($"Erro ao salvar: {ex.Message}");
                ErroAoSalvarUnidade?.Invoke(this, ex.Message);
            }
        }

        private void MostrarErro(string mensagem)
        {
            pnlValidation.Visible = true;
            lblValidationSummary.Text = mensagem;
            lblMensagemUnidade.Text = mensagem;

            ScriptManager.RegisterStartupScript(
                this,
                GetType(),
                "ManterModalAberto",
                "if (window.parent) window.parent.manterModalUnidadeAberto();",
                true
            );
        }
    }
}