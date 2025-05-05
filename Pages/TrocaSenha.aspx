<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TrocaSenha.aspx.cs" Inherits="Projeto1.TrocaSenha" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Troca de Senha</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/css/bootstrap.min.css" rel="stylesheet"/>
    <style>
        body {
            font-family: 'Segoe UI', sans-serif;
            background-color: #f5f5f5;
            height: 100vh;
            display: flex;
            justify-content: center;
            align-items: center;
        }
        
        .password-container {
            background: white;
            border-radius: 10px;
            padding: 2rem;
            width: 90%;
            max-width: 400px;
            box-shadow: 0 5px 15px rgba(0,0,0,0.1);
        }
        
        .form-control {
            border-radius: 6px;
            padding: 0.7rem;
        }
        
        .btn-submit {
            width: 100%;
            padding: 0.8rem;
            border-radius: 6px;
            font-weight: 500;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server" class="password-container">
        <div class="text-center mb-4">
            <h3><i class="bi bi-shield-lock"></i> Troca de Senha</h3>
            <p class="text-muted">Por segurança, altere sua senha padrão</p>
        </div>
        
        <div class="mb-3">
            <label for="txtNovaSenha" class="form-label">Nova Senha</label>
            <asp:TextBox ID="txtNovaSenha" runat="server" TextMode="Password" CssClass="form-control"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvNovaSenha" runat="server" ControlToValidate="txtNovaSenha"
                ErrorMessage="Campo obrigatório" Display="Dynamic" CssClass="text-danger small"></asp:RequiredFieldValidator>
        </div>
        
        <div class="mb-3">
            <label for="txtConfirmarSenha" class="form-label">Confirmar Nova Senha</label>
            <asp:TextBox ID="txtConfirmarSenha" runat="server" TextMode="Password" CssClass="form-control"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvConfirmarSenha" runat="server" ControlToValidate="txtConfirmarSenha"
                ErrorMessage="Campo obrigatório" Display="Dynamic" CssClass="text-danger small"></asp:RequiredFieldValidator>
            <asp:CompareValidator ID="cvSenhas" runat="server" ControlToValidate="txtConfirmarSenha"
                ControlToCompare="txtNovaSenha" ErrorMessage="As senhas não coincidem" 
                Display="Dynamic" CssClass="text-danger small"></asp:CompareValidator>
        </div>
        
        <asp:Button ID="btnSalvarSenha" runat="server" Text="Salvar Nova Senha" 
            CssClass="btn btn-primary btn-submit" OnClick="btnSalvarSenha_Click" />
        <asp:Label ID="lblMensagem" runat="server" CssClass="text-danger mt-3 d-block"></asp:Label>
    </form>
</body>
</html>