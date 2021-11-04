using ApiCatalogoJogos.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiCatalogoJogos.Repositories
{
    public interface IJogoRepository : IDisposable
    {
        List<Jogo> Obter(int pagina, int quantidade);
        Jogo Obter(Guid id);
        List<Jogo> Obter(string nome, string produtora);
        void Inserir(Jogo jogo);
        void Atualizar(Jogo jogo);
        void Remover(Guid id);
    }
}
