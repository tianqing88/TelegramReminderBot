using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ReminderBotAPI.Models;
using Telegram.Bot;

namespace ReminderBotAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            var accessToken = Configuration["Settings:accessToken"];
            TelegramClient telegramClient = new TelegramClient(accessToken);

            // Set up webhook
            string webhookUrl = Configuration["Settings:webhookUrl"];
            int maxConnections = int.Parse(Configuration["Settings:maxConnections"]);
            UpdateType[] allowedUpdates = { UpdateType.MessageUpdate };

            telegramClient.SetWebhook(webhookUrl, maxConnections, allowedUpdates);

            services.AddScoped<ITelegramClient>(client => telegramClient);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
