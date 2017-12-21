using ReminderBotAPI.Models;
using ReminderBotAPI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;

namespace ReminderBotAPI.Services.Implementations
{
    public class BotService: IBotService
    {
        private readonly BotConfiguration botConfiguration;

        public BotService(BotConfiguration botConfiguration)
        {
            this.botConfiguration = botConfiguration;
            Client = new TelegramBotClient(botConfiguration.BotToken); 
        }

        public TelegramBotClient Client { get; }
    }
}
