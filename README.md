# BlogApi

Projeto de estudos criado para explorar APIs com o dotnet.

Nesse contexto, trata-se de um blog com alguns endpoints disponíveis.  

Controle de autenticação e autorização JWT através do Microsoft Identity. 

## Endpoints
### Accounts
- POST: v1/accounts => cria a conta para o usuário e envia a senha por e-mail.
- POST: v1/accounts/login => cria um jwt token para um usuário existente.
- POST: v1/accounts/upload-image => atribui uma imagem de avatar para o usuário que enviou.

### Category
- GET: v1/categories => busca todas as categorias existentes
- POST: v1/categories => inclui uma nova categoria
- GET: v1/categories/{id} => busca uma categoria existente por id
- PUT: v1/categories/{id} => edita uma categoria existente
- DELETE: v1/categories/{id} => apaga uma categoria exisente

### Home
- GET: / => healthy check ❤️

### Post:
- GET: v1/posts => busca posts de forma paginada
- GET: v1/posts/{id} => busca um post especifico por id
- POST: v1/posts/create => cria um novo post
- GET: v1/posts/category/{category} => busca posts por categoria


## Como executar

Para executar o projeto basta clonar o repositório e executar um `dotnet run` na raiz do projeto.  

# IMPORTANTE: 
Você DEVE preencher o arquivo `appsettings.json` com as informações do SEU ambiente. Caso contrário, a aplicação não irá executar.

Esse projeto utiliza o envio de emails com a senha para os usuários conseguirem gerar o token de acesso JWT. Portanto, é necessário configurar o envio de email no `appsettings.json` e/ou alterar o código para que exiba a senha na resposta da requisição.

Outras configurações também são obrigatórias para o bom funcionamento, como connection string e jwt key.
