using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Web;

namespace PPAI20243K6.Clases
{
    internal class Vino
    {
        private int añada { get; set; }
        private DateTime fechaActualizacion { get; set; }
        private bool imagenEtiqueta { get; set; }
        private string nombre { get; set; }
        private int notaDeCataBodega { get; set; }
        private float precioARS { get; set; }

        private List<Reseña> Reseñas;

        private List<Varietal> Varietal;
        private Bodega Bodega;
        private double promedio = 0;
        List<int> puntajes = new List<int>();

        public Vino(int añada, DateTime fechaActualizacion, bool imagenEtiqueta, string nombre, int notaCata, float precioARS, Bodega bodega)
        {
           
            this.añada = añada;
            this.fechaActualizacion = fechaActualizacion;
            this.imagenEtiqueta= imagenEtiqueta;
            this.nombre= nombre;
            this.notaDeCataBodega = notaCata;
            this.precioARS = precioARS;
            this.Varietal= new List<Varietal>();
            this.Reseñas = new List<Reseña>();
            this.Bodega = bodega;
           

        }
        public void setPromedio(double prom)
        {
            this.promedio = prom;
        }
        public double getPromedio()
        {
            return this.promedio;
        }
        public float getPrecio()
        {
            return this.precioARS;
        }
        public string getNombre()
        {
            return nombre;
        }
        public string buscarVarietal()
        {
            return string.Join(", ", this.Varietal.Select(v => v.getDescripcion()));
        }
        public string buscarBodega()
        {
            string nom=this.Bodega.getNombre();
            string reg = this.Bodega.buscarRegionYPais();
            return nom+", "+reg;
        }

        public void agregarVarietal(Varietal var)
        {
            this.Varietal.Add(var);
        }

        public void agregarReseña(Reseña res)
        {
            this.Reseñas.Add(res);
            res.setVino(this);
        }

        public void CalcularRanking()
        {
            // Implementación del método para calcular ranking
        }

        public void CompararEtiqueta()
        {
            // Implementación del método para comparar etiqueta
        }

        public bool EsDeBodega()
        {
            // Implementación del método para verificar si es de bodega
            return true;
        }

        public bool EsDeRegionVitivinicola()
        {
            // Implementación del método para verificar si es de región vitivinícola
            return false;
        }

        public IIterator<Reseña> CrearIterador(List<Reseña> reseñas)
        {
            return new IteradorReseña(reseñas);
        }
        public List<int> buscarVinosConReseña(DateTime fechaDesde, DateTime fechaHasta, bool premium)  
        {

            List<int> puntajesFiltrados = new List<int>();

            IIterator<Reseña> iteradorReseña = CrearIterador(this.Reseñas);
            iteradorReseña.Primero();

            // Mientras no haya llegado al final de la lista
            while (!iteradorReseña.HaFinalizado())
            {
                Reseña reseñaActual = iteradorReseña.Actual();

                // Si pasa los filtros
                if (iteradorReseña.ComprobarFiltros(fechaDesde, fechaHasta, premium))
                {
                    puntajesFiltrados.Add(reseñaActual.getPuntaje());
                }
                // Solo avanzamos al siguiente elemento si no ha terminado la lista
                iteradorReseña.Siguiente();
            }
            return puntajesFiltrados;
        }

        public void CalcularPromedioDeSommelierEnPeriodo(List<int> puntajes)
        {
            double prom = CalcularPuntajePromedio(puntajes);
            setPromedio(prom);
            this.puntajes = puntajes;
        }
        public double CalcularPuntajePromedio(List<int> puntajes)
        {
            double suma = 0;
            double prom = 0;
            foreach (double numero in puntajes)
            {
                suma += numero;
            }
            if (puntajes.Count > 0)
            {
                prom = suma / puntajes.Count;
            } else { prom = 0; }
            return Math.Round(prom, 2);
        }
        public string getArrayPuntajes()
        {
            return string.Join(", ", puntajes);
        }
    }
}
