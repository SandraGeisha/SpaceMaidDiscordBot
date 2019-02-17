using Discord;
using Discord.Commands;
using Exurb1aBot.Model.ViewModel.ImageModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Exurb1aBot.Util.EmbedBuilders{
    public class ImageEmbedBuilder {
        public static async Task<EmbedBuilder> MakeImageEmbed(ImageModel model, int indx, ICommandContext context) {
            EmbedBuilder emb = new EmbedBuilder() {
                Color = Color.Teal,
                Title = model.Titles[indx],
                Url = model.Links[indx],
                ImageUrl = model.Thumbs[indx]
            };

            EmbedFooterBuilder footer = await EmbedBuilderFunctions.AddFooter(context);
            footer.Text += " - Powered by Bing";
            emb.WithFooter(footer);

            return emb;
        }
    
    }
}
