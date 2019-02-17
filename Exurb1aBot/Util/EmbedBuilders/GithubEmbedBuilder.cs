using Discord;
using Discord.Commands;
using Exurb1aBot.Model.ViewModel.GithubModels;
using System.Threading.Tasks;

namespace Exurb1aBot.Util.EmbedBuilders {
    public class GithubEmbedBuilder {
        public static async Task<EmbedBuilder> MakeGithubEmbed(GithubModel model, ICommandContext context) {
            EmbedBuilder emb = new EmbedBuilder() {
                Color = Color.Teal,
                Title = model.ProjectName,
            };

            EmbedFooterBuilder footer = await EmbedBuilderFunctions.AddFooter(context);
            footer.Text += " - Powered by Github";
            emb.WithFooter(footer);

            return emb;
        }
    }
}
