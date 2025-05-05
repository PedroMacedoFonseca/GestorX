<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CadastroUsuario.aspx.cs" Inherits="Projeto1.CadastroUsuario" %>

<form id="form1" runat="server">
    <div class="modal-body-content">
        <asp:HiddenField ID="hfID" runat="server" Value="0" />
    
            <div class="row">
                <div class="col-md-6 mb-3">
                    <label class="form-label">Nome Completo *</label>
                    <asp:TextBox ID="txtNomeCompleto" runat="server" CssClass="form-control" MaxLength="100" required></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvNome" runat="server" ControlToValidate="txtNomeCompleto"
                        ErrorMessage="Campo obrigatório" Display="Dynamic" CssClass="text-danger small"></asp:RequiredFieldValidator>
            </div>
        
            <div class="col-md-6 mb-3">
                <label class="form-label">CPF *</label>
                <asp:TextBox ID="txtCPF" runat="server" CssClass="form-control cpf-mask" required></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvCPF" runat="server" ControlToValidate="txtCPF"
                    ErrorMessage="Campo obrigatório" Display="Dynamic" CssClass="text-danger small"></asp:RequiredFieldValidator>
                <asp:CustomValidator ID="cvCPF" runat="server" ControlToValidate="txtCPF"
                    OnServerValidate="ValidateCPF" ErrorMessage="CPF inválido" Display="Dynamic"
                    CssClass="text-danger small"></asp:CustomValidator>
                <asp:CustomValidator ID="cvCPFExistente" runat="server" 
                    ControlToValidate="txtCPF"
                    OnServerValidate="ValidarCPFExistente"
                    ErrorMessage="CPF já cadastrado"
                    Display="Dynamic"
                    CssClass="text-danger small"></asp:CustomValidator>
            </div>
        </div>
    
        <div class="row">
            <div class="col-md-6 mb-3">
                <label class="form-label">Matrícula *</label>
                <asp:TextBox ID="txtMatricula" runat="server" CssClass="form-control" required></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvMatricula" runat="server" ControlToValidate="txtMatricula"
                    ErrorMessage="Campo obrigatório" Display="Dynamic" CssClass="text-danger small"></asp:RequiredFieldValidator>
            </div>
        
            <div class="col-md-6 mb-3">
                <label class="form-label">Telefone</label>
                <asp:TextBox ID="txtTelefone" runat="server" CssClass="form-control phone-mask"></asp:TextBox>
            </div>
        </div>
    
        <div class="row">
            <div class="col-md-6 mb-3">
                <label class="form-label">Unidade *</label>
                <asp:TextBox ID="txtUnidade" runat="server" CssClass="form-control" required></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvUnidade" runat="server" ControlToValidate="txtUnidade"
                    ErrorMessage="Campo obrigatório" Display="Dynamic" CssClass="text-danger small"></asp:RequiredFieldValidator>
            </div>
        
            <div class="col-md-6 mb-3">
                <label class="form-label">Perfil *</label>
                <asp:DropDownList ID="ddlPerfil" runat="server" CssClass="form-select" required>
                    <asp:ListItem Value="">Selecione...</asp:ListItem>
                    <asp:ListItem Value="Administrador">Administrador</asp:ListItem>
                    <asp:ListItem Value="Colaborador">Colaborador</asp:ListItem>
                    <asp:ListItem Value="Terceirizado">Terceirizado</asp:ListItem>
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="rfvPerfil" runat="server" ControlToValidate="ddlPerfil"
                    ErrorMessage="Selecione um perfil" Display="Dynamic" CssClass="text-danger small"></asp:RequiredFieldValidator>
            </div>
        </div>
    
        <div class="mb-3" id="divSenha" runat="server">
            <label class="form-label">Senha Inicial *</label>
            <div class="input-group">
                <asp:TextBox ID="txtSenha" runat="server" TextMode="Password" CssClass="form-control"></asp:TextBox>
                <button class="btn btn-outline-secondary" type="button" id="btnGerarSenha">
                    <i class="bi bi-arrow-repeat"></i> Gerar
                </button>
            </div>
            <asp:RequiredFieldValidator ID="rfvSenha" runat="server" ControlToValidate="txtSenha"
                ErrorMessage="Campo obrigatório" Display="Dynamic" CssClass="text-danger small"
                Enabled="False"></asp:RequiredFieldValidator>
            <small class="text-muted">A senha inicial será a matrícula do usuário</small>
        </div>

        <div class="d-flex justify-content-between mt-4">
            <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
            <asp:Button ID="btnSalvar" runat="server" Text="Salvar" 
                CssClass="btn btn-primary" OnClick="btnSalvar_Click" />
        </div>
    </div>
</form>

<script type="text/javascript">
    $('#modalCadastro').on('shown.bs.modal', function () {

        $('.cpf-mask').inputmask('999.999.999-99');
        $('.phone-mask').inputmask('(99) [9]9999-9999');

        // Preenche senha automaticamente se for novo cadastro
        if ($('#<%= hfID.ClientID %>').val() == 0) {
            var matricula = $('#<%= txtMatricula.ClientID %>').val();
            if (matricula) {
                $('#<%= txtSenha.ClientID %>').val(matricula);
            }
        }

        // Botão Gerar Senha
        $(document).on('click', '#btnGerarSenha', function () {
            var matricula = $('#<%= txtMatricula.ClientID %>').val();
            if (matricula) {
                $('#<%= txtSenha.ClientID %>').val(matricula);
            } else {
                alert('Informe a matrícula primeiro');
            }
        });
        
        // Controle de exibição do campo senha
        if ($('#<%= hfID.ClientID %>').val() > 0) {
            $('#<%= divSenha.ClientID %>').hide();
            $('#<%= rfvSenha.ClientID %>').hide();
        } else {
            $('#<%= rfvSenha.ClientID %>').show();
        }
    });
</script>