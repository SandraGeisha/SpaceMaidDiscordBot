using Exurb1aBot.Model.ViewModel.GithubModels;
using System.Linq;
using System;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Exurb1aBot.Util.Parsers {
    class GithubParser {
        private const string requestUrl = "https://api.github.com/repos/SandraGeisha/SpaceMaidDiscordBot";

        public static GithubModel GetModel() {
            ApiHelper.InitializeClient();
            HttpClient client = ApiHelper.APIClient;

            HttpResponseMessage response = client.GetAsync(requestUrl).Result;

            if (!response.IsSuccessStatusCode)
                throw new Exception("The github api frowns upon your actions");

            JObject obj = JsonConvert.DeserializeObject<JObject>(response.Content.ReadAsStringAsync().Result);

            response = client.GetAsync($"{requestUrl}/commits").Result;
            JArray ar = JsonConvert.DeserializeObject<JArray>(response.Content.ReadAsStringAsync().Result);

            if (!response.IsSuccessStatusCode)
                throw new Exception("The github api frowns upon your actions");

            //new system for commits
            return new GithubModel() {
                AuthorLink = obj["owner"]["html_url"].ToString(),
                AuthorName = obj["owner"]["login"].ToString(),
                IssueLink = $"{obj["html_url"]}/issues",
                ProjectDescription = obj["description"].ToString(),
                ProjectName = obj["name"].ToString(),
                Commits = ar.Count,
                Stars = obj["stargazers_count"].ToObject<int>(),
                Watchers = obj["watchers"].ToObject<int>()
            };
        }
    }
}
