using ApiCatalogoJogos.Entities;
using ApiCatalogoJogos.Repositories;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Data;

namespace ApiCatalogoJogos.Respositories
{
    public class JogoSqlServerRepository : IJogoRepository
    {
        private readonly SqlConnection sqlConnection;

        public JogoSqlServerRepository(IConfiguration configuration)
        {
            sqlConnection = new SqlConnection(configuration.GetConnectionString("Default"));
            AssertDbExistence();

        }

        public async Task<List<Jogo>> Obter(int pagina, int quantidade)
        {
            var jogos = new List<Jogo>();

            var comando = $"select * from Jogos order by id offset {((pagina - 1) * quantidade)} rows fetch next {quantidade} rows only";

            await sqlConnection.OpenAsync();
            SqlCommand sqlCommand = new SqlCommand(comando, sqlConnection);
            SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();

            while (sqlDataReader.Read())
            {
                jogos.Add(new Jogo
                {
                    Id = (Guid)sqlDataReader["Id"],
                    Nome = (string)sqlDataReader["Nome"],
                    Produtora = (string)sqlDataReader["Produtora"],
                    Preco = Convert.ToDouble(sqlDataReader["Preco"]),
                    Lancamento = Convert.ToInt32(sqlDataReader["Lancamento"])
                });
            }

            await sqlConnection.CloseAsync();

            return jogos;
        }

        public async Task<Jogo> Obter(Guid id)
        {
            Jogo jogo = null;

            var comando = $"select * from Jogos where Id = '{id}'";

            await sqlConnection.OpenAsync();
            SqlCommand sqlCommand = new SqlCommand(comando, sqlConnection);
            SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();

            while (sqlDataReader.Read())
            {
                jogo = new Jogo
                {
                    Id = (Guid)sqlDataReader["Id"],
                    Nome = (string)sqlDataReader["Nome"],
                    Produtora = (string)sqlDataReader["Produtora"],
                    Preco = Convert.ToDouble(sqlDataReader["Preco"]),
                    Lancamento = Convert.ToInt32(sqlDataReader["Lancamento"])
                };
            }

            await sqlConnection.CloseAsync();

            return jogo;
        }

        public async Task<List<Jogo>> Obter(string nome, string produtora)
        {
            var jogos = new List<Jogo>();

            var comando = $"select * from Jogos where Nome = '{nome}' and Produtora = '{produtora}'";

            await sqlConnection.OpenAsync();
            SqlCommand sqlCommand = new SqlCommand(comando, sqlConnection);
            SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();

            while (sqlDataReader.Read())
            {
                jogos.Add(new Jogo
                {
                    Id = (Guid)sqlDataReader["Id"],
                    Nome = (string)sqlDataReader["Nome"],
                    Produtora = (string)sqlDataReader["Produtora"],
                    Preco = Convert.ToDouble(sqlDataReader["Preco"]),
                    Lancamento = Convert.ToInt32(sqlDataReader["Lancamento"])
                });
            }

            await sqlConnection.CloseAsync();

            return jogos;
        }

        public async Task Inserir(Jogo jogo)
        {
            var comando = $"insert Jogos (Id, Nome, Produtora, Preco, Lancamento) values ('{jogo.Id}', '{jogo.Nome}', '{jogo.Produtora}', {jogo.Preco.ToString().Replace(",", ".")}, {jogo.Lancamento})";

            await sqlConnection.OpenAsync();
            SqlCommand sqlCommand = new SqlCommand(comando, sqlConnection);
            sqlCommand.ExecuteNonQuery();
            await sqlConnection.CloseAsync();
        }

        public async Task Atualizar(Jogo jogo)
        {
            var comando = $"update Jogos set Nome = '{jogo.Nome}', Produtora = '{jogo.Produtora}', Preco = {jogo.Preco.ToString().Replace(",", ".")}, Lancamento = {jogo.Lancamento} where Id = '{jogo.Id}'";

            await sqlConnection.OpenAsync();
            SqlCommand sqlCommand = new SqlCommand(comando, sqlConnection);
            sqlCommand.ExecuteNonQuery();
            await sqlConnection.CloseAsync();
        }

        public async Task Remover(Guid id)
        {
            var comando = $"delete from Jogos where Id = '{id}'";

            await sqlConnection.OpenAsync();
            SqlCommand sqlCommand = new SqlCommand(comando, sqlConnection);
            sqlCommand.ExecuteNonQuery();
            await sqlConnection.CloseAsync();
        }

        public void Dispose()
        {
            sqlConnection?.Close();
            sqlConnection?.Dispose();
        }

        //Tenta garantir a existencia do DB, testa se existe, caso não, crie. O método atual não funciona:
        //-para eu poder dar o comando de criar o DB depende de existir conexão
        //-para existir conexão preciso que exista um DB
        private void AssertDbExistence()
        {
            //essa parte funciona para testar a existencia do DB
            try
            {
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand("SELECT * FROM Jogos", sqlConnection);
                sqlCommand.ExecuteNonQuery();
                if (sqlConnection.State == ConnectionState.Open)
                {
                    sqlConnection.Close();
                }
            }
            //porém essa parte não consegue criar. Preciso encontrar uma maneira para tal
            catch
            {
                CreateDb();
                CreateTable();
            }

            
        }

        private void CreateDb()
        {
            String comandoSql;

            comandoSql = "CREATE DATABASE CatalogoJogos ON PRIMARY " +
             "(NAME = CatalogoJogos_Data, " +
             "FILENAME = 'C:\\mysql\\MyDatabaseData.mdf', " +
             "SIZE = 2MB, MAXSIZE = 10MB, FILEGROWTH = 10%)" +
             "LOG ON (NAME = CatalogoJogos_Log, " +
             "FILENAME = 'C:\\mysql\\MyDatabaseLog.ldf', " +
             "SIZE = 1MB, " +
             "MAXSIZE = 5MB, " +
             "FILEGROWTH = 10%)";

            SqlCommand myCommand = new SqlCommand(comandoSql, sqlConnection);

            if (sqlConnection.State != ConnectionState.Open)
                sqlConnection.Open();

            myCommand.ExecuteNonQuery();

            if (sqlConnection.State == ConnectionState.Open)
            {
                sqlConnection.Close();
            }

        }

        private void CreateTable()
        {
            String comandoSql;

            comandoSql = "CREATE TABLE Jogos" +
            "(Id UNIQUEIDENTIFIER NOT NULL," +
            "Nome VARCHAR(255) NOT NULL," +
            "Produtora VARCHAR(255) NOT NULL," +
            "Preco REAL NOT NULL," +
            "Lancamento INT NOT NULL," +
            "PRIMARY KEY(Id))";

            SqlCommand myCommand = new SqlCommand(comandoSql, sqlConnection);

            sqlConnection.Open();
            myCommand.ExecuteNonQuery();

            if (sqlConnection.State == ConnectionState.Open)
            {
                sqlConnection.Close();
            }
        }
    }
}
