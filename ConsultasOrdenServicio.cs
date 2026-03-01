using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using TallerMecanicoWeb.App_Start;
using TallerMecanicoWeb.backend.Coneccion;
using TallerMecanicoWeb.backend.pocos;

namespace TallerMecanicoWeb.backend.Servicios
{
    public class ConsultasOrdenServicio
    {
        private Conexion bd = new Conexion();

        public List<OrdenServicio> ListarOrdenes()
        {
            List<OrdenServicio> lista = new List<OrdenServicio>();
            try
            {
                if (bd.AbrirConexion())
                {
                    string query = "SELECT folio, fechaIngreso, estado, costoTotal, numeroSerieVehiculo FROM OrdenServicio";
                    SqlCommand cmd = new SqlCommand(query, bd.ObtenerConexion());

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new OrdenServicio
                            {
                                Folio = dr["folio"] != DBNull.Value ? Convert.ToInt32(dr["folio"]) : 0,
                                FechaIngreso = dr["fechaIngreso"] != DBNull.Value ? Convert.ToDateTime(dr["fechaIngreso"]) : DateTime.MinValue,
                                Estado = dr["estado"] != DBNull.Value ? dr["estado"].ToString() : "",
                                CostoTotal = dr["costoTotal"] != DBNull.Value ? Convert.ToDecimal(dr["costoTotal"]) : 0m,
                                NumeroSerieVehiculo = dr["numeroSerieVehiculo"] != DBNull.Value ? dr["numeroSerieVehiculo"].ToString() : ""
                            });
                        }
                    }
                }
            }
            finally
            {
                bd.CerrarConexion();
            }
            return lista;
        }

        public bool ActualizarOrden(OrdenServicio orden)
        {
            try
            {
                if (bd.AbrirConexion())
                {
                    string query = @"UPDATE OrdenServicio 
                                     SET estado = @estado, costoTotal = @costo, fechaEntregaReal = @fecha 
                                     WHERE folio = @folio";

                    SqlCommand cmd = new SqlCommand(query, bd.ObtenerConexion());
                    cmd.Parameters.AddWithValue("@estado", orden.Estado);
                    cmd.Parameters.AddWithValue("@costo", orden.CostoTotal);
                    cmd.Parameters.AddWithValue("@fecha", DateTime.Now);
                    cmd.Parameters.AddWithValue("@folio", orden.Folio);

                    return cmd.ExecuteNonQuery() > 0;
                }
                return false;
            }
            finally
            {
                bd.CerrarConexion();
            }
        }

        public List<OrdenServicio> ListarOrdenesDetalle()
        {
            List<OrdenServicio> lista = new List<OrdenServicio>();
            try
            {
                if (bd.AbrirConexion())
                {
                    // Nota: Asegúrate de que esta consulta tenga sentido con los campos de OrdenServicio
                    string query = "SELECT folio, fechaIngreso, estado, costoTotal, numeroSerieVehiculo FROM DetalleOrdenServicio";
                    SqlCommand cmd = new SqlCommand(query, bd.ObtenerConexion());

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new OrdenServicio
                            {
                                // Validamos cada columna para evitar el error de DBNull
                                Folio = dr["folio"] != DBNull.Value ? Convert.ToInt32(dr["folio"]) : 0,

                                FechaIngreso = dr["fechaIngreso"] != DBNull.Value ? Convert.ToDateTime(dr["fechaIngreso"]) : DateTime.MinValue,

                                Estado = dr["estado"] != DBNull.Value ? dr["estado"].ToString() : "",

                                CostoTotal = dr["costoTotal"] != DBNull.Value ? Convert.ToDecimal(dr["costoTotal"]) : 0m,

                                NumeroSerieVehiculo = dr["numeroSerieVehiculo"] != DBNull.Value ? dr["numeroSerieVehiculo"].ToString() : ""
                            });
                        }
                    }
                }
            }
            finally
            {
                bd.CerrarConexion();
            }
            return lista;
        }

        public bool InsertarOrdenConDetalle(OrdenServicio orden, DetalleOrdenServicio detalle)
        {
            Conexion bd = new Conexion();
            if (bd.AbrirConexion())
            {
                SqlTransaction transaccion = bd.ObtenerConexion().BeginTransaction();
                try
                {
                    // Insertar Orden y recuperar el Folio (IDENTITY)
                    string sqlOrden = "INSERT INTO OrdenServicio (fechaIngreso, estado, costoTotal, numeroSerieVehiculo) " +
                                      "VALUES (GETDATE(), @estado, @costo, @vin); SELECT SCOPE_IDENTITY();";

                    SqlCommand cmdO = new SqlCommand(sqlOrden, bd.ObtenerConexion(), transaccion);
                    cmdO.Parameters.AddWithValue("@estado", orden.Estado);
                    cmdO.Parameters.AddWithValue("@costo", orden.CostoTotal);
                    cmdO.Parameters.AddWithValue("@vin", orden.NumeroSerieVehiculo);

                    int nuevoFolio = Convert.ToInt32(cmdO.ExecuteScalar());

                    // Insertar Detalle
                    string sqlDetalle = "INSERT INTO DetalleOrdenServicio (folio, idServicio, descripcion, cantidad, precio) " +
                                        "VALUES (@folio, @idServ, @desc, @cant, @prec)";

                    SqlCommand cmdD = new SqlCommand(sqlDetalle, bd.ObtenerConexion(), transaccion);
                    cmdD.Parameters.AddWithValue("@folio", nuevoFolio);
                    cmdD.Parameters.AddWithValue("@idServ", detalle.IdServicio);
                    cmdD.Parameters.AddWithValue("@desc", detalle.Descripcion);
                    cmdD.Parameters.AddWithValue("@cant", detalle.Cantidad);
                    cmdD.Parameters.AddWithValue("@prec", detalle.Precio);

                    cmdD.ExecuteNonQuery();
                    transaccion.Commit();
                    return true;
                }
                catch
                {
                    transaccion.Rollback();
                    return false;
                }
                finally { bd.CerrarConexion(); }
            }
            return false;
        }

        public bool GuardarOrdenCompleta(OrdenServicio orden, DetalleOrdenServicio detalle, string modo)
        {
            Conexion bd = new Conexion();
            using (SqlConnection con = bd.ObtenerConexion())
            {
                con.Open();
                SqlTransaction transaccion = con.BeginTransaction();

                try
                {
                    if (modo == "Nuevo")
                    {
                        // 1. Insertar Orden y obtener el Folio generado (SCOPE_IDENTITY)
                        string sqlOrden = @"INSERT INTO OrdenServicio (fechaIngreso, estado, costoTotal, numeroSerieVehiculo) 
                                   VALUES (GETDATE(), @estado, @costo, @vin);
                                   SELECT SCOPE_IDENTITY();";

                        SqlCommand cmdO = new SqlCommand(sqlOrden, con, transaccion);
                        cmdO.Parameters.AddWithValue("@estado", orden.Estado);
                        cmdO.Parameters.AddWithValue("@costo", orden.CostoTotal);
                        cmdO.Parameters.AddWithValue("@vin", orden.NumeroSerieVehiculo);

                        int nuevoFolio = Convert.ToInt32(cmdO.ExecuteScalar());

                        // 2. Insertar Detalle
                        string sqlDetalle = @"INSERT INTO DetalleOrdenServicio (folio, idServicio, descripcion, cantidad, precio) 
                                     VALUES (@folio, @idServicio, @desc, @cant, @precio)";

                        SqlCommand cmdD = new SqlCommand(sqlDetalle, con, transaccion);
                        cmdD.Parameters.AddWithValue("@folio", nuevoFolio);
                        cmdD.Parameters.AddWithValue("@idServicio", detalle.IdServicio);
                        cmdD.Parameters.AddWithValue("@desc", detalle.Descripcion);
                        cmdD.Parameters.AddWithValue("@cant", detalle.Cantidad);
                        cmdD.Parameters.AddWithValue("@precio", detalle.Precio);
                        cmdD.ExecuteNonQuery();
                    }
                    else // Modo Editar
                    {
                        string sqlUpdate = @"UPDATE OrdenServicio 
                                    SET estado = @estado, costoTotal = @costo 
                                    WHERE folio = @folio";

                        SqlCommand cmdU = new SqlCommand(sqlUpdate, con, transaccion);
                        cmdU.Parameters.AddWithValue("@estado", orden.Estado);
                        cmdU.Parameters.AddWithValue("@costo", orden.CostoTotal);
                        cmdU.Parameters.AddWithValue("@folio", orden.Folio);
                        cmdU.ExecuteNonQuery();
                    }

                    transaccion.Commit();
                    return true;
                }
                catch (Exception)
                {
                    transaccion.Rollback();
                    return false;
                }
            }
        }


    }
}