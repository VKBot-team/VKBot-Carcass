using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using API;
using Newtonsoft.Json;

namespace VKBot
{
    public class Startup
    {
        private static readonly IBotAPI Api = new BotAPI();

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

                var jsonData = JsonConvert.DeserializeObject<Message>(data);
                await context.Response.WriteAsync(GetResponse(jsonData));

                HandleNewMessage(jsonData);
            });
        }

        private string GetResponse(Message jsonData)
        {
            return jsonData.Type == "confirmation" ? Settings.ConfiramtionKey : "ok";
        }

        private async void HandleNewMessage(Message jsonData)
        {
            if (jsonData.Type != "message_new") return;
            try
            {
                await Api.ExecuteCommand(new[] {"ok", jsonData.Object.UserId.ToString()});
                await Api.ExecuteCommand(GetArgs(jsonData).Append(jsonData.Object.UserId.ToString()).ToArray());
            }
            catch(ArgumentException)
            {
                await Api.ExecuteCommand(new[] {"error", jsonData.Object.UserId.ToString()});
            }
        }

        private static string[] GetArgs(Message jsonData)
        {
            return Regex.Matches(jsonData.Object.Body, @"[\""].+?[\""]|[^ ]+")
                .Select(m => m.Value.Replace("\"", ""))
                .ToArray();
        }
    }
}