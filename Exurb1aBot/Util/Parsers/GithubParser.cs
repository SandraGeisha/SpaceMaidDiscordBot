using Exurb1aBot.Model.ViewModel.GithubModels;
using HtmlAgilityPack;
using System.Linq;
using System;

namespace Exurb1aBot.Util.Parsers {
    class GithubParser {
        public const string ProjectLink = "https://github.com/SandraGeisha/Exurb1aBot";
        public static GithubModel GetModel() {
            string html = ApiHelper.GetHtmlUrl(ProjectLink);
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            return ParseDocument(doc);
        }

        private static GithubModel ParseDocument(HtmlDocument doc) {

            var main = doc.DocumentNode.Descendants("div").Where(d =>
               d.GetAttributeValue("class", "").Equals("application-main ")
            ).FirstOrDefault();

            var list = main.Descendants("ul").Where(d =>
                d.GetAttributeValue("class", "").Equals("pagehead-actions")
            ).FirstOrDefault();

            var listItems = list.Descendants("li").ToList();
            var stars=0;
            var watchers = 0;

            for (int i = 0; i < 2; i++) {
                var value = int.Parse(listItems[i].Descendants("a").Where(d =>
                    d.GetAttributeValue("class", "").Equals(
                        (i % 2==0? "social-count" : "social-count js-social-count")
                    )).FirstOrDefault().InnerHtml.Trim());

                if (i % 2 == 0)
                    watchers = value;
                else
                    stars = value;
            }

            var issueLink = "https://www.github.com"+main.Descendants("svg").Where(d=>
                d.GetAttributeValue("class","").Equals("octicon octicon-issue-opened")
            ).FirstOrDefault().ParentNode.GetAttributeValue("href","");

            var author = main.Descendants("span").Where(d =>
                d.GetAttributeValue("class", "").Equals("author")
            ).FirstOrDefault().FirstChild;
            var authorLink = "https://www.github.com" + author.GetAttributeValue("href", "");
            var authorName = author.InnerHtml;

            var projectName=author.ParentNode.ParentNode.Descendants("strong").First().InnerText;

            var projectDescription = main.Descendants("div").Where(d =>
                d.GetAttributeValue("class", "").Equals("f4")
            ).First().FirstChild.NextSibling.InnerText.Trim();

            var NumberBanner = main.Descendants("ul").Where(d =>
                d.GetAttributeValue("class", "").Equals("numbers-summary")
            ).First();

            var commits = int.Parse(
                NumberBanner.Descendants("li").First().InnerText.Replace("commits","").Trim()
            );

            return new GithubModel {
                Stars = stars,
                Watchers = watchers,
                IssueLink = issueLink,
                AuthorLink = authorLink,
                AuthorName = authorName,
                ProjectName = projectName,
                ProjectDescription = projectDescription,
                Commits = commits
            };
        }
    }
}
