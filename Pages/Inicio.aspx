<%@ Page Title="Início" Language="C#" MasterPageFile="~/MP.Master" AutoEventWireup="true" 
    CodeBehind="Inicio.aspx.cs" Inherits="Projeto1.Inicio" %>
<%@ Register TagPrefix="uc" Namespace="Projeto1.Controls" Assembly="Projeto1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .card {
            border-radius: 10px;
            box-shadow: 0 4px 8px rgba(0,0,0,0.1);
            margin-bottom: 20px;
        }
        
        .card-header {
            border-radius: 10px 10px 0 0 !important;
        }
        
        .badge {
            font-size: 0.9em;
            padding: 5px 10px;
        }
        
        .user-info p {
            margin-bottom: 0.8rem;
        }
        
        .user-info strong {
            display: inline-block;
            width: 120px;
        }
        
        .table-responsive {
            overflow-x: auto;
        }
        
        .table th {
            background-color: #f8f9fa;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-4">
        <asp:Panel ID="pnlAdmin" runat="server" Visible="false">
            <div class="card">
                <div class="card-header bg-primary text-white">
                    <h4 class="mb-0"><i class="bi bi-people-fill"></i> Gerenciamento de Usuários</h4>
                </div>
                <div class="card-body">
                    <asp:LinkButton ID="btnAbrirModalNovoAsp" runat="server" CssClass="btn btn-success" OnClick="btnAbrirModalNovoUsuario_Click">
                        <i class="bi bi-plus-circle"></i> Novo Usuário
                    </asp:LinkButton>
                    
                    <div class="table-responsive">
                        <asp:GridView ID="gvUsuarios" runat="server" AutoGenerateColumns="False"
                            CssClass="table table-striped table-hover" DataKeyNames="ID"
                            OnRowCommand="gvUsuarios_RowCommand" EmptyDataText="Nenhum usuário cadastrado.">
                            <Columns>
                                <asp:BoundField DataField="ID" HeaderText="ID" />
                                <asp:BoundField DataField="NomeCompleto" HeaderText="Nome Completo" />
                                <asp:BoundField DataField="CPF" HeaderText="CPF" />
                                <asp:BoundField DataField="Unidade" HeaderText="Unidade" />
                                <asp:BoundField DataField="Perfil" HeaderText="Perfil" />
                                <asp:BoundField DataField="DataCadastro" HeaderText="Cadastrado em" 
                                    DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                                <asp:TemplateField HeaderText="Ações">
                                    <ItemTemplate>
                                          <asp:LinkButton runat="server" CommandName="Editar" CommandArgument='<%# Eval("ID") %>'
                                            CssClass="btn btn-sm btn-outline-primary"
                                            ToolTip="Editar"
                                            OnClientClick='<%# $"dispararEdicaoPostBack({Eval("ID")}); return false;" %>'>
                                            <i class="bi bi-pencil"></i> Editar
                                        </asp:LinkButton>
                                        <asp:LinkButton runat="server" CommandName="Excluir" CommandArgument='<%# Eval("ID") %>'
                                            CssClass="btn btn-sm btn-outline-danger ms-1" ToolTip="Excluir"
                                            OnClientClick="return confirm('Tem certeza que deseja excluir este usuário?');">
                                            <i class="bi bi-trash"></i>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </asp:Panel>
        
        <asp:Panel ID="pnlColaborador" runat="server" Visible="false">
            <div class="row">
                <div class="col-md-8">
                    <div class="card">
                        <div class="card-header bg-info text-white">
                            <h4 class="mb-0"><i class="bi bi-person-badge"></i> Meus Dados</h4>
                        </div>
                        <div class="card-body">
                            <div class="user-info">
                                <p><strong>Nome Completo:</strong> <asp:Label ID="lblNome" runat="server" /></p>
                                <p><strong>CPF:</strong> <asp:Label ID="lblCPF" runat="server" /></p>
                                <p><strong>Matrícula:</strong> <asp:Label ID="lblMatricula" runat="server" /></p>
                                <p><strong>Unidade:</strong> <asp:Label ID="lblUnidade" runat="server" /></p>
                                <p><strong>Telefone:</strong> <asp:Label ID="lblTelefone" runat="server" /></p>
                                <p><strong>Perfil:</strong> <span class="badge bg-primary"><asp:Label ID="lblPerfil" runat="server" /></span></p>
                                <p><strong>Data de Cadastro:</strong> <asp:Label ID="lblDataCadastro" runat="server" /></p>
                            </div>
                        </div>
                    </div>
                </div>
                
                <div class="col-md-4">
                    <div class="card">
                        <div class="card-header bg-warning text-dark">
                            <h4 class="mb-0"><i class="bi bi-shield-lock"></i> Alterar Senha</h4>
                        </div>
                        <div class="card-body">
                            <div class="mb-3">
                                <label for="txtSenhaAtual" class="form-label">Senha Atual</label>
                                <asp:TextBox ID="txtSenhaAtual" runat="server" TextMode="Password" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="mb-3">
                                <label for="txtNovaSenha" class="form-label">Nova Senha</label>
                                <asp:TextBox ID="txtNovaSenha" runat="server" TextMode="Password" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="mb-3">
                                <label for="txtConfirmarSenha" class="form-label">Confirmar Nova Senha</label>
                                <asp:TextBox ID="txtConfirmarSenha" runat="server" TextMode="Password" CssClass="form-control"></asp:TextBox>
                            </div>
                            <asp:Button ID="btnAlterarSenha" runat="server" Text="Alterar Senha" 
                                CssClass="btn btn-warning" OnClick="btnAlterarSenha_Click" />
                            <asp:Label ID="lblMensagemSenha" runat="server" CssClass="text-danger mt-2 d-block"></asp:Label>
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>
    </div>
    
    <!-- Modal para Cadastro/Edição -->
    <div class="modal fade" id="modalCadastro" tabindex="-1" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <asp:UpdatePanel ID="updCadastro" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="modal-header">
                            <h5 class="modal-title">
                                <asp:Literal ID="litModalTitle" runat="server" Text="Novo Usuário" />
                            </h5>
                            <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
                        </div>
                        <div class="modal-body">
                            <asp:PlaceHolder ID="phCadastroUsuario" runat="server"></asp:PlaceHolder>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">
<script type="text/javascript">
    function abrirModal() {
        $('#modalCadastro').modal('show');
    }

    function dispararEdicaoPostBack(id) {
        __doPostBack('<%= updCadastro.UniqueID %>', 'Editar_' + id);
    }

    function applyMasks() {
        if (typeof $.fn.inputmask === 'function') {
           $('.cpf-mask').inputmask('999.999.999-99');
           $('.phone-mask').inputmask('(99) [9]9999-9999');
        }
    }

    function applyValidators() {
        if (typeof (Page_Validators) !== 'undefined') {
            for (var i = 0; i < Page_Validators.length; i++) {
                ValidatorValidate(Page_Validators[i]);
            }
        }
    }

    $(document).ready(function () {
        $('#modalCadastro').on('shown.bs.modal', function () {
            applyMasks();
            applyValidators();
        });
        if ($('#modalCadastro').is(':visible')) {
            applyMasks();
            applyValidators();
        }
    });

    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function (sender, args) {
        applyMasks(); 
        applyValidators(); 
    });

</script>
</asp:Content>
