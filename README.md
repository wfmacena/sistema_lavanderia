# 🧺 Sistema de Gestão de Lavanderia - Clean T-Shirt

## 📝 Descrição do Projeto

O projeto consiste na evolução do sistema desenvolvido no semestre anterior, tratando-se de um site de serviço de lavanderia literalmente básico "CRUD". Além disso, está sendo implementado um sistema de banco de dados em SQLite, e autenticação com login, contemplando diferentes níveis de acesso, incluindo painel do usuário e painel administrativo.

Sistema web desenvolvido em **ASP.NET Core MVC + Entity Framework + SQLite** para gerenciamento de pedidos de lavanderia.

---

## ✨ Melhorias Aplicadas

- **Autenticação e Autorização:** Implementação de sistema de login com diferentes níveis de acesso (Administrador e Usuário Comum).
- **Gestão de Serviços (CRUD Completo):** Implementação total da gestão de serviços, permitindo que o administrador cadastre, edite e desative (soft delete) os serviços oferecidos pela lavanderia.
- **Integração com WhatsApp:** Inclusão de botão flutuante e atalhos de suporte via WhatsApp para comunicação direta com os clientes.
- **Pagamento via PIX:** Sistema de checkout simulado com geração de QR Code e chave "Copia e Cola" para maior praticidade do cliente.
- **Banco de Dados Relacional:** Migração para SQLite com Entity Framework Core, garantindo persistência e integridade dos dados.
- **Interface Moderna:** UI aprimorada utilizando Bootstrap 5 e ícones do Bootstrap Icons para uma melhor experiência do usuário.

---

## 🚀 Funcionalidades

### 👤 Usuário
- Cadastro com CPF
- Login e autenticação por sessão
- Solicitação de lavagem
- Acompanhamento de pedidos
- **Pagamento via PIX:** Geração de QR Code dinâmico para pedidos pendentes.
- Atualização de perfil (telefone / email)

### 👑 Administrador
- **Gestão de Serviços:** Controle total sobre os tipos de serviços, preços e unidades de medida.
- **Gestão de Clientes:** Cadastro e listagem de clientes vinculados aos usuários.
- **Controle de Pedidos:** Alteração de status (Recebido, Em Lavagem, Pronto, Entregue) e edição detalhada.

### 🧺 Pedidos
- Registro de tipo de lavagem
- Quantidade
- Valor calculado
- Datas de entrada e previsão de entrega
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

👉 Acesse: [https://sistema-lavanderia.onrender.com](https://sistema-lavanderia.onrender.com)

---

## ⚙️ Como executar localmente

```bash
git clone https://github.com/wfmacena/sistema_lavanderia
cd sistema_lavanderia
dotnet restore
dotnet run
```


