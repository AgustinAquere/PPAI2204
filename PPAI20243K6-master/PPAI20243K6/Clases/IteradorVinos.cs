using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPAI20243K6.Clases
{
    internal class IteradorVinos : IIterator<Vino>
    {
        private List<Vino> vinos;
        private int posicion;

        public IteradorVinos(List<Vino> vinos)
        {
            this.vinos = vinos;
            this.posicion = 0;
        }

        public Vino Actual()
        {
            if (HaFinalizado())
            {
                throw new InvalidOperationException("No hay elementos actuales disponibles.");
            }
            return vinos[posicion];
        }

        public bool ComprobarFiltros(Vino elemento, DateTime fechaDesde, DateTime fechaHasta, bool premium)
        {
            return false;
        }

        public void Siguiente()
        {
            if (HaFinalizado())
            {
                throw new InvalidOperationException("No hay más elementos en la colección.");
            }
            posicion++;
        }

        public void Primero()
        {
            posicion = 0;
        }

        public bool HaFinalizado()
        {
            return posicion >= vinos.Count;
        }
    }
}
