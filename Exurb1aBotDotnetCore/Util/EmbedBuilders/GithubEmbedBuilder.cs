using Discord;
using Discord.Commands;
using Exurb1aBot.Model.ViewModel.GithubModels;
using Exurb1aBot.Util.Parsers;
using System.Threading.Tasks;

namespace Exurb1aBot.Util.EmbedBuilders {
    public class GithubEmbedBuilder {

        public static async Task<EmbedBuilder> MakeGithubEmbed(GithubModel model, ICommandContext context) {
            EmbedBuilder emb = new EmbedBuilder() {
                Color = Color.Green,
                Title = model.ProjectName,
                Url = "https://github.com/SandraGeisha/Exurb1aBot",
                ThumbnailUrl = "https://i.imgur.com/ASk5DlY.png"
            };

            emb.WithDescription(model.ProjectDescription);

            emb.AddField("Commits", model.Commits, true);
            emb.AddField("Stars", model.Stars, true);
            emb.AddField("Watchers", model.Watchers, true);
            emb.AddField("Author", model.AuthorName, true);

            emb.AddField("Found a bug,want to contribute or request a feature?", model.IssueLink);


            EmbedFooterBuilder footer = await EmbedBuilderFunctions.AddFooter(context);
            footer.Text += " - Powered by Github";
            emb.WithFooter(footer);

            return emb;
        }
    }
}
