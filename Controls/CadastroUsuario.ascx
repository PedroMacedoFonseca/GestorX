<%@ Control Language="C#" AutoEventWireup="true" 
    CodeBehind="CadastroUsuario.ascx.cs" 
    Inherits="Projeto1.Controls.CadastroUsuario" %>
    <asp:HiddenField ID="hfID" runat="server" />
        <div class="row">
            <div class="col-md-6 mb-3">
                <label class="form-label">Nome Completo *</label>
                <asp:TextBox ID="txtNomeCompleto" runat="server" CssClass="form-control" MaxLength="100"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvNome" runat="server" ControlToValidate="txtNomeCompleto"  EnableClientScript="true"
                    ErrorMessage="Campo obrigatório" ValidationGroup="vgCadastro" Display="Dynamic" CssClass="text-danger small"></asp:RequiredFieldValidator>
            </div>
        
            <div class="col-md-6 mb-3">
                <label class="form-label">CPF *</label>
                <asp:TextBox ID="txtCPF" runat="server" CssClass="form-control cpf-mask"></asp:TextBox>
                <asp:RequiredFieldValidator 
                  ID="rfvCPF" runat="server" 
                  ControlToValidate="txtCPF" 
                  EnableClientScript="true"
                  ErrorMessage="Campo obrigatório" 
                  Display="Dynamic" 
                  CssClass="text-danger small"
                  ValidationGroup="vgCadastro" />
                <asp:CustomValidator ID="cvCPF" runat="server" ControlToValidate="txtCPF"  EnableClientScript="true"
                    OnServerValidate="ValidateCPF" ErrorMessage="CPF inválido" Display="Dynamic"
                    CssClass="text-danger small" ValidationGroup="vgCadastro" ClientValidationFunction="clientValidateCPF" />
                <asp:CustomValidator 
                    ID="cvCPFExistente" 
                    runat="server"
                    ControlToValidate="txtCPF"
                    EnableClientScript="true"
                    ClientValidationFunction="clientValidateCPFExistente"
                    OnServerValidate="ValidarCPFExistente"
                    ErrorMessage="CPF já cadastrado"
                    Display="Dynamic"
                    CssClass="text-danger small"
                    ValidateEmptyText="false"
                    ValidationGroup="vgCadastro" />
            </div>
        </div>
    
        <div class="row">
            <div class="col-md-6 mb-3">
                <label class="form-label">Matrícula *</label>
                <asp:TextBox ID="txtMatricula" runat="server" CssClass="form-control"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvMatricula" runat="server" ControlToValidate="txtMatricula"
                    ErrorMessage="Campo obrigatório" ValidationGroup="vgCadastro" Display="Dynamic" CssClass="text-danger small"></asp:RequiredFieldValidator>
            </div>
        
            <div class="col-md-6 mb-3">
                <label class="form-label">Telefone</label>
                <asp:TextBox ID="txtTelefone" runat="server" CssClass="form-control phone-mask"></asp:TextBox>
            </div>
        </div>
    
        <div class="row">
            <div class="col-md-6 mb-3">
                <label class="form-label">Unidade *</label>
                <asp:DropDownList ID="ddlUnidade" runat="server" CssClass="form-select"
                    DataTextField="Nome" DataValueField="UnidadeID">
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="rfvUnidade" runat="server" ControlToValidate="ddlUnidade"
                    InitialValue="" 
                    ErrorMessage="Campo obrigatório" ValidationGroup="vgCadastro" Display="Dynamic" CssClass="text-danger small"></asp:RequiredFieldValidator>
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
                    ErrorMessage="Selecione um perfil" ValidationGroup="vgCadastro" Display="Dynamic" CssClass="text-danger small"></asp:RequiredFieldValidator>
            </div>
        </div>
    
        <div class="mb-3" id="divSenha" runat="server">
            <label class="form-label">Senha Inicial *</label>
            <div class="input-group">
                <asp:TextBox ID="txtSenha" runat="server" TextMode="Password" CssClass="form-control"></asp:TextBox>
                <button type="button" id="btnGerarSenha" 
                        onclick="usarMatriculaComoSenha(); return false;"
                        class="btn btn-outline-secondary">
                    <i class="bi bi-arrow-repeat"></i> Gerar
                </button>
                <button class="btn btn-outline-secondary" type="button" onclick="toggleSenha()" title="Mostrar/ocultar senha">
                    <i class="bi bi-eye" id="iconeSenha"></i>
                </button>
            </div>
            <asp:RequiredFieldValidator ID="rfvSenha" runat="server" ControlToValidate="txtSenha"
                ErrorMessage="Campo obrigatório" ValidationGroup="vgCadastro" Display="Dynamic" CssClass="text-danger small"
                Enabled="True"></asp:RequiredFieldValidator>
            <small class="text-muted">A senha será visível apenas agora. Após o cadastro, não poderá ser consultada.</small>
        </div>

        <div class="d-flex justify-content-between mt-4">
            <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
            <asp:Button ID="btnSalvar" runat="server" 
                Text="Salvar"
                CssClass="btn btn-primary"
                OnClick="btnSalvar_Click"
                ValidationGroup="vgCadastro"
                UseSubmitBehavior="false" />
        </div>
        <asp:Label ID="lblMensagemErroControle" runat="server" CssClass="text-danger small mt-2 d-block"></asp:Label>

<script type="text/javascript">
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

    function clientValidateCPFExistente(sender, args) {
        const cpf = args.Value.replace(/[^\d]/g, '');
        const idAtual = parseInt($('#<%= hfID.ClientID %>').val() || 0, 10);

    fetch("Inicio.aspx/VerificarCPFExistente", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ cpf: cpf, idAtual: idAtual })
    })
        .then(response => response.json())
        .then(data => args.IsValid = !data.d)
        .catch(() => args.IsValid = false);
    }

    function mostrarErro(campo) {
        $(campo).closest('.mb-3').find('.text-danger').addClass('d-block');
    }

    function clientValidateCPF(sender, args) {
        var cpf = args.Value.replace(/[^\d]/g, '');
        args.IsValid = validarCPF(cpf);
    }

    function validarCPF(cpf) {
        if (cpf.length !== 11) return false;
        if (/^(\d)\1{10}$/.test(cpf)) return false;

        var sum = 0;
        for (var i = 0; i < 9; i++) {
            sum += parseInt(cpf.charAt(i)) * (10 - i);
        }
        var remainder = sum % 11;
        var digit1 = remainder < 2 ? 0 : 11 - remainder;

        if (digit1 !== parseInt(cpf.charAt(9))) return false;

        sum = 0;
        for (var i = 0; i < 10; i++) {
            sum += parseInt(cpf.charAt(i)) * (11 - i);
        }
        remainder = sum % 11;
        var digit2 = remainder < 2 ? 0 : 11 - remainder;

        return digit2 === parseInt(cpf.charAt(10));
    }

    function toggleSenha() {
        const senhaField = document.getElementById('<%= txtSenha.ClientID %>');
        const icone = document.getElementById('iconeSenha');
        if (senhaField.type === "password") {
            senhaField.type = "text";
            icone.classList.remove('bi-eye');
            icone.classList.add('bi-eye-slash');
        } else {
            senhaField.type = "password";
            icone.classList.remove('bi-eye-slash');
            icone.classList.add('bi-eye');
        }
    }

</script>