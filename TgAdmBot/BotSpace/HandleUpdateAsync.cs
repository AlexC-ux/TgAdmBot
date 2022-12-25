﻿using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using TgAdmBot.Database;
using TgAdmBot.Logger;

namespace TgAdmBot.BotSpace
{
    internal partial class Bot
    {
        private async Task HandleUpdateAsync(ITelegramBotClient botClient, Telegram.Bot.Types.Update update, CancellationToken cancellationToken)
        {
            try
            {
                //Servise output
                new Log(Newtonsoft.Json.JsonConvert.SerializeObject(update));

                if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
                {
                    #region Подготовка к обработке сообщения
                    Telegram.Bot.Types.Message message = update.Message!;

                    Database.Chat chat = Database.Chat.GetOrCreate(message);

                    Database.User user = Database.User.GetOrCreate(chat, message.From);
                    #endregion

                    chat.MessagesCount += 1;
                    BotDatabase.db.SaveChanges();


                    if (message.Text != null && message.Type == MessageType.Text)
                    {
                        this.HandleTextMessage(message, user, chat);
                    }
                    else if (message.Voice != null && message.Type == MessageType.Voice)
                    {
                        this.HandleVoiceMessage(message, user, chat);
                    }
                    else if (message.VideoNote != null && message.Type == MessageType.VideoNote)
                    {
                        this.HandleVideoNoteMessage(message, user, chat);
                    }
                    else
                    {
                        user.UpdateStatistic(message);
                    }
                }
                else if (update.Type == UpdateType.InlineQuery)
                {
                    Telegram.Bot.Types.InlineQuery inlineQuery = update.InlineQuery!;
                    new Log(inlineQuery.Query);
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
                new Log($"UPDATE ERROR\n{e.Message}\n{e.InnerException}\n{e.Data}\n{e.Source}\n{e.StackTrace}\n{e.HelpLink}\n{e.HResult}\n{e.TargetSite}", LogType.error);
            }
            
        }

    }
}
