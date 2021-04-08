using Xunit;
using Alura.LeilaoOnline.Core;
using System.Linq;

namespace Alura.LeilaoOnline.Tests
{
    public class LeilaoRecebeLance
    {
        [Fact]
        public void NaoAceitaProximoLanceDadoMesmoClienteRealizouUltimoLance()
        {
            //Arrange - cenário
            var modalidade = new MaiorValor();
            var leilao = new Leilao("Van Gogh", modalidade);
            var fulano = new Interessada("Fulano", leilao);

            leilao.IniciaPregao();
            leilao.RecebeLance(fulano, 800);

            //Act - Método sob teste
            leilao.RecebeLance(fulano, 1000);

            //Asset - Verificação do resultado
            var qtdeEsperada = 1;
            var qtdeObtida = leilao.Lances.Count();
            Assert.Equal(qtdeEsperada, qtdeObtida);
        }

        [Theory]
        [InlineData(2, new double[] { 800, 900 })]
        public void NaoPermiteNovosLancesDadoLeilaoFinalizado(int qtdEsperada, double[] ofertas)
        {
            //Arrange - cenários
            var modalidade = new MaiorValor();
            var leilao = new Leilao("Van Gogh", modalidade);
            var fulano = new Interessada("Fulano", leilao);
            var maria = new Interessada("Maria", leilao);
            
            leilao.IniciaPregao();

            for (int i = 0; i < ofertas.Length; i++)
            {
                var valor = ofertas[i];
                if(i%2 == 0)
                {
                    leilao.RecebeLance(fulano, valor);
                }
                else
                {
                    leilao.RecebeLance(maria, valor);
                }
            }

            leilao.TerminaPregao();

            //Act - método sob teste
            leilao.RecebeLance(fulano, 1000);

            //Assert
            var qtdObtida = leilao.Lances.Count();

            Assert.Equal(qtdEsperada, qtdObtida);
        }

        [Theory]
        [InlineData(0, new double[] { 200, 300, 400, 500 })]
        [InlineData(0, new double[] { 200 })]
        [InlineData(0, new double[] { 200, 300, 400 })]
        [InlineData(0, new double[] { 200, 300, 400, 500, 600, 700 })]
        public void NaoPermiteLanceDadoQuePregaoNaoFoiIniciado(int qtdEsperada, double[] ofertas)
        {
            var modalidade = new MaiorValor();
            var leilao = new Leilao("Van Gogh", modalidade);
            var fulano = new Interessada("Fulano de Tal", leilao);

            foreach (var valor in ofertas)
            {
                leilao.RecebeLance(fulano, valor);
            }

            var qtdObtida = leilao.Lances.Count();

            Assert.Equal(qtdEsperada, qtdObtida);
        }
    }
}
