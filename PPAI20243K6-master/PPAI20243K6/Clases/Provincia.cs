using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPAI20243K6.Clases
{
    internal class Provincia
    {
        private Pais pais;
        private string nombre { get; set; }
        private List<RegionVitivinicola> regiones;

        public Provincia(string nombre, Pais pais)
        {
            this.nombre = nombre;
            this.pais = pais;
        }

        public int ContarRegiones()
        {
            // Implementación del método para contar regiones
            return 0;
        }

        public string getNombre() { return nombre; }
        public void MostrarRegiones()
        {
            // Implementación del método para mostrar regiones
        }

        public void agregarRegiones(RegionVitivinicola region)
        {
            this.regiones.Add(region);
            region.setProvincia(this);
        }

        internal void setPais(Pais pais)
        {
            this.pais = pais;
        }

        internal string buscarPais() { return pais.getNombre(); }

    }
}
