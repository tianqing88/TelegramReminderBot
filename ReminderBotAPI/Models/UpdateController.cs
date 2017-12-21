using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReminderBotAPI.Services.Interfaces;
using Telegram.Bot.Types;

namespace ReminderBotAPI.Models
{
    [Produces("application/json")]
    [Route("api/Update")]
    public class UpdateController : Controller
    {
        readonly IUpdateService _updateService;
        readonly BotConfiguration _config;

        public UpdateController(IUpdateService updateService, BotConfiguration config)
        {
            _updateService = updateService;
            _config = config;
        }

        // POST api/update
        [HttpPost]
        public void Post([FromBody]Update update)
        {
            _updateService.Echo(update);
        }
    }
}