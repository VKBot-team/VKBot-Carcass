using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using VKBot.VKApi;

namespace VKBot
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.Map(Settings.HandlerPath, CallbackHandler);

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello, VK Bot!");
            });
        }

        private void CallbackHandler(IApplicationBuilder app)
        {
            app.Run(async context => 
            {
                string data;
                using (var reader = new StreamReader(context.Request.Body))
                {
                    data = await reader.ReadToEndAsync();
                }

                await HandleNewMessage(context, data);
            });
        }

        private async Task HandleNewMessage(HttpContext context, string data)
        {
            var response = "ok";
            var jsonData = JsonConvert.DeserializeObject<Message>(data);
            if (jsonData.Type == "confirmation")
                response = Settings.ConfiramtionKey;

            await context.Response.WriteAsync(response);

            if (jsonData.Type != "message_new") return;

            var msg = "Теперь я присылаю новое сообщение! ВАААУ, но ты все рано дурак, раз до сих пор мне пишешь :)";
            await VkApi.ExecMethod(Method.MessageSend, new Dictionary<string, string>
            {
                {"user_id", jsonData.Object.UserId.ToString()},
                {"message", msg}
            });
        }
    }
}