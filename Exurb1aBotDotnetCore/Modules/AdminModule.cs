using Discord;
using Discord.Commands;
using Exurb1aBot.Util.EmbedBuilders;
using System;
using System.Threading.Tasks;
using Exurb1aBot.Util.Permissions;
using Discord.WebSocket;
using System.Net.Sockets;
using Exurb1aBot.Model.Domain;
using Exurb1aBot.Util.Extensions;

namespace Exurb1aBot.Modules {
    [Name("Admin Commands")]
    [RequireUserPermission(ChannelPermission.ManageMessages, Group = "perms")]
    [RequireSandra(Group = "perms")]
    public class AdminModule : ModuleBase<SocketCommandContext> {

        private IRoleRepository _roleRepo;

        public AdminModule(IRoleRepository roleRepo) {
            _roleRepo = roleRepo;
        }

        #region Shutdown Command
        [Command("shutdown")]
        public async Task ShutDown([Remainder]string s = "") {
            await Context.Channel.SendMessageAsync("Shutting down...");
            Environment.Exit(0);
        }
        #endregion

        #region Stream Command

        [Command("stream")]
        public async Task Mention(string name, string url = null) {
            await Context.Client.SetGameAsync(name, url, (url != null ? ActivityType.Streaming : ActivityType.Playing));
        }

        [Command("stream")]
        public async Task Mention() {
            await EmbedBuilderFunctions.GiveErrorSyntax("stream", new string[] { "**name**(required)", "**url**(optional,needs to be from twitch)" },
                new string[] { $"{Program.prefix}stream \"existential despair\"",
                    $"{Program.prefix}stream \"existential despair\" \"https://www.twitch.tv/directory/game/Depression%20Quest\"" }, Context);
        }
        #endregion

        #region Nickname
        [Command("nickname")]
        public async Task ChangeNick(string name) {
            IGuildUser user = Context.Guild.GetUser(Context.Client.CurrentUser.Id) as IGuildUser;

            if (user.GuildPermissions.ChangeNickname) {
                await user.ModifyAsync(u => u.Nickname = name, RequestOptions.Default);
                return;
            }

            await Context.Message.Channel.SendMessageAsync("I lack the permissions to change my nickname");
        }

        [Command("nickname")]
        public async Task ChangeNick() {
            IGuildUser user = Context.Guild.GetUser(Context.Client.CurrentUser.Id) as IGuildUser;

            if (user.GuildPermissions.ChangeNickname) {
                await user.ModifyAsync(u => u.Nickname = user.Username, RequestOptions.Default);
                return;
            }

            await Context.Message.Channel.SendMessageAsync("I lack the permissions to change my nickname");
        }
        #endregion

        #region Roles
            #region Assign Roles
                [Command("add assign role")]
                public async Task AddAssignRole(SocketRole role, [Remainder] string _ = "") {
                    if (_roleRepo.Exist(role.Id)) {
                        await Context.Channel.SendMessageAsync("The role you're trying to add is already in the database");
                        return;
                    }

                    if (_roleRepo.Exist(role.Name)) {
                        await Context.Channel.SendMessageAsync("The roles need to have a unique name");
                        return;
                    }

                    Role addedRole = new Role() {
                        ID = role.Id,
                        Name = role.Name,
                        RoleType = Enums.RoleType.Command
                    };

                    _roleRepo.Add(addedRole);
                    await Context.Channel.SendMessageAsync(string.Format("The role {0} is now self assignable.", role.Name));
                }


                [Command("add assign role")]
                public async Task AddAssignRole([Remainder] string _ = "") {
                    await EmbedBuilderFunctions.GiveErrorSyntax("add assign role", new string[] { "**name of the role**(required)" },
                        new string[] { $"{Program.prefix}add assign role Orange" }, Context);
                }

                [Command("remove assign role")]
                public async Task RemoveAssignRole([Remainder] string _ = "") {
                    await EmbedBuilderFunctions.GiveErrorSyntax("remove assign role", new string[] { "**name of the role**(required)" },
                        new string[] { $"{Program.prefix}remove assign role Orange" }, Context);
                }

                [Command("remove assign role")]
                public async Task RemoveAssignRole(SocketRole role) {
                    await RemoveRoll(role);
                }
            #endregion

            #region Reaction Roles
                [Command("add reaction role")]
                public async Task AddReactionRole(SocketRole role, string emote, [Remainder]string _ = "") {
                    bool parse = Emote.TryParse(emote, out Emote _);

                    if (_roleRepo.Exist(role.Id)) {
                        await Context.Channel.SendMessageAsync("The role you're trying to add is already in the database");
                        return;
                    }
                     
                    if (!parse && !emote.IsEmoji()) {
                        await Context.Channel.SendMessageAsync("Couldn't parse the emote you specified");
                        return;
                    }


                    if (_roleRepo.EmojiExist(emote)) {
                        await Context.Channel.SendMessageAsync("The emoji you specified for this role is already used");
                        return;
                    }

                    Role addRole = new Role() {
                        ID = role.Id,
                        Name = role.Name,
                        RoleType = Enums.RoleType.Reaction,
                        ReactionEmote = emote,
                        EmoteType = emote.IsEmoji() ? Enums.EmoteType.Emoji : Enums.EmoteType.Emote
                    };

                    _roleRepo.Add(addRole);
                    await Context.Channel.SendMessageAsync(string.Format("Reaction role {0} added to the database", addRole.Name));
                }

                [Command("add reaction role")]
                public async Task AddReactionRole([Remainder] string _ = "") {
                    await EmbedBuilderFunctions.GiveErrorSyntax("add reaction role", new string[] { "**name of the role**(required)" , "**emoji**(required)"},
                        new string[] { $"{Program.prefix}add reaction role Orange 💕" }, Context);
                }

                [Command("remove reaction role")]
                public async Task RemoveReactionRole([Remainder] string _ = "") {
                    await EmbedBuilderFunctions.GiveErrorSyntax("remove reaction role", new string[] { "**name of the role**(required)" },
                        new string[] { $"{Program.prefix}remove reaction role Orange" }, Context);
                }

                [Command("remove reaction role")]
                public async Task RemoveReactionRole(SocketRole role) {
                    await RemoveRoll(role);
                }
            #endregion
        #endregion

        #region Private functions
        private async Task RemoveRoll(SocketRole role) {
                if (!_roleRepo.Exist(role.Id)) {
                    await Context.Channel.SendMessageAsync("This role is not in the database. Did you forget to add it?");
                    return;
                }

                Role removeRole = _roleRepo.GetByName(role.Name);
                _roleRepo.Remove(removeRole);
                await Context.Channel.SendMessageAsync(string.Format("The role {0} is no longer in the database.", role.Name));
            }
        #endregion
    }
}