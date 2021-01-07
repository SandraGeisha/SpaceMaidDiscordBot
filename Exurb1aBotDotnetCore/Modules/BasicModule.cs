using Discord.Commands;
using System.Threading.Tasks;
using Exurb1aBot.Util.EmbedBuilders;
using Exurb1aBot.Model.ViewModel.WeatherModels;
using Discord;
using Exurb1aBot.Util.Parsers;
using Exurb1aBot.Model.ViewModel.GithubModels;
using System;
using Exurb1aBot.Model.Domain;
using Exurb1aBot.Model.ViewModel;
using Exurb1aBotDotnetCore.Model.Exceptions;

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
            "If {{name}} had another brain, it would be lonely.",
            "I'm not surprised {{name}} doesn't have children, {{name}} is a dead end of evolution.",
            "Senpai will never notice {{name}}.",
            "{{name}} is getting old and will never accomplish anything.",
            "They stopped lobotimizing people for mental illnesses but when they examine {{name}} they might reconsider.",
            "No I'm not insulting {{name}}, I'm describing {{name}}. Facts don't care about their feelings.",
            "If I wanted to kill myself I'd climb {{name}}'s ego and jump to their IQ.",
            "Brains aren't everything. In {{name}}'s case they're nothing.",
            "{{name}} is an oxygen thief!",
            "The last time I saw something like {{name}}, I flushed it.",
            "I am busy now. Can I ignore you some other time?",
            "{{name}} is the reason the gene pool needs a lifeguard.",
            "If {{name}} really spoke their mind, {{name}} would be speechless.",
            "As an outsider, what does {{name}} think of the human race?",
            "So, a thought crossed {{name}}'s mind? Must have been a long and lonely journey."
        };
        #endregion

        #region Constructor
        public BasicModule(CommandService cc, IScoreRepsitory scoreRepo) {
            _cc = cc;
            _scoreRepo = scoreRepo;
        }
        #endregion

        #region Commands
        #region Ping & Pong
        [Command("ping")]
        public async Task Ping([Remainder] string s = "") {
            TimeSpan diff = DateTime.Now.ToUniversalTime().Subtract(Context.Message.Timestamp.UtcDateTime);
            await Context.Channel.SendMessageAsync(string.Format("pong 🏓 ({0} ms)", +Math.Abs(Math.Round(diff.TotalMilliseconds))));
        }

        [Command("pong")]
        public async Task Pong([Remainder] string s = "") {
            TimeSpan diff = DateTime.Now.ToUniversalTime().Subtract(Context.Message.Timestamp.UtcDateTime);
            await Context.Channel.SendMessageAsync(string.Format("ping 🏓 ({0} ms)", +Math.Abs(Math.Round(diff.TotalMilliseconds))));
        }

        #endregion

        [Command("rank")]
        public async Task Rank(IGuildUser user, [Remainder] string _ = "") {
            if (!_scoreRepo.HasScore(user))
                throw new UserHasNoScoreException();

            RankingModel rm = new RankingModel() {
                CreatedRank = _scoreRepo.GiveRankUser(user, Enums.ScoreType.Qouter),
                QuoteRank = _scoreRepo.GiveRankUser(user, Enums.ScoreType.Qouted),
                VCRank = _scoreRepo.GiveRankUser(user, Enums.ScoreType.VC)
            };

            EmbedBuilder emb = await RankEmbedBuilder.BuildRank(Context, rm, user);
            await Context.Message.Channel.SendMessageAsync(embed: emb.Build());
        }

        [Command("rank")]
        public async Task Rank([Remainder] string _ = "") {
            await Rank(Context.Message.Author as IGuildUser);
        }

        #region View all commands
        [Command("commands")]
        [Alias("c","help")]
        public async Task Commands([Remainder] string _ = "") {
            await EmbedBuilderFunctions.GiveAllCommands(_cc, Context);
        }
        #endregion

        #region Source
        [Command("source")]
        public async Task ShowGithubSource([Remainder]string _ = "") {
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

        [Command("fuck")]
        [Alias("f")]
        public async Task FuckThatPerson(IGuildUser user, [Remainder] string s="") {
            await Context.Channel.SendMessageAsync($"Fuck you {user.Mention}");
        }

        [Command("fuck")]
        [Alias("f")]
        public async Task FuckThatPerson([Remainder] string s = "") {
            await EmbedBuilderFunctions.GiveErrorSyntax("fuck", new string[] { "User (required)" },
                new string[] { $"{Program.prefix}fuck @sov"}, Context);
        }

        #region Quick poll

        [Command("qp")]
        [Alias("poll")]
        [RequireBotPermission(ChannelPermission.AddReactions)]
        public async Task QuickPoll([Remainder]string question) {
            var res = await Context.Channel.SendMessageAsync(
                embed: EmbedBuilderFunctions.MakeEmbedPoll(question, Context).Build()
           );

            IEmote check = new Emoji("✅");
            IEmote questionMark = new Emoji("❔");
            IEmote indif = new Emoji("➖");
            IEmote cross = new Emoji("❌");

            await res.AddReactionsAsync(new IEmote[] { check, cross , indif, questionMark});
            var bot = await Context.Channel.GetUserAsync(Context.Client.CurrentUser.Id) as IGuildUser;
            var permissions = bot.GetPermissions(Context.Guild.GetChannel(Context.Message.Channel.Id));

            if (permissions.ManageMessages)
                await Context.Message.DeleteAsync();
        }

        [Command("qp")]
        [Alias("poll")]
        public async Task QuickPoll() {
            await EmbedBuilderFunctions.GiveErrorSyntax("qp", new string[] { "**name**(required)" },
                new string[] { $"{Program.prefix}qp Is the milk gone?" }, Context);
        }

        #endregion

        #region private commands
        private bool EasterInsultEggs(IGuildUser user) {
            if (user.Id == Enums.SandraID) {
                Context.Channel.SendMessageAsync("Why would I insult my owner? He's enough of a joke as it is.");
                return true;
            }

            if (user.Id == Context.Client.CurrentUser.Id) {
                Context.Channel.SendMessageAsync("Me? The knockoff Ub3r? Damn go insult one of your friends instead, oh wait... sorry.");
                return true;
            }

            if (user.Id == Enums.MudaID){
                Context.Channel.SendMessageAsync("Insulting Muda is like kicking a puppy with one eye. Fucking pathetic."); // muda is gay
                return !false;
            }

            return false;
        }
        #endregion
        #endregion
    }
}
