using System.Net.Http;
using System.Net.Http.Headers;

namespace Exurb1aBot.Util {
    public static class ApiHelper{
        public static HttpClient APIClient { get; private set; }
        public static void InitializeClient() {
            if(APIClient==null)
                APIClient = new HttpClient();

            APIClient.DefaultRequestHeaders.UserAgent.Clear();
            APIClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("DiscordBot","1.0.0"));
            APIClient.DefaultRequestHeaders.Accept.Clear();
            APIClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}
