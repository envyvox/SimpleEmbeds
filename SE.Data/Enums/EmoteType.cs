using System;

namespace SE.Data.Enums
{
    public enum EmoteType
    {
        Blank = 0,
        List = 1,
        DiscordLogo = 2,
        TwitterLogo = 3,
        PineappleThinking = 4,
        PineappleReading = 5,
        PineappleLove = 6,
        Mastercard = 7
    }

    public static class EmoteTypeHelper
    {
        public static string Display(this EmoteType emote) => emote switch
        {
            EmoteType.Blank => "<:Blank:773616910867628032>",
            EmoteType.List => "<:List:773616884086996993>",
            EmoteType.DiscordLogo => "<:DiscordLogo:773630216282046464>",
            EmoteType.TwitterLogo => "<:TwitterLogo:773630216433172500>",
            EmoteType.PineappleThinking => "<:PineappleThinking:773692108933824543>",
            EmoteType.PineappleReading => "<:PineappleReading:773693395050692639>",
            EmoteType.PineappleLove => "<:PineappleLove:773693374922227762>",
            EmoteType.Mastercard => "<:Mastercard:773706166881878016>",
            _ => throw new ArgumentOutOfRangeException(nameof(emote), emote, null)
        };
    }
}
