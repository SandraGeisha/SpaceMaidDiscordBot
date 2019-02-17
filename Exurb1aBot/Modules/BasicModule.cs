using Discord.Commands;
using System.Threading.Tasks;
using Exurb1aBot.Util.EmbedBuilders;
using Exurb1aBot.Model.ViewModel.WeatherModels;
using Discord;

namespace Exurb1aBot.Modules {
    [Name("General Commands")]
    public class BasicModule:ModuleBase<SocketCommandContext>{
        #region Fields
        //necessary for the View All Commands
        private readonly CommandService _cc; 
        #endregion

        #region Constructor
        public BasicModule(CommandService cc) {
                _cc = cc;
            }
        #endregion

        #region Commands
            #region Ping
            [Command("ping")]
            public async Task Ping() {
                await Context.Channel.SendMessageAsync("Pong");
            }

            [Command("ping")]
            public async Task Ping([Remainder] string s) {
                await Ping();
            }
            #endregion

            #region View all commands
            [Command("commands")]
            [Alias("c")]
            public async Task Commands() {
                await EmbedBuilderFunctions.GiveAllCommands(_cc, Context);
            }

            [Command("commands")]
            [Alias("c")]
            public async Task Commands([Remainder] string s) {
                await Commands();
            }
            #endregion



            #region Quick poll

            [Command("qp")]
            [RequireBotPermission(ChannelPermission.AddReactions)]
            public async Task QuickPoll([Remainder]string question) {
                var res = await Context.Channel.SendMessageAsync(question);
                IEmote check = new Emoji("✅");
                IEmote cross = new Emoji("❌");
                await res.AddReactionsAsync(new IEmote[] { check, cross });
                var bot = await Context.Channel.GetUserAsync(Context.Client.CurrentUser.Id) as IGuildUser;
                var permissions = bot.GetPermissions(Context.Guild.GetChannel(Context.Message.Channel.Id));
                if (permissions.ManageMessages)
                    await Context.Message.DeleteAsync();
            }

            [Command("qp")]
            public async Task QuickPoll() {
                await EmbedBuilderFunctions.GiveErrorSyntax("qp", new string[] { "**name**(required)" },
                    new string[] { $"{Program.prefix}qp Is the milk gone?" }, Context);
            }

            #endregion 
        #endregion
    }
}
