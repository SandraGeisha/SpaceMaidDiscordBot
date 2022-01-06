using Discord;
using Discord.Commands;
using Exurb1aBot.Model.Domain;
using Exurb1aBot.Model.ViewModel;
using Exurb1aBot.Util.EmbedBuilders;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exurb1aBot.Modules {
    [Name("Ranking Commands")]
    [Group("top")]
    public class RankingModule : ModuleBase {
        internal static int _pageAmount = 10;
        private readonly IScoreRepository _scoreRepo;
        private readonly IUserRepository _userRepo;

        public RankingModule(IScoreRepository scoreRepo, IUserRepository userRepo) {
            _scoreRepo = scoreRepo;
            _userRepo = userRepo;
        }

        #region General Method
        [Command("")]
        public async Task GeneralMethod([Remainder] string s = "") {
            await GiveTopQoutes(0, s);
        }

        [Command("")]
        public async Task GeneralMethod(int page,[Remainder] string s = "") {
            await GiveTopQoutes(page, s);
        }
        #endregion

        [Command("quotes")]
        public async Task GiveTopQoutes([Remainder] string s="") {
            await GiveTopQoutes(0, s);
        }


        [Command("quotes")]
        public async Task GiveTopQoutes(int page = 0, [Remainder] string s = "") {
            page = SanitizePage(page);
            
            Scores[] scores = _scoreRepo.GiveTopNScores(page * _pageAmount, (page + 1) * _pageAmount, Enums.ScoreType.Qouted);
            List<EntityUser> users = GetUsers(scores);

            EmbedBuilder emb = await RankEmbedBuilder.BuildRankEmbed(Context, scores, users.ToArray(), page, Enums.ScoreType.Qouted);
            await Context.Channel.SendMessageAsync(embed: emb.Build());
        }

        [Command("created")]
        public async Task GiveTopCreatedQoutes([Remainder] string s = "") {
            await GiveTopCreatedQoutes(0, s);
        }

        [Command("vc")]
        public async Task GiveTopVCRank([Remainder] string s = "") {
            await GiveTopVCRank(0, s);
        }
        
        [Command("vc")]
        public async Task GiveTopVCRank(int page = 0, [Remainder] string s = "") {
            page = SanitizePage(page);

            Scores[] scores = _scoreRepo.GiveTopNScores(page * _pageAmount, (page + 1) * _pageAmount, Enums.ScoreType.VC);
            List<EntityUser> users = GetUsers(scores);

            EmbedBuilder emb = await RankEmbedBuilder.BuildRankEmbed(Context, scores, users.ToArray(), page, Enums.ScoreType.VC);
            await Context.Channel.SendMessageAsync(embed: emb.Build());
        }

        [Command("created")]
        public async Task GiveTopCreatedQoutes(int page = 0, [Remainder] string s = "") {
            page = SanitizePage(page);

            Scores[] scores = _scoreRepo.GiveTopNScores(page * _pageAmount, (page + 1) * _pageAmount, Enums.ScoreType.Qouter);
            List<EntityUser> users = GetUsers(scores);

            EmbedBuilder emb = await RankEmbedBuilder.BuildRankEmbed(Context, scores, users.ToArray(), page, Enums.ScoreType.Qouter);
            await Context.Channel.SendMessageAsync(embed: emb.Build());
        }


        private List<EntityUser> GetUsers(Scores[] scores) {
            List<EntityUser> users = new List<EntityUser>();

            foreach (ulong id in scores.Select(sc => sc.Id).ToList()) {
                users.Add(_userRepo.GetUserById(id));
            }

            return users;
        }

        private int SanitizePage(int page) {
            if (page > 0)
                page -= 1;

            if (page < 0)
                page = 0;

            int maxPage = _scoreRepo.GiveAmountOfScores() / _pageAmount;

            if (page > maxPage)
                page = maxPage;

            return page;
        }
    }
}
