<%@ Page Title="Início" Language="C#" MasterPageFile="~/MP.Master" AutoEventWireup="true" 
    CodeBehind="Inicio.aspx.cs" Inherits="Projeto1.Inicio" %>
<%@ Register TagPrefix="uc" Namespace="Projeto1.Controls" Assembly="Projeto1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
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
                    
                    <asp:LinkButton ID="btnAbrirModalNovaUnidade" runat="server" CssClass="btn btn-info ms-2" OnClick="btnAbrirModalNovaUnidade_Click">
                        <i class="bi bi-building-add"></i> Cadastrar Unidade
                    </asp:LinkButton>
                    <asp:LinkButton ID="btnGerenciarUnidades" runat="server" CssClass="btn btn-secondary ms-2" OnClick="btnGerenciarUnidades_Click">
                        <i class="bi bi-pencil-square"></i> Gerenciar Unidades
                    </asp:LinkButton>


                    <div class="table-responsive mt-3">
                        <asp:GridView ID="gvUsuarios" runat="server" AutoGenerateColumns="False"
                            CssClass="table table-striped table-hover" DataKeyNames="ID"
                            OnRowCommand="gvUsuarios_RowCommand" EmptyDataText="Nenhum usuário cadastrado.">
                            <Columns>
                                <asp:BoundField DataField="ID" HeaderText="ID" />
                                <asp:BoundField DataField="NomeCompleto" HeaderText="Nome Completo" />
                                <asp:BoundField DataField="CPF" HeaderText="CPF" />
                                <asp:BoundField DataField="NomeUnidade" HeaderText="Unidade" />
                                <asp:BoundField DataField="Perfil" HeaderText="Perfil" />
                                <asp:BoundField DataField="DataCadastro" HeaderText="Cadastrado em" 
                                    DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                                <asp:TemplateField HeaderText="Ações">
                                    <ItemTemplate>
                                        <asp:LinkButton
                                            runat="server"
                                            CommandName="Editar"
                                            CommandArgument='<%# Eval("ID") %>'
                                            CssClass="btn btn-sm btn-outline-primary"
                                            ToolTip="Editar">
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
    
    <div class="modal fade" id="modalCadastro" tabindex="-1" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <asp:UpdatePanel ID="updCadastro" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
                    <ContentTemplate>
                        <div class="modal-header">
                            <h5 class="modal-title">
                                <asp:Literal ID="litModalTitle" runat="server" Text="Novo Usuário" />
                            </h5>
                            <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                        </div>
                        <div class="modal-body">
                            <asp:Label ID="lblModalErrorMessage" runat="server" 
                                CssClass="text-danger small mb-2 d-block" 
                                Visible="false" EnableViewState="true"></asp:Label>
                            <asp:PlaceHolder ID="phCadastroUsuario" runat="server"></asp:PlaceHolder>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>

     <div class="modal fade" id="modalUnidade" tabindex="-1" aria-labelledby="modalUnidadeLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <asp:UpdatePanel ID="updUnidade" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
                    <ContentTemplate>
                        <div class="modal-header">
                            <h5 class="modal-title" id="modalUnidadeLabel">
                                <asp:Literal ID="litModalUnidadeTitle" runat="server" Text="Nova Unidade" />
                            </h5>
                            <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                        </div>
                        <div class="modal-body">
                            <asp:PlaceHolder ID="phCadastroUnidade" runat="server"></asp:PlaceHolder>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>

    <div class="modal fade" id="modalGerenciarUnidades" tabindex="-1" aria-labelledby="modalGerenciarUnidadesLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                 <asp:UpdatePanel ID="updGerenciarUnidades" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="modal-header">
                            <h5 class="modal-title" id="modalGerenciarUnidadesLabel">Gerenciar Unidades</h5>
                            <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                        </div>
                        <div class="modal-body">
                            <asp:GridView ID="gvUnidades" runat="server" AutoGenerateColumns="False"
                                CssClass="table table-striped table-hover" DataKeyNames="UnidadeID"
                                OnRowCommand="gvUnidades_RowCommand" EmptyDataText="Nenhuma unidade cadastrada.">
                                <Columns>
                                    <asp:BoundField DataField="UnidadeID" HeaderText="ID" />
                                    <asp:BoundField DataField="Nome" HeaderText="Nome da Unidade" />
                                    <asp:CheckBoxField DataField="Ativo" HeaderText="Ativa" ReadOnly="true" />
                                    <asp:TemplateField HeaderText="Ações">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btnEditarUnidadeGrid" runat="server" CommandName="EditarUnidade" 
                                                CommandArgument='<%# Eval("UnidadeID") %>' CssClass="btn btn-sm btn-outline-primary" ToolTip="Editar">
                                                <i class="bi bi-pencil"></i>
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="btnExcluirUnidadeGrid" runat="server" CommandName="ExcluirUnidade" 
                                                CommandArgument='<%# Eval("UnidadeID") %>' CssClass="btn btn-sm btn-outline-danger ms-1" ToolTip="Excluir/Inativar"
                                                OnClientClick="return confirm('Tem certeza que deseja excluir/inativar esta unidade?');">
                                                <i class="bi bi-trash"></i>
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Fechar</button>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">
    <script type="module" src="../Scripts/Scripts.js" defer></script>
    <script type="module">
        const errorLabel = document.getElementById('<%= lblModalErrorMessage.ClientID %>');
    </script>
</asp:Content>
