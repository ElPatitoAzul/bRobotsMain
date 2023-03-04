using Newtonsoft.Json.Linq;

namespace BackRobotTDM.Enviroments
{

    public class NodeAPIs
    {

        private static string API = "https://actasalinstante.com:3030";

        public async Task<string> LoadMyServices(string TOKEN)
        {
            string api = $"{API}/api/user/getMyService";
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("x-access-token", TOKEN);
                return await client.GetStringAsync(api);
            }
        }


        public async Task<string> GetARobot(string TOKEN)
        {
            string api = $"{API}/api/user/getMyService";
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("x-access-token", TOKEN);
                return await client.GetStringAsync(api);
            }
        }


    }
}
