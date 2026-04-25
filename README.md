# 🧺 Sistema de Gestão de Lavanderia

Sistema web desenvolvido em **ASP.NET Core MVC + Entity Framework + SQLite** para gerenciamento de pedidos de lavanderia.

Projeto acadêmico desenvolvido para a disciplina de Desenvolvimento de Software.

---

## 🚀 Funcionalidades

### 👤 Usuário
- Cadastro com CPF
- Login e autenticação por sessão
- Solicitação de lavagem
- Acompanhamento de pedidos
- Atualização de perfil (telefone / email)

### 👑 Administrador
- Cadastro de clientes
- Controle total de pedidos
- Alteração de status:
  - Recebido
  - Em Lavagem
  - Pronto
  - Entregue
- Exclusão e edição de pedidos

### 🧺 Pedidos
- Registro de tipo de lavagem
- Quantidade
- Valor
- Datas
- Itens detalhados por peça

---

## 🏗️ Tecnologias utilizadas

- ASP.NET Core MVC (.NET 8)
- Entity Framework Core
- SQLite
- Bootstrap 5
- Docker
- Render (Deploy)
- CronJob.org (manter app ativo)

---

## 🌐 Sistema online

👉 Acesse:

https://sistema-lavanderia.onrender.com

---

## ⚙️ Como executar localmente

```bash
git clone https://github.com/wfmacena/sistema_lavanderia
cd sistema_lavanderia
dotnet restore
dotnet ef database update
dotnet run


