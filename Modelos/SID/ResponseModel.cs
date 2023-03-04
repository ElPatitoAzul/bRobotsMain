using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.SID
{
    public class ResponseModel
    {
        public Guid Id { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string FechaNac { get; set; }
        public string Cadena { get; set; }
        public string Tipo { get; set; }
        public string Busqueda { get; set; }
        public string CURP { get; set; }
        public string Estado { get; set; }
        public bool Found { get; set; }
        public string Comments { get; set; }
        public int CorteId { get; set; }
        public HttpStatusCode StatusResponse { get; set; }

    }
}
