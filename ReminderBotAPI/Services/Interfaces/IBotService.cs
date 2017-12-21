using ReminderBotAPI.Services.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;

namespace ReminderBotAPI.Services.Interfaces
{
    public interface IBotService
    {
        TelegramBotClient Client { get; }
    }
}
