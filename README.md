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
