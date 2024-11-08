using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPAI20243K6.Clases
{
    internal class IteradorReseña : IIterator<Reseña>
    {
        private List<Reseña> reseñas;
        private int posicion;

        public IteradorReseña(List<Reseña> reseñas)
        {
            this.reseñas = reseñas;
            this.posicion = 0;
        }

        public Reseña Actual()
        {
            if (HaFinalizado())
            {
                throw new InvalidOperationException("No hay elementos actuales disponibles.");
            }
            return reseñas[posicion];
        }

        public bool ComprobarFiltros(DateTime fechaDesde, DateTime fechaHasta, bool premium)
        {
            return this.Actual().esFechaValida(fechaDesde, fechaHasta) && this.Actual().sosDeSommelier(premium);
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
            return posicion >= reseñas.Count;
        }
    }
}
