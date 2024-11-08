
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
        List<Vino> vinosConReseña = new List<Vino>();
        string Puntajes;
        List<int> arrPuntajes;
        XLWorkbook workbook = null;
        IXLWorksheet worksheet = null;
        int currentRow = 0;

        private double promedioVino;
        private float precioVino;
        private string nombreVino;
        private string datosBodega;
        private string varietales;

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
            vinosConReseña.Clear();

            bool premium = false;
            if (tipoReseña == "Sommelier")
            {
                premium = true;
            }
            buscarVinosReseñasPeriodo(premium, tipoVisualizacion);


        }

        public void buscarVinosReseñasPeriodo(bool premium, string tipoVis)
        {

            for (int i = 0; i < vinos.Count; i++)
            {
                Vino vinoActual = vinos[i];
                arrPuntajes = vinoActual.buscarVinosConReseña(fechaDesde, fechaHasta, premium);
                if (arrPuntajes.Count() > 0)
                {
                    vinoActual.CalcularPromedioDeSommelierEnPeriodo(arrPuntajes);
                    vinosConReseña.Add(vinoActual);
                }
            }

            vinosConReseña = ordenarVinoPorPromedio(vinosConReseña);


            if (tipoVis == "excel")
            {
                workbook = new XLWorkbook();
                worksheet = workbook.Worksheets.Add("Ranking de Vinos");

                worksheet.Cell(1, 5).Value = "Promedio Puntajes";
                worksheet.Cell(1, 1).Value = "Nombre Vino";
                worksheet.Cell(1, 2).Value = "Precio";
                worksheet.Cell(1, 3).Value = "Bodega";
                worksheet.Cell(1, 4).Value = "Varietal";
                worksheet.Cell(1, 6).Value = "Puntajes";

                var headerRange = worksheet.Range("A1:F1");
                headerRange.Style.Font.Bold = true;
                headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;
                headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                headerRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                headerRange.Style.Border.OutsideBorderColor = XLColor.Black;

                currentRow = 2;
            }

            for (int i = 0; i < vinosConReseña.Count; i++)
            {
                Vino vinoActual = vinosConReseña[i];
                precioVino = vinoActual.getPrecio();
                nombreVino = vinoActual.getNombre();
                promedioVino = vinoActual.getPromedio();
                datosBodega = vinoActual.buscarBodega();
                varietales = vinoActual.buscarVarietal();
                Puntajes = vinoActual.getArrayPuntajes();

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
                    puntajes.Value = Puntajes;
                    fila.Cells.Add(puntajes);

                    PantallaAsociada.agregarFilaGrd(fila);
                }
                if (tipoVis == "excel")
                {

                    worksheet.Cell(currentRow, 5).Value = promedioVino;
                    worksheet.Cell(currentRow, 1).Value = nombreVino;
                    worksheet.Cell(currentRow, 2).Value = precioVino;
                    worksheet.Cell(currentRow, 3).Value = datosBodega;
                    worksheet.Cell(currentRow, 4).Value = varietales;
                    worksheet.Cell(currentRow, 6).Value = Puntajes;

                    var dataRange = worksheet.Range($"A{currentRow}:F{currentRow}");
                    dataRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    dataRange.Style.Border.OutsideBorderColor = XLColor.Black;
                    dataRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                    currentRow++;
                }
            }

            if (tipoVis == "excel")
            {
                worksheet.Columns().AdjustToContents();
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


        public void FinCU()
        {
            // Implementación del método para finalizar el caso de uso
            // PantallaAsociada.Close();

        }
    }
}
