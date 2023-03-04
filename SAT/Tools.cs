using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SAT
{
    public class Tools
    {
        public HtmlDocument StringHTML(string SOURCE)
        {
            var _doc = new HtmlDocument();
            _doc.LoadHtml(SOURCE);
            return _doc;
        }

        //public string ManipulateCookies(string[] COOKIES)
        //{
        //    var _cookies = string.Empty;
        //    foreach(var cookie in COOKIES)
        //    {
        //        var _important = cookie.Split(';')[0];
        //        _cookies += $"{_important}; ";
        //    }
        //    return _cookies;
        //}

        //public CookieContainer SetCookies( HttpResponseHeaders Headers)
        //{
        //    var cookieContainer = new CookieContainer();
        //    var _cookies = ManipulateHeader(Headers);
        //    cookieContainer.Add(new Cookie());
        //}

        public Modelos.SAT.Cookies ManipulateHeader(HttpResponseHeaders HEADER)
        {
            var _RFC = string.Empty;
            var _IPC = string.Empty;
            var _JSession = string.Empty;
            var _F5Connect = string.Empty;
            var _setCookie = string.Empty;

            List<string> _cookies = new List<string>();
            foreach (var _header in HEADER)
            {
                if(_header.Key.Contains("Set-Cookie"))
                {
                    foreach (var _cookie in _header.Value)
                    {
                        var _important = _cookie.Split(';')[0];

                        if (_important.Contains("RFC_AMP")) _RFC = _important;
                        else if (_important.Contains("IPCZQX0369a1f199")) _IPC = _important;
                        else if (_important.Contains("JSESSIONID")) _JSession = _important;
                        else if (_important.Contains("F5-CONTENCION")) _F5Connect = _important;
                        _setCookie += $"{_important}; ";
                    }
                }
            }

            var _response = new Modelos.SAT.Cookies();
            _response.RFC = _RFC;
            _response.IPC = _IPC;
            _response.JSession = _JSession;
            _response.F5 = _F5Connect;
            _response.SetCookie = _setCookie;

            return _response;
        }


        public string ReplaceCookie(string COOKIES, string KEY, string VALUE)
        {
            var parts = COOKIES.Split(';');
            var _cookies = "";
  
            foreach(var _part in parts)
            {
                var _splitv = _part.Split('=');
                var _values = "";
                if (_splitv[0] == KEY)
                {
                    _splitv[1] = VALUE;
                    _values = $"{KEY}={VALUE}";
                }
                else _values = _part;

                if (_values.Length > 0) _values += ';';
                _cookies += $"{_values}";

            }

            return _cookies;
        }

        public string FindCookieValue(string COOKIES, string KEY)
        {
            var _value = "";
            var parts = COOKIES.Split(';');
            foreach(var _part in parts)
            {
                var _splitv = _part.Split('=');
                if (_splitv[0] == KEY) _value = _splitv[1];
            }

            return _value;
        }

        public string ManipulateCDATA(string INNERHTML)
        {
            try
            {
                var _txt = INNERHTML;
                var _iR = _txt.IndexOf("CDATA");
                var _lR = _txt.Length;
                var _data = _txt.Substring(_iR! + 6, _lR! - _iR! - 9);
                return _data;
            }
            catch
            {
                return "";
            }

        }

        private readonly static List<string> _states = new List<string>() { 
            "CHIAPAS", 
            "BAJA CALIFORNIA SUR", 
            "BAJA CALIFORNIA", 
            "YUCATAN", 
            "VERACRUZ",
            "VERACRUZ DE IGNACIO DE LA",
            "VERACRUZ DE IGNACIO DE LA LLAVE",
            "COAHUILA","COAHUILA DE ZARAGOZA",
            "MICHOACAN", "MICHOACAN DE OCAMPO",
            "TLAXCALA", "DURANGO", "AGUASCALIENTES",
            "HIDALGO","PUEBLA","QUERETARO","CHIHUAHUA", 
            "OAXACA","SONORA","SAN LUIS POTOSI","SINALOA",
            "GUERRERO","ZACATECAS", "TAMAULIPAS", "MORELOS", 
            "TABASCO", "GUANAJUATO", "COLIMA", "JALISCO", 
            "CIUDAD DE MEXICO", "CDMX", "CAMPECHE", "NUEVO LEON", 
            "MEXICO","CIUDAD DE MEXICO",
            "QUINTANA ROO",
            "NAYARIT"
        };

        public string STATE(string FULLTEXT)
        {

            var _state = _states.Where(d => FULLTEXT.Contains(d) ).FirstOrDefault();
            if (_state != null)
            {
                return _state;
            }
            return "";
        }


    }
}
