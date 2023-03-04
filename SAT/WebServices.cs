using BackRobotTDM.Scripts.SID.Models;
using Modelos.SAT;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace SAT
{
    public class WebServices
    {
        private readonly static Urls _urls = new Urls();
        private readonly static string _api = "https://auth.siat.sat.gob.mx/nidp/idff";

        // 0
        public async Task<HttpResponseMessage> Init()
        {
            var http = new HttpClient();
            var _result = await http.GetAsync(_urls.Init);
            return _result;
        }
        // 1
        public async Task<HttpResponseMessage> LogIn(string SET_COOKIE, string guid, string lGuid)
        {
            var http = new HttpClient();
            var _route = $"{_api}/sso?id=pe-fiel-empl&sid=0&option=credential&sid=0&target=https%3A%2F%2Frfcampe.siat.sat.gob.mx%2Fapp%2Fseg%2Femp%2FaccesoEF%3Furl%3Dhttps%3A%2F%2Frfcampe.siat.sat.gob.mx%2Fapp%2FPE%2FIdcSiat%2FSACVisorTributario%2FSACBusquedaVisorTributario.jsf";
            //http.DefaultRequestHeaders.Clear();
            http.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
            http.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
            http.DefaultRequestHeaders.Add("Accept-Language", "es-ES,es;q=0.9");
            http.DefaultRequestHeaders.Add("Cache-Control", "max-age=0");
            http.DefaultRequestHeaders.Add("Connection", "keep-alive");
            //http.DefaultRequestHeaders.Add("Content-Type", "application/x-www-form-urlencoded");
            //JSESSIONID=D83E098ECAB6F6EC562138DF950D6502; UrnNovellNidpClusterMemberId=~03~02fbf~1B~1A~1F~7B~7E; F5-PROD-SIAT-AUTH-443=!Gk1ZOpbYPiddfCefEf4+RrzZf1MVP/s9uQ9KYNMvUtBPByHKA6Riwmspzn6Oj143iog5dQZVMw4LMQ==
            http.DefaultRequestHeaders.Add("Cookie", SET_COOKIE);
            http.DefaultRequestHeaders.Add("Host", "auth.siat.sat.gob.mx");
            http.DefaultRequestHeaders.Add("Origin", "https://auth.siat.sat.gob.mx");
            http.DefaultRequestHeaders.Add("Referer", "https://auth.siat.sat.gob.mx/nidp/idff/sso?id=pe-fiel-empl&sid=0&option=credential&sid=0&target=https%3A%2F%2Frfcampe.siat.sat.gob.mx%2Fapp%2Fseg%2Femp%2FaccesoEF%3Furl%3Dhttps%3A%2F%2Frfcampe.siat.sat.gob.mx%2Fapp%2FPE%2FIdcSiat%2FSACVisorTributario%2FSACBusquedaVisorTributario.jsf");
            http.DefaultRequestHeaders.Add("sec-ch-ua", "\"Chromium\";v=\"106\", \"Not.A/Brand\";v=\"24\", \"Opera\";v=\"92\"");
            http.DefaultRequestHeaders.Add("sec-ch-ua-mobile", "?0");
            http.DefaultRequestHeaders.Add("sec-ch-ua-platform", "\"Windows\"");
            http.DefaultRequestHeaders.Add("Sec-Fetch-Dest", "document");
            http.DefaultRequestHeaders.Add("Sec-Fetch-Mode", "navigate");
            http.DefaultRequestHeaders.Add("Sec-Fetch-Site", "same-origin");
            http.DefaultRequestHeaders.Add("Sec-Fetch-User", "?1");
            http.DefaultRequestHeaders.Add("Upgrade-Insecure-Requests", "1");
            http.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/106.0.0.0 Safari/537.36 OPR/92.0.0.0");

            var _content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("seeder", ""),
                new KeyValuePair<string, string>("arc", ""),
                new KeyValuePair<string, string>("tan", ""),
                new KeyValuePair<string, string>("placer", ""),
                new KeyValuePair<string, string>("ip", "189.250.56.217"),
                new KeyValuePair<string, string>("secuence", ""),
                new KeyValuePair<string, string>("option", "credential"),
                new KeyValuePair<string, string>("token", "V1hwb2EwNVVWbXhaZWtGMFdrZGFiRmxwTURCTmVrRXpURmRLYUU5VWEzUk5SMUV4VDBSbk5VMVVWbTFQUjBsM2ZGQlBRVTA0T1RBMU1ERkxWVGg4TURBd01ERXdNREF3TURBMU1ETTBOVFV5TnprPSNaR3hMY2xCMGFuaHlhMjVZU21wTlJISnVTMFUzU0dsRk1qTm9lV3BSUzNsWmJuTktTVlJPVEd0bWNEWllla3hwTWpKYVlXTklPR05GYWxjck1YRm1WVEpPVWxsdmJIcFpjV0pCVVV0M05tdGxiVkpRU1hSTlluZFJZVlY1YldkNGQzSm9aMmt4VEc1TWJEUXlRM042U0VkSE5WSXhaakpNUkhOaUswdFJaV2hLT0RWUk9HUllSelZHVml0RWFYbHljbUphTTA1b09VaHNWMjExU1VSVVEzbHliMDFPWkZkM2RXWmpibnBsYmtOS04wOUlOemRXVWpKSFNHd3lZbTFrWlZkQlJrTkNOR0pxT1dkWWFVZFZiMG8wUVRaUVNuZFRiVk4yUm1oRmFFd3dUV3RoT0M5c1NIbEJPWFpPUTNSdFJIQndWV2xySzBKUGFqTTJUbWhGZEdGQ2R5OTBRamhUY205RFRHaGxaemt4TkZWRGMxUlhSblk0YzBOT1YzVkpORk5zV0VoUWVsRm1lVWhWZEdoSU1sUnRSR2xaV25KNWFYQTNaRFpLTVZsQ1ZEQjJja0paTldVNU5ITjRaV0V6WjBoR1ZXSlJQVDA9"),
                new KeyValuePair<string, string>("credentialsProvided", ""),
                new KeyValuePair<string, string>("guid", guid),
                new KeyValuePair<string, string>("lGuid", lGuid),
                new KeyValuePair<string, string>("credentialsRequired", "CERT"),
                new KeyValuePair<string, string>("ks", "null"),
            });

            var _result = await http.PostAsync(_route, _content);
            return _result;
        }
        // 2
        public async Task<HttpResponseMessage> SSOSID(string SET_COOKIES)
        {
            var http = new HttpClient();
            var _route = $"https://auth.siat.sat.gob.mx/nidp/idff/sso?sid=0";

            http.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
            http.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
            http.DefaultRequestHeaders.Add("Accept-Language", "es-ES,es;q=0.9");
            http.DefaultRequestHeaders.Add("Cache-Control", "max-age=0");
            http.DefaultRequestHeaders.Add("Connection", "keep-alive");
            //http.DefaultRequestHeaders.Add("Content-Type", "application/x-www-form-urlencoded");
            //JSESSIONID=D83E098ECAB6F6EC562138DF950D6502; F5-PROD-SIAT-AUTH-443=!Gk1ZOpbYPiddfCefEf4+RrzZf1MVP/s9uQ9KYNMvUtBPByHKA6Riwmspzn6Oj143iog5dQZVMw4LMQ==
            http.DefaultRequestHeaders.Add("Cookie", $"{SET_COOKIES}");
            http.DefaultRequestHeaders.Add("Host", "auth.siat.sat.gob.mx");
            http.DefaultRequestHeaders.Add("Origin", "https://auth.siat.sat.gob.mx");
            http.DefaultRequestHeaders.Add("Referer", "https://auth.siat.sat.gob.mx/nidp/idff/sso?id=pe-fiel-empl&sid=0&option=credential&sid=0&target=https%3A%2F%2Frfcampe.siat.sat.gob.mx%2Fapp%2Fseg%2Femp%2FaccesoEF%3Furl%3Dhttps%3A%2F%2Frfcampe.siat.sat.gob.mx%2Fapp%2FPE%2FIdcSiat%2FSACVisorTributario%2FSACBusquedaVisorTributario.jsf");
            http.DefaultRequestHeaders.Add("sec-ch-ua", "\"Chromium\";v=\"106\", \"Not.A/Brand\";v=\"24\", \"Opera\";v=\"92\"");
            http.DefaultRequestHeaders.Add("sec-ch-ua-mobile", "?0");
            http.DefaultRequestHeaders.Add("sec-ch-ua-platform", "\"Windows\"");
            http.DefaultRequestHeaders.Add("Sec-Fetch-Dest", "document");
            http.DefaultRequestHeaders.Add("Sec-Fetch-Mode", "navigate");
            http.DefaultRequestHeaders.Add("Sec-Fetch-Site", "same-origin");
            http.DefaultRequestHeaders.Add("Sec-Fetch-User", "?1");
            http.DefaultRequestHeaders.Add("Upgrade-Insecure-Requests", "1");
            http.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/106.0.0.0 Safari/537.36 OPR/92.0.0.0");
            var _result = await http.GetAsync(_route);
            return _result;
        }
        // 3
        public async Task<HttpResponseMessage> SPASSERTION(string SET_COOKIES)
        {
            var http = new HttpClient();
            var _route = $"https://rfcampe.siat.sat.gob.mx/nesp/idff/spassertion_consumer?SAMLart=AAPG7xPGeCWXsT47w%2BeNh9SJhlYF5woLfU%2F14Har5Jv64LBpVqMxDXW6&RelayState=MA%3D%3D";

            http.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
            http.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
            http.DefaultRequestHeaders.Add("Accept-Language", "es-ES,es;q=0.9");
            http.DefaultRequestHeaders.Add("Cache-Control", "max-age=0");
            http.DefaultRequestHeaders.Add("Connection", "keep-alive");
            //http.DefaultRequestHeaders.Add("Content-Type", "application/x-www-form-urlencoded");
            //JSESSIONID=D83E098ECAB6F6EC562138DF950D6502; F5-PROD-SIAT-AUTH-443=!Gk1ZOpbYPiddfCefEf4+RrzZf1MVP/s9uQ9KYNMvUtBPByHKA6Riwmspzn6Oj143iog5dQZVMw4LMQ==
            http.DefaultRequestHeaders.Add("Cookie", $"{SET_COOKIES}");
            http.DefaultRequestHeaders.Add("Host", "auth.siat.sat.gob.mx");
            http.DefaultRequestHeaders.Add("Origin", "https://auth.siat.sat.gob.mx");
            http.DefaultRequestHeaders.Add("Referer", "https://auth.siat.sat.gob.mx/nidp/idff/sso?id=pe-fiel-empl&sid=0&option=credential&sid=0&target=https%3A%2F%2Frfcampe.siat.sat.gob.mx%2Fapp%2Fseg%2Femp%2FaccesoEF%3Furl%3Dhttps%3A%2F%2Frfcampe.siat.sat.gob.mx%2Fapp%2FPE%2FIdcSiat%2FSACVisorTributario%2FSACBusquedaVisorTributario.jsf");
            http.DefaultRequestHeaders.Add("sec-ch-ua", "\"Chromium\";v=\"106\", \"Not.A/Brand\";v=\"24\", \"Opera\";v=\"92\"");
            http.DefaultRequestHeaders.Add("sec-ch-ua-mobile", "?0");
            http.DefaultRequestHeaders.Add("sec-ch-ua-platform", "\"Windows\"");
            http.DefaultRequestHeaders.Add("Sec-Fetch-Dest", "document");
            http.DefaultRequestHeaders.Add("Sec-Fetch-Mode", "navigate");
            http.DefaultRequestHeaders.Add("Sec-Fetch-Site", "same-origin");
            http.DefaultRequestHeaders.Add("Sec-Fetch-User", "?1");
            http.DefaultRequestHeaders.Add("Upgrade-Insecure-Requests", "1");
            http.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/106.0.0.0 Safari/537.36 OPR/92.0.0.0");
            var _result = await http.GetAsync(_route);
            return _result;
        }
        // 4
        public async Task<HttpResponseMessage> AccessEF(string SET_COOKIES)
        {
            var http = new HttpClient();
            var _route = $"https://rfcampe.siat.sat.gob.mx/app/seg/emp/accesoEF?url=https://rfcampe.siat.sat.gob.mx/app/PE/IdcSiat/SACVisorTributario/SACBusquedaVisorTributario.jsf";


            http.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
            http.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
            http.DefaultRequestHeaders.Add("Accept-Language", "es-ES,es;q=0.9");
            http.DefaultRequestHeaders.Add("Cache-Control", "max-age=0");
            http.DefaultRequestHeaders.Add("Connection", "keep-alive");
            //http.DefaultRequestHeaders.Add("Content-Type", "application/x-www-form-urlencoded");
            //JSESSIONID=D83E098ECAB6F6EC562138DF950D6502; F5-PROD-SIAT-AUTH-443=!Gk1ZOpbYPiddfCefEf4+RrzZf1MVP/s9uQ9KYNMvUtBPByHKA6Riwmspzn6Oj143iog5dQZVMw4LMQ==
            http.DefaultRequestHeaders.Add("Cookie", $"{SET_COOKIES}");
            http.DefaultRequestHeaders.Add("Host", "auth.siat.sat.gob.mx");
            http.DefaultRequestHeaders.Add("Origin", "https://auth.siat.sat.gob.mx");
            http.DefaultRequestHeaders.Add("Referer", "https://auth.siat.sat.gob.mx/nidp/idff/sso?id=pe-fiel-empl&sid=0&option=credential&sid=0&target=https%3A%2F%2Frfcampe.siat.sat.gob.mx%2Fapp%2Fseg%2Femp%2FaccesoEF%3Furl%3Dhttps%3A%2F%2Frfcampe.siat.sat.gob.mx%2Fapp%2FPE%2FIdcSiat%2FSACVisorTributario%2FSACBusquedaVisorTributario.jsf");
            http.DefaultRequestHeaders.Add("sec-ch-ua", "\"Chromium\";v=\"106\", \"Not.A/Brand\";v=\"24\", \"Opera\";v=\"92\"");
            http.DefaultRequestHeaders.Add("sec-ch-ua-mobile", "?0");
            http.DefaultRequestHeaders.Add("sec-ch-ua-platform", "\"Windows\"");
            http.DefaultRequestHeaders.Add("Sec-Fetch-Dest", "document");
            http.DefaultRequestHeaders.Add("Sec-Fetch-Mode", "navigate");
            http.DefaultRequestHeaders.Add("Sec-Fetch-Site", "same-origin");
            http.DefaultRequestHeaders.Add("Sec-Fetch-User", "?1");
            http.DefaultRequestHeaders.Add("Upgrade-Insecure-Requests", "1");
            http.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/106.0.0.0 Safari/537.36 OPR/92.0.0.0");
            var _result = await http.GetAsync(_route);
            return _result;
        }
        // 5
        public async Task<HttpResponseMessage> SSO(string SAML, string SET_COOKIES)
        {
            var http = new HttpClient();
            var _route = $"https://auth.siat.sat.gob.mx/nidp/saml2/sso";

            http.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
            http.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
            http.DefaultRequestHeaders.Add("Accept-Language", "es-ES,es;q=0.9");
            http.DefaultRequestHeaders.Add("Cache-Control", "max-age=0");
            http.DefaultRequestHeaders.Add("Connection", "keep-alive");
            //http.DefaultRequestHeaders.Add("Content-Type", "application/x-www-form-urlencoded");
            //JSESSIONID=D83E098ECAB6F6EC562138DF950D6502; F5-PROD-SIAT-AUTH-443=!Gk1ZOpbYPiddfCefEf4+RrzZf1MVP/s9uQ9KYNMvUtBPByHKA6Riwmspzn6Oj143iog5dQZVMw4LMQ==
            http.DefaultRequestHeaders.Add("Cookie", $"{SET_COOKIES}");
            http.DefaultRequestHeaders.Add("Host", "auth.siat.sat.gob.mx");
            http.DefaultRequestHeaders.Add("Origin", "https://auth.siat.sat.gob.mx");
            http.DefaultRequestHeaders.Add("Referer", "https://auth.siat.sat.gob.mx/nidp/idff/sso?id=pe-fiel-empl&sid=0&option=credential&sid=0&target=https%3A%2F%2Frfcampe.siat.sat.gob.mx%2Fapp%2Fseg%2Femp%2FaccesoEF%3Furl%3Dhttps%3A%2F%2Frfcampe.siat.sat.gob.mx%2Fapp%2FPE%2FIdcSiat%2FSACVisorTributario%2FSACBusquedaVisorTributario.jsf");
            http.DefaultRequestHeaders.Add("sec-ch-ua", "\"Chromium\";v=\"106\", \"Not.A/Brand\";v=\"24\", \"Opera\";v=\"92\"");
            http.DefaultRequestHeaders.Add("sec-ch-ua-mobile", "?0");
            http.DefaultRequestHeaders.Add("sec-ch-ua-platform", "\"Windows\"");
            http.DefaultRequestHeaders.Add("Sec-Fetch-Dest", "document");
            http.DefaultRequestHeaders.Add("Sec-Fetch-Mode", "navigate");
            http.DefaultRequestHeaders.Add("Sec-Fetch-Site", "same-origin");
            http.DefaultRequestHeaders.Add("Sec-Fetch-User", "?1");
            http.DefaultRequestHeaders.Add("Upgrade-Insecure-Requests", "1");
            http.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/106.0.0.0 Safari/537.36 OPR/92.0.0.0");

            var _content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("SAMLRequest", SAML)
            });
            var _result = await http.PostAsync(_route, _content);
            return _result;
        }
        // 6
        public async Task<HttpResponseMessage> ACS(string SAML, string SET_COOKIES)
        {
            var http = new HttpClient();
            var _route = $"https://rfcampe.siat.sat.gob.mx/saml2/sp/acs/post";

            http.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
            http.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
            http.DefaultRequestHeaders.Add("Accept-Language", "es-ES,es;q=0.9");
            http.DefaultRequestHeaders.Add("Cache-Control", "max-age=0");
            http.DefaultRequestHeaders.Add("Connection", "keep-alive");
            //http.DefaultRequestHeaders.Add("Content-Type", "application/x-www-form-urlencoded");
            //JSESSIONID=D83E098ECAB6F6EC562138DF950D6502; F5-PROD-SIAT-AUTH-443=!Gk1ZOpbYPiddfCefEf4+RrzZf1MVP/s9uQ9KYNMvUtBPByHKA6Riwmspzn6Oj143iog5dQZVMw4LMQ==
            http.DefaultRequestHeaders.Add("Cookie", $"{SET_COOKIES}");
            http.DefaultRequestHeaders.Add("Host", "auth.siat.sat.gob.mx");
            http.DefaultRequestHeaders.Add("Origin", "https://auth.siat.sat.gob.mx");
            http.DefaultRequestHeaders.Add("Referer", "https://auth.siat.sat.gob.mx/nidp/idff/sso?id=pe-fiel-empl&sid=0&option=credential&sid=0&target=https%3A%2F%2Frfcampe.siat.sat.gob.mx%2Fapp%2Fseg%2Femp%2FaccesoEF%3Furl%3Dhttps%3A%2F%2Frfcampe.siat.sat.gob.mx%2Fapp%2FPE%2FIdcSiat%2FSACVisorTributario%2FSACBusquedaVisorTributario.jsf");
            http.DefaultRequestHeaders.Add("sec-ch-ua", "\"Chromium\";v=\"106\", \"Not.A/Brand\";v=\"24\", \"Opera\";v=\"92\"");
            http.DefaultRequestHeaders.Add("sec-ch-ua-mobile", "?0");
            http.DefaultRequestHeaders.Add("sec-ch-ua-platform", "\"Windows\"");
            http.DefaultRequestHeaders.Add("Sec-Fetch-Dest", "document");
            http.DefaultRequestHeaders.Add("Sec-Fetch-Mode", "navigate");
            http.DefaultRequestHeaders.Add("Sec-Fetch-Site", "same-origin");
            http.DefaultRequestHeaders.Add("Sec-Fetch-User", "?1");
            http.DefaultRequestHeaders.Add("Upgrade-Insecure-Requests", "1");
            http.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/106.0.0.0 Safari/537.36 OPR/92.0.0.0");

            var _content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("SAMLResponse", SAML)
            });
            var _result = await http.PostAsync(_route, _content);
            return _result;
        }
        // 7
        public async Task<HttpResponseMessage> Acceso(string SET_COOKIES)
        {
            var http = new HttpClient();
            var _route = $"https://rfcampe.siat.sat.gob.mx/app/seg/emp/accesoEF?url=https://rfcampe.siat.sat.gob.mx/app/PE/IdcSiat/SACVisorTributario/SACBusquedaVisorTributario.jsf";

            http.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
            http.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
            http.DefaultRequestHeaders.Add("Accept-Language", "es-ES,es;q=0.9");
            http.DefaultRequestHeaders.Add("Cache-Control", "max-age=0");
            http.DefaultRequestHeaders.Add("Connection", "keep-alive");
            //http.DefaultRequestHeaders.Add("Content-Type", "application/x-www-form-urlencoded");
            //JSESSIONID=D83E098ECAB6F6EC562138DF950D6502; F5-PROD-SIAT-AUTH-443=!Gk1ZOpbYPiddfCefEf4+RrzZf1MVP/s9uQ9KYNMvUtBPByHKA6Riwmspzn6Oj143iog5dQZVMw4LMQ==
            http.DefaultRequestHeaders.Add("Cookie", $"name=value; {SET_COOKIES}");
            http.DefaultRequestHeaders.Add("Host", "auth.siat.sat.gob.mx");
            http.DefaultRequestHeaders.Add("Origin", "https://auth.siat.sat.gob.mx");
            http.DefaultRequestHeaders.Add("Referer", "https://auth.siat.sat.gob.mx/nidp/idff/sso?id=pe-fiel-empl&sid=0&option=credential&sid=0&target=https%3A%2F%2Frfcampe.siat.sat.gob.mx%2Fapp%2Fseg%2Femp%2FaccesoEF%3Furl%3Dhttps%3A%2F%2Frfcampe.siat.sat.gob.mx%2Fapp%2FPE%2FIdcSiat%2FSACVisorTributario%2FSACBusquedaVisorTributario.jsf");
            http.DefaultRequestHeaders.Add("sec-ch-ua", "\"Chromium\";v=\"106\", \"Not.A/Brand\";v=\"24\", \"Opera\";v=\"92\"");
            http.DefaultRequestHeaders.Add("sec-ch-ua-mobile", "?0");
            http.DefaultRequestHeaders.Add("sec-ch-ua-platform", "\"Windows\"");
            http.DefaultRequestHeaders.Add("Sec-Fetch-Dest", "document");
            http.DefaultRequestHeaders.Add("Sec-Fetch-Mode", "navigate");
            http.DefaultRequestHeaders.Add("Sec-Fetch-Site", "same-origin");
            http.DefaultRequestHeaders.Add("Sec-Fetch-User", "?1");
            http.DefaultRequestHeaders.Add("Upgrade-Insecure-Requests", "1");
            http.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/106.0.0.0 Safari/537.36 OPR/92.0.0.0");

            var _result = await http.GetAsync(_route);
            return _result;
        }
        // 8
        // (4)RFC_AMP_CUE_CONT_9080_e, (3)IPCZQX0369a1f199, (7)JSESSIONID, (7)F5-CONTENCION-rfcampe-443
        public async Task<HttpResponseMessage> Visor(string SET_COOKIES)
        {
            var _handle = new HttpClientHandler();
            _handle.AutomaticDecompression = DecompressionMethods.GZip;
            _handle.UseCookies = false;
            var http = new HttpClient(_handle);
            var _route = $"https://rfcampe.siat.sat.gob.mx/app/PE/IdcSiat/SACVisorTributario/SACBusquedaVisorTributario.jsf";

            http.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
            http.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
            http.DefaultRequestHeaders.Add("Accept-Language", "es-ES,es;q=0.9");
            http.DefaultRequestHeaders.Add("Cache-Control", "max-age=0");
            http.DefaultRequestHeaders.Add("Connection", "keep-alive");
            //http.DefaultRequestHeaders.Add("Content-Type", "application/x-www-form-urlencoded");
            //JSESSIONID=D83E098ECAB6F6EC562138DF950D6502; F5-PROD-SIAT-AUTH-443=!Gk1ZOpbYPiddfCefEf4+RrzZf1MVP/s9uQ9KYNMvUtBPByHKA6Riwmspzn6Oj143iog5dQZVMw4LMQ==
            http.DefaultRequestHeaders.Add("Cookie", $"{SET_COOKIES}");
            http.DefaultRequestHeaders.Add("Host", "auth.siat.sat.gob.mx");
            http.DefaultRequestHeaders.Add("Origin", "https://auth.siat.sat.gob.mx");
            http.DefaultRequestHeaders.Add("Referer", "https://auth.siat.sat.gob.mx/");
            http.DefaultRequestHeaders.Add("sec-ch-ua", "\"Chromium\";v=\"106\", \"Not.A/Brand\";v=\"24\", \"Opera\";v=\"92\"");
            http.DefaultRequestHeaders.Add("sec-ch-ua-mobile", "?0");
            http.DefaultRequestHeaders.Add("sec-ch-ua-platform", "\"Windows\"");
            http.DefaultRequestHeaders.Add("Sec-Fetch-Dest", "document");
            http.DefaultRequestHeaders.Add("Sec-Fetch-Mode", "navigate");
            http.DefaultRequestHeaders.Add("Sec-Fetch-Site", "same-origin");
            http.DefaultRequestHeaders.Add("Sec-Fetch-User", "?1");
            http.DefaultRequestHeaders.Add("Upgrade-Insecure-Requests", "1");
            http.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/106.0.0.0 Safari/537.36 OPR/92.0.0.0");
            http.DefaultRequestHeaders.Host = "rfcampe.siat.sat.gob.mx";
            var _result = await http.GetAsync(_route);
            return _result;
        }
        public HttpResponseMessage BuscarFisica(Work _req)
        {
            var _handle = new HttpClientHandler();
            _handle.AutomaticDecompression = DecompressionMethods.GZip;
            _handle.UseCookies = false;

            var http = new HttpClient(_handle);

            var _route = $"https://rfcampe.siat.sat.gob.mx/app/PE/IdcSiat/SACVisorTributario/SACBusquedaVisorTributario.jsf";

            http.DefaultRequestHeaders.Add("Cookie", $"{_req.COOKIES}");
            http.DefaultRequestHeaders.Add("Faces-Request", "partial/ajax");
            http.DefaultRequestHeaders.Host = "rfcampe.siat.sat.gob.mx";
            http.DefaultRequestHeaders.Add("Origin", "https://rfcampe.siat.sat.gob.mx");
            http.DefaultRequestHeaders.Add("Referer", "https://rfcampe.siat.sat.gob.mx/app/PE/IdcSiat/SACVisorTributario/SACBusquedaVisorTributario.jsf");
            http.DefaultRequestHeaders.Add("Sec-Fetch-Dest", "empty");
            http.DefaultRequestHeaders.Add("Connection", "keep-alive");
            http.DefaultRequestHeaders.Add("sec-ch-ua", "\"Chromium\";v=\"106\", \"Not.A/Brand\";v=\"24\", \"Opera\";v=\"92\"");
            http.DefaultRequestHeaders.Add("sec-ch-ua-mobile", "?0");
            http.DefaultRequestHeaders.Add("sec-ch-ua-platform", "\"Windows\"");
            http.DefaultRequestHeaders.Add("Sec-Fetch-Mode", "cors");
            http.DefaultRequestHeaders.Add("Sec-Fetch-Site", "same-origin");
            http.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/106.0.0.0 Safari/537.36 OPR/92.0.0.0");
            http.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");
            http.DefaultRequestHeaders.Add("Accept", "application/xml, text/xml, */*; q=0.01");
            http.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
            http.DefaultRequestHeaders.Add("Accept-Language", "es-ES,es;q=0.9");

            var _content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("javax.faces.partial.ajax", "true"),
                new KeyValuePair<string, string>("javax.faces.source", "visorForm:btnBuscar"),
                new KeyValuePair<string, string>("javax.faces.partial.execute", "@all"),
                new KeyValuePair<string, string>("javax.faces.partial.render", "visorForm:tablaResultados visorForm:datosBusquedaPersonaFisica"),
                new KeyValuePair<string, string>("visorForm:btnBuscar", "visorForm:btnBuscar"),
                new KeyValuePair<string, string>("visorForm", "visorForm"),
                new KeyValuePair<string, string>("visorForm:radioBtnContribuyente", "false"),
                new KeyValuePair<string, string>("visorForm:busquedaRFCFisica", _req.RFC),
                new KeyValuePair<string, string>("visorForm:busquedaCurp", _req.CURP),
                new KeyValuePair<string, string>("visorForm:busquedaNombre", ""),
                new KeyValuePair<string, string>("visorForm:busquedaApePaterno", ""),
                new KeyValuePair<string, string>("visorForm:busquedaApeMaterno", ""),
                new KeyValuePair<string, string>("visorForm:tablaResultados_scrollState", ""),
                new KeyValuePair<string, string>("javax.faces.ViewState", _req.VIEW_STATE),
            });

            var _result = http.PostAsync(_route, _content).Result;
            //Console.WriteLine("hOLA: " + _result);
            return _result;
        }
        public async Task<HttpResponseMessage> BuscarCURP(string COOKIES, string VIEW_STATE, string CURP)
        {

            var _handle = new HttpClientHandler();
            _handle.AutomaticDecompression = DecompressionMethods.GZip;
            _handle.UseCookies = false;

            var http = new HttpClient(_handle);


            var _route = $"https://rfcampe.siat.sat.gob.mx/app/PE/IdcSiat/SACVisorTributario/SACBusquedaVisorTributario.jsf";
            http.DefaultRequestHeaders.Add("Cookie", $"{COOKIES}");
            http.DefaultRequestHeaders.Add("Faces-Request", "partial/ajax");
            http.DefaultRequestHeaders.Host = "rfcampe.siat.sat.gob.mx";
            http.DefaultRequestHeaders.Add("Origin", "https://rfcampe.siat.sat.gob.mx");
            http.DefaultRequestHeaders.Add("Referer", "https://rfcampe.siat.sat.gob.mx/app/PE/IdcSiat/SACVisorTributario/SACBusquedaVisorTributario.jsf");
            http.DefaultRequestHeaders.Add("Sec-Fetch-Dest", "empty");
            http.DefaultRequestHeaders.Add("Connection", "keep-alive");
            http.DefaultRequestHeaders.Add("sec-ch-ua", "\"Chromium\";v=\"106\", \"Not.A/Brand\";v=\"24\", \"Opera\";v=\"92\"");
            http.DefaultRequestHeaders.Add("sec-ch-ua-mobile", "?0");
            http.DefaultRequestHeaders.Add("sec-ch-ua-platform", "\"Windows\"");
            http.DefaultRequestHeaders.Add("Sec-Fetch-Mode", "cors");
            http.DefaultRequestHeaders.Add("Sec-Fetch-Site", "same-origin");
            http.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/106.0.0.0 Safari/537.36 OPR/92.0.0.0");
            http.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");
            http.DefaultRequestHeaders.Add("Accept", "application/xml, text/xml, */*; q=0.01");
            http.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
            http.DefaultRequestHeaders.Add("Accept-Language", "es-ES,es;q=0.9");

            //http.DefaultRequestHeaders.Add("Content-Type", "application/x-www-form-urlencoded");
            //JSESSIONID=D83E098ECAB6F6EC562138DF950D6502; F5-PROD-SIAT-AUTH-443=!Gk1ZOpbYPiddfCefEf4+RrzZf1MVP/s9uQ9KYNMvUtBPByHKA6Riwmspzn6Oj143iog5dQZVMw4LMQ==
            var _content = new FormUrlEncodedContent(new[]
{
                new KeyValuePair<string, string>("javax.faces.partial.ajax", "true"),
                new KeyValuePair<string, string>("javax.faces.source", "visorForm:btnBuscar"),
                new KeyValuePair<string, string>("javax.faces.partial.execute", "@all"),
                new KeyValuePair<string, string>("javax.faces.partial.render", "visorForm:tablaResultados visorForm:datosBusquedaPersonaFisica"),
                new KeyValuePair<string, string>("visorForm:btnBuscar", "visorForm:btnBuscar"),
                new KeyValuePair<string, string>("visorForm", "visorForm"),
                new KeyValuePair<string, string>("visorForm:radioBtnContribuyente", "false"),
                new KeyValuePair<string, string>("visorForm:busquedaRFCFisica", ""),
                new KeyValuePair<string, string>("visorForm:busquedaCurp", CURP),
                new KeyValuePair<string, string>("visorForm:busquedaNombre", ""),
                new KeyValuePair<string, string>("visorForm:busquedaApePaterno", ""),
                new KeyValuePair<string, string>("visorForm:busquedaApeMaterno", ""),
                new KeyValuePair<string, string>("visorForm:tablaResultados_scrollState", ""),
                new KeyValuePair<string, string>("javax.faces.ViewState", VIEW_STATE),
            });

            var _result = await http.PostAsync(_route, _content);
            return _result;
        }
        public HttpResponseMessage BuscarMoral(WorkMoral _req)
        {
            var _handle = new HttpClientHandler();
            _handle.AutomaticDecompression = DecompressionMethods.GZip;
            _handle.UseCookies = false;

            var http = new HttpClient(_handle);

            var _route = $"https://rfcampe.siat.sat.gob.mx/app/PE/IdcSiat/SACVisorTributario/SACBusquedaVisorTributario.jsf";

            http.DefaultRequestHeaders.Add("Cookie", $"{_req.COOKIES}");
            http.DefaultRequestHeaders.Add("Faces-Request", "partial/ajax");
            http.DefaultRequestHeaders.Host = "rfcampe.siat.sat.gob.mx";
            http.DefaultRequestHeaders.Add("Origin", "https://rfcampe.siat.sat.gob.mx");
            http.DefaultRequestHeaders.Add("Referer", "https://rfcampe.siat.sat.gob.mx/app/PE/IdcSiat/SACVisorTributario/SACBusquedaVisorTributario.jsf");
            http.DefaultRequestHeaders.Add("Sec-Fetch-Dest", "empty");
            http.DefaultRequestHeaders.Add("Connection", "keep-alive");
            http.DefaultRequestHeaders.Add("sec-ch-ua", "\"Chromium\";v=\"106\", \"Not.A/Brand\";v=\"24\", \"Opera\";v=\"92\"");
            http.DefaultRequestHeaders.Add("sec-ch-ua-mobile", "?0");
            http.DefaultRequestHeaders.Add("sec-ch-ua-platform", "\"Windows\"");
            http.DefaultRequestHeaders.Add("Sec-Fetch-Mode", "cors");
            http.DefaultRequestHeaders.Add("Sec-Fetch-Site", "same-origin");
            http.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/106.0.0.0 Safari/537.36 OPR/92.0.0.0");
            http.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");
            http.DefaultRequestHeaders.Add("Accept", "application/xml, text/xml, */*; q=0.01");
            http.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
            http.DefaultRequestHeaders.Add("Accept-Language", "es-ES,es;q=0.9");

            var _content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("javax.faces.partial.ajax", "true"),
                new KeyValuePair<string, string>("javax.faces.source", "visorForm:j_idt53"),
                new KeyValuePair<string, string>("javax.faces.partial.execute", "@all"),
                new KeyValuePair<string, string>("javax.faces.partial.render", "visorForm:tablaResultados visorForm:datosBusquedaPersonaMoral"),
                new KeyValuePair<string, string>("visorForm:j_idt53", "visorForm:j_idt53"),
                new KeyValuePair<string, string>("visorForm", "visorForm"),
                new KeyValuePair<string, string>("visorForm:radioBtnContribuyente", "true"),
                new KeyValuePair<string, string>("visorForm:busquedaRFCPersonaMoral", _req.RFC!),
                new KeyValuePair<string, string>("visorForm:busquedaRazonSocial", ""),
                new KeyValuePair<string, string>("visorForm:tablaResultados_scrollState", ""),
                new KeyValuePair<string, string>("javax.faces.ViewState", _req.VIEW_STATE!),
            });

            var _result = http.PostAsync(_route, _content).Result;
            return _result;
        }
        public async Task<HttpResponseMessage> ClickRFCMoral(string COOKIES, string VIEW_STATE, string RFC)
        {
            var _handle = new HttpClientHandler();
            _handle.AutomaticDecompression = DecompressionMethods.GZip;
            _handle.UseCookies = false;

            var http = new HttpClient(_handle);

            var _route = $"https://rfcampe.siat.sat.gob.mx/app/PE/IdcSiat/SACVisorTributario/SACBusquedaVisorTributario.jsf";
            http.DefaultRequestHeaders.Add("Cookie", $"{COOKIES}");
            http.DefaultRequestHeaders.Add("Faces-Request", "partial/ajax");
            http.DefaultRequestHeaders.Host = "rfcampe.siat.sat.gob.mx";
            http.DefaultRequestHeaders.Add("Origin", "https://rfcampe.siat.sat.gob.mx");
            http.DefaultRequestHeaders.Add("Referer", "https://rfcampe.siat.sat.gob.mx/app/PE/IdcSiat/SACVisorTributario/SACBusquedaVisorTributario.jsf");
            http.DefaultRequestHeaders.Add("Sec-Fetch-Dest", "empty");
            http.DefaultRequestHeaders.Add("Connection", "keep-alive");
            http.DefaultRequestHeaders.Add("sec-ch-ua", "\"Chromium\";v=\"106\", \"Not.A/Brand\";v=\"24\", \"Opera\";v=\"92\"");
            http.DefaultRequestHeaders.Add("sec-ch-ua-mobile", "?0");
            http.DefaultRequestHeaders.Add("sec-ch-ua-platform", "\"Windows\"");
            http.DefaultRequestHeaders.Add("Sec-Fetch-Mode", "cors");
            http.DefaultRequestHeaders.Add("Sec-Fetch-Site", "same-origin");
            http.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/106.0.0.0 Safari/537.36 OPR/92.0.0.0");
            http.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");
            http.DefaultRequestHeaders.Add("Accept", "application/xml, text/xml, */*; q=0.01");
            http.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
            http.DefaultRequestHeaders.Add("Accept-Language", "es-ES,es;q=0.9");
            var _content = new FormUrlEncodedContent(new[]
{
                new KeyValuePair<string, string>("javax.faces.partial.ajax", "true"),
                new KeyValuePair<string, string>("javax.faces.source", "visorForm:tablaResultados:0:j_idt62"),
                new KeyValuePair<string, string>("javax.faces.partial.execute", "@all"),
                new KeyValuePair<string, string>("visorForm:tablaResultados:0:j_idt62","visorForm:tablaResultados:0:j_idt62"),
                new KeyValuePair<string, string>("rowIdx", "0"),
                new KeyValuePair<string, string>("visorForm", "visorForm"),
                new KeyValuePair<string, string>("visorForm:radioBtnContribuyente", "true"),
                new KeyValuePair<string, string>("visorForm:busquedaRFCFisica", RFC),
                new KeyValuePair<string, string>("visorForm:busquedaCurp", ""),
                new KeyValuePair<string, string>("visorForm:busquedaRazonSocial", ""),
                new KeyValuePair<string, string>("visorForm:tablaResultados_scrollState", "0,0"),
                new KeyValuePair<string, string>("javax.faces.ViewState", VIEW_STATE),
            });

            var _result = await http.PostAsync(_route, _content);
            return _result;
        }
        public async Task<HttpResponseMessage> ClickRFC(string COOKIES, string VIEW_STATE, string CURP)
        {
            var _handle = new HttpClientHandler();
            _handle.AutomaticDecompression = DecompressionMethods.GZip;
            _handle.UseCookies = false;

            var http = new HttpClient(_handle);

            var _route = $"https://rfcampe.siat.sat.gob.mx/app/PE/IdcSiat/SACVisorTributario/SACBusquedaVisorTributario.jsf";
            http.DefaultRequestHeaders.Add("Cookie", $"{COOKIES}");
            http.DefaultRequestHeaders.Add("Faces-Request", "partial/ajax");
            http.DefaultRequestHeaders.Host = "rfcampe.siat.sat.gob.mx";
            http.DefaultRequestHeaders.Add("Origin", "https://rfcampe.siat.sat.gob.mx");
            http.DefaultRequestHeaders.Add("Referer", "https://rfcampe.siat.sat.gob.mx/app/PE/IdcSiat/SACVisorTributario/SACBusquedaVisorTributario.jsf");
            http.DefaultRequestHeaders.Add("Sec-Fetch-Dest", "empty");
            http.DefaultRequestHeaders.Add("Connection", "keep-alive");
            http.DefaultRequestHeaders.Add("sec-ch-ua", "\"Chromium\";v=\"106\", \"Not.A/Brand\";v=\"24\", \"Opera\";v=\"92\"");
            http.DefaultRequestHeaders.Add("sec-ch-ua-mobile", "?0");
            http.DefaultRequestHeaders.Add("sec-ch-ua-platform", "\"Windows\"");
            http.DefaultRequestHeaders.Add("Sec-Fetch-Mode", "cors");
            http.DefaultRequestHeaders.Add("Sec-Fetch-Site", "same-origin");
            http.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/106.0.0.0 Safari/537.36 OPR/92.0.0.0");
            http.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");
            http.DefaultRequestHeaders.Add("Accept", "application/xml, text/xml, */*; q=0.01");
            http.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
            http.DefaultRequestHeaders.Add("Accept-Language", "es-ES,es;q=0.9");
            var _content = new FormUrlEncodedContent(new[]
{
                new KeyValuePair<string, string>("javax.faces.partial.ajax", "true"),
                new KeyValuePair<string, string>("javax.faces.source", "visorForm:btnBuscar"),
                new KeyValuePair<string, string>("javax.faces.partial.execute", "@all"),
                new KeyValuePair<string, string>("visorForm:tablaResultados:0:j_idt62","visorForm:tablaResultados:0:j_idt62"),
                new KeyValuePair<string, string>("rowIdx", "0"),
                new KeyValuePair<string, string>("visorForm", "visorForm"),
                new KeyValuePair<string, string>("visorForm:radioBtnContribuyente", "false"),
                new KeyValuePair<string, string>("visorForm:busquedaRFCFisica", ""),
                new KeyValuePair<string, string>("visorForm:busquedaCurp", CURP),
                new KeyValuePair<string, string>("visorForm:busquedaNombre", ""),
                new KeyValuePair<string, string>("visorForm:busquedaApePaterno", ""),
                new KeyValuePair<string, string>("visorForm:busquedaApeMaterno", ""),
                new KeyValuePair<string, string>("visorForm:tablaResultados_scrollState", "0,0"),
                new KeyValuePair<string, string>("javax.faces.ViewState", VIEW_STATE),
            });

            var _result = await http.PostAsync(_route, _content);
            return _result;
        }
        public HttpResponseMessage ClickMoral(WorkMoral _req)
        {
            var _handle = new HttpClientHandler();
            _handle.AutomaticDecompression = DecompressionMethods.GZip;
            _handle.UseCookies = false;

            var http = new HttpClient(_handle);

            var _route = $"https://rfcampe.siat.sat.gob.mx/app/PE/IdcSiat/SACVisorTributario/SACBusquedaVisorTributario.jsf";

            http.DefaultRequestHeaders.Add("Cookie", $"{_req.COOKIES}");
            http.DefaultRequestHeaders.Add("Faces-Request", "partial/ajax");
            http.DefaultRequestHeaders.Host = "rfcampe.siat.sat.gob.mx";
            http.DefaultRequestHeaders.Add("Origin", "https://rfcampe.siat.sat.gob.mx");
            http.DefaultRequestHeaders.Add("Referer", "https://rfcampe.siat.sat.gob.mx/app/PE/IdcSiat/SACVisorTributario/SACBusquedaVisorTributario.jsf");
            http.DefaultRequestHeaders.Add("Sec-Fetch-Dest", "empty");
            http.DefaultRequestHeaders.Add("Connection", "keep-alive");
            http.DefaultRequestHeaders.Add("sec-ch-ua", "\"Not?A_Brand\";v=\"8\", \"Chromium\";v=\"108\", \"Google Chrome\";v=\"108\"");
            http.DefaultRequestHeaders.Add("sec-ch-ua-mobile", "?0");
            http.DefaultRequestHeaders.Add("sec-ch-ua-platform", "\"Windows\"");
            http.DefaultRequestHeaders.Add("Sec-Fetch-Mode", "cors");
            http.DefaultRequestHeaders.Add("Sec-Fetch-Site", "same-origin");
            http.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/106.0.0.0 Safari/537.36 OPR/92.0.0.0");
            http.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");
            http.DefaultRequestHeaders.Add("Accept", "application/xml, text/xml, */*; q=0.01");
            http.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
            http.DefaultRequestHeaders.Add("Accept-Language", "es-ES,es;q=0.9");

            var _content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("javax.faces.partial.ajax", "true"),
                new KeyValuePair<string, string>("javax.faces.source", "visorForm:radioBtnContribuyente"),
                new KeyValuePair<string, string>("javax.faces.partial.execute", "visorForm:radioBtnContribuyente"),
                new KeyValuePair<string, string>("javax.faces.partial.render", "visorForm:panelBusquedaContribuyente visorForm:tablaResultados"),
                new KeyValuePair<string, string>("javax.faces.behavior.event", "change"),
                new KeyValuePair<string, string>("javax.faces.partial.event", "change"),
                new KeyValuePair<string, string>("visorForm", "visorForm"),
                new KeyValuePair<string, string>("visorForm:radioBtnContribuyente", "true"),
                new KeyValuePair<string, string>("visorForm:busquedaRFCFisica", ""),
                new KeyValuePair<string, string>("visorForm:busquedaCurp", ""),
                new KeyValuePair<string, string>("visorForm:busquedaNombre", ""),
                new KeyValuePair<string, string>("visorForm:busquedaApePaterno", ""),
                new KeyValuePair<string, string>("visorForm:busquedaApeMaterno", ""),
                new KeyValuePair<string, string>("visorForm:tablaResultados_scrollState", ""),
                new KeyValuePair<string, string>("javax.faces.ViewState", _req.VIEW_STATE!),
            });

            var _result = http.PostAsync(_route, _content).Result;
            return _result;
        }
        public async Task<HttpResponseMessage> InitVisualizador(string COOKIES)
        {
            var _handle = new HttpClientHandler();
            _handle.AutomaticDecompression = DecompressionMethods.GZip;
            _handle.UseCookies = false;
            var http = new HttpClient(_handle);
            var _route = $"https://rfcampe.siat.sat.gob.mx/app/PE/IdcSiat/SACVisorTributario/VisorTributarioInit.jsf";
            http.DefaultRequestHeaders.Add("Cookie", $"{COOKIES}");
            http.DefaultRequestHeaders.Add("Faces-Request", "partial/ajax");
            http.DefaultRequestHeaders.Host = "rfcampe.siat.sat.gob.mx";
            http.DefaultRequestHeaders.Add("Origin", "https://rfcampe.siat.sat.gob.mx");
            http.DefaultRequestHeaders.Add("Referer", "https://rfcampe.siat.sat.gob.mx/app/PE/IdcSiat/SACVisorTributario/SACBusquedaVisorTributario.jsf");
            http.DefaultRequestHeaders.Add("Sec-Fetch-Dest", "empty");
            http.DefaultRequestHeaders.Add("Connection", "keep-alive");
            http.DefaultRequestHeaders.Add("sec-ch-ua", "\"Chromium\";v=\"106\", \"Not.A/Brand\";v=\"24\", \"Opera\";v=\"92\"");
            http.DefaultRequestHeaders.Add("sec-ch-ua-mobile", "?0");
            http.DefaultRequestHeaders.Add("sec-ch-ua-platform", "\"Windows\"");
            http.DefaultRequestHeaders.Add("Sec-Fetch-Dest", "document");
            http.DefaultRequestHeaders.Add("Sec-Fetch-Mode", "navigate");
            http.DefaultRequestHeaders.Add("Sec-Fetch-Site", "same-origin");
            http.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/106.0.0.0 Safari/537.36 OPR/92.0.0.0");
            http.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");
            http.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
            http.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
            http.DefaultRequestHeaders.Add("Accept-Language", "es-ES,es;q=0.9");
            http.DefaultRequestHeaders.Add("Sec-Fetch-User", "?1");
            http.DefaultRequestHeaders.Add("Upgrade-Insecure-Requests", "1");
            return await http.GetAsync(_route);
        }
        public async Task<HttpResponseMessage> HTMLResumen(string COOKIES)
        {
            var _handle = new HttpClientHandler();
            _handle.AutomaticDecompression = DecompressionMethods.GZip;
            _handle.UseCookies = false;
            var http = new HttpClient(_handle);
            var _route = $"https://rfcampe.siat.sat.gob.mx/app/PE/IdcSiat/SACVisorTributario/VisorTributarioResumen.jsf";
            http.DefaultRequestHeaders.Add("Cookie", $"{COOKIES}");
            http.DefaultRequestHeaders.Add("Faces-Request", "partial/ajax");
            http.DefaultRequestHeaders.Host = "rfcampe.siat.sat.gob.mx";
            http.DefaultRequestHeaders.Add("Origin", "https://rfcampe.siat.sat.gob.mx");
            http.DefaultRequestHeaders.Add("Referer", "https://rfcampe.siat.sat.gob.mx/app/PE/IdcSiat/SACVisorTributario/SACBusquedaVisorTributario.jsf");
            http.DefaultRequestHeaders.Add("Sec-Fetch-Dest", "empty");
            http.DefaultRequestHeaders.Add("Connection", "keep-alive");
            http.DefaultRequestHeaders.Add("sec-ch-ua", "\"Chromium\";v=\"106\", \"Not.A/Brand\";v=\"24\", \"Opera\";v=\"92\"");
            http.DefaultRequestHeaders.Add("sec-ch-ua-mobile", "?0");
            http.DefaultRequestHeaders.Add("sec-ch-ua-platform", "\"Windows\"");
            http.DefaultRequestHeaders.Add("Sec-Fetch-Dest", "document");
            http.DefaultRequestHeaders.Add("Sec-Fetch-Mode", "navigate");
            http.DefaultRequestHeaders.Add("Sec-Fetch-Site", "same-origin");
            http.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/106.0.0.0 Safari/537.36 OPR/92.0.0.0");
            http.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");
            http.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
            http.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
            http.DefaultRequestHeaders.Add("Accept-Language", "es-ES,es;q=0.9");
            http.DefaultRequestHeaders.Add("Upgrade-Insecure-Requests", "1");
            return await http.GetAsync(_route);
        }
        public async Task<HttpResponseMessage> ResumenTributario(string COOKIES, string VIEW_STATE)
        {
            var _handle = new HttpClientHandler();
            _handle.AutomaticDecompression = DecompressionMethods.GZip;
            _handle.UseCookies = false;

            var http = new HttpClient(_handle);

            var _route = $"https://rfcampe.siat.sat.gob.mx/app/PE/IdcSiat/SACVisorTributario/VisorTributarioResumen.jsf";
            http.DefaultRequestHeaders.Add("Cookie", $"{COOKIES}");
            http.DefaultRequestHeaders.Add("Faces-Request", "partial/ajax");
            http.DefaultRequestHeaders.Host = "rfcampe.siat.sat.gob.mx";
            http.DefaultRequestHeaders.Add("Origin", "https://rfcampe.siat.sat.gob.mx");
            http.DefaultRequestHeaders.Add("Referer", "https://rfcampe.siat.sat.gob.mx/app/PE/IdcSiat/SACVisorTributario/VisorTributarioResumen.jsf");
            http.DefaultRequestHeaders.Add("Sec-Fetch-Dest", "empty");
            http.DefaultRequestHeaders.Add("Connection", "keep-alive");
            http.DefaultRequestHeaders.Add("sec-ch-ua", "\"Chromium\";v=\"106\", \"Not.A/Brand\";v=\"24\", \"Opera\";v=\"92\"");
            http.DefaultRequestHeaders.Add("sec-ch-ua-mobile", "?0");
            http.DefaultRequestHeaders.Add("sec-ch-ua-platform", "\"Windows\"");
            http.DefaultRequestHeaders.Add("Sec-Fetch-Dest", "empty");
            http.DefaultRequestHeaders.Add("Sec-Fetch-Mode", "cors");
            http.DefaultRequestHeaders.Add("Sec-Fetch-Site", "same-origin");
            http.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/106.0.0.0 Safari/537.36 OPR/92.0.0.0");
            http.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");
            http.DefaultRequestHeaders.Add("Accept", "application/xml, text/xml, */*; q=0.01");
            http.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
            http.DefaultRequestHeaders.Add("Accept-Language", "es-ES,es;q=0.9");
            var _content = new FormUrlEncodedContent(new[]
{
                new KeyValuePair<string, string>("javax.faces.partial.ajax", "true"),
                new KeyValuePair<string, string>("javax.faces.source", $"visorForm:j_idt232"),
                new KeyValuePair<string, string>("javax.faces.partial.execute", "@all"),
                new KeyValuePair<string, string>($"visorForm:j_idt232",$"visorForm:j_idt232"),
                new KeyValuePair<string, string>("visorForm", "visorForm"),
                new KeyValuePair<string, string>("visorForm:j_idt14:comboAnio_input", "2023"),
                new KeyValuePair<string, string>("visorForm:j_idt14:comboAnio_focus", ""),
                new KeyValuePair<string, string>("visorForm:j_idt14:tipoTramite_focus", ""),
                new KeyValuePair<string, string>("visorForm:j_idt14_active", "0"),
                new KeyValuePair<string, string>("javax.faces.ViewState", VIEW_STATE),
            });

            return await http.PostAsync(_route, _content);
        }
        public async Task<HttpResponseMessage> DownloadRFC(string COOKIES, string FILENAME)
        {

            var _handle = new HttpClientHandler();
            _handle.AutomaticDecompression = DecompressionMethods.GZip;
            _handle.UseCookies = false;

            var http = new HttpClient(_handle);


            var _route = $"https://rfcampe.siat.sat.gob.mx/app/PE/IdcSiat/IdcGeneraConstancia.jsf";
            http.DefaultRequestHeaders.Clear();
            http.DefaultRequestHeaders.Host = "rfcampe.siat.sat.gob.mx";
            //http.DefaultRequestHeaders.Add("Origin", "https://rfcampe.siat.sat.gob.mx");
            //http.DefaultRequestHeaders.Add("Sec-Fetch-Dest", "document");
            //http.DefaultRequestHeaders.Add("Connection", "keep-alive");
            http.DefaultRequestHeaders.Add("sec-ch-ua", "\" Not A; Brand\";v=\"99\", \"Chromium\";v=\"102\", \"Google Chrome\";v=\"102\"");
            http.DefaultRequestHeaders.Add("sec-ch-ua-mobile", "?0");
            http.DefaultRequestHeaders.Add("sec-ch-ua-platform", "\"Windows\"");
            http.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");
            http.DefaultRequestHeaders.Add("Sec-Fetch-Site", "same-origin");
            http.DefaultRequestHeaders.Add("Cookie", $"{COOKIES}");
            http.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/102.0.5005.115 Safari/537.36");
            //http.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
            http.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
            http.DefaultRequestHeaders.Add("Accept-Language", "es-ES,es;q=0.9");
            http.DefaultRequestHeaders.Add("Sec-Fetch-User", "?1");
            http.DefaultRequestHeaders.Add("Upgrade-Insecure-Requests", "1");
            var _result = await http.GetAsync(_route);
            return _result;
            //var _str = await _result.Content.ReadAsStringAsync();
            //var _path = @$"cache/rfcs/{FILENAME}";

            //using (var fs = new FileStream(_path, FileMode.CreateNew))
            //{
            //    await _result.Content.CopyToAsync(fs);
            //}

            //if (!!System.IO.File.Exists(_path))
            //{
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}

        }
        public async Task<string> LogIn()
        {
            var username = "_Net_core";
            var password = "vTo8*XHVv7K!854^2b";

            string api = "https://actasalinstante.com:3030/api/user/signin/";

            using (var client = new HttpClient())
            {
                var content = new FormUrlEncodedContent(new[] {
                    new KeyValuePair<string, string>("username", username),
                    new KeyValuePair<string, string>("password", password)
                });
                var result = await client.PostAsync(api, content);
                var resultString = await result.Content.ReadAsStringAsync();
                return resultString;
            }
        }
        public async Task<string> ADD_CORTE(string dataset, string document, string level0, string filename, string nombreacta, string state)
        {
            var _log = JsonConvert.DeserializeObject<LoginModel>(await LogIn());



            string api = "https://actasalinstante.com:3030/api/actas/reg/new/";
            var client = new HttpClient();

            var content = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string, string>("dataset", dataset),
                new KeyValuePair<string, string>("document", document),
                new KeyValuePair<string, string>("level0", level0),
                new KeyValuePair<string, string>("namefile", filename),
                new KeyValuePair<string, string>("nameinside", nombreacta),
                new KeyValuePair<string, string>("state", state)
            });
            client.DefaultRequestHeaders.Add("x-access-token", _log.token);

            var result = await client.PostAsync(api, content);
            var resultString = await result.Content.ReadAsStringAsync();
            return resultString;

        }
    }
}
