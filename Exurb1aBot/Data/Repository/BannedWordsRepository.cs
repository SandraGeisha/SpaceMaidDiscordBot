using Exurb1aBot.Model.Domain;
using Exurb1aBot.Model.ViewModel;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Exurb1aBot.Model.Exceptions.BannedWordExceptions;

namespace Exurb1aBot.Data.Repository {
    class BannedWordsRepository : IBannedWordsRepository {
        #region Attributes
        private ApplicationDbContext _context;
        private DbSet<BannedWord> _bannedWords;
        #endregion

        #region Constructor
        public BannedWordsRepository(ApplicationDbContext context) {
            _context = context;
            _bannedWords = context.BannedWords;
        }
        #endregion

        #region Methods
        public void AddWord(string word) {
            if (IsBanned(word))
                throw new WordAlreadyBannedException();

            BannedWord bw = new BannedWord(word);
            _bannedWords.Add(bw);
        }

        public ICollection<BannedWord> GetAllBannedWords() {
            return _bannedWords.ToList();
        }

        public bool IsBanned(string word) {
            return _bannedWords.Count(bw => bw.Word.Equals(word)) != 0;
        }

        public void RemoveBannedWord(string word) {
            if (!IsBanned(word))
                throw new WordNotBannedException();

            _bannedWords.Remove(_bannedWords.FirstOrDefault(bw => bw.Word.Equals(word)));
        }

        public void SaveChanges() {
            _context.SaveChanges();
        } 
        #endregion
    }
}
