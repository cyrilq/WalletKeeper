using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.InputMessageContents;
using Telegram.Bot.Types.ReplyMarkups;

namespace WalletKeeperBot
{
    class Program
    {
        private static readonly TelegramBotClient Bot = new TelegramBotClient("293942981:AAECoeXWb-5WJY1Az8JPyvUoaZ-du2-tuCc");
        static void Main(string[] args)
        {
            Bot.OnMessage += BotOnMessageReceived;
            Bot.OnMessage += BotOnPhotoReceived;

            var me = Bot.GetMeAsync().Result;
            Console.Title = me.Username;

            Bot.StartReceiving();
            Console.ReadLine();
            Bot.StopReceiving();
        }

        private static async void BotOnMessageReceived(object sender, MessageEventArgs messageEventArgs)
        {
            var message = messageEventArgs.Message;

            if (message == null || message.Type != MessageType.TextMessage) return;

            if (message.Text.StartsWith("/spending"))
            {
                await Bot.SendChatActionAsync(message.Chat.Id, ChatAction.UploadDocument);

                const string file = @"../../DOCS/resumen23_Cirilo.pdf";

                var fileName = file.Split('\\').Last();

                using (var fileStream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    var fts = new FileToSend(fileName, fileStream);
                    await Bot.SendDocumentAsync(message.Chat.Id, fts, "Nice spending brah");
                }
            }
        }
        private static async void BotOnPhotoReceived(object sender, MessageEventArgs messageEventArgs)
        {
            var message = messageEventArgs.Message;

            if (message == null || message.Type != MessageType.PhotoMessage) return;

            await Bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);
            const string weVeGotYourPhoto = "He have got your photo! Congratz!";

            await Bot.SendTextMessageAsync(message.Chat.Id, weVeGotYourPhoto);
        }

    }
}