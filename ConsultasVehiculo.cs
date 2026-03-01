using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using TallerMecanicoWeb.backend.Coneccion;
using TallerMecanicoWeb.backend.pocos;

namespace TallerMecanicoWeb.backend.Servicios
{
    public class ConsultasVehiculo
    {
        private Conexion bd = new Conexion();

        // Agrega esto a ConsultasVehiculo.cs o crea una nueva clase ConsultasCliente.cs
        public List<Cliente> ListarClientes()
        {
            List<Cliente> lista = new List<Cliente>();
            try
            {
                if (bd.AbrirConexion())
                {
                    string query = "SELECT idCliente, nombre + ' ' + apellidoPaterno AS nombreCompleto FROM Cliente";
                    SqlCommand cmd = new SqlCommand(query, bd.ObtenerConexion());
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Cliente
                            {
                                IdCliente = dr["idCliente"] != DBNull.Value ? Convert.ToInt32(dr["idCliente"]) : 0,
                                NombreCompleto = dr["nombreCompleto"] != DBNull.Value ? dr["nombreCompleto"].ToString() : ""
                            });
                        }
                    }
                }
            }
            finally { bd.CerrarConexion(); }
            return lista;
        }

        // Actualiza o agrega este método para filtrar vehículos
        public List<Vehiculo> ListarVehiculosPorCliente(int idCliente)
        {
            List<Vehiculo> lista = new List<Vehiculo>();
            try
            {
                if (bd.AbrirConexion())
                {
                    string query = "SELECT numeroSerie, placas, marca + ' ' + modelo AS info FROM Vehiculo WHERE idCliente = @id";
                    SqlCommand cmd = new SqlCommand(query, bd.ObtenerConexion());
                    cmd.Parameters.AddWithValue("@id", idCliente);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Vehiculo
                            {
                                NumeroSerie = dr["numeroSerie"].ToString(),
                                Placas = dr["placas"].ToString(),
                                Marca = dr["info"].ToString()
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