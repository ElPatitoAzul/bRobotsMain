using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.Peticiones
{
    public class MyReqsModel
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public string Search { get; set; }
        public string CURP { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string RFC { get; set; }
        public string Ciudad { get; set; }
        public string Estado { get; set; }
        public int? UserId { get; set; }
        public string Comments { get; set; }
        public int? TransposeId { get; set; }
        public bool Downloaded { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
