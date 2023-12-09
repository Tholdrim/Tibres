using System.Collections.Generic;

namespace Tibres.Commands
{
    public static class Permissions
    {
        public static readonly IPermission ManageExpressions = new Permission("Manage Expressions")
        {
            Description = "to upload custom emojis",
            Selector = p => p.ManageEmojisAndStickers,
            Filter = (mainGuildId, currentGuildId) => mainGuildId == currentGuildId,
            IsOptional = true
        };

        public static readonly IPermission UseExternalEmoji = new Permission("Use External Emoji")
        {
            Description = "to display custom emojis",
            Selector = p => p.UseExternalEmojis,
            Filter = (mainGuildId, currentGuildId) => mainGuildId != currentGuildId,
            IsOptional = true
        };

        internal static IEnumerable<Permission> GetAll()
        {
            yield return (Permission)ManageExpressions;
            yield return (Permission)UseExternalEmoji;
        }
    }
}
