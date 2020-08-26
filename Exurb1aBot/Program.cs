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
using Exurb1aBot.Model.Exceptions.QuoteExceptions;
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.Extensions.Options;
using Exurb1aBot.Util.Workers;

[assembly: UserSecretsId("Exurb1aBotSecrets")]
namespace Exurb1aBot {
    class Program {
        private DiscordSocketClient _client;
        private IServiceProvider _services;
        private CommandService _commands;
        private ApplicationDbContext _context;
        private VCWorkerService _workerService;
        public static string prefix = "-";

        static void Main(string[] args) {
            new Program().MainAsync().GetAwaiter().GetResult();
        }

        public async Task MainAsync() {
            _client = new DiscordSocketClient();
            _commands = new CommandService();

            DatabaseConnection();
            DependencyInjection();

            IScoreRepsitory scoreRepo = _services.GetService<IScoreRepsitory>();
            _workerService = new VCWorkerService(scoreRepo);

            Console.WriteLine("Initializing API");
            ApiHelper.InitializeClient();

            await InstallCommands();

            _client.Log += Log;
            _client.ReactionAdded += ReactionAdded;

            _client.UserVoiceStateUpdated += UserVCUpdated;

            await _client.LoginAsync(TokenType.Bot, "NzQzODcyNjU2OTAxNzk5OTg4.Xza_ZQ.MjHmCcKkVNf751PY-M0hylUEc6E");
            await _client.StartAsync();

            // Block this task until the program is closed.
            await Task.Delay(-1);
        }

        private async Task UserVCUpdated(SocketUser arg1, SocketVoiceState arg2, SocketVoiceState arg3) {
            if (arg3.VoiceChannel != null) {
                await _workerService.AddWorkerToChannel(arg3.VoiceChannel);
            }
        }

        private async Task ReactionAdded(Cacheable<IUserMessage,ulong> ch,ISocketMessageChannel chanel,SocketReaction reaction) {
            IUserMessage msg = await ch.GetOrDownloadAsync();  
            if (reaction.Emote.Name == "💬" && !msg.Author.IsBot) {
                    try {
                    await QuoteModule.BotAddQuote(_services.GetService<IQouteRepository>(),
                        _services.GetService<IScoreRepsitory>(), _services.GetService<IUserRepository>(),
                        chanel, msg.Content, msg.Id, reaction.User.GetValueOrDefault(null) as IGuildUser
                        , msg.Author as IGuildUser, msg.Timestamp.DateTime);
                    }
                        catch (Exception e) {
                        if (e.GetType().Equals(typeof(QuotingYourselfException)))
                            await msg.Channel.SendMessageAsync("A bit narcissistic to quote yourself, no?");
                    }
            }
        }

        private void DatabaseConnection() {
             _context = new ApplicationDbContext();
            Console.WriteLine("Initializing database");
            _context.Initialize();
        }

        private void DependencyInjection() {
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddEnvironmentVariables();
            builder.AddUserSecrets<Secrets>();
            IConfigurationRoot configuration = builder.Build();


            _services = new ServiceCollection()
                .AddSingleton(_client)
                .AddSingleton(_context)
                .AddScoped< IQouteRepository, QouteRepository>()
                .AddScoped<IUserRepository,UserRepository>()
                .AddScoped<IScoreRepsitory,ScoreRepository>()
                .AddScoped<ILocationRepository,LocationRepository>()
                .AddOptions()
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

            if (msg.HasStringPrefix(prefix, ref argPos) || msg.HasMentionPrefix(_client.CurrentUser, ref argPos)){
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
                case "Image not found":
                    await channel.SendMessageAsync("I couldn't find any images matching your request, sorry chap...");
                    break;
                case "We couldn't find a location":
                    await channel.SendMessageAsync("We couldn't find the location, or perhaps you were searching for Birmingham ya drugie.");
                    break;
                case "word already banned":
                    await channel.SendMessageAsync("Banning a word twice won't unban it ;) ");
                    break;
                case "There is no location set":
                    await channel.SendMessageAsync("You don't have a location set you dummy, " +
                        $"use the command `{prefix}weather set`");
                    break;
                case "word not banned":
                    await channel.SendMessageAsync($"This word is not in the banlist. To view the list use `{prefix}banword list`");
                    break;
                case "You're trying to quote yourself":
                    await channel.SendMessageAsync("A bit narcissistic to quote yourself, no?");
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

        public static void AddUserStore() {


        }
    }
}
