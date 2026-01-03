# FilmesAPI 🎬

![Status](https://img.shields.io/badge/Status-Em%20Desenvolvimento-yellow)

API REST desenvolvida em C# com .NET Core para gerenciamento de um catálogo de filmes. O projeto utiliza Entity Framework Core para persistência de dados.

## 🔨 Funcionalidades Atuais

- **CRUD Completo:**
  - `POST /filme`: Cadastrar novos filmes.
  - `GET /filme`: Listar todos os filmes.
  - `GET /filme/{id}`: Detalhes de um filme específico.
  - `PUT /filme/{id}`: Atualizar dados de um filme.
  - `DELETE /filme/{id}`: Remover um filme do catálogo.
- **Tratamento de Erros:** Retornos HTTP adequados (200, 201, 204, 404, 500).

## 🛠️ Tecnologias Utilizadas

- C#
- .NET 6+ (ASP.NET Core)
- Entity Framework Core
- Banco de Dados (MySQL)
- Swagger (para documentação e testes)

## 🚀 Próximos Passos (Roadmap)

Este projeto está em evolução constante. As próximas melhorias planejadas são:

- [ ] **DTOs (Data Transfer Objects):** Implementar DTOs para separar a camada de domínio da camada de apresentação, garantindo mais segurança e controle sobre os dados recebidos e enviados.
- [ ] **Async/Await:** Refatorar os métodos do Controller para utilizar programação assíncrona (`ToListAsync`, `SaveChangesAsync`), melhorando a performance e escalabilidade da API.
- [ ] **Filtros de Busca:** Implementar buscas específicas (ex: por nome, por ano, etc).
- [ ] **Paginação:** Implementar `Skip` e `Take` para lidar com grandes volumes de dados.

## 📝 Como rodar

1. Clone o repositório.
2. Configure a string de conexão no `appsettings.json`.
3. Execute as migrations: `dotnet ef database update`.
4. Rode o projeto: `dotnet run`.

## 👩🏻‍💻 Desenvolvido por 

[**Graciane**](mailto:graciane.dev@gmail.com)

