using Discord;
using Discord.Commands;
using Exurb1aBot.Model.ViewModel.WeatherModels;
using System;
using System.Threading.Tasks;

namespace Exurb1aBot.Util.EmbedBuilders {
    public static class WeatherEmbedBuilder {

        public static async Task DisplayWeather(WeatherModel wm,SocketCommandContext context) {
            EmbedBuilder eb = new EmbedBuilder {
                Color = Color.Green
            };

            eb.WithTitle($"Weather for {wm.Name},{wm.Sys.Country}");
            eb.WithDescription($"{wm.WeatherDescriptionModel.Description}");

            eb.AddField("Temperature", $"{ToFarenheit(wm.Main.Temp)}°F ({ToCelcius(wm.Main.Temp)}°C)",true);
            eb.AddField("Humidity",$"{wm.Main.Humidity}%",true);
            
            eb.AddField("Wind speed", $"{ToKmPerHour(wm.Wind.Speed)}km/h ({ToMilesPerHour(wm.Wind.Speed)}miles/h)", true);
            eb.AddField("Max Temperature", $"{ToFarenheit(wm.Main.Temp_max)}°F ({ToCelcius(wm.Main.Temp_max)}°C)", true);
            eb.AddField("Min Temperature", $"{ToFarenheit(wm.Main.Temp_min)}°F ({ToCelcius(wm.Main.Temp_min)}°C)", true);

            eb.WithThumbnailUrl($"http://openweathermap.org/img/w/{wm.WeatherDescriptionModel.Icon}.png");

            EmbedFooterBuilder efb = EmbedBuilderFunctions.AddFooter(context).Result;
            efb.Text += " - Powered by the API OpenWeatherMap";
            eb.WithFooter(efb);

            await context.Channel.SendMessageAsync(embed: eb.Build());
        }


        private static string ToCelcius(double k) {
            return string.Format("{0:0.00}",(k - 272.15)); 
        }

        private static string ToFarenheit(double k) {
            return string.Format("{0:0.00}",( k * (9.0 / 5.0)-459.67));
        }

        private static string ToKmPerHour(double ms) {
            return string.Format("{0:0.00}", ms * 3.6);
        }

        private static string ToMilesPerHour(double ms) {
            return string.Format("{0:0.00}",ms* 2.23693629);
        }
    }
}
