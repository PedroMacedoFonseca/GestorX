# Gestor X

**Sistema genérico de cadastro e gestão de pessoas com autenticação segura, controle por perfil e unidades de atuação.**

## 🧩 Visão Geral

O **Gestor X** é um sistema web desenvolvido com ASP.NET Web Forms, voltado para o cadastro e gerenciamento de pessoas em diferentes tipos de organização. Ideal para projetos que exigem controle de acesso, segmentação por perfil e associação com unidades ou setores.

## 🔐 Segurança

- As senhas são criptografadas utilizando o algoritmo **SHA256**.
- A autenticação é feita através de **CPF + Senha**.
- O **CPF** é utilizado como **chave primária** no banco de dados, garantindo unicidade e segurança na identificação dos usuários.

## 🗂️ Estrutura de Cadastro

O sistema possui dois cadastros principais:

1. **Usuário**  
   - Perfis disponíveis: **Administrador**, **Colaborador** e **Terceirizado**  
   - Associados a uma unidade de atuação

2. **Unidade de Atuação**  
   - Representa setores, departamentos ou locais físicos da organização  
   - Pode ser facilmente adaptado para diferentes contextos

> Embora a presença de unidades possa parecer específica, o sistema foi estruturado de forma genérica e adaptável a diversos tipos de instituição.

## 💾 Banco de Dados

- O sistema utiliza um banco de dados local com extensão **`.mdf`**, compatível com **Microsoft SQL Server**.
- Ideal para ambientes de teste e demonstração.
- Estrutura simples, com fácil adaptação para servidores ou bancos externos.

## ⚙️ Tecnologias Utilizadas

- ASP.NET Web Forms (C#)
- ADO.NET para acesso direto ao banco (sem uso de ORMs)
- Bootstrap 5 + Bootstrap Icons
- SQL Server (.mdf)

## 🚀 Como Executar o Projeto Localmente

1. Clone o repositório:
   ```bash
   git clone https://github.com/seuusuario/CorpGestor.git
2. Abra o projeto no Visual Studio (versão 2019 ou superior).
3. Restaure o banco de dados .mdf na instância local do SQL Server Express ou conecte a um servidor de sua preferência.
4. Execute o projeto


## Autor

Desenvolvido por Pedro, estudante de Análise e Desenvolvimento de Sistemas, com foco em Back-End, C#, ASP.NET e aplicações web seguras e escaláveis.
