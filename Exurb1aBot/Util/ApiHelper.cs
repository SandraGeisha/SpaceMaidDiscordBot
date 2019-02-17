using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Exurb1aBot.Util {
    public static class ApiHelper{
        public static HttpClient APIClient { get; private set; }
        public static void InitializeClient() {
            if(APIClient==null)
                APIClient = new HttpClient();

            APIClient.DefaultRequestHeaders.Accept.Clear();
            APIClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public static string GetHtmlUrl(string url) {
            HttpClient HTMLClient = new HttpClient();
            HTMLClient.DefaultRequestHeaders.Accept.Clear();

            return HTMLClient.GetStringAsync(url).Result;
        }

    }
}
