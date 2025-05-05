<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="Projeto1.login" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Login - Sistema</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/css/bootstrap.min.css" rel="stylesheet"/>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.8.1/font/bootstrap-icons.css"/>
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery.inputmask/5.0.7-beta.19/jquery.inputmask.min.js"></script>
    <style>
        body {
            font-family: 'Segoe UI', sans-serif;
            background-color: #f5f5f5;
            background-image: url('../Images/bg.png');
            background-size: cover;
            background-position: center;
            height: 100vh;
            display: flex;
            justify-content: center;
            align-items: center;
        }
        
        .login-container {
            background: rgba(255, 255, 255, 0.95);
            border-radius: 15px;
            padding: 2.5rem;
            width: 90%;
            max-width: 400px;
            box-shadow: 0 10px 30px rgba(0, 0, 0, 0.2);
        }
        
        .form-control {
            border-radius: 6px;
            padding: 0.7rem;
        }
        
        .btn-login {
            width: 100%;
            padding: 0.8rem;
            border-radius: 6px;
            font-weight: 500;
        }
        
        .error-message {
            color: #dc3545;
            margin-top: 1rem;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server" class="login-container">
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
        
        <script type="text/javascript">
            $(document).ready(function () {
                $('#txtCPF').inputmask('999.999.999-99', {
                    placeholder: '___.___.___-__',
                    clearIncomplete: true,
                    showMaskOnHover: false
                });
            });
        </script>
    </form>
</body>
</html>