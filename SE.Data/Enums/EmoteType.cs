using System.Collections.Generic;

namespace SE.Data.Enums
{
    public enum EmoteType
    {
        Blank = 0,
        List = 1,
        DiscordLogo = 2,
        TwitterLogo = 3
    }

    public static class EmoteTypeHelper
    {
        private static readonly Dictionary<EmoteType, string> Emotes =
            new Dictionary<EmoteType, string>
            {
                {EmoteType.Blank, "<:Blank:773616910867628032>"},
                {EmoteType.List, "<:List:773616884086996993>"},
                {EmoteType.DiscordLogo, "<:DiscordLogo:773630216282046464>"},
                {EmoteType.TwitterLogo, "<:TwitterLogo:773630216433172500>"}
            };

        public static string Display(this EmoteType emoteType)
        {
            Emotes.TryGetValue(emoteType, out var value);
            return value ?? emoteType.ToString();
        }
    }
}