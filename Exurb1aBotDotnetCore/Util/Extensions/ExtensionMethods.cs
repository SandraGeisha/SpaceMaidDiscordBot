using Discord;
using Exurb1aBot.Model.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using IMessage = Discord.IMessage;

namespace Exurb1aBot.Util.Extensions {
    public static class ExtensionMethods {
        public static bool IsEmoji(this string input) {
            Regex rx = new Regex(Enums.EmojiRegex,RegexOptions.Compiled | RegexOptions.IgnoreCase);
            return rx.IsMatch(input);
        }

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

        public static IAsyncEnumerable<TEntity> AsAsyncEnumerable<TEntity>(this Microsoft.EntityFrameworkCore.DbSet<TEntity> obj) where TEntity : class {
            return Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.AsAsyncEnumerable(obj);
        }
        public static IQueryable<TEntity> Where<TEntity>(this Microsoft.EntityFrameworkCore.DbSet<TEntity> obj, System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate) where TEntity : class {
            return System.Linq.Queryable.Where(obj, predicate);
        }

    }
}
