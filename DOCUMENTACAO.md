# DOCUMENTAÇÃO TÉCNICA E ACADÊMICA: SISTEMA DE GESTÃO PARA LAVANDERIA (SaaS)

**EQUIPE DE DESENVOLVIMENTO:**
*   **William Ferreira Macena** - RA: 2223104409
*   **Felipe Henrique Fernandes dos Santos** - RA: 2223101192
*   **Marcelo Vitor Duarte Ramos de Jesus** - RA: 2223100385
*   **Ricardo Aparecido Toledo** - RA: 2223100444

**Resumo Acadêmico:**
Este documento apresenta a especificação técnica, arquitetural e funcional do Sistema de Gestão para Lavanderia desenvolvido em ASP.NET Core 8. O projeto evoluiu de um CRUD acadêmico para uma plataforma profissional com suporte a múltiplos idiomas, persistência em nuvem (PostgreSQL) e interface moderna responsiva.

---

## 1. INTRODUÇÃO
O presente projeto visa solucionar a carência de sistemas simplificados e eficientes para o gerenciamento de pequenas e médias lavanderias. Através de uma arquitetura robusta e escalável, o sistema permite o controle de clientes, serviços e pedidos com foco em usabilidade e segurança.

## 2. TECNOLOGIAS UTILIZADAS
A pilha tecnológica foi selecionada com base em critérios de performance, suporte da comunidade e facilidade de deploy em ambientes de nuvem (PaaS):

*   **Linguagem:** C# (C-Sharp)
*   **Framework:** ASP.NET Core MVC 8.0
*   **Banco de Dados:**
    *   *Desenvolvimento:* SQLite (Local)
    *   *Produção:* PostgreSQL (Render)
*   **ORM:** Entity Framework Core (EF Core)
*   **Frontend:** Bootstrap 5, JavaScript Vanilla, CSS3 (Custom Variables)
*   **Hospedagem:** Render (Web Service + PostgreSQL Managed)

## 3. ARQUITETURA DO SISTEMA
O sistema segue o padrão de projeto **MVC (Model-View-Controller)**, garantindo a separação de responsabilidades:

*   **Models:** Representam a lógica de negócio e as entidades do banco de dados (ex: `Cliente`, `Pedido`, `Usuario`).
*   **Views:** Interfaces de usuário desenvolvidas com Razor Pages para renderização dinâmica no servidor.
*   **Controllers:** Responsáveis pela intermediação entre a interface e o banco de dados, tratando as requisições HTTP.

## 4. FUNCIONALIDADES PRINCIPAIS
1.  **Gestão de Clientes:** Cadastro completo com validação de CPF e proteção contra exclusão de registros com vínculos ativos.
2.  **Sistema de Pedidos Dinâmicos:** Interface moderna que permite a adição de múltiplas peças de roupa em um único pedido, com cálculo automático de subtotal.
3.  **Checkout e Pagamentos:** Simulação de pagamento via PIX com geração de QR Code e chave "Copia e Cola".
4.  **Internacionalização (i18n):** Suporte nativo para Português (PT), Inglês (EN) e Espanhol (ES) via atributos `data-i18n`.
5.  **Tematização:** Alternador de Tema Claro/Escuro (Light/Dark Mode) persistente via variáveis CSS.

## 5. PERSISTÊNCIA E SEGURANÇA
*   **Hibridismo de Dados:** O sistema utiliza uma lógica de detecção de ambiente para alternar entre SQLite e PostgreSQL, garantindo que os dados não sejam perdidos após o deploy no Render (devido ao sistema de arquivos efêmero).
*   **Autenticação:** Filtro de autenticação customizado (`AuthFilter`) que gerencia o acesso às rotas baseado na sessão do usuário.

## 6. VERSIONAMENTO E COLABORAÇÃO
O ciclo de vida do desenvolvimento de software (SDLC) deste projeto foi gerenciado através de sistemas de controle de versão distribuídos:

*   **Git & GitHub:** Utilizados para o rastreamento de alterações, controle de histórico e colaboração entre os integrantes da equipe.
*   **GitHub Desktop:** Empregado como a interface gráfica principal para facilitar as operações de *commit*, *push* e *pull*, garantindo a integridade do código fonte e a sincronização contínua com o repositório remoto.
*   **Gestão de Ambientes:** Uso estratégico do arquivo `.gitignore` para separar configurações de ambiente local (SQLite) das configurações de produção, otimizando o fluxo de deploy contínuo (CI/CD).

## 7. QUALIDADE E BOAS PRÁTICAS
O projeto aplica princípios de **Clean Code** e **SOLID**, buscando um código legível e de fácil manutenção. Foram utilizados:
*   **XML Documentation:** Comentários estruturados em métodos críticos para facilitar a integração de novos desenvolvedores.
*   **Validações Robustas:** Uso de `DataAnnotations` nos Models para garantir a integridade dos dados na entrada (frontend e backend).
*   **Segurança de Rotas:** Filtros de autorização customizados que impedem o acesso a áreas administrativas sem as credenciais adequadas.

## 8. REFERÊNCIAS BIBLIOGRÁFICAS
Para o embasamento teórico deste projeto, foram consultadas as seguintes obras:
1.  **SOMMERVILLE, Ian.** *Engenharia de Software*. 10ª ed. Pearson, 2018. (Base para gestão de requisitos e ciclo de vida).
2.  **MARTIN, Robert C.** *Código Limpo: Habilidades Práticas do Agile Software*. Alta Books, 2009. (Diretrizes para escrita de código profissional).
3.  **FOWLER, Martin.** *Padrões de Arquitetura de Aplicações Corporativas*. Bookman, 2006. (Fundamentação do padrão MVC).

---
**Data de Emissão:** 05 de Junho de 2026