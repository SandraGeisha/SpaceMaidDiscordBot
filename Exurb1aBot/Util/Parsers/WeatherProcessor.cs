using System.Net.Http;
using System.Threading.Tasks;
using System;
using Exurb1aBot.Model.ViewModel.WeatherModels;

namespace Exurb1aBot.Util.Parsers {
    public class WeatherProcessor {
        public static async Task<WeatherModel> GetWeatherDataName(string cityName) {
            string url = $"http://api.openweathermap.org/data/2.5/weather?q={cityName}&appid=46b1b9317f344a6bd4050459a7c7992f";
            using (HttpResponseMessage response = await ApiHelper.APIClient.GetAsync(url)) {
                if (response.IsSuccessStatusCode) {
                    WeatherModel wm = await response.Content.ReadAsAsync<WeatherModel>();
                    return wm;
                }else {
                    if (response.ReasonPhrase == "Not Found")
                        return null;
                    else
                        throw new Exception(response.ReasonPhrase);
                }
            }
        }
    }
}
