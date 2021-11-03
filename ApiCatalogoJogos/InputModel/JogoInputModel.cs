using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ApiCatalogoJogos.InputModel
{
    public class JogoInputModel
    {
        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "O nome do jogo deve conter entre 3 e 100 caracteres.")]
        public string Nome { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "O nome da produtora deve conter entre 3 e 100 caracteres.")]
        public string Produtora { get; set; }
        [Required]
        [Range(1, 1000, ErrorMessage = "O preco deve ser no minimo 1 real e no maximo 1000 reais.")]
        public double Preco { get; set; }
        [Required]
        [Range(1900, 2100, ErrorMessage = "O lancamenteo precisa ser um inteiro entre 1900 e 2100")]
        public int Lancamento { get; set; }


    }
}
