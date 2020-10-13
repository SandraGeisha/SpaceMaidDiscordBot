using Discord;
using Discord.Commands;
using Exurb1aBot.Model.Domain;
using Exurb1aBot.Util.EmbedBuilders;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exurb1aBot.Util.Permissions;
using Discord.WebSocket;

namespace Exurb1aBot.Modules {
    [Name("Roles Commands")]
    [Group("roles")]
    public class RoleModule : ModuleBase<SocketCommandContext> {
        private readonly IRoleRepository _roleRepo;

        public RoleModule(IRoleRepository roleRepository) {
            _roleRepo = roleRepository;
        }

        [Command("view")]
        [Alias("")]
        public async Task GetRoles([Remainder] string s = "") {
            IEnumerable<Role> roles = _roleRepo.GetAllByType(Enums.RoleType.Command);

            if (!roles.Any()) {
                await Context.Channel.SendMessageAsync("There's no roles configured.");
                return;
            }

            Embed emb = EmbedBuilderFunctions.ShowCommandRolesEmbed(roles, Context);
            await Context.Channel.SendMessageAsync(embed: emb);
        }

        [Command("assign")]
        [RequireRole(533965735878328320, Group = "perms")]
        [RequireSandra(Group = "perms")]
        public async Task AssignRole(string name, [Remainder] string _ = "") {
            IEnumerable<Role> roles = _roleRepo.GetAllByType(Enums.RoleType.Command);

            if (!roles.Any(r => r.Name.ToLower().Equals(name.ToLower()))) {
                await Context.Channel.SendMessageAsync("We couldn't find the role you're looking for.");
                return;
            }

            Role role = roles.FirstOrDefault(r => r.Name.ToLower().Equals(name.ToLower()));
            if (role != null) {
                SocketRole roleSocket = Context.Guild.GetRole(role.ID);
                
                if (roleSocket == null) {
                    await Context.Channel.SendMessageAsync("Something went wrong with finding this role.");
                    return;
                }

                await (Context.User as IGuildUser).AddRoleAsync(roleSocket);
            }
        }

        [Command("assign")]
        [RequireRole(533965735878328320, Group = "perms")]
        [RequireSandra(Group = "perms")]
        public async Task AssignRole([Remainder] string _ = "") {
            await EmbedBuilderFunctions.GiveErrorSyntax("roles assign", new string[] { "**name of the role**(required)" },
                new string[] { $"{Program.prefix}roles assign Orange" }, Context);
        }
    }
}
