using ApiCatalogoJogos.InputModel;
using ApiCatalogoJogos.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiCatalogoJogos.Services
{
    public interface IJogoService : IDisposable
    {
        List<JogoViewModel> Obter(int pagina, int quantidade);
        JogoViewModel Obter(Guid id);
        JogoViewModel Inserir(JogoInputModel jogo);
        void Atualizar(Guid id, JogoInputModel jogo);
        void Atualizar(Guid id, double preco);
        void Remover(Guid id);
    }
}
