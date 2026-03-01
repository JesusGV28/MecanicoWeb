using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace TallerMecanicoWeb.backend.Coneccion
{
    public class Conexion
    {
        private SqlConnection conexion;

        public Conexion()
        {
            // Leemos la cadena de conexión directamente del Web.config de forma segura
            string cadena = ConfigurationManager.ConnectionStrings["MiConexion"].ConnectionString;
            conexion = new SqlConnection(cadena);
        }

        public bool AbrirConexion()
        {
            try
            {
                if (conexion.State == ConnectionState.Closed)
                {
                    conexion.Open();
                    return true;
                }
                return true;
            }
            catch (Exception ex)
            {
                // Si falla, aquí sabremos por qué
                throw new Exception("Error al abrir conexión: " + ex.Message);
            }
        }

        public void CerrarConexion()
        {
            if (conexion != null && conexion.State == ConnectionState.Open)
                conexion.Close();
        }

        public SqlConnection ObtenerConexion()
        {
            return conexion;
        }
    }
}