using Discord.Commands;
using System.Threading.Tasks;
using Exurb1aBot.Util.EmbedBuilders;
using Exurb1aBot.Model.ViewModel.WeatherModels;
using Discord;
using Exurb1aBot.Util.Parsers;
using Exurb1aBot.Model.ViewModel.GithubModels;
using System;

namespace Exurb1aBot.Modules {
    [Name("General Commands")]
    public class BasicModule:ModuleBase<SocketCommandContext>{
        #region Fields
        //necessary for the View All Commands
        private readonly CommandService _cc;
        private readonly string[] Insults = new string[]{
            "I do not consider {{name}} a vulture. I consider {{name}} something a vulture would eat.",
            "People clap when they see {{name}}. They clap their hands over their eyes.",
            "{{name}}'s face is proof that god has a sense of humour.",
            "In the lands of the witless, {{name}} would be king.",
            "I'd prefer a battle of wits, but {{name}} seems to be unarmed.",
            "I regard {{name}} with an indifference bordering on aversion.",
            "{{name}} is the reason god made the middlefinger.",
            "Sometimes I need what only {{name}} can provide, Their absence.",
            "{{name}}'s inferiority complex is fully justified.",
            "{{name}} has delusions of adequacy.",
            "if {{name}} had another brain, it would be lonely",
            "I'm not surprised {{name}} doesn't have children,{{name}} is a dead end of evolution",
            "Senpai will never notice {{name}}",
            "{{name}} is getting old and will accomplished nothing.",
            "They stopped lobotimizing people for mental illnesses but when they'd examine {{name}} they might reconsider",
            "No I'm not insulting {{name}}, I'm describing {{name}}. Facts don't care about their feelings.",
            "If I wanted to kill myself I'd climb {{name}} their ego and jump to their IQ.",
            "Brains aren't everything. In {{name}}s case they're nothing.",
            "{{name}} is an oxygen thief!",
            "The last time I saw something like {{name}} , I flushed it.",
            "'m busy now. Can I ignore you some other time?",
            "{{name}} is the reason the gene pool needs a lifeguard.",
            "If {{name}} really spoke their mind, {{name}} would be speechless.",
            "As an outsider, what does {{name}} think of the human race?",
            "So, a thought crossed {{name}} mind? Must have been a long and lonely journey."
        };
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

            #region Source
                [Command("source")]
                public async Task ShowGithubSource() {
                    GithubModel gm = GithubParser.GetModel();
                    EmbedBuilder ebm = await GithubEmbedBuilder.MakeGithubEmbed(gm, Context);
                    await Context.Channel.SendMessageAsync(embed: ebm.Build());
                }

                [Command("source")]
                public async Task ShowGithubSource([Remainder]string s) {
                    await ShowGithubSource();
                }
        #endregion

            #region Insult
        [Command("insult")]
        public async Task Insult() {
            await EmbedBuilderFunctions.GiveErrorSyntax("Insult", new string[] { "**name**(@ mention,required)" },
                new string[] { $"{Program.prefix}Insult @Exurb1aBot#0069" }, Context);
        }

        [Command("insult")]
        public async Task Insult(IGuildUser user) {
            Random r = new Random();
            var message = Insults[r.Next(0, Insults.Length)].Replace("{{name}}", user.Mention);
            await Context.Channel.SendMessageAsync(message);
        }

        [Command("insult")]
        public async Task Insult([Remainder] string s) {
            await Insult();
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
