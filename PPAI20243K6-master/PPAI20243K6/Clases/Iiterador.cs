using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPAI20243K6.Clases
{
    internal interface IIterator<Elementos>
    {
        Elementos Actual(); // Devuelve el elemento actual
        bool ComprobarFiltros(DateTime fechaDesde, DateTime fechaHasta, bool premium); //Comprueba si el elemento cumple con ciertos filtros
        void Siguiente(); // Devuelve el siguiente elemento
        void Primero(); // Resetea el iterador al primer elemento
        bool HaFinalizado(); // Verifica si se ha llegado al final de la colección
    }
}
