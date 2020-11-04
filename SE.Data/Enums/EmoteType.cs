using System.Collections.Generic;

namespace SE.Data.Enums
{
    public enum EmoteType
    {
        Blank = 0,
        List = 1,
        DiscordLogo = 2,
        TwitterLogo = 3,
        PineappleConfused = 4,
        PineappleThinking = 5,
        PineappleReading = 6,
        PineappleLove = 7,
        PineappleCrying = 10
    }

    public static class EmoteTypeHelper
    {
        private static readonly Dictionary<EmoteType, string> Emotes =
            new Dictionary<EmoteType, string>
            {
                {EmoteType.Blank, "<:Blank:773616910867628032>"},
                {EmoteType.List, "<:List:773616884086996993>"},
                {EmoteType.DiscordLogo, "<:DiscordLogo:773630216282046464>"},
                {EmoteType.TwitterLogo, "<:TwitterLogo:773630216433172500>"},
                {EmoteType.PineappleConfused, "<:PineappleConfused:773691866339344394>"},
                {EmoteType.PineappleThinking, "<:PineappleThinking:773692108933824543>"},
                {EmoteType.PineappleReading, "<:PineappleReading:773693395050692639>"},
                {EmoteType.PineappleLove, "<:PineappleLove:773693374922227762>"},
                {EmoteType.PineappleCrying, "<:PineappleCrying:773693352046362704>"}
            };

        public static string Display(this EmoteType emoteType)
        {
            Emotes.TryGetValue(emoteType, out var value);
            return value ?? emoteType.ToString();
        }
    }
}