using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.Robots
{
    public class GetAllRobotsModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string AccessToken { get; set; }
        public string Status { get; set; }
        public string Source { get; set; }
        public string System { get; set; }
        public int? Limit { get; set; }
        public int? Current { get; set; }
    }
}
