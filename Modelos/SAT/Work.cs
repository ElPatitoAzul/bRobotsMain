using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.SAT
{
    public class Work
    {
        public string COOKIES { get; set; }
        public string VIEW_STATE { get; set; }
        public string CURP { get; set; }
        public string RFC { get; set; }
        public Guid Id { get; set; }
        public int UserId { get; set; }
    }

    public class WorkMoral
    {
        public string ?COOKIES { get; set; }
        public string ?VIEW_STATE { get; set; }
        public string ?RFC { get; set; }
        public Guid Id { get; set; }
        public int UserId { get; set; }
    }
}
