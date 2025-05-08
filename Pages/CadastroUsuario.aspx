<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CadastroUsuario.aspx.cs" Inherits="Projeto1.CadastroUsuario" %>

<form id="form1" runat="server"> 
    <div class="modal-body-content">
        <asp:HiddenField ID="hfID" runat="server" Value="0" />
    
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
                    OnServerValidate="ValidarCPFExistente"
                    ErrorMessage="CPF já cadastrado"
                    Display="Dynamic"
                    CssClass="text-danger small"
                    ValidateEmptyText="true"
                    ValidationGroup="vgCadastro" 
                    ClientValidationFunction="ValidarCPFExistente" />
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
                <asp:TextBox ID="txtUnidade" runat="server" CssClass="form-control"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvUnidade" runat="server" ControlToValidate="txtUnidade"
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
                <button class="btn btn-outline-secondary" type="button" id="btnGerarSenha" onclick="usarMatriculaComoSenha()">
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
                OnClientClick="if (!Page_ClientValidate('vgCadastro')) return false;" 
                UseSubmitBehavior="false" />
            <asp:Label ID="lblDebug" runat="server" CssClass="text-info small" />
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
        const cpfRaw = $('#<%= txtCPF.ClientID %>').val();
        const cpf = cpfRaw.replace(/\D/g, '');

        if (cpf.length !== 11) { args.IsValid = false; return; }
        if (/^(\d)\1{10}$/.test(cpf)) { args.IsValid = false; return; }
        const calcDigit = (nums, mults) => {
            let sum = 0;
            for (let i = 0; i < mults.length; i++) {
                sum += parseInt(nums[i]) * mults[i];
            }
            let resto = sum % 11;
            return resto < 2 ? 0 : 11 - resto;
        };

        const mult1 = [10, 9, 8, 7, 6, 5, 4, 3, 2];
        const mult2 = [11, 10, 9, 8, 7, 6, 5, 4, 3, 2];

        const dig1 = calcDigit(cpf, mult1);
        const dig2 = calcDigit(cpf + dig1, mult2);

        args.IsValid = (cpf.endsWith(`${dig1}${dig2}`));
    }


    function validarAntesDeSalvar() {
        Page_ClientValidate('vgCadastro');
        if (!Page_IsValid) {
            $('.text-danger').show(); 
            return false;
        }
        return confirm('Deseja realmente salvar os dados?');
    }
</script>
