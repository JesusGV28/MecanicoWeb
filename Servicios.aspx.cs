using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TallerMecanicoWeb.backend.Servicios;

namespace TallerMecanicoWeb
{
    public partial class Servicios : System.Web.UI.Page
    {
        ConsultasOrdenServicio consultas = new ConsultasOrdenServicio();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarGrid();
            }
        }

        private void CargarGrid()
        {
            GridViewDetalleOrdenServisio.DataSource = consultas.ListarOrdenes();
            GridViewDetalleOrdenServisio.DataBind();
        }

        protected void GridViewDetalleOrdenServisio_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            Session["Modo"] = "Nuevo";
            Session["FolioSeleccionado"] = null;
            // El símbolo ~/ indica la raíz del sitio web
            Response.Redirect("~/Frontend/frmServicios.aspx");
        }

        protected void btnDetalleservicio_Click(object sender, CommandEventArgs e)
        {
            string folio = e.CommandArgument.ToString();

            // 2. Redirigimos a la página de detalle pasando el folio por la URL
            // Nota: Asegúrate de que la ruta sea correcta (si está en la carpeta Frontend)
            // Cambia la ruta a relativa a la raíz
            Response.Redirect("~/Frontend/detalleServicio.aspx?folio=" + folio);
        }
    }
}