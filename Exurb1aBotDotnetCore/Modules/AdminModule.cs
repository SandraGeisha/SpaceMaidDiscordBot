using Discord;
using Discord.Commands;
using Exurb1aBot.Util.EmbedBuilders;
using System;
using System.Threading.Tasks;
using Exurb1aBot.Util.Permissions;
using Discord.WebSocket;

namespace Exurb1aBot.Modules {
    [Name("Admin Commands")]
    [RequireSandra]
    public class AdminModule : ModuleBase<SocketCommandContext> {

        #region Shutdown Command
        [Command("shutdown")]
        public async Task ShutDown([Remainder]string s = "") {
            await Context.Channel.SendMessageAsync("Shutting down...");
            Environment.Exit(0);
        }
        #endregion

        #region Stream Command

        [Command("stream")]
        public async Task Mention(string name, string url = null) {
            await Context.Client.SetGameAsync(name, url, (url != null ? ActivityType.Streaming : ActivityType.Playing));
        }

        [Command("stream")]
        public async Task Mention() {
            await EmbedBuilderFunctions.GiveErrorSyntax("stream", new string[] { "**name**(required)", "**url**(optional,needs to be from twitch)" },
                new string[] { $"{Program.prefix}stream \"existential despair\"",
                    $"{Program.prefix}stream \"existential despair\" \"https://www.twitch.tv/directory/game/Depression%20Quest\"" }, Context);
        }
        #endregion

        #region Nickname
        [Command("nickname")]
        public async Task ChangeNick(string name) {
            var guilds = Context.Client.Guilds;
            foreach (SocketGuild guild in guilds) {
              IGuildUser user = guild.GetUser(Context.Client.CurrentUser.Id) as IGuildUser;

              if (user.GuildPermissions.ChangeNickname) {
                  await user.ModifyAsync(u => u.Nickname = name, RequestOptions.Default);
              }
            }
        }

        [Command("nickname")]
        public async Task ChangeNick() {

          var guilds = Context.Client.Guilds;

          foreach (SocketGuild guild in guilds) {
            IGuildUser user = guild.GetUser(Context.Client.CurrentUser.Id) as IGuildUser;

            if (user.GuildPermissions.ChangeNickname) {
              await user.ModifyAsync(u => u.Nickname = user.Username, RequestOptions.Default);
            }
          }
        }
        #endregion

    }
}