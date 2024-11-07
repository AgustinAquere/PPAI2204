
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;


namespace PPAI20243K6.Clases
{
    internal class GestorGenerarRankingVinos
    {
        public PantallaRankingVinos PantallaAsociada;
        private int reportesAGenerar { get; set; }
        private DateTime fechaDesde;
        private DateTime fechaHasta;
        public bool fechasValidas;
        List<Vino> arrayVinos = new List<Vino>();
        List<Vino> vinosConReseña = new List<Vino>();
        string arrPuntajes;
        XLWorkbook workbook = null;
        IXLWorksheet worksheet = null;
        int currentRow = 0;

        private double promedioVino;
        private float precioVino;
        private string nombreVino;
        private string datosBodega;
        private string varietales;
        private double calificacionGral;

        string connectionString;
        Persistencia persistencia;
        private List<Pais> paises;
        private List<Provincia> provincias;
        private List<RegionVitivinicola> regiones;
        private List<Bodega> bodegas;
        private List<Vino> vinos;




        public GestorGenerarRankingVinos(PantallaRankingVinos pantalla)
        {

            PantallaAsociada = pantalla;
            connectionString = "Data Source=H:/Usuario/Desktop/PPAI2204/PPAI20243K6-master/PPAI20243K6/Resources/db-tpi-dsi.db;Version=3;";
            persistencia = new Persistencia(connectionString);
            persistencia.OpenConnection();
            paises = persistencia.desmaterializarPaises();
            provincias = persistencia.desmaterializarProvincias(paises);
            regiones = persistencia.desmaterializarRegiones(provincias);
            bodegas = persistencia.desmaterializarBodegas(regiones);
            vinos = persistencia.desmaterializarVinos(bodegas);
            persistencia.desmaterializarVarietal(vinos);
            persistencia.desmaterializarReseñas(vinos);

            persistencia.CloseConnection();





        }
        public void generarRanking()
        {
            //cargar los objetos



            PantallaAsociada.solicitarFechasDesdeHasta();
            // Implementación del método para generar ranking
        }

        public void fechasConsideracionReseñas(DateTime fechaDesde, DateTime fechaHasta)
        {
            // Implementación del método para considerar fechas de reseñas
            validarFechas(fechaDesde, fechaHasta);
        }

        public void validarFechas(DateTime fechaDesde, DateTime fechaHasta)
        {
            // Implementación del método para validar fechas
            if (fechaDesde < fechaHasta)
            {
                this.fechaDesde = fechaDesde;
                this.fechaHasta = fechaHasta;
                fechasValidas = true;
            }
            else
            {
                fechasValidas = false;
            }
        }



        public void tomarTipoReseña(string tipoReseña)
        {
            // Implementación del método para tomar tipo de reseña

            PantallaAsociada.pedirSeleccionFormasVisualizacion();

        }

        public string tomarTipoFormasVisualizacion(bool chkExcel, bool chkpdf, bool chkpantalla)
        {
            // Implementación del método para tomar tipo de formas de visualización
            string tipoVis = "";
            if (chkExcel)
            {
                tipoVis = "excel";
            }
            else
            {
                if (chkpdf)
                {
                    tipoVis = "pdf";
                }
                else
                {
                    if (chkpantalla)
                    {
                        tipoVis = "pantalla";
                    }

                }
            }
            return tipoVis;
        }

        public void tomarConfirmacion(string tipoReseña, string tipoVisualizacion)
        {
            // Implementación del método para tomar confirmación
            //----
            vinosConReseña.Clear();

            //----
            bool premium = false;
            if (tipoReseña == "Sommelier")
            {
                premium = true;
            }
            buscarVinosReseñasPeriodo(premium, tipoVisualizacion);


        }

        public IIterator<Vino> CrearIterador(List<Vino> vinosAIterar)
        {
            return new IteradorVinos(vinosAIterar);
        }

        public void buscarVinosReseñasPeriodo(bool premium, string tipoVis)
        {

            //Recorremos el array de vinos, para poder obtener los vinos con reseña utilizamos el metoo buscarVinosConReseña
            //En caso que tengan una reseña con las fechas indicadas y sea premium, se crea un array nuevo de vinos con reseña

            IIterator<Vino> iteradorFiltrado = CrearIterador(vinos);
            iteradorFiltrado.Primero();
            while (!iteradorFiltrado.HaFinalizado())
            {
                Vino vinoActual = iteradorFiltrado.Actual();
                if (vinoActual.buscarVinosConReseña(fechaDesde, fechaHasta, premium))
                {
                    vinosConReseña.Add(vinoActual);
                }
                iteradorFiltrado.Siguiente();
            }



            //Para los vinos llamamos los metodos para calcular el promedio de puntaje en el periodo y tambien los ordenamos por promedio
            CalcularPromedioDeSommelierEnPeriodo(vinosConReseña, premium);
            Console.WriteLine(vinosConReseña.Count());


            vinosConReseña = ordenarVinoPorPromedio(vinosConReseña);
            Console.WriteLine(vinosConReseña.Count());


            if (tipoVis == "excel")
            {
                workbook = new XLWorkbook();
                worksheet = workbook.Worksheets.Add("Ranking de Vinos");
                worksheet.Cell(1, 1).Value = "Promedio";
                worksheet.Cell(1, 2).Value = "Nombre Vino";
                worksheet.Cell(1, 3).Value = "Precio";
                worksheet.Cell(1, 4).Value = "Bodega";
                worksheet.Cell(1, 5).Value = "Varietal";
                worksheet.Cell(1, 6).Value = "Puntajes";

                currentRow = 2;

            }

            IIterator<Vino> iteradorVinos = CrearIterador(vinosConReseña);
            iteradorVinos.Primero();
            while (iteradorVinos.HaFinalizado() == false)
            {
                Vino vinoActual = iteradorVinos.Actual();
                precioVino = vinoActual.getPrecio();
                nombreVino = vinoActual.getNombre();
                promedioVino = vinoActual.getPromedio();
                datosBodega = vinoActual.buscarBodega();
                varietales = vinoActual.buscarVarietal();
                arrPuntajes = vinoActual.getArrayPuntajes(premium);

                if (tipoVis == "pantalla")
                {

                    DataGridViewRow fila = new DataGridViewRow();
                    DataGridViewTextBoxCell prom = new DataGridViewTextBoxCell();
                    prom.Value = promedioVino;
                    fila.Cells.Add(prom);

                    DataGridViewTextBoxCell nombre = new DataGridViewTextBoxCell();
                    nombre.Value = nombreVino;
                    fila.Cells.Add(nombre);

                    DataGridViewTextBoxCell precio = new DataGridViewTextBoxCell();
                    precio.Value = precioVino;
                    fila.Cells.Add(precio);


                    DataGridViewTextBoxCell bodega = new DataGridViewTextBoxCell();
                    bodega.Value = datosBodega;
                    fila.Cells.Add(bodega);

                    DataGridViewTextBoxCell datoVarietal = new DataGridViewTextBoxCell();
                    datoVarietal.Value = varietales;
                    fila.Cells.Add(datoVarietal);

                    DataGridViewTextBoxCell puntajes = new DataGridViewTextBoxCell();
                    puntajes.Value = arrPuntajes;
                    fila.Cells.Add(puntajes);

                    PantallaAsociada.agregarFilaGrd(fila);
                }
                if (tipoVis == "excel")
                {

                    worksheet.Cell(currentRow, 1).Value = promedioVino;
                    worksheet.Cell(currentRow, 2).Value = nombreVino;
                    worksheet.Cell(currentRow, 3).Value = precioVino;
                    worksheet.Cell(currentRow, 4).Value = datosBodega;
                    worksheet.Cell(currentRow, 5).Value = varietales;
                    worksheet.Cell(currentRow, 6).Value = arrPuntajes;

                    currentRow++;
                }

                iteradorVinos.Siguiente();
            }

            if (tipoVis == "excel")
            {
                // Guardar el archivo Excel
                string filePath = "H:/Usuario/Desktop/PPAI2204/RankingDeVinos.xlsx";
                workbook.SaveAs(filePath);

                // Informar al usuario de que el archivo se ha guardado
                MessageBox.Show($"El archivo Excel ha sido guardado en {filePath}", "Archivo Guardado", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            FinCU();
        }
        //--------------

        //Metodo para ordenar el aray de vinos por puntaje promedio
        public List<Vino> ordenarVinoPorPromedio(List<Vino> vinosAOrdenar)
        {
            // Asegurarse de que no hay elementos nulos y usar el valor predeterminado de 0 si el promedio no está calculado
            return vinosAOrdenar
                .Where(v => v != null)  // Filtrar cualquier vino nulo
                .OrderByDescending(v => v.getPromedio())  // Ordenar por promedio de manera descendente
                .ToList();
        }



        //Metodo para calcular el promedio de puntaje de los vinos
        public void CalcularPromedioDeSommelierEnPeriodo(List<Vino> VinosDeSomellier, bool premium)
        {
            IIterator<Vino> iteradorVino = CrearIterador(VinosDeSomellier);
            iteradorVino.Primero();
            while (iteradorVino.HaFinalizado() == false)
            {
                Vino vinoActual = iteradorVino.Actual();
                vinoActual.CalcularPromedioDeSommelierEnPeriodo(premium);
                iteradorVino.Siguiente();
            }

        }



        public void FinCU()
        {
            // Implementación del método para finalizar el caso de uso
            PantallaAsociada.Close();

        }
    }
}
