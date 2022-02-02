using Discord;
using System.Collections.Generic;
using static Exurb1aBot.Model.Domain.Enums;

namespace Exurb1aBot.Model.Domain {
    public interface IScoreRepository {
        Scores GetScoreByUser(IUser user, ulong serverId);
        bool HasScore(IUser user, ulong serverID);
        void Increment(IUser user, ScoreType type, ulong serverId, int val = 1);
        int GetScore(IUser user, ScoreType type, ulong serverId);
        Scores[] GiveTopNScores(int start, int end, ScoreType type, ulong serverId);
        int GiveRankUser(IUser user, ScoreType type, ulong serverId);
        int GiveAmountOfScores(ulong serverId);
        void Decrement(IUser user, ScoreType type, ulong serverId, int val = 1);
    }
}
