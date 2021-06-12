using System;

namespace SE.Data.Enums
{
    public enum ReplyType : byte
    {
        AboutDesc,
        AboutContactsFieldName,
        AboutContactsFieldDesc,
        AboutDonateFieldName,
        AboutDonateFieldDesc,
        MessageEmbedNull,
        MessageWithIdNull,
        AboutInviteFieldName,
        InviteFieldDesc,
        WhereToFindMessageIdFieldDesc,
        WhereToFindMessageIdFieldName,
        HowToInviteBotFieldName,
        WhereGetJsonCodeFieldName,
        WhereGetJsonCodeFieldDesc
    }

    public static class ReplyTypeHelper
    {
        private static string Localize(this ReplyType replyType) => replyType switch
        {
            ReplyType.AboutDesc =>
                "{0} is a simple bot to help you send and edit embed messages using the [eb.nadeko.bot](https://eb.nadeko.bot/) website.\n*All rights to [eb.nadeko.bot](https://eb.nadeko.bot/) belong to the developers of nadeko.*",

            ReplyType.AboutContactsFieldName =>
                "Contacts",

            ReplyType.AboutContactsFieldDesc =>
                "{0} Discord: <@550493599629049858>\n{1} Twitter: [@evkkxo](https://twitter.com/evkkxo)",

            ReplyType.AboutDonateFieldName =>
                "Support",

            ReplyType.AboutDonateFieldDesc =>
                "You can support this small but useful bot by sending any amount you like. I will be grateful to you for every penny {0}\n{1} `5375 4141 0460 6651` EUGENE GARBUZOV",

            ReplyType.MessageEmbedNull =>
                "The message does not contain embed.",

            ReplyType.MessageWithIdNull =>
                "No messages found with this ID.",

            ReplyType.AboutInviteFieldName =>
                "Invite a bot to your server",

            ReplyType.InviteFieldDesc =>
                "{0} Click [here to open the invite link](https://discord.com/oauth2/authorize?client_id=773594420066254899&scope=bot&permissions=519168).",

            ReplyType.WhereToFindMessageIdFieldDesc =>
                "{0} Read [«Where can I find my User/Server/Message ID?»](https://support.discord.com/hc/en-us/articles/206346498-Where-can-I-find-my-User-Server-Message-ID-).",

            ReplyType.WhereToFindMessageIdFieldName =>
                "Where can I find my message ID {0}",

            ReplyType.HowToInviteBotFieldName =>
                "How to invite a bot to your server {0}",

            ReplyType.WhereGetJsonCodeFieldName =>
                "Where can I get the required \"json code\" {0}",

            ReplyType.WhereGetJsonCodeFieldDesc =>
                "{0} Open [eb.nadeko.bot](https://eb.nadeko.bot/) website.\nFill in the fields you need in left half of the screen.\nCopy the code on the right side of the screen and paste it into your command.",

            _ => throw new ArgumentOutOfRangeException(nameof(replyType), replyType, null)
        };

        public static string Parse(this ReplyType replyType) => replyType.Localize();

        public static string Parse(this ReplyType replyType, params object[] replacements)
        {
            try
            {
                return string.Format(Parse(replyType), replacements);
            }
            catch (FormatException)
            {
                return "`An error occurred while displaying the response. Please show this to kkxo#0001`";
            }
        }
    }
}
