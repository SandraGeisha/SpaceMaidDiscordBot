using Discord;
using Discord.Commands;
using Exurb1aBot.Model.Domain;
using Exurb1aBot.Model.Exceptions.LocationExceptions;
using Exurb1aBot.Model.ViewModel;
using Exurb1aBot.Model.ViewModel.WeatherModels;
using Exurb1aBot.Util.EmbedBuilders;
using Exurb1aBot.Util.Parsers;
using System;
using System.Threading.Tasks;

namespace Exurb1aBot.Modules {
    [Name("Weather Commands")]
    [Group("weather")]
    [Alias("w")]
    public class WeatherModule : ModuleBase {
        private ILocationRepository _repo;

        public WeatherModule(ILocationRepository lr) {
            _repo = lr;
         }

        #region Weather
        [Name("weather")]
        [Command("")]
        public async Task GetWeather(string name,[Remainder] string r) {
            if (name == "set" && r.Trim().Length != 0)
                await SetLocation(r);
            else
                await GetWeather(name + " " + r);
        }

        [Name("weather")]
        [Command("")]
        public async Task GetWeather(string name) {
            if (name == "set")
                await SetLocation();
            else {
                WeatherModel wm = await WeatherProcessor.GetWeatherDataName(name);
                if (wm == null)
                    throw new NoLocationFoundException();
                else {
                    wm.FixMapping();
                    await WeatherEmbedBuilder.DisplayWeather(wm, Context as SocketCommandContext);
                }
            }
        }

        [Command("")]
        public async Task GetWeather() {
            EntityUser user = new EntityUser(Context.Message.Author as IGuildUser);
            if (!_repo.UserHasLocation(user))
                throw new NoLocationAssociatedException();
            else {
                Location l = _repo.GetLocation(user);
                await GetWeather(l.LocationName,"");
            }
        }

        [Command("set")]
        public async Task SetLocation() {
            await EmbedBuilderFunctions.GiveErrorSyntax("weather search", new string[] { "**Location name**(required)" },
             new string[] { $"{Program.prefix}w set Birmingham", $"{Program.prefix}weather set Birmingham" },
             Context);
        }

        [Command("set")]
        public async Task SetLocation([Remainder]string location) {
            EntityUser user = new EntityUser(Context.Message.Author as IGuildUser);

            //check if location exists
            WeatherModel wm = await WeatherProcessor.GetWeatherDataName(location);
            if (wm == null)
                throw new NoLocationFoundException();

            if (_repo.UserHasLocation(user)) {
                _repo.ChangeLocation(user, location);
                _repo.SaveChanges();
                await Context.Channel.SendMessageAsync($"Location updated to {location}");
            }else {
                _repo.AddLocation(user, location);
                _repo.SaveChanges();
                await Context.Channel.SendMessageAsync($"Location set to {location}");
            }
        }
        #endregion
    }
}
