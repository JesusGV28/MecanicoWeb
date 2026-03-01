using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Services.Description;
using TallerMecanicoWeb.backend.Coneccion;
using TallerMecanicoWeb.backend.pocos;

namespace TallerMecanicoWeb.backend.Servicios
{
    public class ConsultasServicio
    {
        private Conexion bd = new Conexion();

        public List<Servicio> ListarServicios()
        {
            List<Servicio> lista = new List<Servicio>();
            try
            {
                if (bd.AbrirConexion())
                {
                    string query = "SELECT idServicio, nombreServicio FROM Servicio";
                    SqlCommand cmd = new SqlCommand(query, bd.ObtenerConexion());
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Servicio
                            {
                                IdServicio = Convert.ToInt32(dr["idServicio"]),
                                NombreServicio = dr["nombreServicio"].ToString()
                            });
                        }
                    }
                }
            }
            finally { bd.CerrarConexion(); }
            return lista;
        }
    }
}