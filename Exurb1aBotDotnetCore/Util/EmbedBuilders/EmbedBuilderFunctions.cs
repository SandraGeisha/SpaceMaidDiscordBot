using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Exurb1aBot.Model.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exurb1aBot.Util.Extensions;

namespace Exurb1aBot.Util.EmbedBuilders {
    public static class EmbedBuilderFunctions {
        private static EmbedFooterBuilder embf = new EmbedFooterBuilder();

        public static async Task GiveAllCommands(CommandService _commands, ICommandContext context, string ErrorReason = null) {
            IEnumerable<ModuleInfo> modInfo = _commands.Modules;
            EmbedBuilder ebm = new EmbedBuilder();

            if (ErrorReason == null) {
                ebm.WithTitle("Command list");
                ebm.WithColor(Color.Green);
            }
            else {
                ebm.WithTitle(ErrorReason);
                ebm.WithColor(Color.Red);
            }

            ebm.WithDescription("List of all available commands:");

            foreach (ModuleInfo mi in modInfo) {
                ebm.AddField(mi.Name, String.Join(", ", mi.Commands.Select(mx => mx.Name).Where(name=>name.Trim().Length!=0).Distinct().ToArray()));
            }

            ebm.WithFooter(AddFooter(context).Result);

            await context.Channel.SendMessageAsync(embed: ebm.Build());
        }

        public async static Task<EmbedBuilder> MakeHelp(string title,string description,
            string url,string commandName,string[] parameters, string[] Examples,ICommandContext context) {
            EmbedBuilder eb = new EmbedBuilder();

            eb.WithColor(Color.Green);
            eb.WithTitle(title);

            eb.WithDescription(description);
            eb.WithThumbnailUrl(url);

            eb.AddField("command name", commandName, true);
            eb.AddField("Parameters", string.Join(",",parameters), true);
            eb.AddField("Examples", string.Join("\r\n",Examples));

            eb.WithFooter(await AddFooter(context));

            return eb;
        }

        public async static Task<EmbedFooterBuilder> AddFooter(ICommandContext context) {
            IGuildUser user = await context.Guild.GetUserAsync((ulong)401452008957280257);
            embf.WithIconUrl(user.GetAvatarUrl());
            embf.Text = $"Made by {user.Nickname??user.Username}";
            return embf;
        }

        public async static Task GiveErrorSyntax(string command, string[] parameters, string[] examples, ICommandContext context) {
            EmbedBuilder ebm = new EmbedBuilder() {
                Color = Color.Red
            };

            ebm.WithTitle("Command Error Syntax");
            ebm.WithDescription($"Error in running the command {command}");

            ebm.AddField("Parameters", string.Join(" ", parameters));

            ebm.AddField("Examples", string.Join("\r\n", examples));

            ebm.WithFooter(await AddFooter(context));

            await context.Channel.SendMessageAsync(embed: ebm.Build());
        }

        public async static Task DisplayQuote(Quote q, IGuildUser[] users, ICommandContext context) {
            EmbedBuilder ebm = new EmbedBuilder() {
                Color = Color.Green
            };

            IGuildUser quotee = users[0];
            ebm.WithTitle($"Quote #{q.Id} by {(quotee == null ? q.Qoutee.Username : (quotee.Nickname??quotee.Username))}");

            if (quotee != null) 
                ebm.WithThumbnailUrl(quotee.GetAvatarUrl());
            if (ebm.ThumbnailUrl == null)
                ebm.WithThumbnailUrl("https://discordapp.com/assets/dd4dbc0016779df1378e7812eabaa04d.png");

            ebm.AddField("Quote", $"```\r\n{q.QuoteText.RemoveAbuseCharacters()}\r\n```");

            EmbedFooterBuilder efb = new EmbedFooterBuilder();
            IGuildUser creator = users[1];

            if (creator != null) 
                efb.WithIconUrl(creator.GetAvatarUrl());
            if (efb.IconUrl == null)
                efb.WithIconUrl("https://discordapp.com/assets/dd4dbc0016779df1378e7812eabaa04d.png");

            efb.WithText($"Quoted by {(creator == null ? q.Creator.Username : (creator.Nickname??creator.Username))} on {q.Time.ToShortDateString()}");
            ebm.WithFooter(efb);

            await context.Channel.SendMessageAsync(embed: ebm.Build());
        }

        public  static EmbedBuilder MakeEmbedPoll(string question, ICommandContext context) {
            return new EmbedBuilder{
                Title = "Quick Poll",
                Description = question,
                Color = Color.Green,
                Timestamp = DateTime.Now
            }.WithFooter(Footer => { 
                Footer.Text = "Poll by " + context.User.Username;
                Footer.IconUrl = context.User.GetAvatarUrl() ?? context.User.GetDefaultAvatarUrl();
            });
        }       
        
        public  static EmbedBuilder MakeReactionMessageEmbed(string message, ICommandContext context) {
            return new EmbedBuilder {
                Title = "React To This Message!",
                Description = message,
                Color = Color.Green,
                Timestamp = DateTime.Now
            }.WithFooter(AddFooter(context).Result);
        }
        
        public async static Task UnhandledException(string ex,ISocketMessageChannel channel) {
            EmbedBuilder builder = new EmbedBuilder {
                ImageUrl = "https://37.media.tumblr.com/40bc69a6ae90bfe6a90fbdce4fb7516b/tumblr_n6k977lF4T1rna0heo1_1280.gif",
                Color = Color.Red,
                Description = "Error: " + ex

            };
            await channel.SendMessageAsync(embed: builder.Build());
        }

        public static Embed ShowCommandRolesEmbed(IEnumerable<Role> roles, ICommandContext context) {
            EmbedBuilder emb = new EmbedBuilder() {
                Color = Color.Green,
                Title = "Roles",
                Description = "Once you reach Tier 12, you can assign these roles yourself with the roles assign command.",
            }.WithFooter(AddFooter(context).Result);

            foreach (Role r in roles) {
                emb.AddField("Role Name", r.Name, true);
            }

            return emb.Build();
        }

        public static Embed ShowReactionRolesEmbed(IEnumerable<Role> roles, ICommandContext context) {
            EmbedBuilder emb = new EmbedBuilder() {
                Color = Color.Green,
                Title = "Reaction Roles",
                Description = "This is a list of all reaction roles currently configured.",
            }.WithFooter(AddFooter(context).Result);

            foreach (Role r in roles) {
                emb.AddField("Role Name", r.Name, true);
                emb.AddField("Emote type", r.EmoteType, true);
                emb.AddField("Emote", (r.EmoteType == Enums.EmoteType.Emoji ? (IEmote)new Emoji(r.ReactionEmote) : (IEmote)Emote.Parse(r.ReactionEmote)),true);
            }

            return emb.Build();
        }
    }
}
