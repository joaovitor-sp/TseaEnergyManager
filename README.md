# TSEA Energy Manager ⚡

Sistema de gerenciamento de equipamentos construído com uma arquitetura distribuída em **.NET 8**. 
O projeto demonstra a centralização de regras de negócio consumidas simultaneamente por clientes de diferentes plataformas (Web e Desktop nativo).

---

## 🏛️ Arquitetura

O sistema adota os princípios de Clean Architecture, separando responsabilidades em 4 camadas principais:

- **`Tsea.Domain`:** Core do sistema. Contém os modelos de domínio e regras de negócio puras, sendo compartilhado entre o servidor e os clientes.
- **`Tsea.Api`:** Backend construído com **ASP.NET Core Minimal APIs**. Gerencia a comunicação com o banco de dados via **Entity Framework Core (Unit of Work)**.
- **`Tsea.Desktop`:** Cliente corporativo nativo em **WPF (Windows Presentation Foundation)**. Construído inteiramente sobre o padrão **MVVM** com *CommunityToolkit.Mvvm*.
- **`Tsea.Web`:** Dashboard gerencial em **Blazor WebAssembly**. Interface SPA fortemente tipada rodando em C# nativamente no navegador, sem dependência de JavaScript.

---

## 🛠️ Tech Stack

- **Plataforma:** .NET 8.0
- **Linguagem:** C# 12
- **Banco de Dados:** PostgreSQL 
- **ORM:** Entity Framework Core
- **Design Patterns:** MVVM, Dependency Injection, Repository/UoW (via EF)

---

## 🚀 Como Executar

### Pré-requisitos
- Visual Studio 2022
- Docker Desktop

### 1. Banco de Dados
Na raiz do projeto, suba o contêiner do PostgreSQL (`docker-compose.yml` já configurado para a porta 5433):
```bash
docker-compose up -d
```

### 2. Inicialização
No Visual Studio, o ecossistema precisa rodar simultaneamente:
1. Clique com o botão direito na **Solução** > **Propriedades**.
2. Vá em **Vários Projetos de Inicialização**.
3. Defina a ação **Iniciar** para os seguintes projetos:
   - `Tsea.Api`
   - `Tsea.Desktop`
   - `Tsea.Web`

Pressione `F5`. O Visual Studio abrirá automaticamente a API, a interface Web no navegador e o aplicativo Desktop. As alterações feitas em qualquer um dos clientes refletem globalmente no banco de dados.

## Testes

Os testes da API ficam no projeto `Tsea.Api.Tests` e são separados em:

- **Unitários:** verificam regras simples do domínio e a configuração do modelo do Entity Framework Core.
- **Integração:** iniciam a API real em memória, exercitam os endpoints CRUD por HTTP e usam um SQLite em memória isolado do PostgreSQL local.

Para executá-los:

```bash
dotnet test
```
