using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPAI20243K6.Clases
{
    internal class RegionVitivinicola
    {
        private string descripcionReg { get; set; }
        private string nombreReg { get; set; }

        private Provincia provinciaRegion { get; set; }

        public RegionVitivinicola(string descripcion, string nombre, Provincia provincia)
        {
           
            this.descripcionReg = descripcion;

            this.nombreReg = nombre;

            this.provinciaRegion = provincia;
          
        }
        public int ContarBodegas()
        {
            // Implementación del método para contar bodegas
            return 0;
        }
        public string getNombre()
        {
            return nombreReg;
        }
        public string getDescripcion()
        {
            return descripcionReg;
        }
        public string buscarProvPais()
        {
            string provPais = provinciaRegion.getNombre();

            provPais = provPais + ", " + provinciaRegion.buscarPais();
            
            return provPais;
        }

        public void setProvincia(Provincia provincia)
        {
            this.provinciaRegion = provincia;
        }

    }
}
