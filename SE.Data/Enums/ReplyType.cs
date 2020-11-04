using System;

namespace SE.Data.Enums
{
    public enum ReplyType
    {
        SomethingGoneWrong = 0,
        CommandErrorUnknownCommand = 1,
        CommandErrorParseFailed = 2,
        CommandErrorBadArgCount = 3,
        CommandErrorObjectNotFound = 4,
        CommandErrorMultipleMatches = 5,
        HelpHowToSendMessageWithoutEmbedFieldName,
        HelpHowToSendMessageWithoutEmbedFieldDesc,
        HelpHowToSendMessageWithEmbedFieldName,
        HelpHowToSendMessageWithEmbedFieldDesc,
        HelpHowToModifyMessageWithoutEmbedFieldName,
        HelpHowToModifyMessageWithoutEmbedFieldDesc,
        HelpHowToModifyMessageWithEmbedFieldName,
        HelpHowToModifyMessageWithEmbedFieldDesc,
        HelpWhereCanIFindMyMessageIdFieldName,
        HelpWhereCanIFindMyMessageIdFieldDesc,
        HelpHowToInviteToMyServerFieldName,
        HelpHowToInviteToMyServerFieldDesc,
        HelpWhatPermissionsNeedFieldName,
        HelpWhatPermissionsNeedFieldDesc,
        AboutDesc,
        AboutContactsFieldName,
        AboutContactsFieldDesc
    }

    public static class ReplyTypeHelper
    {
        private static string Localize(this ReplyType replyType)
        {
            return replyType switch
            {
                ReplyType.SomethingGoneWrong => "Oops, it seems something went wrong...",
                ReplyType.CommandErrorUnknownCommand => "I'm not sure what you want to do, it might be worth reading the `-help` for the available commands?",
                ReplyType.CommandErrorParseFailed => "After tricky mathematical calculations, I came up with an error. Which one? I don't know, just an error. I think it was your fault!",
                ReplyType.CommandErrorBadArgCount => "You seem to have incorrectly specified the arguments after the command, maybe you should read the `-help` for the available commands and how to use them correctly?",
                ReplyType.CommandErrorObjectNotFound => "After complex mathematical calculations, I came to the conclusion that such an object does not exist. What object? I don’t know, you were looking for it ...",
                ReplyType.CommandErrorMultipleMatches => "Uh, it seems there are several options, I'm not going to decide for you!",
                ReplyType.HelpHowToSendMessageWithoutEmbedFieldName => "{0} How to send message without embed?",
                ReplyType.HelpHowToSendMessageWithoutEmbedFieldDesc => "Just type `-send YOUR_MESSAGE` in the channel you want to send.",
                ReplyType.HelpHowToSendMessageWithEmbedFieldName => "{0} How to send message with embed?",
                ReplyType.HelpHowToSendMessageWithEmbedFieldDesc => "Open [eb.nadeko.bot](https://eb.nadeko.bot/) and fill in the fields you need in the left half of the screen.\nCopy the code from the right side of the screen and paste it after the command `-send YOUR_CODE`.",
                ReplyType.HelpHowToModifyMessageWithoutEmbedFieldName => "{0} How to modify message without embed?",
                ReplyType.HelpHowToModifyMessageWithoutEmbedFieldDesc => "Just type `-modify MESSAGE_ID NEW_MESSAGE` in the channel where you want to modify message.",
                ReplyType.HelpHowToModifyMessageWithEmbedFieldName => "{0} How to modify message with embed?",
                ReplyType.HelpHowToModifyMessageWithEmbedFieldDesc => "Copy a new code from [eb.nadeko.bot](https://eb.nadeko.bot/).\nPaste it in command `-modify MESSAGE_ID NEW_CODE`.",
                ReplyType.HelpWhereCanIFindMyMessageIdFieldName => "{0} Where can i find my message ID?",
                ReplyType.HelpWhereCanIFindMyMessageIdFieldDesc => "Read [«Where can I find my User/Server/Message ID?»](https://support.discord.com/hc/en-us/articles/206346498-Where-can-I-find-my-User-Server-Message-ID-).",
                ReplyType.HelpHowToInviteToMyServerFieldName => "{0} How to invite a bot to my server?",
                ReplyType.HelpHowToInviteToMyServerFieldDesc => "Click [here to open the invite link.](https://discord.com/oauth2/authorize?client_id=773594420066254899&scope=bot&permissions=519168)",
                ReplyType.HelpWhatPermissionsNeedFieldName => "{0} What permissions does a user need to use commands?",
                ReplyType.HelpWhatPermissionsNeedFieldDesc => "Commands `-send` and `-modify` require «**Manage Messages**» permission.",
                ReplyType.AboutDesc => "SimpleEmbeds is a simple bot to help you send and modify embed messages using the [eb.nadeko.bot](https://eb.nadeko.bot/) website.\n*All rights to [eb.nadeko.bot](https://eb.nadeko.bot/) belong to the developers of nadeko.*",
                ReplyType.AboutContactsFieldName => "Contacts",
                ReplyType.AboutContactsFieldDesc => "{0} Discord: kkxo#1722\n{1} Twitter: [@re_kkxo](https://twitter.com/re_kkxo)",
                _ => throw new ArgumentOutOfRangeException(nameof(replyType), replyType, null)
            };
        }
        
        public static string Parse(this ReplyType replyType)
        {
            return replyType.Localize();
        }

        public static string Parse(this ReplyType replyType, params object[] replacements)
        {
            try
            {
                return string.Format(Parse(replyType), replacements);
            }
            catch (FormatException)
            {
                return "`An error occurred while displaying the response. Please show this to kkxo#1722`";
            }
        }
    }
}