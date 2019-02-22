using Discord;
using Discord.Commands;
using Exurb1aBot.Util.EmbedBuilders;
using System;
using Exurb1aBot.Model.Domain;
using Exurb1aBot.Util.Extensions;
using System.Threading.Tasks;
using System.Linq;
using Discord.WebSocket;

namespace Exurb1aBot.Modules { 
    [Name("Admin Commands")]
   public class AdminModule : ModuleBase<SocketCommandContext> {
        #region Attribute
        private IBannedWordsRepository _repo;
        public static IUserMessage _trackedList;
        private static string[] _listContents;
        private static int _indx;
        #endregion

        #region Constructor
        public AdminModule(IBannedWordsRepository repo) {
            _repo = repo;
        } 
        #endregion

        #region Shutdown Command
        [Command("shutdown"), RequireUserPermission(ChannelPermission.ManageMessages)]
        public async Task ShutDown() {
            await Context.Channel.SendMessageAsync("Shutting down...");
            Environment.Exit(0);
        } 
        #endregion

        #region Stream Command
        [Command("stream"), RequireUserPermission(ChannelPermission.ManageMessages)]
        public async Task Mention(string name, string url = null) {
            await Context.Client.SetGameAsync(name, url, (url != null ? ActivityType.Streaming : ActivityType.Playing));
        }

        [Command("stream"), RequireUserPermission(ChannelPermission.ManageMessages)]
        public async Task Mention() {
            await EmbedBuilderFunctions.GiveErrorSyntax("stream", new string[] { "**name**(required)", "**url**(optional,needs to be from twitch)" },
                new string[] { $"{Program.prefix}stream \"existential despair\"",
                    $"{Program.prefix}stream \"existential despair\" \"https://www.twitch.tv/directory/game/Depression%20Quest\"" }, Context);
        }

        [Command("stream"), RequireUserPermission(ChannelPermission.ManageMessages)]
        public async Task Mention([Remainder]string s) {
            await Mention();
        }
        #endregion

        #region Banword
        [RequireUserPermission(GuildPermission.ManageMessages)]
        [Command("banword")]
        public async Task BanWordHelp() {
            EmbedBuilder eb = await EmbedBuilderFunctions.MakeHelp("BanWord command help", "This command is for banning words from being searched (weather not included)",
                "https://i.imgur.com/Zd5uncM.png", "banword", new string[] { "help", "add", "remove", "list" }, new string[] {
                    "banword add This searchterm is bad m'kay", "banword remove cats are despicable","banword help","banword list"
                }, Context);
            await Context.Channel.SendMessageAsync(embed: eb.Build());
        }

        [RequireUserPermission(GuildPermission.ManageMessages)]
        [Command("banword")]
        public async Task BanWord([Remainder]string arg) {
            string[] command = arg.Split(' ');
            switch (command[0]) {
                case "help":
                    await BanWordHelp();
                    break;
                case "add":
                    await AddBanWord(string.Join(" ", command).Remove(0, 4));
                    break;
                case "remove":
                    await RemoveBanWord(string.Join(" ", command).Remove(0, 7));
                    break;
                case "list":
                    await ViewBannedWordList(string.Join(" ", command).Remove(0, 5));
                    break;
                default:
                    await BanWordHelp();
                    break;
            }
        }

        [RequireUserPermission(GuildPermission.ManageMessages)]
        [Command("banword")]
        public async Task AddBanWord(string word) {
            if (word != null && word.Trim().Length != 0) {
                _repo.AddWord(word.ToLower());
                _repo.SaveChanges();
                await Context.Channel.SendMessageAsync($"**{word.RemoveAbuseCharacters()}** banned from being searched");
            }
            else {
                await EmbedBuilderFunctions.GiveErrorSyntax("banword add", new string[] { "word(required)" },
                     new string[] { "banword add kitties are now banned" }, Context);
            }
        }

        [RequireUserPermission(GuildPermission.ManageMessages)]
        [Command("banword")]
        public async Task RemoveBanWord(string word) {
            if (word != null && word.Trim().Length != 0) {
                _repo.RemoveBannedWord(word);
                _repo.SaveChanges();
                await Context.Channel.SendMessageAsync($"**{word.RemoveAbuseCharacters()}** removed and is now searchable");
            }
            else {
                await EmbedBuilderFunctions.GiveErrorSyntax("banword remove", new string[] { "word(required)" },
                     new string[] { "banword remove kitties are now unbanned" }, Context);
            }
        }

        [RequireUserPermission(GuildPermission.ManageMessages)]
        [Command("banword")]
        public async Task ViewBannedWordList(string x) {
            _indx = 0;
            _listContents = _repo.GetAllBannedWords().OrderBy(bw=>bw.Word)
                .Select(bw=>bw.Word.RemoveAbuseCharacters()).ToArray();

            if (_listContents.Count() == 0)
                await Context.Channel.SendMessageAsync("There are no banned words in this server");
            else
                await DisplayList(Context.Channel);
            
        }

        public static async Task ChangeIndex(ISocketMessageChannel context,bool forward) {
            if (_indx != 0 && !forward)
                _indx--;
            if (_indx != (int)Math.Ceiling((decimal)_listContents.Count() / 10)&&forward)
                _indx++;

           await DisplayList(context);
        }
        #endregion

        #region Display
        private static async Task DisplayList(ISocketMessageChannel context) {
            if (_trackedList != null)
                await _trackedList.DeleteMessage();

            var msg = "```diff\r\n";
            for(int i = 10 * _indx; i < (i + 10 > _listContents.Count() ? _listContents.Count() : i + 10); i++) {
                msg += $"-{_listContents[i]}\r\n";    
            }
            msg += "```";

            _trackedList = await context.SendMessageAsync(msg);

            await _trackedList.RemoveReactions();
            await _trackedList.AddNavigations(_indx, (int)Math.Ceiling((decimal)_listContents.Count() / 10));
        }
        #endregion
    }
}
