using Discord.Commands;
using System.Threading.Tasks;
using Exurb1aBot.Util.EmbedBuilders;
using Exurb1aBot.Model.ViewModel.WeatherModels;
using Discord;
using Exurb1aBot.Util.Parsers;
using Exurb1aBot.Model.ViewModel.GithubModels;
using System;
using System.Collections.Generic;
using System.Linq;
using Exurb1aBot.Model.Domain;
using Exurb1aBot.Model.ViewModel;

namespace Exurb1aBot.Modules {
    [Name("General Commands")]
    public class BasicModule : ModuleBase<SocketCommandContext> {
        #region Fields
        //necessary for the View All Commands
        private readonly CommandService _cc;
        private readonly IScoreRepsitory _scoreRepo;
        private readonly string[] Insults = new string[]{
            "I do not consider {{name}} a vulture. I consider {{name}} something a vulture would eat.",
            "People clap when they see {{name}}. They clap their hands over their eyes.",
            "{{name}}'s face is proof that God has a sense of humour.",
            "In the Lands of the Witless, {{name}} would be King.",
            "I'd prefer a battle of wits, but {{name}} seems to be unarmed.",
            "I regard {{name}} with an indifference bordering on aversion.",
            "{{name}} is the reason God made the middle finger.",
            "Sometimes I need what only {{name}} can provide, their absence.",
            "{{name}}'s inferiority complex is fully justified.",
            "{{name}} has delusions of adequacy.",
            "If {{name}} had another brain, it would be lonely",
            "I'm not surprised {{name}} doesn't have children, {{name}} is a dead end of evolution",
            "Senpai will never notice {{name}}",
            "{{name}} is getting old and will have accomplished nothing.",
            "They stopped lobotimizing people for mental illnesses but when they examine {{name}} they might reconsider",
            "No I'm not insulting {{name}}, I'm describing {{name}}. Facts don't care about their feelings.",
            "If I wanted to kill myself I'd climb {{name}} their ego and jump to their IQ.",
            "Brains aren't everything. In {{name}}'s case they're nothing.",
            "{{name}} is an oxygen thief!",
            "The last time I saw something like {{name}}, I flushed it.",
            "I am busy now. Can I ignore you some other time?",
            "{{name}} is the reason the gene pool needs a lifeguard.",
            "If {{name}} really spoke their mind, {{name}} would be speechless.",
            "As an outsider, what does {{name}} think of the human race?",
            "So, a thought crossed {{name}}'s mind? Must have been a long and lonely journey."
        };
        private List<string[]> AssignableRoles = new List<string[]>();

        #endregion

        #region Constructor
        public BasicModule(CommandService cc, IScoreRepsitory scoreRepo) {
            _cc = cc;
            _scoreRepo = scoreRepo;
            AssignableRoles.Add(new string[] { // colours
                "red", "orange", "yellow", "green", "blue", "purple", "brown", "white", "black"
            });
            AssignableRoles.Add(new string[] { "man", "woman", "enby" }); // genders
            AssignableRoles.Add(new string[] { "guitar player", "classic music", "metal music", // interests
                "rap music", "tabletop nerd", "chess player" });
        }
        #endregion

        #region Commands
        #region Ping & Pong
        [Command("ping")]
        public async Task Ping([Remainder] string s = "") {
            await Context.Channel.SendMessageAsync($"Pong ({GetMS()} ms) :ping_pong:");
        }

        [Command("pong")]
        public async Task Pong([Remainder] string s = "") {
            await Context.Channel.SendMessageAsync($"Ping ({GetMS()} ms) :ping_pong:");
        }

        #endregion

        [Command("rank")]
        public async Task Rank([Remainder] string s = "") {
            IGuildUser user = Context.Message.Author as IGuildUser;

            RankingModel rm = new RankingModel() {
                CreatedRank = _scoreRepo.GiveRankUser(user, Enums.ScoreType.Qouter),
                QuoteRank = _scoreRepo.GiveRankUser(user, Enums.ScoreType.Qouted),
                VCRank = _scoreRepo.GiveRankUser(user, Enums.ScoreType.VC)
            };

            EmbedBuilder emb = await RankEmbedBuilder.BuildRank(Context, rm, user);
            await Context.Message.Channel.SendMessageAsync(embed: emb.Build());
        }

        #region View all commands
        [Command("commands")]
        [Alias("c", "help")]
        public async Task Commands([Remainder] string s = "") {
            await EmbedBuilderFunctions.GiveAllCommands(_cc, Context);
        }
        #endregion

        #region Source
        [Command("source")]
        public async Task ShowGithubSource([Remainder]string s="") {
            GithubModel gm = GithubParser.GetModel();
            EmbedBuilder ebm = await GithubEmbedBuilder.MakeGithubEmbed(gm, Context);
            await Context.Channel.SendMessageAsync(embed: ebm.Build());
        }
        #endregion

        #region Insult
        [Command("insult")]
        public async Task Insult([Remainder] string s="") {
            await EmbedBuilderFunctions.GiveErrorSyntax("Insult", new string[] { "**name**(@ mention,required)" },
                new string[] { $"{Program.prefix}Insult @Exurb1aBot#0069" }, Context);
        }

        [Command("insult")]
        public async Task Insult(IGuildUser user) {
            Random r = new Random();
            if (!EasterInsultEggs(user)) {
                var message = Insults[r.Next(0, Insults.Length)].Replace("{{name}}", user.Mention);
                await Context.Channel.SendMessageAsync(message);
            }
        }
        #endregion

        #region Polls

        [Command("qp")]
        [Alias("poll")]
        [RequireBotPermission(ChannelPermission.AddReactions)]
        //public async Task QuickPoll([Remainder]string question) {
        //    var res = await Context.Channel.SendMessageAsync(question);
        //    IEmote check = new Emoji("✅");
        //    IEmote cross = new Emoji("❌");
        //    await res.AddReactionsAsync(new IEmote[] { check, cross });
        //    var bot = await Context.Channel.GetUserAsync(Context.Client.CurrentUser.Id) as IGuildUser;
        //    var permissions = bot.GetPermissions(Context.Guild.GetChannel(Context.Message.Channel.Id));
        //    if (permissions.ManageMessages)
        //        await Context.Message.DeleteAsync();
        //}
        public async Task QuickPoll([Remainder]string Question) {
            await Context.Message.DeleteAsync();
            var Embed = new EmbedBuilder {
                Title = "Quick Poll",
                Description = Question
            }
            .WithTimestamp(DateTime.Now);
            var Res = await Context.Channel.SendMessageAsync(embed: Embed.Build());
            IEmote Check = new Emoji("✅");
            IEmote Cross = new Emoji("❌");
            IEmote Line = new Emoji("➖");
            IEmote _Question = new Emoji("❔");
            await Res.AddReactionsAsync(new IEmote[] { Check, Cross, Line, _Question }); 
        }

        [Command("qp")]
        [Alias("poll")]
        public async Task QuickPoll() {
            await EmbedBuilderFunctions.GiveErrorSyntax("qp", new string[] { "**name**(required)" },
                new string[] { $"{Program.prefix}qp Is the milk gone?" }, Context);
        }

        #endregion

        #region Custom Roles

        [Command("addrole")]
        public async Task AddRole(string arg) {
            Discord.WebSocket.SocketRole CheckedRole;
            sbyte RoleType = -1;
            for (sbyte i = 0; i < AssignableRoles.Count; i++) RoleType += Convert.ToSByte(AssignableRoles[i].Contains(arg.ToLower()) ? i+1 : 0);
            if (RoleType == -1) {
                await Context.Message.DeleteAsync();
                await Context.Channel.SendMessageAsync((Context.User as IGuildUser).Mention + " that role either does not exist or is not eligible.");
            }
            else {
                foreach (string CheckingRole in AssignableRoles[RoleType]) {
                    CheckedRole = Context.Guild.Roles.FirstOrDefault(x => x.Name.ToLower() == CheckingRole.ToLower());
                    if ((Context.User as Discord.WebSocket.SocketGuildUser).Roles.Contains(CheckedRole))
                    await (Context.User as IGuildUser).RemoveRoleAsync(CheckedRole);
                }
                var Role = Context.Guild.Roles.FirstOrDefault(x => x.Name.ToLower().Equals(arg.ToLower()));
                await (Context.User as IGuildUser).AddRoleAsync(Role);
                await Context.Message.DeleteAsync();
                await Context.Channel.SendMessageAsync((Context.User as IGuildUser).Mention + ", enjoy your fancy new role!");
            }
        }

        #endregion

        #region private commands
        private int GetMS() {
            DateTime time_message = Context.Message.Timestamp.DateTime;
            return (int)Math.Round(DateTime.Now.Subtract(time_message).TotalMilliseconds / 100000);
        }

        private bool EasterInsultEggs(IGuildUser user) {
            if (user.Id == Enums.SandraID) {
                Context.Channel.SendMessageAsync("Why would I insult my owner? He's enough of a joke as it is.");
                return true;
            }

            if (user.Id == Context.Client.CurrentUser.Id) {
                Context.Channel.SendMessageAsync("Me? The knockoff Ub3r? Damn go insult one of your friends instead, oh wait... sorry.");
                return true;
            }

            return false;
        }
        #endregion
        #endregion
    }
}
