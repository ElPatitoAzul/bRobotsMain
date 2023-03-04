using Newtonsoft.Json;
using File = System.IO.File;

namespace BackRobotTDM.Scripts.SID
{
    public class WebServices
    {
        private static Tools Tools = new Tools();
        public async Task<string> OBTAIN_DATA(string TYPE, string SEARCH, string DATA, string ACCESS_TOKEN, string USER_ID, string SID_USER)
        {
            //type: 1.nacimiento||2.defuncion||3.matrimonio||4.divorcio
            //searchBy: curp||cadena
            try
            {
                var handler = new HttpClientHandler();
                handler.ClientCertificateOptions = ClientCertificateOption.Manual;
                handler.ServerCertificateCustomValidationCallback =
                    (httpRequestMessage, cert, cetChain, policyErrors) =>
                    {
                        return true;
                    };

                handler.CheckCertificateRevocationList = true;
                var api = $"https://201.134.128.86:8001/sirabi-consultas/{TYPE.ToLower()}/{SEARCH.ToLower()}/{DATA.ToUpper()}/{ACCESS_TOKEN}/{USER_ID}/{SID_USER.ToUpper()}?access_token={ACCESS_TOKEN}&\"{DATA.ToUpper()}\"=";
                using (var client = new HttpClient(handler))
                {
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Add("Host", "187.157.31.246:8004");
                    client.DefaultRequestHeaders.Add("Referer", "https://187.157.31.246:8004/sirabi/acto/certificacion.html?v=16072022");
                    client.DefaultRequestHeaders.Add("sec-ch-ua", "\" Not A; Brand\";v=\"99\", \"Chromium\";v=\"102\", \"Google Chrome\";v=\"102\"");
                    client.DefaultRequestHeaders.Add("sec-ch-ua-mobile", "?0");
                    client.DefaultRequestHeaders.Add("sec-ch-ua-platform", "\"Windows\"");
                    client.DefaultRequestHeaders.Add("Sec-Fetch-Dest", "empty");
                    client.DefaultRequestHeaders.Add("Sec-Fetch-Mode", "cors");
                    client.DefaultRequestHeaders.Add("Sec-Fetch-Site", "same-origin");
                    client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/102.0.5005.115 Safari/537.36");
                    var result = await client.GetStringAsync(api);
                    return result.Substring(1, result.Length - 2);
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("401 (Unauthorized)."))
                {
                    //await LogIn();
                    return "Unauthorized";
                    //login
                }
                return "";
            }
        }

        public async Task REFRESH_TOKEN(string ACCESS_TOKEN)
        {
            var handler = new HttpClientHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.ServerCertificateCustomValidationCallback =
            (httpRequestMessage, cert, cetChain, policyErrors) =>
            {
                return true;
            };
            handler.CheckCertificateRevocationList = true;
            var http = new HttpClient(handler);
            handler.CheckCertificateRevocationList = true;
            var api = $"https://201.134.128.86:8001/sirabi-consultas/consulta/parametrolocal/BOND?access_token={ACCESS_TOKEN}";
            http.DefaultRequestHeaders.Clear();
            http.DefaultRequestHeaders.Add("Host", "187.157.31.246:8004");
            http.DefaultRequestHeaders.Add("Referer", "https://187.157.31.246:8004/sirabi/acto/certificacion.html?v=16072022");
            http.DefaultRequestHeaders.Add("sec-ch-ua", "\" Not A; Brand\";v=\"99\", \"Chromium\";v=\"102\", \"Google Chrome\";v=\"102\"");
            http.DefaultRequestHeaders.Add("sec-ch-ua-mobile", "?0");
            http.DefaultRequestHeaders.Add("sec-ch-ua-platform", "\"Windows\"");
            http.DefaultRequestHeaders.Add("Sec-Fetch-Dest", "empty");
            http.DefaultRequestHeaders.Add("Sec-Fetch-Mode", "cors");
            http.DefaultRequestHeaders.Add("Sec-Fetch-Site", "same-origin");
            http.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/102.0.5005.115 Safari/537.36");
            var result = await http.GetAsync(api);
            Console.WriteLine(result.Content.ReadAsStringAsync().Result);
        }

        public async Task<bool> DOWNLOAD_FILE(Guid ID_REQ, string FOLIO, string CADENA, string USER_ID, string ACCESS_TOKEN, string SID_USER)
        {
            var path = Tools._PATHER_("cache/");
            if (path)
            {
                var handler = new HttpClientHandler();
                handler.ClientCertificateOptions = ClientCertificateOption.Manual;
                handler.ServerCertificateCustomValidationCallback =
                    (httpRequestMessage, cert, cetChain, policyErrors) =>
                    {
                        return true;
                    };

                handler.CheckCertificateRevocationList = true;
                var http = new HttpClient(handler);
                var api = $"https://201.134.128.86:8001/sirabi-consultas/acta/folio/{FOLIO}-S/{CADENA}/1/{USER_ID}/F/{ACCESS_TOKEN}/{USER_ID}/{SID_USER.ToUpper()}?access_token={ACCESS_TOKEN}";
                http.DefaultRequestHeaders.Clear();
                http.DefaultRequestHeaders.Add("Host", "201.134.128.86:8001");
                http.DefaultRequestHeaders.Add("Referer", "https://201.134.128.86:8001/sirabi/acto/certificacion.html?v=24082022");
                http.DefaultRequestHeaders.Add("sec-ch-ua", "\" Not A; Brand\";v=\"99\", \"Chromium\";v=\"102\", \"Google Chrome\";v=\"102\"");
                http.DefaultRequestHeaders.Add("sec-ch-ua-mobile", "?0");
                http.DefaultRequestHeaders.Add("sec-ch-ua-platform", "\"Windows\"");
                http.DefaultRequestHeaders.Add("Sec-Fetch-Dest", "empty");
                http.DefaultRequestHeaders.Add("Sec-Fetch-Mode", "cors");
                http.DefaultRequestHeaders.Add("Sec-Fetch-Site", "same-origin");
                http.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/102.0.5005.115 Safari/537.36");
                var result = await http.GetAsync(api);
                Console.WriteLine(result.StatusCode.ToString());
                if (result.StatusCode.ToString() == "OK")
                {

                    using (var fs = new FileStream($"cache/{ID_REQ.ToString()}-{CADENA}.pdf", FileMode.CreateNew))
                    {
                        await result.Content.CopyToAsync(fs);
                    }
                    if (File.Exists($"cache/{ID_REQ.ToString()}-{CADENA}.pdf"))
                    {
                        return true;
                    }
                    else return false;
                }
                else return false;
            }
            else return false;
        }

        public async Task<string> LogIn()
        {
            var username = "ROBOT";
            var password = "3j4whM99x8EA5";

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
            var LogIn = JsonConvert.DeserializeObject<Models.LoginModel>(this.LogIn().Result);

            string api = "https://actasalinstante.com:3030/api/actas/reg/new/";

            using (var client = new HttpClient())
            {
                var content = new FormUrlEncodedContent(new[] {
                    new KeyValuePair<string, string>("dataset", dataset),
                    new KeyValuePair<string, string>("document", document),
                    new KeyValuePair<string, string>("level0", level0),
                    new KeyValuePair<string, string>("namefile", filename),
                    new KeyValuePair<string, string>("nameinside", nombreacta),
                    new KeyValuePair<string, string>("state", state)
                });

                client.DefaultRequestHeaders.Add("x-access-token", LogIn.token);
                var result = await client.PostAsync(api, content);
                var resultString = await result.Content.ReadAsStringAsync();
                return resultString;
            }
        }

    }
}
