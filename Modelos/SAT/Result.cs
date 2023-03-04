using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.SAT
{
    public class Result
    {
        public string Resultado { get; set; }
        public string ViewState { get; set; }


        public class Search
        {
            public bool Found { get; set; }
            public bool Download { get; set; }
            public int CorteId { get; set; }
            public bool ValidToken { get; set; }
            public string Names { get; set; }
            public string Apellidos { get; set; }
            public string RFC { get; set; }
            public string CURP { get; set; }
            public string CIUDAD { get; set; }
            public string Estado { get; set; }
            public Access NewToken { get; set; }

        }

    }


}
