using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TallerMecanicoWeb.backend.pocos;
using TallerMecanicoWeb.backend.Servicios;

namespace TallerMecanicoWeb.Frontend
{
    public partial class detalleServicio : System.Web.UI.Page
    {
        ConsultaDetalleOrden consulta = new ConsultaDetalleOrden();

        private void CargarDetalles(int folio)
        {
            // Obtenemos la lista desde la clase ConsultaDetalleOrden.cs
            List<DetalleOrdenServicio> lista = consulta.ObtenerPorFolio(folio);

            if (lista != null && lista.Count > 0)
            {
                lblFolio.Text = "Folio: #" + folio;
                gvDetalles.DataSource = lista;
                gvDetalles.DataBind();

                // Calculamos el total usando la propiedad Importe de la clase POCO
                decimal total = lista.Sum(d => d.Importe);
                lblTotal.Text = total.ToString("C"); // Formato moneda
            }
            else
            {
                // Si la orden existe pero no tiene servicios aún
                lblFolio.Text = "Folio: #" + folio + " (Sin servicios registrados)";
                gvDetalles.DataSource = null;
                gvDetalles.DataBind();
                lblTotal.Text = "$0.00";
            }
        }

        protected void btnVolver_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Frontend/Servicios.aspx");
        }
    
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                // Capturamos el folio de la URL 
                if (Request.QueryString["folio"] != null)
                {
                    int folio;
                    if (int.TryParse(Request.QueryString["folio"], out folio))
                    {
                        CargarDetalles(folio);
                    }
                }
                else
                {
                    // Si no hay folio, regresamos a la lista
                    Response.Redirect("~/Frontend/Servicios.aspx");
                }
            }

        }
    }
}