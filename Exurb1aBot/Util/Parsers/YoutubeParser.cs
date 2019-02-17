using Exurb1aBot.Model.Exceptions;
using Exurb1aBot.Model.ViewModel.YoutubeModels;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Exurb1aBot.Util.Parsers {
    class YoutubeParser {
        public static YoutubeViewModel SearchYoutubeUrl(string searchTerm) {
            string html = ApiHelper.GetHtmlUrl($"https://www.youtube.com/results?search_query={searchTerm}&sp=EgIQAQ%253D%253D");
            HtmlDocument youtubeHtml = new HtmlDocument();
            youtubeHtml.LoadHtml(html);

            var node = youtubeHtml.DocumentNode.Descendants("div")
                .Where(d => d.GetAttributeValue("class", "").Equals("search-message")).FirstOrDefault();

            if (node != null && node.InnerText.StartsWith("No results for"))
                throw new NoVideoFoundException();

            YoutubeViewModel ytvm = new YoutubeViewModel();
            Parse(ytvm, youtubeHtml);
            return ytvm;
        }

        private static void Parse(YoutubeViewModel ytvm, HtmlDocument htmlDoc) {
            var videosList = htmlDoc.DocumentNode.Descendants("div").Where(node => 
            node.GetAttributeValue("id", "contents").Equals("")).ToList();
            var videos = videosList[0].Descendants().Where(d => d.GetAttributeValue("class", "").StartsWith("yt-lockup "))
                .Distinct().ToList().Take(5);

            string channelLink=null;

            ICollection<string> times = new List<string>();
            ICollection<string> thumbs = new List<string>();
            ICollection<string> links = new List<string>();
            ICollection<string> titles = new List<string>();
            ICollection<string> channels = new List<string>();
            ICollection<string> viewCounts = new List<string>();
            ICollection<string> descriptions = new List<string>();

            foreach (var vid in videos) {new List<string>();
                var time = vid.Descendants("span").Where(d => d.GetAttributeValue("class", "").Equals("video-time"))
                    .FirstOrDefault().InnerHtml;
                var img = vid.Descendants("img").Where(d => d.ParentNode.GetAttributeValue("class", "").Equals("yt-thumb-simple"))
                    .FirstOrDefault().GetAttributeValue("src","");

                var ANodeTitleAndLink = vid.Descendants("a").Where(d => d.ParentNode.GetAttributeValue("class", "").Equals("yt-lockup-title "))
                    .FirstOrDefault();
                var link = ANodeTitleAndLink.GetAttributeValue("href", "");
                var title = ANodeTitleAndLink.FirstChild.InnerHtml;

                var ANodeAuthor = vid.Descendants("a").Where(d => d.ParentNode.GetAttributeValue("class", "").Equals("yt-lockup-byline "))
                    .FirstOrDefault();
                var channel = ANodeAuthor.InnerHtml;
                channelLink = $"https://www.youtube.com{ANodeAuthor.GetAttributeValue("href", "")}";

                var viewCount = vid.Descendants("ul").Where(d => d.GetAttributeValue("class", "").Equals("yt-lockup-meta-info"))
                    .FirstOrDefault().LastChild.InnerHtml.Replace(" views","");

                var description = vid.Descendants("div").Where(d => d.GetAttributeValue("class", "").StartsWith("yt-lockup-description"))
                    .FirstOrDefault().InnerText;

                descriptions.Add(description);
                thumbs.Add(img);
                times.Add(time);
                links.Add($"https://www.youtube.com{link}");
                titles.Add(title);
                channels.Add(channel);
                viewCounts.Add(viewCount);
            }

            ytvm.Channels = channels.ToArray();
            ytvm.Descriptions = descriptions.ToArray();
            ytvm.Thumbs = thumbs.ToArray();
            ytvm.Times = times.ToArray();
            ytvm.Titles = titles.ToArray();
            ytvm.Urls = links.ToArray();
            ytvm.ViewCounts = viewCounts.ToArray();
        }

    }
}
