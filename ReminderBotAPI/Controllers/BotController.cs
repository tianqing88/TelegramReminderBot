using Microsoft.AspNetCore.Mvc;
using ReminderBotAPI.Models;
using ReminderBotAPI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Args;

namespace ReminderBotAPI.Controllers
{
    [Route("api/BotController")]
    public class BotController : Controller
    {
        private readonly IBotService botService;
        private static ReplyKeyboardMarkup markup = new ReplyKeyboardMarkup();
        public static string type;
        public static string description;
        public static long time;

        public BotController(IBotService botService)
        {
            this.botService = botService;
        }
        
        public void ReceiveMessage(object sender, MessageEventArgs e)
        {
            var txt = e.Message.Text;
            var cid = e.Message.Chat.Id;
            var name = e.Message.From.FirstName + " " + e.Message.From.LastName;
            var uid = e.Message.From.Id;
            
            if (txt == "/start" || txt == "Cancel")
            {
                SetUpStartKeyboard();

                var time = CheckTimeOfTheDay();

                var message = String.Format("Good {0} {1}, how can I help you today?", time, name);

                botService.Client.SendTextMessageAsync(cid, message, ParseMode.Default, false, false, 0, markup);
            }
            else if (txt == "General Reminder")
            {
                type = "General Reminder";
                markup.Keyboard = new KeyboardButton[][]
                {
                    new KeyboardButton[]
                    {
                        new KeyboardButton("Cancel")
                    }
                };

                botService.Client.SendTextMessageAsync(cid, "Please enter the description for your reminder.", ParseMode.Default, false, false, 0, markup);
            }
            else if (e.Message.Type == MessageType.TextMessage && !IsValidTime(e.Message.Text))
            {
                long hour = GetCurrentDatetime();
                description = e.Message.Text;
                time = hour;
                markup.Keyboard = new KeyboardButton[][]
                {
                    new KeyboardButton[]
                    {
                        new KeyboardButton(String.Format("{0}:00", hour)),
                        new KeyboardButton(String.Format("{0}:00", hour + 1))
                    },
                    new KeyboardButton[]
                    {
                        new KeyboardButton(String.Format("{0}:00", hour + 2)),
                        new KeyboardButton(String.Format("{0}:00", hour + 3))
                    }
                };

                botService.Client.SendTextMessageAsync(cid, "Please choose a time for your reminder.", ParseMode.Default, false, false, 0, markup);
            }

            if (IsValidTime(e.Message.Text))
            {
                ReplyKeyboardRemove remove = new ReplyKeyboardRemove()
                {
                    RemoveKeyboard = true
                };

                string message = String.Format("You are all set! \nReminding for: {0} \nTime: {1}:00", description, time);
                Console.WriteLine(message);
                botService.Client.SendTextMessageAsync(cid, message, ParseMode.Default, false, false, 0, remove);
            }
        }

        public IActionResult GetMessage(MessageModel model)
        {
            var cid = model.Id;
            var name = model.Username;
            var txt = model.Text;

            if (txt == "/start" || txt == "Cancel")
            {
                SetUpStartKeyboard();

                var time = CheckTimeOfTheDay();

                var message = String.Format("Good {0} {1}, how can I help you today?", time, name);

                return new JsonResult(message);
            }
            else if (txt == "General Reminder")
            {
                type = "General Reminder";
                markup.Keyboard = new KeyboardButton[][]
                {
                    new KeyboardButton[]
                    {
                        new KeyboardButton("Cancel")
                    }
                };

                return new JsonResult("ok");
            }

            if (IsValidTime(model.Text))
            {
                ReplyKeyboardRemove remove = new ReplyKeyboardRemove()
                {
                    RemoveKeyboard = true
                };

                string message = String.Format("You are all set! \nReminding for: {0} \nTime: {1}:00", description, time);
                Console.WriteLine(message);
                return new JsonResult("ok");
            }

            return new JsonResult("ok");
        }
        
        private static string CheckTimeOfTheDay()
        {
            if (DateTime.Now.Hour >= 0 && DateTime.Now.Hour < 12)
            {
                return "Morning";
            }
            else if (DateTime.Now.Hour >= 12 && DateTime.Now.Hour < 18)
            {
                return "Afternoon";
            }
            else
            {
                return "Evening";
            }
        }

        #region Additional Methods
        private static void SetUpStartKeyboard()
        {
            markup.ResizeKeyboard = true;
            markup.Keyboard = new KeyboardButton[][]
            {
                new KeyboardButton[]
                {
                    new KeyboardButton("General Reminder")
                }
            };
        }

        private static bool CheckTimeFormat(string time)
        {
            TimeSpan timeSpan;

            return TimeSpan.TryParse(time, out timeSpan);
        }

        private static long GetCurrentDatetime()
        {
            return DateTime.Now.Hour;
        }

        private static bool IsValidTime(string time)
        {
            Regex checktime = new Regex(@"^(?:0?[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$");
            return checktime.IsMatch(time);
        }
        #endregion
    }
}