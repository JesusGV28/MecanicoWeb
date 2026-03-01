using MySql.Data.MySqlClient;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TallerMecanicoWeb.App_Start;
using TallerMecanicoWeb.backend.Coneccion;
using TallerMecanicoWeb.backend.pocos;
using TallerMecanicoWeb.backend.Servicios;

namespace TallerMecanicoWeb.Frontend
{
    public partial class frmServicios : System.Web.UI.Page
    {
        private Conexion bd = new Conexion();
        ConsultasVehiculo cVehiculo = new ConsultasVehiculo();
        ConsultasOrdenServicio cOrden = new ConsultasOrdenServicio();
        ConsultasServicio cServicio = new ConsultasServicio();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarClientes();
                CargarServicios();
                if (ddlClientes.Items.Count > 0)
                {
                    CargarVehiculos(Convert.ToInt32(ddlClientes.SelectedValue));
                }
            }
        }

        private void CargarServicios()
        {
            ddlServicios.DataSource = cServicio.ListarServicios();
            ddlServicios.DataTextField = "NombreServicio";
            ddlServicios.DataValueField = "IdServicio";
            ddlServicios.DataBind();
            ddlServicios.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Seleccione un servicio --", "0"));
        }

        private void CargarClientes()
        {
            ddlClientes.DataSource = cVehiculo.ListarClientes(); // Suponiendo que el método está ahí
            ddlClientes.DataTextField = "NombreCompleto"; // El nombre que armamos en el SQL
            ddlClientes.DataValueField = "IdCliente";
            ddlClientes.DataBind();
            ddlClientes.Items.Insert(0, new ListItem("-- Seleccione un Cliente --", "0"));
        }

        private void CargarVehiculos(int idCliente)
        {
            if (idCliente > 0)
            {
                ddlVehiculos.DataSource = cVehiculo.ListarVehiculosPorCliente(idCliente);
                ddlVehiculos.DataTextField = "Marca"; // Ajusta según tu clase Vehiculo
                ddlVehiculos.DataValueField = "NumeroSerie";
                ddlVehiculos.DataBind();
                ddlVehiculos.Items.Insert(0, new ListItem("-- Seleccione vehículo --", "0"));
            }
            else
            {
                ddlVehiculos.Items.Clear();
            }
        }

        // Este evento se dispara gracias al AutoPostBack="true"
        protected void ddlClientes_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idCliente = Convert.ToInt32(ddlClientes.SelectedValue);
            CargarVehiculos(idCliente);
        }


        private void LimpiarCampos()
        {
            TextBox1.Text = "";
            TextBox2.Text = "1";
            TextBox3.Text = "";
            TextBox4.Text = "";
        }



        protected void TextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        

        public bool InsertarOrdenConDetalle(OrdenServicio orden, DetalleOrdenServicio detalle)
        {
            bool exito = false;
            if (bd.AbrirConexion())
            {
                // Iniciamos una transacción
                SqlTransaction tr = bd.ObtenerConexion().BeginTransaction();
                try
                {
                    // 1. Insertar la Orden
                    string sqlOrden = "INSERT INTO OrdenServicio (fechaIngreso, fechaEntregaEstimada, estado, costoTotal, numeroSerieVehiculo) " +
                                      "VALUES (NOW(), @fechaE, @estado, @costo, @vin); SELECT LAST_INSERT_ID();";

                    SqlCommand cmdO = new SqlCommand(sqlOrden, bd.ObtenerConexion(), tr);
                    cmdO.Parameters.AddWithValue("@fechaE", DateTime.Now.AddDays(3)); // Ejemplo: 3 días estimada
                    cmdO.Parameters.AddWithValue("@estado", orden.Estado);
                    cmdO.Parameters.AddWithValue("@costo", orden.CostoTotal);
                    cmdO.Parameters.AddWithValue("@vin", orden.NumeroSerieVehiculo);

                    // Obtenemos el Folio recién generado
                    int nuevoFolio = Convert.ToInt32(cmdO.ExecuteScalar());

                    // 2. Insertar el Detalle usando el nuevo Folio
                    string sqlDetalle = "INSERT INTO DetalleOrdenServicio (folio, idServicio, descripcion, cantidad, precio) " +
                                        "VALUES (@folio, @idServ, @desc, @cant, @prec)";

                    SqlCommand cmdD = new SqlCommand(sqlDetalle, bd.ObtenerConexion(), tr);
                    cmdD.Parameters.AddWithValue("@folio", nuevoFolio);
                    cmdD.Parameters.AddWithValue("@idServ", detalle.IdServicio);
                    cmdD.Parameters.AddWithValue("@desc", detalle.Descripcion);
                    cmdD.Parameters.AddWithValue("@cant", detalle.Cantidad);
                    cmdD.Parameters.AddWithValue("@prec", detalle.Precio);

                    cmdD.ExecuteNonQuery();

                    // Si todo salió bien, guardamos cambios
                    tr.Commit();
                    exito = true;
                }
                catch (Exception ex)
                {
                    tr.Rollback(); // Si hubo error, deshacemos todo
                    Console.WriteLine("Error en transacción: " + ex.Message);
                }
                finally
                {
                    bd.CerrarConexion();
                }
            }
            return exito;
        }

        private void MostrarAlerta(string mensaje)
        {
            string script = $"alert('{mensaje}');";
            ScriptManager.RegisterStartupScript(this, GetType(), "ServerControlScript", script, true);
        }

        private void LimpiarSesion()
        {
            Session["Modo"] = null;
            Session["FolioSeleccionado"] = null;
        }


        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                // VALIDACIÓN MANUAL: Por si los validadores de ASP no atrapan algo
                if (string.IsNullOrEmpty(ddlVehiculos.SelectedValue) || ddlVehiculos.SelectedValue == "0")
                {
                    Response.Write("<script>alert('Error: Debe seleccionar un vehículo.');</script>");
                    return;
                }

                // 1. Llenar objeto Orden (según tu clase en App_Start)
                OrdenServicio orden = new OrdenServicio();
                orden.NumeroSerieVehiculo = ddlVehiculos.SelectedValue;
                orden.Estado = DropDownList2.SelectedValue;
                orden.FechaIngreso = DateTime.Now;
                // Calculamos costo total: cantidad * precio
                orden.CostoTotal = Convert.ToDecimal(TextBox3.Text) * Convert.ToInt32(TextBox2.Text);

                // 2. Llenar objeto Detalle (según tu clase en pocos)
                DetalleOrdenServicio detalle = new DetalleOrdenServicio();
                detalle.IdServicio = 1; // ID temporal de tu tabla Servicio
                detalle.Descripcion = TextBox1.Text;
                detalle.Cantidad = Convert.ToInt32(TextBox2.Text);
                detalle.Precio = Convert.ToDecimal(TextBox3.Text);

                string modo = Session["Modo"] != null ? Session["Modo"].ToString() : "Nuevo";

                if (modo == "Editar")
                {
                    orden.Folio = Convert.ToInt32(Session["FolioSeleccionado"]);
                    detalle.Folio = orden.Folio;
                }

                // 3. Intentar guardar
                if (cOrden.GuardarOrdenCompleta(orden, detalle, modo))
                {
                    Session["Modo"] = null;
                    Session["FolioSeleccionado"] = null;
                    // Asegúrate de que el nombre coincida exactamente, 
                    // si está en la raíz, no debe llevar la carpeta Frontend/
                    Response.Redirect("~/Frontend/Servicios.aspx");
                }
                else
                {
                    Response.Write("<script>alert('El método de base de datos devolvió FALSE.');</script>");
                }
            }
            catch (Exception ex)
            {
                // Esto te dirá exactamente qué falló (ej. "Input string was not in a correct format")
                Response.Write("<script>alert('Error: " + ex.Message + "');</script>");
            }
        }



        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }

        // Dentro de la clase ConsultasOrdenServicio
    }
}