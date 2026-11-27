# Modelo API Backend .Net 9.0 - 2025

### AUTOR
<ins>WEIDERSON MENDES QUEIROZ - ANALISTA DESENVOLVEDOR .NET</ins>

+ LINKDIN
www.linkedin.com/in/weiderson

+ EMAIL
weidersn@protonmail.com

### RECURSOS
+ AutoMapper
+ DTOs
+ Repository
+ UnitOfWork
+ Autenticação e Autorização (JWT)
+ Versionamento

### VERSÕES
+ .NET 9.0
+ ENTITY FRAMEWORK CORE 9.04
+ MYSQL Server version 8.0.42
+ Visual Studio 2022

### CONFIGURAÇÃO DO BANCO DE DADOS
Por padrão, a aplicação está configurada em `Program.cs` para usar o banco InMemory (`desafiobackend`). Os dados são temporários e não são gravados no MySQL.

Para conectar ao MySQL:
1. Configure a connection string `ConnectionStrings:DefaultConnection`, preferencialmente pela variável de ambiente `ConnectionStrings__DefaultConnection`. Exemplo para PowerShell:
   `$env:ConnectionStrings__DefaultConnection = "server=localhost;database=desafiobackend2;user id=SEU_USUARIO;password=SUA_SENHA"`
2. Em `Program.cs`, substitua o registro InMemory por `builder.Services.AddDbContext<Contexto>();`. O `Contexto` configura o provedor MySQL usando a connection string.
3. Execute as migrations abaixo para criar/atualizar o banco.

Não coloque senhas reais no README nem faça commit delas no repositório.

### COMANDO PARA EF MIGRATIONS
+ dotnet ef migrations add NewMigration 
+ dotnet ef database update --project ModeloBackEndAPI.csproj
+ Para popular as tabelas do banco descomente o código referente a "app.Services.CreateScope()" no Program.cs, e rode aplicação.
+ Em seguida, comente o código novamente. Caso prefira, rode os arquivos .sql para popular o banco.

### CONTA
+ Será possível excluir a CONTA caso o registro não possua saldo.
+ Será possível excluir CONTA caso o registro esteja ATIVO.
+ Será possível editar a CONTA caso o registro esteja coma situação ATIVA.
+ O download do documento relacionado a conta pode ser realizado através do nome/extensão do arquivo salvo ex. _1224456111626299747.jpg

### TRANSAÇÕES
+ Será possível adicionar TRANSAÇÕES caso a conta não esteja excluída LOGICA/FISICAMENTE.
+ Será possível adicionar TRANSAÇÕES caso a conta não esteja INATIVA/EXCLUÍDA.
+ A transação entre contas é informada tanto na busca da conta de ORIGEM quanto na de DESTINO.
+ Caso ocorra algum erro durante o cadastro da TRANSAÇÃO, o processo é desfeito através de Transaction sem salvar lixo no banco. 

### EXTRATO
+ A listagem de EXTRATO contém o SALDO na primeira linha e todas as TRANSAÇÕES, incluindo quando uma conta efetua e também recebe uma TRANFERÊNCIA entre CONTAS. 

### USUARIOS
+ Caso execute o método Seed para popular as tabelas. será necessário rodar o método de update para atualizar a senha antes de efetuar o login

### AUTENTICAÇÃO JWT
1. Cadastre um usuário pelo endpoint `POST /api/v1/Usuario`.
2. Faça login pelo endpoint `POST /api/v1/Usuario/Login` usando o mesmo e-mail e senha. A resposta contém o token JWT.
3. Envie o token no cabeçalho `Authorization` como `Bearer <token>` para chamar endpoints protegidos.

No código atual, somente estes métodos exigem autenticação:
+ `DELETE /api/v2/Conta/{id}` — exclusão física de uma conta.
+ `DELETE /api/v2/Usuario/{email}` — exclusão física de um usuário.

Os demais endpoints não estão marcados com `[Authorize]` atualmente. Com o banco InMemory, o usuário cadastrado precisa ser criado novamente após reiniciar a aplicação.

### VERSOES V1 E V2
+ Os endpoints da versão v1 não exige autenticação/autorização (Incluindo os métodos de exclusão lógica.).
+ Na versão v2, somente os dois métodos de exclusão física listados acima exigem autenticação/autorização.

### LINKS EXTERNOS
+ MYSQL
https://dev.mysql.com/downloads/installer/

+ RECEITAWS
O sistema consulta a ReceitaWS para obter o nome associado ao CNPJ da conta (não consulta CPF):
https://receitaws.com.br/v1/cnpj/{cnpj}

+ VISUAL STUDIO
https://visualstudio.microsoft.com/pt-br/downloads/
