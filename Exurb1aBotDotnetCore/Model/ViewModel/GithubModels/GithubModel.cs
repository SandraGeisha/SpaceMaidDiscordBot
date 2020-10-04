using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exurb1aBot.Model.ViewModel.GithubModels {
    public class GithubModel {
        public string AuthorLink { get; set; }
        public string ProjectDescription { get; set; }
        public int Commits { get; set; }
        public int Stars { get; set; }
        public int Watchers { get; set; }
        public string IssueLink { get; set; }
        public string ProjectName { get; set; }
        public string AuthorName { get; set; }
    }
}
