using System.Collections.Generic;

namespace Tibres.Commands
{
    internal static class Permissions
    {
        public static readonly IEnumerable<Permission> All =
        [
            new("Use External Emoji")
            {
                Description = "to display custom emojis",
                Selector = p => p.UseExternalEmojis,
                Filter = (mainGuildId, currentGuildId) => mainGuildId != currentGuildId
            },
            new("Manage Expressions")
            {
                Description = "to upload custom emojis",
                Selector = p => p.ManageEmojisAndStickers,
                Filter = (mainGuildId, currentGuildId) => mainGuildId == currentGuildId,
                IsOptional = true
            }
        ];
    }
}
