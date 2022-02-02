using Discord;
using Exurb1aBot.Model.Domain;
using Exurb1aBot.Model.ViewModel;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Exurb1aBot.Data.Repository {
    public class ScoreRepository : IScoreRepository {
        #region Private readonly fields
        private readonly ApplicationDbContext _context;
        private readonly DbSet<Scores> _scoreDbSet;
        private readonly IUserRepository _userRepo;
        private readonly IServerRepository _serverRepo;
        #endregion

        #region Constructor
        public ScoreRepository(ApplicationDbContext context,IUserRepository userRepository, IServerRepository serverRepo) {
            _context = context;
            _scoreDbSet = context.Scores;
            _userRepo = userRepository;
            _serverRepo = serverRepo;
        }
        #endregion

        #region Interface methods
        public int GetScore(IUser user, Enums.ScoreType type, ulong serverId) {
            if (HasScore(user,serverId)) {
                Scores score = GetScoreByUser(user,serverId);

                switch (type) {
                    case Enums.ScoreType.Qouted:
                        return score.Quoted;
                    case Enums.ScoreType.Qouter:
                        return score.Quotes_Created;
                }
            }

            return 0;
        }

        public Scores GetScoreByUser(IUser user, ulong serverId) {
            return _scoreDbSet.FirstOrDefault(s => s.Id.Equals(user.Id) && s.Server.ID == serverId);
        }

        public Scores[] GiveTopNScores(int start, int end, Enums.ScoreType type, ulong serverId) {
            List<Scores> scores = GiveScoresOrderedBy(type,serverId);
            return scores.Skip(start).Take(end - start).ToArray();
        }

        public int GiveRankUser(IUser user, Enums.ScoreType type, ulong serverId) {
            List<Scores> scores = GiveScoresOrderedBy(type,serverId);
            return scores.IndexOf(scores.FirstOrDefault(s => s.Id.Equals(user.Id))) + 1;
        }

        public bool HasScore(IUser user, ulong serverId) {
            return _scoreDbSet.Any(s => s.Id.Equals(user.Id) && s.Server.ID == serverId);
        }

        public void Increment(IUser user, Enums.ScoreType type, ulong serverId, int val = 1) {
            bool hasScore = HasScore(user,serverId);

            EntityUser eu = _userRepo.GetUserById(user.Id);

            if (eu == null) {
                eu = new EntityUser(user as IGuildUser);
                _userRepo.AddUser(eu);
            }

            Server server = _serverRepo.GetById(serverId);
            if (server == null) {
              server = new Server() { ID = serverId };
              _serverRepo.Add(server);
            }

            Scores score = hasScore ? GetScoreByUser(user,serverId) : new Scores() {
                Id = user.Id,
                Quoted = 0,
                Quotes_Created = 0,
                Server = server
            };

            switch (type) {
                case Enums.ScoreType.Qouted:
                    score.Quoted += val;
                    break;
                case Enums.ScoreType.Qouter:
                    score.Quotes_Created += val;
                    break;
            }

            if (!hasScore)
                _scoreDbSet.Add(score);

            _context.SaveChanges();
        }
        #endregion

        #region Private helper methods
        private List<Scores> GiveScoresOrderedBy(Enums.ScoreType type, ulong serverId) {
            IEnumerable<Scores> scores = _scoreDbSet.ToList().Where(s => s.Server.ID == serverId).ToList();

            switch (type) {
                case Enums.ScoreType.Qouted:
                    scores = scores.OrderBy(s => s.Quoted);
                    break;
                case Enums.ScoreType.Qouter:
                    scores = scores.OrderBy(s => s.Quotes_Created);
                    break;
            }

            return scores.Reverse().ToList();
        }

        public int GiveAmountOfScores(ulong serverId) {
            return _scoreDbSet.ToList()
                .Where(s=>s.Server.ID == serverId).Count();
        }

        public void Decrement(IUser user, Enums.ScoreType type, ulong serverId, int val = 1) {
            if (!HasScore(user,serverId))
                throw new System.Exception("The fuck?");

            Scores s = GetScoreByUser(user,serverId);
            
            switch (type) {
                case Enums.ScoreType.Qouted:
                    s.Quoted -= val;
                    break;
                default:
                    s.Quotes_Created -= val;
                    break;
      }

            _context.SaveChanges();
        }

    #endregion
  }
}
