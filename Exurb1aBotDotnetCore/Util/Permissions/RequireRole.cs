using Discord;
using Discord.Commands;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Exurb1aBot.Util.Permissions {
    class RequireRole : PreconditionAttribute {
        private ulong _roleId;

        public RequireRole(ulong roleID) {
            _roleId = roleID;
        }

        public override Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider services) {

            if ((context.Message.Author as IGuildUser).RoleIds.Contains(_roleId)) {
                return Task.FromResult(PreconditionResult.FromSuccess());
            }

            string role = context.Guild.GetRole(_roleId)?.Name;
            return Task.FromResult(PreconditionResult.FromError($"You do not have the required role ({role ?? "undefined"}) to run this command."));
        }
    }
}
