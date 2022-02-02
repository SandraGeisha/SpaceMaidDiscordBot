using Discord;
using Discord.Commands;
using Exurb1aBot.Model.Domain;
using Exurb1aBot.Model.ViewModel;
using Exurb1aBot.Modules;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Threading.Tasks;

namespace Exurb1aBot.Util.EmbedBuilders {
    public static class RankEmbedBuilder {
        public async static Task<EmbedBuilder> BuildRank(ICommandContext context, RankingModel ranking , IGuildUser user) {
            EmbedFooterBuilder footer = await EmbedBuilderFunctions.AddFooter(context);

            EmbedBuilder builder = new EmbedBuilder() {
                Color = Color.Green,
                Title = $"Rank for {user.Nickname ?? user.Username}",
                ThumbnailUrl = user.GetAvatarUrl(),
                Description = $"So you want to know your standing huh? Well here it is.",
                Footer = footer
            };

            builder.AddField("Qouted Ranking", $"#{ranking.QuoteRank}", true);
            builder.AddField("Quotes Created Ranking", $"#{ranking.CreatedRank}", true);

            return builder;
        }

        public async static Task<EmbedBuilder> BuildRankEmbed(ICommandContext context,Scores[] scores, EntityUser[] users, int page, Enums.ScoreType type) {
            string title = $"Server Ranking: Times Quoted- page {page+1}";
            string status = "Quotes";

            switch (type) {
                case Enums.ScoreType.Qouter:
                    title = $"Server Ranking: Quotes Created - page {page + 1}";
                    status = "Quotes created";
                    break;
            }


            EmbedBuilder emb = new EmbedBuilder() { 
                Title = title,
                Color = Color.Green
            };

            foreach (Scores score in scores) {
                int s = score.Quoted;

                switch (type) {
                    case Enums.ScoreType.Qouter:
                        s = score.Quotes_Created;
                        break;
                }

                int indx = scores.IndexOf(score);
                int rank = indx + 1 + (page * RankingModule._pageAmount);

                EntityUser user = users[indx];
                IGuildUser userg = await context.Guild.GetUserAsync(user.Id);

                if (userg != null) {
                    emb.AddField(new EmbedFieldBuilder() {
                        Name = $"Rank {rank}",
                        Value = $"{userg.Nickname ?? userg.Username} ({s} {status})",
                        IsInline = false
                    });
                } else {
                    emb.AddField(new EmbedFieldBuilder() {
                        Name = $"Rank {rank}",
                        Value = $"{user.Username} ({s} {status})",
                        IsInline = false
                    });
                }
            }

            EmbedFooterBuilder efb = await EmbedBuilderFunctions.AddFooter(context);
            emb.WithFooter(efb);

            return emb;
        }
    }
}
