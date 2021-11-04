using ApiCatalogoJogos.Entities;
using ApiCatalogoJogos.Exceptions;
using ApiCatalogoJogos.InputModel;
using ApiCatalogoJogos.Repositories;
using ApiCatalogoJogos.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiCatalogoJogos.Services
{
    public class JogoService : IJogoService
    {
        private readonly IJogoRepository _jogoRepository;

        public JogoService(IJogoRepository jogoRepository)
        {
            _jogoRepository = jogoRepository;
        }

        public List<JogoViewModel> Obter(int pagina, int quantidade)
        {
            var jogos = _jogoRepository.Obter(pagina, quantidade);

            return jogos.Select(jogo => new JogoViewModel
            {
                Id = jogo.Id,
                Nome = jogo.Nome,
                Produtora = jogo.Produtora,
                Preco = jogo.Preco,
                Lancamento = jogo.Lancamento
            }).ToList();
        }

        public JogoViewModel Obter(Guid id)
        {
            var jogo =  _jogoRepository.Obter(id);

            if (jogo == null)
                return null;

            return new JogoViewModel
            {
                Id = jogo.Id,
                Nome = jogo.Nome,
                Produtora = jogo.Produtora,
                Preco = jogo.Preco,
                Lancamento = jogo.Lancamento
            };
        }

        public JogoViewModel Inserir(JogoInputModel jogo)
        {
            var entidadeJogo = _jogoRepository.Obter(jogo.Nome, jogo.Produtora);

            if (entidadeJogo.Count > 0)
                throw new JogoJaCadastradoException();

            var jogoInsert = new Jogo
            {
                Id = Guid.NewGuid(),
                Nome = jogo.Nome,
                Produtora = jogo.Produtora,
                Preco = jogo.Preco,
                Lancamento = jogo.Lancamento
            };

            _jogoRepository.Inserir(jogoInsert);

            return new JogoViewModel
            {
                Id = jogoInsert.Id,
                Nome = jogo.Nome,
                Produtora = jogo.Produtora,
                Preco = jogo.Preco,
                Lancamento = jogo.Lancamento
            };
        }

        public void Atualizar(Guid id, JogoInputModel jogo)
        {
            var entidadeJogo = _jogoRepository.Obter(id);

            if (entidadeJogo == null)
                throw new JogoNaoCadastradoException();

            entidadeJogo.Nome = jogo.Nome;
            entidadeJogo.Produtora = jogo.Produtora;
            entidadeJogo.Preco = jogo.Preco;
            entidadeJogo.Lancamento = jogo.Lancamento;

            _jogoRepository.Atualizar(entidadeJogo);
        }

        public void Atualizar(Guid id, double preco)
        {
            var entidadeJogo = _jogoRepository.Obter(id);

            if (entidadeJogo == null)
                throw new JogoNaoCadastradoException();

            entidadeJogo.Preco = preco;

            _jogoRepository.Atualizar(entidadeJogo);
        }

        public void Remover(Guid id)
        {
            var jogo = _jogoRepository.Obter(id);

            if (jogo == null)
                throw new JogoNaoCadastradoException();

            _jogoRepository.Remover(id);
        }

        public void Dispose()
        {
            _jogoRepository?.Dispose();
        }
    }
}
