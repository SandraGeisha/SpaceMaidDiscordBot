using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using IMessage = Discord.IMessage;

namespace Exurb1aBot.Util.Extensions {
    public static class ExtensionMethods {
        public static string RemoveAbuseCharacters(this string input) {
            return input
                .Replace("`", "'");
        }

         public static async Task AddNavigations(this IMessage msg,int indx,int maxLength) {
            var ms = msg as IUserMessage;

            if (indx != 0)
                await ms.AddReactionAsync(new Emoji("⬅"));

            if (indx != maxLength - 1)
                await ms.AddReactionAsync(new Emoji("➡"));
        }

        public static async Task RemoveReactions(this IMessage msg) {
            var ms = msg as IUserMessage;
            try {
                await ms.RemoveAllReactionsAsync();
            }
            catch (Discord.Net.HttpException e) {
                if (e.HttpCode == System.Net.HttpStatusCode.NotFound)
                    Console.WriteLine("The original message was deleted");
            }
        }

        public static async Task DeleteMessage(this IMessage msg) {
            var ms = msg as IUserMessage;
            try {
                await ms.DeleteAsync();
            }
            catch (Discord.Net.HttpException e) {
                if (e.HttpCode == System.Net.HttpStatusCode.NotFound)
                    Console.WriteLine("The original message was deleted");
            }
        }
    }
}
