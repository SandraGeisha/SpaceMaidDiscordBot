namespace Exurb1aBot.Model.ViewModel {
    public class BannedWord {
        #region Properties
        public int WordId { get; set; }
        public string Word { get; set; } 
        #endregion

        #region Constructors
        protected BannedWord() { }

        public BannedWord(string word) {
            Word = word;
        }
        #endregion
    }
}
