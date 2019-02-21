using Discord;
using Discord.Commands;
using Exurb1aBot.Model.ViewModel.WeatherModels;
using Exurb1aBot.Model.ViewModel.YoutubeModels;
using System;
using System.Threading.Tasks;
using Exurb1aBot.Util.Parsers;
using Exurb1aBot.Util.EmbedBuilders;
using Exurb1aBot.Model.ViewModel.ImageModels;
using Exurb1aBot.Model.Domain;
using Exurb1aBot.Data.Repository;
using Exurb1aBot.Model.ViewModel;
using Exurb1aBot.Model.Exceptions.LocationExceptions;

namespace Exurb1aBot.Modules {
    [Name("Search Commands")]
    public class SearchModule :ModuleBase<SocketCommandContext>{
        public static IMessage _tracked { get; private set; }
        private static YoutubeViewModel _yvm;
        private static ImageModel _ivm;
        private static int _indx;
        private static SocketCommandContext _context;
        private static SearchEnum _enum;
       

        /*
        #region Youtube
        [Command("youtube")]
        [Alias("yt")]
        public async Task YoutubeSearch() {
            await EmbedBuilderFunctions.GiveErrorSyntax("youtube", new string[] { "**Search Term**(required)" },
                new string[] { $"{Program.prefix}yt And nothing can ever ruin this", $"{Program.prefix}youtube 27 Exurb1a",
                    $"{Program.prefix}yt cat" }, Context);
        }

        [Command("youtube")]
        [Alias("yt")]
        public async Task YoutubeSearch([Remainder]string searchterm) {

            if (_tracked != null)
                await DeleteReactions(_tracked);

            _yvm = YoutubeParser.SearchYoutubeUrl(searchterm);
            _indx = 0;

            EmbedBuilder emb = await YoutubeEmbedBuilder.BuildVideo(_yvm, _indx, Context);
            _tracked = await Context.Channel.SendMessageAsync(embed: emb.Build());

            _enum = SearchEnum.Youtube;
            _context = Context;
            await AddReactions(_tracked);
        }
        #endregion
        */

        [Command("image")]
        [Alias("img")]
        public async Task ImageSearch([Remainder]string searchterm) {

            if (_tracked != null)
                await DeleteReactions(_tracked);

            _ivm = ImageParser.MakeImageModel(searchterm);
            _indx = 0;

            EmbedBuilder emb = await ImageEmbedBuilder.MakeImageEmbed(_ivm, _indx, Context);
            _tracked = await Context.Channel.SendMessageAsync(embed: emb.Build());

            _enum = SearchEnum.Image;
            _context = Context;

            await AddReactions(_tracked);
        }

        [Command("image")]
        [Alias("img")]
        public async Task ImageSearch() {
            await EmbedBuilderFunctions.GiveErrorSyntax("image", new string[] { "**Search Term**(required)" },
                    new string[] { $"{Program.prefix}image 27 Exurb1a",
                        $"{Program.prefix}img cat" }, Context);
        }


        #region Interactive frame
        public async static void ChangeFrame(bool forward) {
            var index = _indx;


            if (_indx != (_enum.Equals(SearchEnum.Youtube)?_yvm.Titles.Length:_ivm.Titles.Length) - 1 && forward)
                _indx++;
            if (_indx != 0 && !forward)
                _indx--;

            if (index != _indx) {
                var msg = _tracked as IUserMessage;

                EmbedBuilder emb;
                if (_enum.Equals(SearchEnum.Youtube))
                    emb = YoutubeEmbedBuilder.BuildVideo(_yvm, _indx, _context).Result;
                else
                    emb = ImageEmbedBuilder.MakeImageEmbed(_ivm, _indx, _context).Result;

                await msg.ModifyAsync(m => {
                    m.Embed = emb.Build();
                });

                await DeleteReactions(_tracked);
                await AddReactions(_tracked);
            }
        } 
        #endregion

        #region Helper Functions
        private static async Task AddReactions(IMessage msg) {
            var ms = msg as IUserMessage;

            if (_indx != 0)
                await ms.AddReactionAsync(new Emoji("⬅"));

            if (_indx != (_enum.Equals(SearchEnum.Youtube)?_yvm.Titles.Length : _ivm.Titles.Length )- 1)
                await ms.AddReactionAsync(new Emoji("➡"));
        }

        private static async Task DeleteReactions(IMessage msg) {
            var ms = msg as IUserMessage;
            try {
                await ms.RemoveAllReactionsAsync();
            }catch(Discord.Net.HttpException e) {
                if (e.HttpCode == System.Net.HttpStatusCode.NotFound)
                    Console.WriteLine("The original message was deleted");
            }
        } 
        #endregion
    }
}
