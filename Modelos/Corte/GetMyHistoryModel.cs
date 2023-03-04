using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.Corte
{
    public class GetMyHistoryModel
    {
        public int id { get; set; }
        public string? document { get; set; }
        public string? state { get; set; }
        public string? dataset { get; set; }
        public string? nameinside { get; set; }
        public int? level0 { get; set; }
        public int? level1 { get; set; }
        public int? level2 { get; set; }
        public int? level3 { get; set; }
        public int? level4 { get; set; }
        public int? level5 { get; set; }
        public DateOnly? corte { get; set; }
        public int? idcreated { get; set; }
        public DateTime? createdAt { get; set; }
    }
}
