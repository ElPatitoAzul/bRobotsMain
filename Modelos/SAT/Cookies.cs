using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.SAT
{
    public class Cookies
    {
        public string RFC { get; set; }
        public string IPC { get; set; }
        public string JSession { get; set; }
        public string F5 { get; set; }
        public string SetCookie { get; set; }

    }

    public class CookiesKeys
    {
        public string ZNPC { get; set; }
        public string RFC { get; set; }
        public string F5 { get; set; }
        public string JSESSIONID { get; set; }
        public string IPC { get; set; }
    }

    public static class Keys
    {
        public static string ZNPC = "ZNPCQ003-31383900";
        public static string RFC = "RFC_AMP_CUE_CONT_9080_e";
        public static string F5 = "F5-CONTENCION-rfcampe-443";
        public static string JSESSIONID = "JSESSIONID";
        public static string IPC = "IPCZQX0369a1f199";
    }





}
