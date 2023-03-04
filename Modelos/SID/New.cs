using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.SID
{
    public class New
    {
        public string Type { get; set; }
        public string Search { get; set; }
        public string Data { get; set; }
        public string Estado { get; set; }
        public Preferences Preferences { get; set; }
        public int UserId { get; set; }
        public string UserIp { get; set; }
    }
}
