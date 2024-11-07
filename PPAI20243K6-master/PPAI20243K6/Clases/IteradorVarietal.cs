using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPAI20243K6.Clases
{
    public class IteradorVarietal : IIterator<Varietal>
    {
        internal List<Varietal> varietal;
        internal int posicion;

        public IteradorVarietal(List<Varietal> varietal)
        {
            this.varietal = varietal;
            this.posicion = 0;
        }

        public Varietal Actual()
        {
            if (HaFinalizado())
            {
                throw new InvalidOperationException("No hay elementos actuales disponibles.");
            }
            return varietal[posicion];
        }

        public bool ComprobarFiltros(Varietal elemento, DateTime fechaDesde, DateTime fechaHasta, bool premium)
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
            return posicion >= varietal.Count;
        }
    }
}
