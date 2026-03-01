using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TallerMecanicoWeb.backend.pocos
{
    public class DetalleOrdenServicio
    {
        public int Folio { get; set; }           // Llave primaria compuesta
        public int IdServicio { get; set; }      // Llave primaria compuesta
        public string Descripcion { get; set; }
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }

        // Propiedad calculada (Importe es PERSISTED en SQL)
        public decimal Importe => Cantidad * Precio;
    }
}