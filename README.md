# DIO-APICatalagoDeJogos
Projeto de estudo do bootcampo DIO/Take Blip

##API em .ASP NET de repositorio de jogos com banco de dados SQL
É uma interface para um repositório em SQL para jogos. Acrescentei o campo "lancamento" na base de dados.

##Principal desafio/problema
O principal problema que tive com o projeto foi entender o funcionamento do tipo Guid e entender que o Database em SQL não é criado em Runtime.
O problema no código é que a base de dados foi criada manualmente é não através do código, não gostei disso e acho que é muito importante que a própria API, ao iniciar, verifique a existencia da base de dados e a crie se ela não existir.
Queria implementar isso de uma forma que eu já tinha usado com Python e SQLite (pseudocodigo): 
```
if(!database.exists()) then create
```
Porém não achei forma direta de implementar isso no ambiente dotnet.
<br>
Então tentei fazer um teste de conexão com:
```
try
{
open_conection();
}
catch
{
create_database();
}
```
O que também não funcionou, tive problemas para fechar a conexão depois de abrí-la no teste. Mas vou explorar mais, tem de haver uma maneira de fazer isso.

### SQLite
Acredito ter resolvido o problema. Mudei o database para o SQLite invés do Sql Server. Assim consegui criar um DB local.
A configuração e criação do DB está pronta e parece funcionar, agora preciso reescrver os outros métodos do __repository__ para poder testar.
Fiz todas as alterações no código para funcionar com o SQLite, porém tive que remover toda computação assincrona. Eu não soube fazer o acesso ao banco de dados Sqlite usando computação assincrona. Isso me leva a concluir que é melhor continuar usando o Sql Server, já que a perfomance prejudicada demais para uma aplicação Web.

## Enfim
Gostaria se ter conseguido fazer funcionar com a criação da base de dados em runtime, porém preciso aceitar que isso está além do meu nível no momento e encerrar o projeto por aqui mesmo. Vou deixar o projeto separado em 3 branches:
- main: o projeto funcionando com Sql Server, porém o Db precisa ser criado manualmente antes.
- create_db_at_runtime: minha tentativa de criar o Db em runtime (caso não existisse) com Sql Server. Essa solução não funcionou.
- local_db_using_sqlite: tentei usar o sqlite para criar o db em runtime. Deu certo, porém com o sqlite precisei remover toda a computação assincrona. Acabei desistindo dessa ideia por causa do impacto na performance que isso teria. Além disso ficaram alguns problemas de sintaxe nas queries, nem todas foram traduzidas para o sqlite.