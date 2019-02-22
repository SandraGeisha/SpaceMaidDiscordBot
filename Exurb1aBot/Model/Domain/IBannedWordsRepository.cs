using Exurb1aBot.Model.ViewModel;
using System.Collections.Generic;

namespace Exurb1aBot.Model.Domain {
    public interface IBannedWordsRepository {
        #region Methods
        ICollection<BannedWord> GetAllBannedWords();
        void RemoveBannedWord(string word);
        bool IsBanned(string word);
        void AddWord(string word);
        void SaveChanges();
        #endregion
    }
}
