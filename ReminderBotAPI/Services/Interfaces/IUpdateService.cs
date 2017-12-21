using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace ReminderBotAPI.Services.Interfaces
{
    public interface IUpdateService
    {
        void Echo(Update update);
    }
}
