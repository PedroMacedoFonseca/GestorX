<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CadastroUsuario.aspx.cs" Inherits="Projeto1.CadastroUsuario" %>

<form id="form1" runat="server"> 
    <div class="modal-body-content">
        <asp:HiddenField ID="hfID" runat="server" Value="0" />
    
        <div class="row">
            <div class="col-md-6 mb-3">
                <label class="form-label">Nome Completo *</label>
                <asp:TextBox ID="txtNomeCompleto" runat="server" CssClass="form-control" MaxLength="100"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvNome" runat="server" ControlToValidate="txtNomeCompleto"
                    ErrorMessage="Campo obrigatório" Display="Dynamic" CssClass="text-danger small"></asp:RequiredFieldValidator>
            </div>
        
            <div class="col-md-6 mb-3">
                <label class="form-label">CPF *</label>
                <asp:TextBox ID="txtCPF" runat="server" CssClass="form-control cpf-mask"></asp:TextBox>
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
                    CssClass="text-danger small"
                    ValidateEmptyText="true" 
                    ClientValidationFunction="clientValidateCPF" />
            </div>
        </div>
    
        <div class="row">
            <div class="col-md-6 mb-3">
                <label class="form-label">Matrícula *</label>
                <asp:TextBox ID="txtMatricula" runat="server" CssClass="form-control"></asp:TextBox>
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
                <asp:TextBox ID="txtUnidade" runat="server" CssClass="form-control"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvUnidade" runat="server" ControlToValidate="txtUnidade"
                    ErrorMessage="Campo obrigatório" Display="Dynamic" CssClass="text-danger small"></asp:RequiredFieldValidator>
            </div>
        
            <div class="col-md-6 mb-3">
                <label class="form-label">Perfil *</label>
                <asp:DropDownList ID="ddlPerfil" runat="server" CssClass="form-select">
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
                <button class="btn btn-outline-secondary" type="button" id="btnGerarSenha" onclick="usarMatriculaComoSenha()">
                    <i class="bi bi-arrow-repeat"></i> Gerar
                </button>
                <button class="btn btn-outline-secondary" type="button" onclick="toggleSenha()" title="Mostrar/ocultar senha">
                    <i class="bi bi-eye" id="iconeSenha"></i>
                </button>
            </div>
            <asp:RequiredFieldValidator ID="rfvSenha" runat="server" ControlToValidate="txtSenha"
                ErrorMessage="Campo obrigatório" Display="Dynamic" CssClass="text-danger small"
                Enabled="True"></asp:RequiredFieldValidator>
            <small class="text-muted">A senha será visível apenas agora. Após o cadastro, não poderá ser consultada.</small>
        </div>

        <div class="d-flex justify-content-between mt-4">
            <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
            <asp:Button ID="btnSalvar" runat="server" 
                Text="Salvar"
                CssClass="btn btn-primary"
                OnClick="btnSalvar_Click"
                OnClientClick="return validarAntesDeSalvar()" />
        </div>
    </div>
</form>

<script type="text/javascript">
    $(document).ready(function () {

        var isEditMode = $('#<%= hfID.ClientID %>').val() !== "0";

       if (isEditMode) {
           $('#<%= divSenha.ClientID %>').hide();
            if (typeof (ValidatorEnable) === 'function' && document.getElementById('<%= rfvSenha.ClientID %>')) {
                ValidatorEnable(document.getElementById('<%= rfvSenha.ClientID %>'), false);
            }
        } else {
            $('#<%= divSenha.ClientID %>').show();
            var matricula = $('#<%= txtMatricula.ClientID %>').val();
            if (matricula) {
                $('#<%= txtSenha.ClientID %>').val(matricula);
            }
            if (typeof(ValidatorEnable) === 'function' && document.getElementById('<%= rfvSenha.ClientID %>')) {
               ValidatorEnable(document.getElementById('<%= rfvSenha.ClientID %>'), true);
           }
       }
   });

    function usarMatriculaComoSenha() {
        const txtMatricula = document.getElementById('<%= txtMatricula.ClientID %>');
        const txtSenha = document.getElementById('<%= txtSenha.ClientID %>');

        if (txtMatricula && txtSenha) {
            const matricula = txtMatricula.value.trim();
            if (matricula) {
                txtSenha.value = matricula;
            } else {
                alert('Digite a matrícula antes de gerar a senha.');
            }
        }
    }

    function toggleSenha() {
        const txtSenha = document.getElementById('<%= txtSenha.ClientID %>');
        const icone = document.getElementById('iconeSenha');

        if (txtSenha.type === 'password') {
            txtSenha.type = 'text';
            icone.classList.remove('bi-eye');
            icone.classList.add('bi-eye-slash');
        } else {
            txtSenha.type = 'password';
            icone.classList.remove('bi-eye-slash');
            icone.classList.add('bi-eye');
        }
    }

    function clientValidateCPF(source, args) {
        const cpf = $('#<%= txtCPF.ClientID %>').val().replace(/\D/g, '');
        const idAtual = $('#<%= hfID.ClientID %>').val();
        args.IsValid = true; 
    }

    function validarAntesDeSalvar() {
        if (typeof (Page_ClientValidate) === 'function') {
            Page_ClientValidate(''); 
            if (!Page_IsValid) {
                return false;
            }
        }
        return true; 
    }
</script>
