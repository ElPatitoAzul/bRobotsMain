using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.SID
{
    public class Worker
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public string Search { get; set; }
        public string Data { get; set; }
        public string Estado { get; set; }
        public Preferences Preferences { get; set; }
        public int Level0 { get; set; }
        public string Name { get; set; }
        public string AccessToken { get; set; }
        public string Username { get; set; }
        public string UserId { get; set; }
    }
}
