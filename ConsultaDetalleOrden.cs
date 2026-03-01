using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using TallerMecanicoWeb.backend.Coneccion;
using TallerMecanicoWeb.backend.pocos;

namespace TallerMecanicoWeb.backend.Servicios
{
    public class ConsultaDetalleOrden
    {
        private Conexion bd = new Conexion();

        public List<DetalleOrdenServicio> ObtenerPorFolio(int folio)
        {
            List<DetalleOrdenServicio> detalles = new List<DetalleOrdenServicio>();

            try
            {
                if (bd.AbrirConexion())
                {
                    string query = "SELECT * FROM DetalleOrdenServicio WHERE folio = @folio";
                    // Usamos ObtenerConexion() de tu clase Conexion
                    SqlCommand cmd = new SqlCommand(query, bd.ObtenerConexion());
                    cmd.Parameters.AddWithValue("@folio", folio);

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            detalles.Add(new DetalleOrdenServicio
                            {
                                // Usamos una función auxiliar para verificar si el valor es DBNull
                                Folio = dr["folio"] != DBNull.Value ? Convert.ToInt32(dr["folio"]) : 0,
                                IdServicio = dr["idServicio"] != DBNull.Value ? Convert.ToInt32(dr["idServicio"]) : 0,
                                Descripcion = dr["descripcion"] != DBNull.Value ? dr["descripcion"].ToString() : "",
                                Cantidad = dr["cantidad"] != DBNull.Value ? Convert.ToInt32(dr["cantidad"]) : 0,
                                Precio = dr["precio"] != DBNull.Value ? Convert.ToDecimal(dr["precio"]) : 0m
                            });
                        }
                    }
                }
            }
            finally
            {
                bd.CerrarConexion();
            }
            return detalles;
        }

        public bool InsertarDetalle(DetalleOrdenServicio detalle)
        {
            try
            {
                if (bd.AbrirConexion())
                {
                    string query = @"INSERT INTO DetalleOrdenServicio (folio, idServicio, descripcion, cantidad, precio) 
                                 VALUES (@folio, @idServicio, @desc, @cant, @precio)";

                    SqlCommand cmd = new SqlCommand(query, bd.ObtenerConexion());
                    cmd.Parameters.AddWithValue("@folio", detalle.Folio);
                    cmd.Parameters.AddWithValue("@idServicio", detalle.IdServicio);
                    cmd.Parameters.AddWithValue("@desc", detalle.Descripcion);
                    cmd.Parameters.AddWithValue("@cant", detalle.Cantidad);
                    cmd.Parameters.AddWithValue("@precio", detalle.Precio);

                    return cmd.ExecuteNonQuery() > 0;
                }
                return false;
            }
            finally
            {
                bd.CerrarConexion();
            }
        }
    }
}