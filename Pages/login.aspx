<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="Projeto1.login" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Login - Sistema</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/css/bootstrap.min.css" rel="stylesheet"/>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.8.1/font/bootstrap-icons.css"/>
    <link rel="stylesheet" href="../CSS/login.css" />
</head>
<body>
    <form id="form1" runat="server" class="login-container">
         <asp:ScriptManager runat="server"></asp:ScriptManager>
        <div class="text-center mb-4">
            <h2><i class="bi bi-person-circle"></i> Acesso ao Sistema</h2>
        </div>
        
        <div class="mb-3">
            <label for="txtCPF" class="form-label">CPF</label>
            <asp:TextBox ID="txtCPF" runat="server" CssClass="form-control" placeholder="Digite seu CPF" ClientIDMode="Static"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvUsuario" runat="server" ControlToValidate="txtCPF" 
                ErrorMessage="CPF é obrigatório." Display="Dynamic" CssClass="text-danger small"></asp:RequiredFieldValidator>
            <asp:CustomValidator ID="cvCPF" runat="server" ControlToValidate="txtCPF"
                OnServerValidate="ValidateCPF" ErrorMessage="CPF inválido" Display="Dynamic" 
                CssClass="text-danger small"></asp:CustomValidator>
        </div>
        
        <div class="mb-3">
            <label for="txtSenha" class="form-label">Senha</label>
            <asp:TextBox ID="txtSenha" runat="server" TextMode="Password" CssClass="form-control" placeholder="Digite sua senha"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvSenha" runat="server" ControlToValidate="txtSenha" 
                ErrorMessage="Senha é obrigatória." Display="Dynamic" CssClass="text-danger small"></asp:RequiredFieldValidator>
        </div>
        
        <asp:Button ID="btnLogin" runat="server" Text="Entrar" CssClass="btn btn-primary btn-login" OnClick="btnLogin_Click" />
        <asp:Label ID="lblMensagemErro" runat="server" Text="" CssClass="error-message d-block text-center mt-3"></asp:Label>
        
    </form>

    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery.inputmask/5.0.6/jquery.inputmask.min.js"></script>
    <script type="text/javascript">
            $(document).ready(function () {
                $('#txtCPF').inputmask('999.999.999-99', {
                    placeholder: '___.___.___-__',
                    clearIncomplete: true,
                    showMaskOnHover: false
                });
            });
    </script>
</body>
</html>