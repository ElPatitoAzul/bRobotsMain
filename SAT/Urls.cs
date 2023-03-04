using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAT
{
    public class Urls
    {
        public string Init { get; } = "https://auth.siat.sat.gob.mx/nidp/idff/sso?id=pe-fiel-empl&sid=0&option=credential&sid=0&target=https%3A%2F%2Frfcampe.siat.sat.gob.mx%2Fapp%2Fseg%2Femp%2FaccesoEF%3Furl%3Dhttps%3A%2F%2Frfcampe.siat.sat.gob.mx%2Fapp%2FPE%2FIdcSiat%2FSACVisorTributario%2FSACBusquedaVisorTributario.jsf";
        public string Visor { get; } = "https://rfcampe.siat.sat.gob.mx/app/PE/IdcSiat/SACVisorTributario/SACBusquedaVisorTributario.jsf";
        public string Download { get; } = "https://rfcampe.siat.sat.gob.mx/app/PE/IdcSiat/IdcGeneraConstancia.jsf";

    }
}
