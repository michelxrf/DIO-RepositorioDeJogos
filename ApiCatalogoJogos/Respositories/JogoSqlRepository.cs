using ApiCatalogoJogos.Entities;
using ApiCatalogoJogos.Repositories;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace ApiCatalogoJogos.Respositories
{
    public class JogoSqlServerRepository : IJogoRepository
    {
        private readonly SqlConnection sqlConnection;
        private readonly String DbLocation = "Data Source=CatalogoJogos.db";

        public JogoSqlServerRepository(IConfiguration configuration)
        {
            CriaDb();
        }

        public List<Jogo> Obter(int pagina, int quantidade)
        {
            var jogos = new List<Jogo>();

            using (SqliteConnection sqlConnection = new SqliteConnection(DbLocation))
            {
                sqlConnection.Open();

                var comando = sqlConnection.CreateCommand();

                comando.CommandText = $"select * from Jogos order by id offset {((pagina - 1) * quantidade)} rows fetch next {quantidade} rows only";
                SqliteDataReader sqlDataReader = comando.ExecuteReader();

                while (sqlDataReader.Read())
                {
                    jogos.Add(new Jogo
                    {
                        Id = Guid.Parse((string)sqlDataReader["Id"]),
                        Nome = (string)sqlDataReader["Nome"],
                        Produtora = (string)sqlDataReader["Produtora"],
                        Preco = Convert.ToDouble(sqlDataReader["Preco"]),
                        Lancamento = Convert.ToInt32(sqlDataReader["Lancamento"])
                    });
                }
            }

            return jogos;
        }

        public Jogo Obter(Guid id)
        {
            Jogo jogo = null;

            using (SqliteConnection sqlConnection = new SqliteConnection(DbLocation))
            {
                sqlConnection.Open();

                var comando = sqlConnection.CreateCommand();
                comando.CommandText = $"select * from Jogos where Id = '{id}'";

                SqliteDataReader sqlDataReader = comando.ExecuteReader();

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

            }

            return jogo;
        }

        public List<Jogo> Obter(string nome, string produtora)
        {
            var jogos = new List<Jogo>();

            using (SqliteConnection sqlConnection = new SqliteConnection(DbLocation))
            {
                sqlConnection.Open();

                var comando = sqlConnection.CreateCommand();
                comando.CommandText = $"select * from Jogos where Nome = '{nome}' and Produtora = '{produtora}'";
                SqliteDataReader sqlDataReader = comando.ExecuteReader();

                while (sqlDataReader.Read())
                {
                    jogos.Add(new Jogo
                    {
                        Id = Guid.Parse((string)sqlDataReader["Id"]),
                        Nome = (string)sqlDataReader["Nome"],
                        Produtora = (string)sqlDataReader["Produtora"],
                        Preco = Convert.ToDouble(sqlDataReader["Preco"]),
                        Lancamento = Convert.ToInt32(sqlDataReader["Lancamento"])
                    });
                }
            }

            return jogos;
        }

        public void Inserir(Jogo jogo)
        {
            using (SqliteConnection sqlConnection = new SqliteConnection(DbLocation))
            {
                sqlConnection.Open();

                var comando = sqlConnection.CreateCommand();
                comando.CommandText = $"insert Jogos (Id, Nome, Produtora, Preco, Lancamento) values ('{jogo.Id}', '{jogo.Nome}', '{jogo.Produtora}', {jogo.Preco.ToString().Replace(",", ".")}, {jogo.Lancamento})";
                comando.ExecuteNonQuery();
            }
        }

        public void Atualizar(Jogo jogo)
        {
            using (SqliteConnection sqlConnection = new SqliteConnection(DbLocation))
            {
                sqlConnection.Open();

                var comando = sqlConnection.CreateCommand();
                comando.CommandText = $"update Jogos set Nome = '{jogo.Nome}', Produtora = '{jogo.Produtora}', Preco = {jogo.Preco.ToString().Replace(",", ".")}, Lancamento = {jogo.Lancamento} where Id = '{jogo.Id}'";
                comando.ExecuteNonQuery();
            }
        }

        public void Remover(Guid id)
        {
            using (SqliteConnection sqlConnection = new SqliteConnection(DbLocation))
            {
                sqlConnection.Open();

                var comando = sqlConnection.CreateCommand();
                comando.CommandText = $"delete from Jogos where Id = '{id}'";
                comando.ExecuteNonQuery();
            }

        }

        public void Dispose()
        {
            sqlConnection?.Close();
            sqlConnection?.Dispose();
        }

        private void CriaDb()
        {
            try
            {
                using (SqliteConnection sqlConnection = new SqliteConnection(DbLocation))
                {
                    sqlConnection.Open();

                    var comando = sqlConnection.CreateCommand();

                    comando.CommandText = "SELECT * FROM Jogos";
                    comando.ExecuteNonQuery();
                    
                }
                
            }
            catch
            {
                using (SqliteConnection sqlConnection = new SqliteConnection(DbLocation))
                {
                    sqlConnection.Open();

                    var comando = sqlConnection.CreateCommand();
                    
                    comando.CommandText = @"CREATE TABLE Jogos 
                    (Id text UNIQUE NOT NULL,
                    Nome text NOT NULL,
                    Produtora text NOT NULL,
                    Preco real NOT NULL,
	                Lancamento int NOT NULL,
                    PRIMARY KEY (Id));";

                    comando.ExecuteNonQuery();

                }

                
            }
        }
    }
}
