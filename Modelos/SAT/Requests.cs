using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.SAT
{
    public class Requests
    {
        public string CURP { get; set; }
        public string RFC { get; set; }
        public int UserId { get; set; }
        public string UserIp { get; set; }

    }

    public class MoralRequest
    {
        public string RFC { set; get; }
        public int UserId { get; set; }
        public string UserIp { get; set; }
    }
}
