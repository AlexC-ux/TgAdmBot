﻿using Telegram.Bot;
using TgAdmBot.Database;

namespace TgAdmBot.BotSpace
{
    internal partial class Bot
    {

        private async Task HandleTextMessage(Telegram.Bot.Types.Message message, Database.User user, Database.Chat chat)
        {
            Database.User? replUser = chat.Users.SingleOrDefault(u => u.TelegramUserId == message.ReplyToMessage?.From?.Id);
            if (replUser == null && message.ReplyToMessage != null)
            {
                replUser = Database.User.GetOrCreate(chat, message.ReplyToMessage.From);
            }





            if (message.Text.Contains("@all"))
            {
                botClient.SendTextMessageAsync(message.Chat, chat.GetAllMentions(), Telegram.Bot.Types.Enums.ParseMode.Markdown);
            }


            switch (message.Text.ToLower().Replace($"@{botClient.GetMeAsync().Result.Username!}", "").Split()[0])
            {
                case "/stt":
                    if (replUser != null)
                    {
                        VoiceMessage? voiseMessage = BotDatabase.db.VoiceMessages.SingleOrDefault(vm => vm.Chat.ChatId == chat.ChatId && vm.MessageId == message.ReplyToMessage.MessageId);
                        if (voiseMessage == null)
                        {
                            botClient.SendTextMessageAsync(chatId: message.Chat, BotPhrases.OldMessagePh[0]);
                        }
                        else
                        {
                            botClient.SendTextMessageAsync(chatId: message.Chat, voiseMessage.recognizedText, replyToMessageId: message.ReplyToMessage.MessageId);
                        }
                    }
                    else
                    {
                        botClient.SendTextMessageAsync(message.Chat, BotPhrases.ReplyToMesage[0]);
                    }
                    break;
                case "/help":
                    botClient.SendTextMessageAsync(message.Chat, BotPhrases.HelpMessage[0]);
                    break;
                case "1":
                    if (user.LastMessage == "/help")
                    {
                        botClient.SendTextMessageAsync(message.Chat, BotPhrases.HelpMessage[1]);
                    }
                    break;
                case "2":
                    if (user.LastMessage == "/help")
                    {
                        botClient.SendTextMessageAsync(message.Chat, BotPhrases.HelpMessage[2]);
                    }
                    break;
                case "3":
                    if (user.LastMessage == "/help")
                    {
                        botClient.SendTextMessageAsync(message.Chat, BotPhrases.HelpMessage[3]);
                    }
                    break;
                case "/chatstat":

                    BotDatabase.db.SaveChanges();
                    botClient.SendTextMessageAsync(message.Chat, chat.GetInfo());
                    break;
                case "/setdefaultadmins":
                    if (user.UserRights == UserRights.creator)
                    {
                        BotDatabase.db.SaveChanges();
                        botClient.SendTextMessageAsync(message.Chat, chat.SetDefaultAdmins());
                        break;
                    }
                    break;
                case "/rules":

                    BotDatabase.db.SaveChanges();
                    botClient.SendTextMessageAsync(message.Chat, $"Правила чата:\n{chat.Rules}");
                    break;
                case "/setrules":

                    BotDatabase.db.SaveChanges();
                    if (user.UserRights < UserRights.helper)
                    {
                        botClient.SendTextMessageAsync(message.Chat, BotPhrases.NextChatRules[0]);
                        break;
                    }
                    break;
                case "/admin":
                    if (user.UserRights < UserRights.administrator)
                    {

                        if (replUser != null)
                        {
                            BotDatabase.db.SaveChanges();
                            if (user.UserRights < replUser.UserRights)
                            {
                                replUser.UserRights = UserRights.administrator;
                                BotDatabase.db.SaveChanges();
                                botClient.SendTextMessageAsync(message.Chat, BotPhrases.setRights[0]+"администратор");
                                break;
                            }
                            else
                            {
                                botClient.SendTextMessageAsync(message.Chat, BotPhrases.NotEnoughtRights[0]);
                                break;
                            }
                        }
                        else
                        {
                            botClient.SendTextMessageAsync(message.Chat, BotPhrases.ReplyToMesage[0]);
                            break;
                        }
                    }
                    break;
                case "/moder":
                    if (user.UserRights < UserRights.moderator)
                    {

                        if (replUser != null)
                        {
                            if (user.UserRights < replUser.UserRights)
                            {
                                replUser.UserRights = UserRights.moderator;
                                BotDatabase.db.SaveChanges();
                                botClient.SendTextMessageAsync(message.Chat, BotPhrases.setRights[0] +"модератор");
                                break;
                            }
                            else
                            {
                                botClient.SendTextMessageAsync(message.Chat, BotPhrases.NotEnoughtRights[0]);
                                break;
                            }
                        }

                        else
                        {
                            botClient.SendTextMessageAsync(message.Chat, BotPhrases.ReplyToMesage[0]);
                            break;
                        }
                    }
                    break;
                case "/helper":
                    if (user.UserRights < UserRights.helper)
                    {
                        if (replUser != null)
                        {
                            if (user.UserRights < replUser.UserRights)
                            {
                                replUser.UserRights = UserRights.helper;
                                BotDatabase.db.SaveChanges();
                                botClient.SendTextMessageAsync(message.Chat, BotPhrases.setRights[0]+"помощник");
                                break;
                            }
                            else
                            {
                                botClient.SendTextMessageAsync(message.Chat, BotPhrases.NotEnoughtRights[0]);
                                break;
                            }
                        }

                        else
                        {
                            botClient.SendTextMessageAsync(message.Chat, BotPhrases.ReplyToMesage[0]);
                            break;
                        }
                    }
                    break;
                case "/normal":
                    if (user.UserRights < UserRights.normal)
                    {
                        if (replUser != null)
                        {
                            if (user.UserRights < replUser.UserRights)
                            {
                                replUser.UserRights = UserRights.normal;
                                BotDatabase.db.SaveChanges();
                                botClient.SendTextMessageAsync(message.Chat, BotPhrases.setRights[0] + "нормал");
                                break;
                            }
                            else
                            {
                                botClient.SendTextMessageAsync(message.Chat, BotPhrases.NotEnoughtRights[0]);
                                break;
                            }
                        }

                        else
                        {
                            botClient.SendTextMessageAsync(message.Chat, BotPhrases.ReplyToMesage[0]);
                            break;
                        }
                    }
                    break;
                case "/mute":
                    if (user.UserRights < UserRights.helper)
                    {
                        if (replUser != null)
                        {
                            if (user.UserRights < replUser.UserRights)
                            {
                                if (replUser.IsMuted)
                                {
                                    replUser.IsMuted = false;
                                    BotDatabase.db.SaveChanges();
                                    replUser.Unmute();
                                    botClient.SendTextMessageAsync(message.Chat, $"Хорошо, по просьбе [{user.Nickname}](tg://user?id={user.TelegramUserId}) я разрешаю [{replUser.Nickname}](tg://user?id={replUser.TelegramUserId}) говорить", Telegram.Bot.Types.Enums.ParseMode.Markdown);
                                    break;
                                }
                                else
                                {
                                    if (message.Text.Split().Length == 2)
                                    {
                                        try
                                        {
                                            int time = Convert.ToInt32(message.Text.Split()[1]);
                                            if (time > 0 && time < 99999)
                                            {
                                                replUser.IsMuted = true;
                                                BotDatabase.db.SaveChanges();
                                                replUser.Mute(time);
                                                botClient.SendTextMessageAsync(message.Chat, $"Ммм,  [{user.Nickname}](tg://user?id={user.TelegramUserId}), какой ты строгий администратор... Но хорошо, ладно, только ради тебя, запрещаю [{replUser.Nickname}](tg://user?id={replUser.TelegramUserId}) говорить", Telegram.Bot.Types.Enums.ParseMode.Markdown);
                                                break;
                                            }
                                            else
                                            {
                                                botClient.SendTextMessageAsync(message.Chat, "Эй! Это же почти вечность! Давай выберем другое время, допустим от 0 до 99999", Telegram.Bot.Types.Enums.ParseMode.Markdown);
                                                break;
                                            }
                                        }
                                        catch
                                        {
                                            botClient.SendTextMessageAsync(message.Chat,"Так, окей, а на сколько минут запретить то?" , Telegram.Bot.Types.Enums.ParseMode.Markdown);
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        botClient.SendTextMessageAsync(message.Chat, "Так, окей, а на сколько минут запретить то?", Telegram.Bot.Types.Enums.ParseMode.Markdown);
                                        break;
                                    }
                                }
                            }

                        }
                        else
                        {
                            botClient.SendTextMessageAsync(message.Chat, BotPhrases.ReplyToMesage[0]);
                            break;
                        }
                    }
                    else
                    {
                        botClient.SendTextMessageAsync(message.Chat, BotPhrases.NotEnoughtRights[0], Telegram.Bot.Types.Enums.ParseMode.Markdown);
                        break;
                    }
                    break;
                case "/stat":
                    if (user.UserRights < UserRights.helper)
                    {
                        if (replUser != null)
                        {
                            if (user.UserRights < replUser.UserRights)
                            {
                                botClient.SendTextMessageAsync(message.Chat, replUser.GetInfo(), Telegram.Bot.Types.Enums.ParseMode.Markdown);
                                break;
                            }
                            else
                            {
                                botClient.SendTextMessageAsync(message.Chat, BotPhrases.NotEnoughtRights[0]);
                                break;
                            }

                        }
                        else
                        {
                            botClient.SendTextMessageAsync(message.Chat, user.GetInfo(), Telegram.Bot.Types.Enums.ParseMode.Markdown);
                            break;
                        }
                    }
                    break;
                case "/setwarninglimitaction":
                    botClient.SendTextMessageAsync(message.Chat, chat.SetWarningLimitAction(message.From.Id, message.Text));
                    break;
                case "/muted":
                    if (user.UserRights < UserRights.helper)
                    {
                        botClient.SendTextMessageAsync(message.Chat, chat.GetMutedUsers(), Telegram.Bot.Types.Enums.ParseMode.Markdown);
                        break;
                    }
                    break;
                case "/ban":
                    if (replUser != null)
                    {
                        if (user.UserRights < UserRights.moderator && user.UserRights < replUser.UserRights)
                        {
                            replUser.Ban();
                            botClient.SendTextMessageAsync(message.Chat, $"Ну ты, [{user.Nickname}](tg://user?id={user.TelegramUserId}), прям сама строгость! Прощай [{replUser.Nickname}](tg://user?id={replUser.TelegramUserId}) " +
                                $"если передумаешь то напиши /unban в ответ на любое его сообщение, а затем пригласи его в чат " +
                                $"или вручную удалить его из черного списка чата, а затем пригласи ", Telegram.Bot.Types.Enums.ParseMode.Markdown);
                            break;
                        }
                        else
                        {
                            botClient.SendTextMessageAsync(message.Chat, BotPhrases.NotEnoughtRights[0],
                            Telegram.Bot.Types.Enums.ParseMode.Markdown);
                            break;
                        }
                    }
                    else
                    {
                        botClient.SendTextMessageAsync(message.Chat, BotPhrases.ReplyToMesage[0],
                            Telegram.Bot.Types.Enums.ParseMode.Markdown);
                        break;
                    }
                case "/unban":
                    if (replUser != null)
                    {
                        if (user.UserRights < UserRights.moderator && user.UserRights < replUser.UserRights)
                        {

                            replUser.UnBan();
                            botClient.SendTextMessageAsync(message.Chat, $"[{user.Nickname}](tg://user?id={user.TelegramUserId}), ты милашка! Прощаем " +
                                $"[{replUser.Nickname}](tg://user?id={replUser.TelegramUserId})! Теперь он снова может быть приглашен в чат",
                                Telegram.Bot.Types.Enums.ParseMode.Markdown);
                            break;
                        }
                        else
                        {
                            botClient.SendTextMessageAsync(message.Chat, BotPhrases.NotEnoughtRights[0],
                                Telegram.Bot.Types.Enums.ParseMode.Markdown);
                            break;
                        }
                    }
                    else
                    {
                        botClient.SendTextMessageAsync(message.Chat, BotPhrases.ReplyToMesage[0],
                            Telegram.Bot.Types.Enums.ParseMode.Markdown);
                        break;
                    }
                case "/warn":
                    if (replUser != null)
                    {
                        if (user.UserRights < UserRights.helper && user.UserRights < replUser.UserRights)
                        {
                            //TODO переделать. Будет отображаться неверное число варнов
                            if (replUser.WarnsCount != chat.WarnsLimit - 1)
                            {
                                replUser.Warn();
                                if (message.Text.Length > 6)
                                {
                                    botClient.SendTextMessageAsync(message.Chat, $"[{user.Nickname}](tg://user?id={user.TelegramUserId}) предупреждает тебя, [{user.Nickname}](tg://user?id={user.TelegramUserId})\nза {message.Text.Substring(5)}\nПредупреждений до наказания {chat.WarnsLimit - replUser.WarnsCount}", Telegram.Bot.Types.Enums.ParseMode.Markdown);
                                    break;
                                }
                                else
                                {
                                    botClient.SendTextMessageAsync(message.Chat, $"[{user.Nickname}](tg://user?id={user.TelegramUserId}) предупреждает тебя, [{replUser.Nickname}](tg://user?id={replUser.TelegramUserId})\nПредупреждений до наказания {chat.WarnsLimit - replUser.WarnsCount}", Telegram.Bot.Types.Enums.ParseMode.Markdown);
                                    break;
                                }
                            }
                            else
                            {
                                botClient.SendTextMessageAsync(message.Chat, replUser.WarningLimitAction());
                            }

                        }
                        else
                        {
                            botClient.SendTextMessageAsync(message.Chat, BotPhrases.NotEnoughtRights[0], Telegram.Bot.Types.Enums.ParseMode.Markdown);
                            break;
                        }
                    }
                    else
                    {
                        botClient.SendTextMessageAsync(message.Chat, BotPhrases.ReplyToMesage[0], Telegram.Bot.Types.Enums.ParseMode.Markdown);
                        break;
                    }
                    break;
                case "/unwarn":
                    if (replUser != null)
                    {
                        if (user.UserRights < UserRights.helper && user.UserRights < replUser.UserRights)
                        {
                            replUser.WarnsCount = 0;
                            BotDatabase.db.SaveChanges();
                            botClient.SendTextMessageAsync(message.Chat, $"[{user.Nickname}](tg://user?id={user.TelegramUserId}) прощает " +
                                $"[{replUser.Nickname}](tg://user?id={replUser.TelegramUserId})",
                                Telegram.Bot.Types.Enums.ParseMode.Markdown);
                            break;

                        }
                        else
                        {
                            botClient.SendTextMessageAsync(message.Chat, BotPhrases.NotEnoughtRights[0],
                                Telegram.Bot.Types.Enums.ParseMode.Markdown);
                            break;
                        }
                    }
                    else
                    {
                        botClient.SendTextMessageAsync(message.Chat, BotPhrases.ReplyToMesage[0],
                            Telegram.Bot.Types.Enums.ParseMode.Markdown);
                        break;
                    }
                case "/warns":
                    if (user.UserRights < UserRights.helper)
                    {
                        botClient.SendTextMessageAsync(message.Chat, chat.GetWarnedUsers(),
                            Telegram.Bot.Types.Enums.ParseMode.Markdown);
                        break;
                    }
                    break;
                case "/voicemessage":
                    if (user.UserRights < UserRights.moderator)
                    {
                        if (!chat.VoiceMessagesDisallowed)
                        {
                            chat.VoiceMessagesDisallowed = true;
                            botClient.SendTextMessageAsync(message.Chat, "Ура! теперь без гс!");
                        }
                        else
                        {
                            chat.VoiceMessagesDisallowed = false;
                            botClient.SendTextMessageAsync(message.Chat, "Ладно уж, кидайте свои кружочки... Даже гс можно(");
                        }
                        break;
                    }
                    else
                    {
                        botClient.SendTextMessageAsync(message.Chat, BotPhrases.NotEnoughtRights[0]);
                        break;
                    }
                case "/nick":
                    if (message.Text.Length > 8)
                    {
                        if (message.Text.Length < 30)
                        {
                            try
                            {
                                user.Nickname = message.Text.Substring(6);
                                BotDatabase.db.SaveChanges();
                                botClient.SendTextMessageAsync(message.Chat, $"Окей, теперь ты [{user.Nickname}](tg://user?id={user.TelegramUserId})", Telegram.Bot.Types.Enums.ParseMode.Markdown);
                            }
                            catch
                            {
                                botClient.SendTextMessageAsync(message.Chat, $"Кажется какие-то символы были недопустимы для ника", Telegram.Bot.Types.Enums.ParseMode.Markdown);
                            }
                        }
                        else
                        {
                            botClient.SendTextMessageAsync(message.Chat, $"Разве это ник? Белиберда какая-то! Слишком длинно, я не запомню!", Telegram.Bot.Types.Enums.ParseMode.Markdown);
                        }
                    }
                    else
                    {
                        botClient.SendTextMessageAsync(message.Chat, $"Коротко не всегда хорошо, давай что-нибудь другое", Telegram.Bot.Types.Enums.ParseMode.Markdown);
                    }
                    break;
                case "/nicks":
                    botClient.SendTextMessageAsync(message.Chat, user.Chat.GetChatNicknames(), Telegram.Bot.Types.Enums.ParseMode.Markdown);
                    break;
                case "/rnd":
                    if (message.Text.Length > 6)
                    {
                        botClient.SendTextMessageAsync(message.Chat, BotGames.GetRandomNumber(message.Text), Telegram.Bot.Types.Enums.ParseMode.Markdown);
                    }
                    break;
                case "/chs":
                    if (message.Text.Length > 11)
                    {
                        user.LastMessage = message.Text;
                        BotDatabase.db.SaveChanges();
                        botClient.SendTextMessageAsync(message.Chat, BotGames.Chose(message.Text), Telegram.Bot.Types.Enums.ParseMode.Markdown);
                    }
                    else
                    {
                        botClient.SendTextMessageAsync(message.Chat, BotPhrases.ShortMessage[0], Telegram.Bot.Types.Enums.ParseMode.Markdown);
                    }
                    break;
                case "/me":
                    if (message.Text.Length > 4)
                    {
                        if (message.ReplyToMessage != null)
                        {
                            string mestext = message.Text.Substring(4);
                            botClient.SendTextMessageAsync(message.Chat, $"[{user.Nickname}](tg://user?id={user.TelegramUserId}) " + mestext + $" [{replUser.Nickname}](tg://user?id={replUser.TelegramUserId})", Telegram.Bot.Types.Enums.ParseMode.Markdown);
                            botClient.DeleteMessageAsync(message.Chat.Id, message.MessageId);

                        }
                        else
                        {
                            string mestext = message.Text.Substring(4);
                            botClient.SendTextMessageAsync(message.Chat, $"[{user.Nickname}](tg://user?id={user.TelegramUserId}) " + mestext, Telegram.Bot.Types.Enums.ParseMode.Markdown);
                            botClient.DeleteMessageAsync(message.Chat.Id, message.MessageId);
                        }
                    }
                    break;
                case "/wh":
                    if (message.Text.Length > 4)
                    {
                        botClient.SendTextMessageAsync(message.Chat, BotGames.Who(message.Text, user.Chat), Telegram.Bot.Types.Enums.ParseMode.Markdown);
                    }
                    else
                    {
                        botClient.SendTextMessageAsync(message.Chat, BotPhrases.ShortMessage[0], Telegram.Bot.Types.Enums.ParseMode.Markdown);
                    }
                    break;
                case "/prob":
                    if (message.Text.Length > 5)
                    {
                        botClient.SendTextMessageAsync(message.Chat, BotGames.Probability(message.Text), Telegram.Bot.Types.Enums.ParseMode.Markdown);
                    }
                    else
                    {
                        botClient.SendTextMessageAsync(message.Chat, BotPhrases.ShortMessage[0], Telegram.Bot.Types.Enums.ParseMode.Markdown);
                    }
                    break;
                default:
                    switch (user.LastMessage)
                    {
                        case "/setrules":
                            if (user.UserRights < UserRights.moderator)
                            {
                                if (message.Text.Length > 15)
                                {
                                    if (message.Text.Length < 10000)
                                    {

                                        string rules = message.Text.Substring(9);
                                        chat.Rules = rules;
                                        botClient.SendTextMessageAsync(message.Chat, $"Теперь у нас есть законы", Telegram.Bot.Types.Enums.ParseMode.Markdown);
                                    }
                                    else
                                    {
                                        botClient.SendTextMessageAsync(message.Chat, "Как-то слишком длинно, я не запомню...", Telegram.Bot.Types.Enums.ParseMode.Markdown);
                                    }
                                }
                                else
                                {
                                    botClient.SendTextMessageAsync(message.Chat, "Разве же это правила? Так пару символов просто, давай что-нибудь подробнее!", Telegram.Bot.Types.Enums.ParseMode.Markdown);
                                }
                            }
                            else
                            {
                                botClient.SendTextMessageAsync(message.Chat, BotPhrases.NotEnoughtRights[0], Telegram.Bot.Types.Enums.ParseMode.Markdown);
                            }
                            break;
                        default:
                            break;
                    }
                    break;

            }

            user.UpdateStatistic(message);
        }
    }
}
