﻿using Telegram.Bot;
using TgAdmBot.Database;
using TgAdmBot.VoskRecognition;

namespace TgAdmBot.BotSpace
{

    internal partial class Bot
    {
        private async void HandleVideoNoteMessage(Telegram.Bot.Types.Message message, Database.User user, Database.Chat chat)
        {
            if (chat.VoiceMessagesDisallowed)
            {
                botClient.DeleteMessageAsync(message.Chat, message.MessageId);
            }
            else
            {
                BotDatabase.db.Add(new VoiceMessage { Chat = chat, MessageId = message.MessageId, fileId = message.VideoNote.FileId, fileUniqueId = message.VideoNote.FileUniqueId });
                BotDatabase.db.SaveChanges();
                SpeechRecognizer.AddVideoNoteMessageToQueue(new VideoNoteRecognitionObject { chat = chat, videoNoteMessage = message });
            }
            user.UpdateStatistic(message);
        }
    }
}
