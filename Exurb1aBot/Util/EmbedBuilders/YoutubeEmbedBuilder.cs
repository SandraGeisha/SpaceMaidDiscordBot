using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Exurb1aBot.Model.ViewModel.YoutubeModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Exurb1aBot.Util.EmbedBuilders {
    class YoutubeEmbedBuilder {
        public async static Task<EmbedBuilder> BuildVideo(YoutubeViewModel yvm,int indx,ICommandContext context) {
            EmbedBuilder emb = new EmbedBuilder();
            emb.WithColor(Color.Teal);

            emb.WithThumbnailUrl(yvm.Thumbs[indx]);
            emb.WithUrl(yvm.Urls[indx]);

            emb.WithTitle(yvm.Titles[indx]);
            emb.WithDescription(yvm.Descriptions[indx]);

            emb.AddField("View Count", yvm.ViewCounts[indx],true);
            emb.AddField("Duration", yvm.Times[indx], true);

            emb.AddField("Channel", yvm.Channels[indx]);

            EmbedFooterBuilder efb = await EmbedBuilderFunctions.AddFooter(context);
            efb.Text += " - powered by youtube";
            emb.WithFooter(efb);

            return emb;
        }
    }
}
