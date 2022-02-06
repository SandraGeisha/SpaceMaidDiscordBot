using Discord.Commands;
using Exurb1aBot.Model.Domain;
using System;
using System.Threading.Tasks;

namespace Exurb1aBot.Util.Permissions {
    class RequireSandra : PreconditionAttribute {
        public override Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider services) {

            if (context.Message.Author.Id.Equals(Enums.SandraID)) {
                return Task.FromResult(PreconditionResult.FromSuccess());
            }

            return Task.FromResult(PreconditionResult.FromError("Only developers can do these actions"));
        }
    }
}
