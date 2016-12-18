﻿using System;
using System.IO;
using System.Linq;
using DataBase;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using GoogleCloudSamples;
using System.Drawing;
using System.Net;
using System.Collections.Generic;
using DataBaseCon = DataBase.Program;

namespace WalletKeeperBot
{
    class Program
    {
        public static string ParseString(string str)
		{
			var splittedString = str.ToLower().Split(new char[] { '\n' });
			var targetWords = new List<string>()
			{
				"итог", "итого", "итог:", "итого:"
			};
			if (!splittedString.Intersect(targetWords).Any())
				return "Cannot get the price from the photo.\nTry again please.";
			else 
			{
				//int indexOfSum = 0;
				float Sum = 0;
				int indexOfResult = 0;//Array.IndexOf(splittedString, targetWords);
				for (int i = 0; i < targetWords.Count; i++)
				{
					indexOfResult = Array.IndexOf(splittedString, targetWords[i]);
					if (indexOfResult > -1) break;
				}
                {
                    //double k;
                    try
                    {
                        Console.WriteLine(Convert.ToDouble(splittedString[indexOfResult - 1].Substring(0, splittedString[indexOfResult - 1].Length - 3)));
                    }
                    catch { }
                }
                if (float.TryParse(splittedString[indexOfResult - 1].Substring(0, splittedString[indexOfResult - 1].Length - 3), out Sum) ||
					float.TryParse(splittedString[indexOfResult + 1].Substring(0, splittedString[indexOfResult + 1].Length - 3), out Sum))
				{
					return $"Sum is {Sum} rubles.";
				}
				else return "Sum is undefined";
			}
        }
        
        private static readonly TelegramBotClient Bot = new TelegramBotClient(WalletKeeper.Config.API_KEY);

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

            else if (message.Text.StartsWith("/spending"))
            {
               
                    //TODO: add text message about user spendings

                    DataBaseCon.SelectRows((int)message.Chat.Id);
                await Bot.SendTextMessageAsync(message.Chat.Id, DataBaseCon.SelectRows((int)message.Chat.Id));

            }
            else if (message.Text.StartsWith("/start"))
            {
                await Bot.SendTextMessageAsync(message.Chat.Id, $"Hello, {message.Chat.FirstName}" + WalletKeeper.Constants.START_MESSAGE);
            }
            else if (message.Text.StartsWith("/help"))
            {
                await Bot.SendTextMessageAsync(message.Chat.Id, WalletKeeper.Constants.HELP_MESSAGE);
            }

        }
        private static async void BotOnPhotoReceived(object sender, MessageEventArgs messageEventArgs)
        {
            var message = messageEventArgs.Message;

            if (message == null || message.Type != MessageType.PhotoMessage) return;

            var fileId = message.Photo[message.Photo.Length - 1].FileId;
            
            var file = await Bot.GetFileAsync(fileId);

            var stream = file.FileStream;

            using (Stream output = new FileStream($"../../Photo/img{message.Chat.Id}{fileId}.jpg", FileMode.Append))
            {
                byte[] buffer = new byte[32 * 1024];
                int read;

                while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    output.Write(buffer, 0, read);
                }
            }

            await Bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);

            await Bot.SendTextMessageAsync(message.Chat.Id, WalletKeeper.Constants.WE_HAVE_GOT_YOUR_MESSAGE);

            string imagePath = $"../../Photo/img{message.Chat.Id}{fileId}.jpg";
            TextDetection newTD = new TextDetection();
            string text = newTD.photo2string(imagePath);

            string result = ParseString(text);

            double result1 = Double.Parse(result);

            await Bot.SendTextMessageAsync(message.Chat.Id, result);

            Console.WriteLine(text);
            DataBaseCon pr = new DataBaseCon();
            DataBaseCon.InsertUser((int)message.Chat.Id, message.Chat.FirstName);
            //DataBaseCon.InsertAmount((int)message.Chat.Id, result1);
            //.SelectRows((int)message.Chat.Id);
            //DataBaseCon.DeleteRows((int)message.Chat.Id);
            
            
           }
        

    }
}
