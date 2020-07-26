using Discord;
using Exurb1aBot.Model.Domain;
using Exurb1aBot.Model.ViewModel;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Exurb1aBot.Data.Repository {
    public class ScoreRepository : IScoreRepsitory {
        #region Private readonly fields
        private readonly ApplicationDbContext _context;
        private readonly DbSet<Scores> _scoreDbSet;
        private readonly IUserRepository _userRepo;
        #endregion

        #region Constructor
        public ScoreRepository(ApplicationDbContext context,IUserRepository userRepository) {
            _context = context;
            _scoreDbSet = context.Scores;
            _userRepo = userRepository;
        }
        #endregion

        #region Interface methods
        public int GetScore(IUser user, Enums.ScoreType type) {
            if (HasScore(user)) {
                Scores score = GetScoreByUser(user);

                switch (type) {
                    case Enums.ScoreType.Qouted:
                        return score.Quoted;
                    case Enums.ScoreType.Qouter:
                        return score.Quotes_Created;
                    case Enums.ScoreType.VC:
                        return score.VC_Score;
                }
            }

            return 0;
        }

        public Scores GetScoreByUser(IUser user) {
            return _scoreDbSet.FirstOrDefault(s => s.Id.Equals(user.Id));
        }

        public Scores[] GiveTopNScores(int start, int end, Enums.ScoreType type) {
            List<Scores> scores = GiveScoresOrderedBy(type);
            return scores.Skip(start).Take(end - start).ToArray();
        }

        public int GiveRankUser(IUser user, Enums.ScoreType type) {
            List<Scores> scores = GiveScoresOrderedBy(type);
            return scores.IndexOf(scores.FirstOrDefault(s => s.Id.Equals(user.Id))) + 1;
        }

        public bool HasScore(IUser user) {
            return _scoreDbSet.Any(s => s.Id.Equals(user.Id));
        }

        public void Increment(IUser user, Enums.ScoreType type, int val = 1) {
            bool hasScore = HasScore(user);

            EntityUser eu = _userRepo.GetUserById(user.Id);

            if (eu == null) {
                eu = new EntityUser(user as IGuildUser);
                _userRepo.AddUser(eu);
            }

            Scores score = hasScore ? GetScoreByUser(user) : new Scores() {
                Id = user.Id,
                Quoted = 0,
                Quotes_Created = 0,
                VC_Score = 0
            };

            switch (type) {
                case Enums.ScoreType.Qouted:
                    score.Quoted += val;
                    break;
                case Enums.ScoreType.Qouter:
                    score.Quotes_Created += val;
                    break;
                case Enums.ScoreType.VC:
                    score.VC_Score += val;
                    break;
            }

            if (!hasScore)
                _scoreDbSet.Add(score);

            _context.SaveChanges();
        }
        #endregion

        #region Private helper methods
        private List<Scores> GiveScoresOrderedBy(Enums.ScoreType type) {
            List<Scores> scores = _scoreDbSet.ToList();
            switch (type) {
                case Enums.ScoreType.Qouted:
                    scores = scores.OrderBy(s => s.Quoted).ToList();
                    break;
                case Enums.ScoreType.Qouter:
                    scores = scores.OrderBy(s => s.Quotes_Created).ToList();
                    break;
                case Enums.ScoreType.VC:
                    scores = scores.OrderBy(s => s.VC_Score).ToList();
                    break;
            }

            scores.Reverse();
            return scores;
        }

        public int GiveAmountOfScores() {
            return _scoreDbSet.Count();
        }

        public void Decrement(IUser user, Enums.ScoreType type, int val = 1) {
            if (!HasScore(user))
                throw new System.Exception("The fuck?");

            Scores s = GetScoreByUser(user);
            
            switch (type) {
                case Enums.ScoreType.Qouted:
                    s.Quoted -= val;
                    break;
                case Enums.ScoreType.Qouter:
                    s.Quotes_Created -= val;
                    break;
                default:
                    s.VC_Score -= val;
                    break;
            }

            _context.SaveChanges();
        }
        #endregion
    }
}
