using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Drawing;
using System.Windows.Forms;

namespace PPAI20243K6.Clases
{
    internal class Persistencia
    {
        private SQLiteConnection _connection;
        private string _connectionString;

        public Persistencia(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Abre la conexión con la base de datos
        public void OpenConnection()
        {
            try
            {
                _connection = new SQLiteConnection(_connectionString);
                _connection.Open();
                Console.WriteLine("Conexión abierta correctamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al abrir la conexión: " + ex.Message);
            }
        }

        // Cierra la conexión con la base de datos
        public void CloseConnection()
        {
            if (_connection != null && _connection.State == System.Data.ConnectionState.Open)
            {
                _connection.Close();
                Console.WriteLine("Conexión cerrada correctamente.");
            }
        }

        public List<Pais> desmaterializarPaises()
        {
            List<Pais> paises = new List<Pais>();

            try
            {
                string query = "SELECT * FROM Pais";
                using (var command = new SQLiteCommand(query, _connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            paises.Add(new Pais(
                            reader["nombre"].ToString()
                            ));

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al buscar Paises: " + ex.Message);
            }
            return paises;
        }

        public List<Provincia> desmaterializarProvincias(List<Pais> paises)
        {
            List<Provincia> provincias = new List<Provincia>();

            try
            {
                string query = "SELECT * FROM Provincia";
                using (var command = new SQLiteCommand(query, _connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int paisIndex = Convert.ToInt32(reader["id_pais"]) - 1;

                            provincias.Add(new Provincia(
                            reader["nombre"].ToString(),
                            paises[paisIndex]
                            ));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al buscar Provincias: " + ex.Message);
            }
            return provincias;
        }

        public List<RegionVitivinicola> desmaterializarRegiones(List<Provincia> provincias)
        {
            List<RegionVitivinicola> regiones = new List<RegionVitivinicola>();
            try
            {
                string query = "SELECT * FROM RegionVitivinicola";
                using (var command = new SQLiteCommand(query, _connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int provinciasIndex = Convert.ToInt32(reader["id_provincia"]) - 1;

                            regiones.Add(new RegionVitivinicola(
                            reader["descripcion"].ToString(),
                            reader["nombre"].ToString(),
                            provincias[provinciasIndex]
                            ));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al buscar Regiones: " + ex.Message);
            }
            return regiones;
        }

        public List<Bodega> desmaterializarBodegas(List<RegionVitivinicola> regiones)
        {
            List<Bodega> bodegas = new List<Bodega>();

            try
            {
                string query = "SELECT * FROM Bodega";
                using (var command = new SQLiteCommand(query, _connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int regionesIndex = Convert.ToInt32(reader["id_region"]) - 1;

                            bodegas.Add(new Bodega(
                            reader["ubicacion"].ToString(),
                            reader["descripcion"].ToString(),
                            reader["historia"].ToString(),
                            reader["nombre"].ToString(),
                            Convert.ToInt32(reader["periodo_actualizacion"]),
                            regiones[regionesIndex]
                            ));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al buscar Regiones: " + ex.Message);
            }
            return bodegas;
        }

        public List<Vino> desmaterializarVinos(List<Bodega> bodegas)
        {
            List<Vino> vinos = new List<Vino>();

            try
            {
                string query = "SELECT * FROM Vino";
                using (var command = new SQLiteCommand(query, _connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int bodegasIndex = Convert.ToInt32(reader["id_bodega"]) - 1;

                            vinos.Add(new Vino(
                            Convert.ToInt32(reader["añada"]),
                            Convert.ToDateTime(reader["fecha_actualizacion"]),
                            Convert.ToBoolean(reader["imagen_etiqueta"]),
                            reader["nombre"].ToString(),
                            Convert.ToInt32(reader["nota_cata"]),
                            Convert.ToSingle(reader["precio_ars"]),
                            bodegas[bodegasIndex]
                            ));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al buscar Regiones: " + ex.Message);
            }
            return vinos;
        }

        public void desmaterializarVarietal(List<Vino> vinos)
        {

            try
            {
                string query = "SELECT * FROM Varietal";
                using (var command = new SQLiteCommand(query, _connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int vinoIndex = Convert.ToInt32(reader["id_vino"]) - 1;

                            Varietal varietal = new Varietal(
                                reader["descripcion"].ToString(),
                                Convert.ToInt32(reader["porcentajeComposicion"])
                                );
                            vinos[vinoIndex].agregarVarietal(varietal);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al buscar Varietal: " + ex.Message);
            }
        }

        public void desmaterializarReseñas(List<Vino> vinos)
        {

            try
            {
                string query = "SELECT * FROM Reseña";
                using (var command = new SQLiteCommand(query, _connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int vinoIndex = Convert.ToInt32(reader["id_vino"]) - 1;

                            Reseña reseña = new Reseña(
                                reader["comentario"].ToString(),
                                Convert.ToBoolean(reader["esPremium"]),
                                Convert.ToDateTime(reader["fecha_reseña"]),
                                Convert.ToInt32(reader["puntaje"])
                                );
                            vinos[vinoIndex].agregarReseña(reseña);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al buscar Reseña: " + ex.Message);
            }
        }

    }
}
