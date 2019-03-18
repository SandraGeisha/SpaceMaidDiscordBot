using Discord;
using Discord.Commands;
using System;
using System.Threading.Tasks;
using Exurb1aBot.Util.Parsers;
using Exurb1aBot.Util.EmbedBuilders;
using Exurb1aBot.Model.ViewModel.ImageModels;
using Exurb1aBot.Model.Domain;
using Exurb1aBot.Util.Extensions;
/*
namespace Exurb1aBot.Modules {
    [Name("Search Commands")]
    public class SearchModule :ModuleBase<SocketCommandContext>{
        public static IMessage _tracked { get; private set; }
        private static ImageModel _ivm;
        private static int _indx;
        private static SocketCommandContext _context;
        private static SearchEnum _enum;
        private IBannedWordsRepository _repo;

        public SearchModule(IBannedWordsRepository repo) {
            _repo = repo;
        }

        [Command("image")]
        [Alias("img")]
        public async Task ImageSearch([Remainder]string searchterm) {
            if (_repo.IsBanned(searchterm.ToLower())){
                await Context.Channel.SendMessageAsync("You can't search for this, you dumbwit :) ");
            }
            else {
                if (_tracked != null)
                    await _tracked.RemoveReactions();

                _ivm = ImageParser.MakeImageModel(searchterm);
                _indx = 0;

                EmbedBuilder emb = await ImageEmbedBuilder.MakeImageEmbed(_ivm, _indx, Context);
                _tracked = await Context.Channel.SendMessageAsync(embed: emb.Build());

                _enum = SearchEnum.Image;
                _context = Context;

                await _tracked.AddNavigations(_indx, _ivm.Titles.Length);
            }
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


            if (_indx != (_ivm.Titles.Length) - 1 && forward)
                _indx++;
            if (_indx != 0 && !forward)
                _indx--;

            if (index != _indx) {
                var msg = _tracked as IUserMessage;

                EmbedBuilder emb;
                //if (_enum.Equals(SearchEnum.Image))
                emb = ImageEmbedBuilder.MakeImageEmbed(_ivm, _indx, _context).Result;

                await msg.ModifyAsync(m => {
                    m.Embed = emb.Build();
                });

                await _tracked.RemoveReactions();
                await _tracked.AddNavigations(_indx,_ivm.Titles.Length);
            }
        } 
        #endregion
    }
}
*/