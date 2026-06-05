# 🧺 Sistema de Gestão de Lavanderia - Clean T-Shirt

## 📋 Contexto do Projeto Original
Este projeto é uma evolução de um protótipo acadêmico desenvolvido anteriormente. O sistema original era um CRUD (Create, Read, Update, Delete) básico, com as seguintes limitações:
- **Persistência Volátil:** Não havia um banco de dados robusto, dependendo de listas em memória ou arquivos simples.
- **Segurança Inexistente:** Não havia controle de acesso ou níveis de permissão (Administrador vs. Usuário).
- **Interface Estática:** UI rudimentar sem responsividade ou foco em UX.
- **Funcionalidades Limitadas:** Apenas o registro básico de clientes, sem gestão de pedidos complexos ou pagamentos.

## 🚀 Melhorias Propostas (Evolução Acadêmica)
Para esta disciplina, foram implementadas as seguintes melhorias significativas:

### 1. Autenticação e Autorização (Sessões e Filtros)
- **Problema:** Qualquer pessoa podia acessar qualquer parte do sistema.
- **Solução:** Implementação de `AuthFilter` e gerenciamento de sessões para distinguir Administradores de Clientes.
- **Justificativa:** Garantir a integridade dos dados e a privacidade das informações dos usuários.

### 2. Gestão de Serviços com Soft Delete
- **Problema:** Excluir um serviço deletava registros históricos de pedidos vinculados.
- **Solução:** Implementação de um campo `Ativo` (bool) para exclusão lógica.
- **Justificativa:** Preservar a integridade referencial e o histórico financeiro da lavanderia.

### 3. Pagamento via PIX (Simulação de Checkout)
- **Problema:** O fluxo de fechamento do pedido era incompleto.
- **Solução:** Geração dinâmica de QR Code e chave "Copia e Cola".
- **Justificativa:** Modernizar o sistema para a realidade do mercado brasileiro (SaaS local).

### 4. Internacionalização (i18n) e Tematização
- **Problema:** Interface rígida e pouco acessível para diferentes contextos.
- **Solução:** Suporte a PT, EN, ES e Dark/Light Mode.
- **Justificativa:** Demonstrar competência técnica em desenvolvimento de software globalizado.

## 🌍 Impacto Social
O **Clean T-Shirt** foi projetado para impactar positivamente a sociedade através de:
1.  **Apoio ao Empreendedorismo Local:** Fornece uma ferramenta profissional gratuita (ou de baixo custo) para que lavanderias de bairro possam competir com grandes redes.
2.  **Redução do Desperdício de Papel:** Digitalização completa do fluxo de pedidos e comprovantes.
3.  **Transparência para o Consumidor:** Permite que o cliente acompanhe o status do seu pedido em tempo real, gerando confiança na relação de serviço.

## 📅 Cronograma de Implementação (3 Meses)
- **Mês 1: Planejamento e Arquitetura**
    - Análise do código antigo e definição da nova arquitetura MVC.
    - Modelagem do banco de dados relacional (SQLite/PostgreSQL).
- **Mês 2: Desenvolvimento do Core**
    - Implementação do sistema de login e filtros de segurança.
    - Desenvolvimento do CRUD de Serviços e Pedidos Dinâmicos.
- **Mês 3: Refinamento e Deploy**
    - Implementação de i18n e Dark Mode.
    - Testes de integração e deploy no Render.

---

## ✨ Funcionalidades Atuais

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

👉 Acesse: [https://sistema-lavanderia-flov.onrender.com/](https://sistema-lavanderia-flov.onrender.com/)

---

## ⚙️ Como executar localmente

```bash
git clone https://github.com/wfmacena/sistema_lavanderia
cd sistema_lavanderia
dotnet restore
dotnet run
```


