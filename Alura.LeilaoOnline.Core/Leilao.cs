using System;
using System.Collections.Generic;
using System.Linq;

namespace Alura.LeilaoOnline.Core
{
    public enum EstadoLeilao
    {
        LeilaoAntesDoPregao,
        LeilaoEmAndamento,
        LeilaoFinalizado
    }

    public class Leilao
    {
        private IModalidadeAvaliacao _avaliador;
        private IList<Lance> _lances;
        public Interessada _ultimoCliente;
        public IEnumerable<Lance> Lances => _lances;
        public string Peca { get; }
        public Lance Ganhador { get; private set; }
        public EstadoLeilao Estado { get; set; }


        public double ValorDestino { get; }

        public Leilao(string peca, IModalidadeAvaliacao avaliador)
        {
            Peca = peca;
            _lances = new List<Lance>();
            Estado = EstadoLeilao.LeilaoAntesDoPregao;
            _avaliador = avaliador;
        }

        private bool NovoLanceEhAceito(Interessada cliente, double valor)
        {
            return Estado == EstadoLeilao.LeilaoEmAndamento && _ultimoCliente != cliente;
        }

        public void RecebeLance(Interessada cliente, double valor)
        {
            if(NovoLanceEhAceito(cliente, valor))
            {
                _lances.Add(new Lance(cliente, valor));
                _ultimoCliente = cliente;
            }
        }

        public void IniciaPregao()
        {
            Estado = EstadoLeilao.LeilaoEmAndamento;
        }

        public void TerminaPregao()
        {
            if(Estado != EstadoLeilao.LeilaoEmAndamento)
            {
                throw new InvalidOperationException("Não é possível terminar o leilao sem antes iniciá-lo. Para isso, use o método IniciaPrega()");
            }

            if(ValorDestino > 0)
            {
                //Modalidade do lance maior mais próximo
                Ganhador = _avaliador.Avalia(this);
            }
            else
            {
                //Modalidade do maior Lance
                Ganhador = _avaliador.Avalia(this);

                Estado = EstadoLeilao.LeilaoFinalizado;
            }
        }
    }
}