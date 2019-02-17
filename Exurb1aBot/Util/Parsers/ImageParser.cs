using Exurb1aBot.Model.ViewModel.ImageModels;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Linq;
using System;
using System.Collections.Generic;
using Exurb1aBot.Model.Exceptions;

namespace Exurb1aBot.Util.Parsers {
    public class ImageParser {
        public static ImageModel MakeImageModel(string searchTerm) {
            string html = ApiHelper.GetHtmlUrl($"https://www.bing.com/images/search?q={searchTerm}&qs=n&form=QBLH&scope=images&sp=-1&pq=catgirl&sc=3-7&sk=&cvid=48225CE36ABD4854857CB22D0AF6C77E");
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

            ImageModel img = ParseHtmlDocument(doc);
            return img;
        }

        private static ImageModel ParseHtmlDocument(HtmlDocument doc) {
            var Row = doc.DocumentNode.Descendants("div").Where(d => d.GetAttributeValue("class", "").Equals("row"))
                .FirstOrDefault();

            if (Row == null)
                throw new ImageNotFoundException();

            var Images = Row.Descendants("div").Where(d => d.GetAttributeValue("class", "").Equals("item")).ToList();

            ICollection<string> links = new List<string>();
            ICollection<string> thumbs = new List<string>();
            ICollection<string> titles = new List<string>();

            foreach(var img in Images) {
                var i = img.Descendants("img").FirstOrDefault();
                thumbs.Add(i.GetAttributeValue("src", ""));

                var refA = img.Descendants("a").Where(d => d.GetAttributeValue("class", "").Equals("tit"))
                    .FirstOrDefault();
                links.Add(refA.GetAttributeValue("href", ""));
                titles.Add(refA.NextSibling.InnerText);
            }

            ImageModel imgModel = new ImageModel() {
                Links = links.ToArray(),
                Titles = titles.ToArray(),
                Thumbs = thumbs.ToArray()
            };

            return imgModel;
        }
    }
}
