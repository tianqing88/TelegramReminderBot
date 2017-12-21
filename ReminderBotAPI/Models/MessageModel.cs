using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReminderBotAPI.Models
{
    public class MessageModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Text { get; set; }
    }
}
