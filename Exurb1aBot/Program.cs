using System;
using System.Threading.Tasks;
using Discord.WebSocket;
using Discord.Commands;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Discord;
using Exurb1aBot.Data;
using Exurb1aBot.Model.Domain;
using Exurb1aBot.Data.Repository;
using Exurb1aBot.Util;
using Exurb1aBot.Modules;
using Exurb1aBot.Util.EmbedBuilders;

namespace Exurb1aBot {
    class Program {
        private DiscordSocketClient _client;
        private IServiceProvider _services;
        private CommandService _commands;
        private ApplicationDbContext _context;
        public static string prefix = ".";

        static void Main(string[] args) {
            new Program().MainAsync().GetAwaiter().GetResult();
        }

        public async Task MainAsync() {
            _client = new DiscordSocketClient();
            _commands = new CommandService();

            DatabaseConnection();
            DependencyInjection();

            Console.WriteLine("Initializing API");
            ApiHelper.InitializeClient();

            await InstallCommands();

            _client.Log += Log;
            _client.ReactionAdded += ReactionAdded;

            string token = "NTM5NTAzNjUwMDg3NjMyODk3.D0sJGQ.KepoFHwT1dpkA1_Lxiu531oihQE"; // Remember to keep this private! Fuck off
            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            // Block this task until the program is closed.
            await Task.Delay(-1);
        }


        private async Task ReactionAdded(Cacheable<IUserMessage,ulong> ch,ISocketMessageChannel chanel,SocketReaction reaction) {
            IUserMessage msg = await ch.GetOrDownloadAsync();
            if (msg.Id == SearchModule._tracked?.Id && reaction.UserId!=_client.CurrentUser.Id) {
                if (reaction.Emote.Name == "➡")
                    SearchModule.ChangeFrame(true);
                if (reaction.Emote.Name == "⬅")
                    SearchModule.ChangeFrame(false);
            }if (reaction.Emote.Name == "💬" && !msg.Author.IsBot) {
                    await QuoteModule.BotAddQuote(_services.GetService<IQouteRepository>(), chanel, msg.Content,msg.Id, reaction.User.GetValueOrDefault(null) as IGuildUser
                        , msg.Author as IGuildUser);
            }
        }

        private void DatabaseConnection() {
             _context = new ApplicationDbContext();
            Console.WriteLine("Initializing database");
            //_context.Database.EnsureDeleted();
            //_context.Database.EnsureCreated();
            _context.Initialize();
        }

        private void DependencyInjection() {
            _services = new ServiceCollection()
                .AddSingleton(_client)
                .AddSingleton(_context)
                .AddScoped< IQouteRepository, QouteRepository>()
                .AddScoped<IUserRepository,UserRepository>()
                .AddScoped<ILocationRepository,LocationRepository>()
                .BuildServiceProvider();
        }


        public async Task InstallCommands() {
            //Command in current assembly handled
            _client.MessageReceived += HandleCommands;

            Console.WriteLine("Initializing commands...");
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        }

        private async Task HandleCommands(SocketMessage arg) {

            if (arg == null || arg.Author.IsBot)
                return;

            SocketUserMessage msg = arg as SocketUserMessage;
            int argPos = 0;

            if (msg.HasStringPrefix(".", ref argPos) || msg.HasMentionPrefix(_client.CurrentUser, ref argPos)){
                var context = new SocketCommandContext(_client, msg);
                IResult result = await _commands.ExecuteAsync(context, argPos, _services);
                
                if (!result.IsSuccess) {

                    switch (result.Error) {
                        case CommandError.UnknownCommand:
                            await ListCommands(context);
                            break;
                        case CommandError.UnmetPrecondition:
                            await NoPermission(result.ErrorReason, msg.Channel);
                            break;
                        case CommandError.BadArgCount:
                            if (context.Message.ToString().StartsWith(prefix + "quote "))
                                await ListCommands(context);
                            break;
                       default:
                            await SendException(result.ErrorReason, arg.Channel);
                            break;
                    }
                }
            }
        }

        private async Task ListCommands(ICommandContext context) {
            await EmbedBuilderFunctions.GiveAllCommands(_commands,context,"Unkown Command");
        }

        private async Task NoPermission(string reason, ISocketMessageChannel channel) {
            await channel.SendMessageAsync("It seems you can't run this commands because:" +
                "```Diff\n" +
                $"-{reason}\n" +
                "```");
        }

        private async Task SendException(string ex,ISocketMessageChannel channel) {
            switch (ex) {
                case "There are no quotes in the server, Wow quote more losers":
                    await channel.SendMessageAsync(ex);
                    break;
                case "Empty Quote":
                    await channel.SendMessageAsync("What's the point in adding something empty huh?");
                    break;
                case "We couldn't find the user":
                    await channel.SendMessageAsync("I couldn't find this user, are you sure it's not an imaginary friend?");
                    break;
                case "No quotes found":
                    await channel.SendMessageAsync("This user has no quotes, wow talk more loser.");
                    break;
                case "we couldn't find the quote":
                    await channel.SendMessageAsync("I couldn't find the quote you're looking for.");
                    break;
                case "We couldn't find the video you're looking for":
                    await channel.SendMessageAsync("I couldn't find any videos, sorry chap...");
                    break;
                case "Image not found":
                    await channel.SendMessageAsync("I couldn't find any images matching your request, sorry chap...");
                    break;
                default:
                    await EmbedBuilderFunctions.UnhandledException(ex, channel);
                    break;
            }

        }

        public Task Log(LogMessage log) {
            Console.WriteLine(log.Message);
            return Task.CompletedTask;
        }
    }
}
