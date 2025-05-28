<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CadastroUnidade.ascx.cs" Inherits="Projeto1.Controls.CadastroUnidade" %>

<asp:HiddenField ID="hfUnidadeID" runat="server" Value="0" />

<asp:Panel ID="pnlValidation" runat="server" CssClass="alert alert-danger mb-3" Visible="false">
    <i class="bi bi-exclamation-circle-fill me-2"></i>
    <asp:Label ID="lblValidationSummary" runat="server" />
</asp:Panel>

<div class="row">
    <div class="col-md-12 mb-3">
        <label class="form-label">Nome da Unidade *</label>
        <asp:TextBox ID="txtNomeUnidade" runat="server" CssClass="form-control" MaxLength="150"></asp:TextBox>
        <asp:RequiredFieldValidator ID="rfvNomeUnidade" runat="server" ControlToValidate="txtNomeUnidade"
            ErrorMessage="Nome da unidade é obrigatório." ValidationGroup="vgUnidade" Display="Dynamic" CssClass="text-danger small"></asp:RequiredFieldValidator>
    </div>
</div>
<div class="row">
    <div class="col-md-12 mb-3">
        <asp:CheckBox ID="chkUnidadeAtiva" runat="server" Text=" Unidade Ativa" Checked="true" />
    </div>
</div>

<div class="d-flex justify-content-between mt-4">
    <button type="button" class="btn btn-secondary" onclick="fecharModalUnidade(); return false;">Cancelar</button>
    <asp:Button ID="btnSalvarUnidade" runat="server"
        Text="Salvar Unidade"
        CssClass="btn btn-primary"
        OnClick="btnSalvarUnidade_Click"
        ValidationGroup="vgUnidade" />
    <asp:Label ID="lblSuccessMessage" runat="server" CssClass="text-success small mt-2 d-block" Visible="false">
        Unidade salva com sucesso!
    </asp:Label>
</div>
<asp:Label ID="lblMensagemUnidade" runat="server" CssClass="text-danger small mt-2 d-block"></asp:Label>