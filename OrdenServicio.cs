using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TallerMecanicoWeb.App_Start
{
    public class OrdenServicio
    {
        public int Folio { get; set;}
        public DateTime FechaIngreso { get; set;}
        public string Estado { get; set;}
        public decimal CostoTotal { get; set;}
        public string NumeroSerieVehiculo { get; set; }
    }
}