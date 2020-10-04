using Discord;
using static Exurb1aBot.Model.Domain.Enums;

namespace Exurb1aBot.Model.Domain {
    public interface IScoreRepsitory {
        Scores GetScoreByUser(IUser user);
        bool HasScore(IUser user);
        void Increment(IUser user, ScoreType type, int val = 1);
        int GetScore(IUser user, ScoreType type);
        Scores[] GiveTopNScores(int start, int end, ScoreType type);
        int GiveRankUser(IUser user, ScoreType type);
        int GiveAmountOfScores();
        void Decrement(IUser user, ScoreType type, int val = 1);
    }
}
