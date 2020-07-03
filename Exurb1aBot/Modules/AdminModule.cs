using Discord;
using Discord.Commands;
using Exurb1aBot.Util.EmbedBuilders;
using System;
using Exurb1aBot.Model.Domain;
using Exurb1aBot.Util.Extensions;
using System.Threading.Tasks;
using System.Linq;
using Discord.WebSocket;
using Exurb1aBot.Util.Permissions;

namespace Exurb1aBot.Modules {
    [Name("Admin Commands")]
    [RequireUserPermission(ChannelPermission.ManageMessages,Group ="perms")]
    [RequireSandra(Group ="perms")]
    public class AdminModule : ModuleBase<SocketCommandContext> {

        #region Shutdown Command
        [Command("shutdown")]
        public async Task ShutDown([Remainder]string s="") {
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
        public async Task Mention([Remainder]string s ="") {
            await EmbedBuilderFunctions.GiveErrorSyntax("stream", new string[] { "**name**(required)", "**url**(optional,needs to be from twitch)" },
                new string[] { $"{Program.prefix}stream \"existential despair\"",
                    $"{Program.prefix}stream \"existential despair\" \"https://www.twitch.tv/directory/game/Depression%20Quest\"" }, Context);
        }
        #endregion
    }
}
