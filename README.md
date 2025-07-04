#### Descrição do Projeto
Este projeto é uma web api desenvolvida para servir como exemplo ou base para o Hackathon.
Utiliza ASP.NET Core para fornecer serviços de autenticação e exemplo de chamadas http básicas.

---

#### Pré-requisitos
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

---

#### Configuração do Projeto
1. Clone o repositório.
2. Abra o projeto utilizando o Visual Studio ou outro editor de sua preferência.

---

#### Instalação e Execução
1. No diretório `BackHackathon.Api` do projeto, execute os seguintes comandos:
   ```
   dotnet build
   dotnet run
   ```
2. O servidor será iniciado localmente e estará acessível em `http://localhost:5035/swagger/index.html` por padrão.

---

#### Estrutura do Projeto
- **Auth**: Contém os serviços relacionados à autenticação.
- **Exemplo**: Contém os serviços de exemplo.
- **Controllers**: Contém os controladores da API.

---

#### Dependências Principais
- **Swagger**: Utilizado para documentação e teste da API.
- **Microsoft.AspNetCore**: Framework principal para construção da aplicação web.
