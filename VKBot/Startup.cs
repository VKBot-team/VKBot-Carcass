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
                File.WriteAllText(@"D:\Проекты\VKBOT_IIS\1.txt", "azz");
                await context.Response.WriteAsync("Hello, VKBOT!");
            });
        }

        private void CallbackHandler(IApplicationBuilder app)
        {
            app.Run(async context => 
            {
                File.WriteAllText(@"D:\Проекты\VKBOT_IIS\1.txt", "azzzzzz");
                string data;
                var response = string.Empty;
                using (var reader = new StreamReader(context.Request.Body))
                {
                    data = await reader.ReadToEndAsync();
                }

                var jsonData = JsonConvert.DeserializeObject<Dictionary<string, object>>(data);

                File.WriteAllText(@"D:\Проекты\VKBOT_IIS\1.txt", jsonData["type"].ToString());

                switch (jsonData["type"].ToString())
                {
                    case "confirmation":
                        response = Settings.ConfiramtionKey;
                        break;
                    case "message_new":
                        response = "ok";
                        break;
                }

                await context.Response.WriteAsync(response);

                if (jsonData["type"].ToString() == "message_new")
                {
                    var msg = "Я тестовый бот, ничего тебе не скажу. Ну а что, ты думал что я аниме чтоли буду присылать?!";
                    var uri = $"https://api.vk.com/method/messages.send?user_id={GetUserId(data)}&message={msg}&access_token={Settings.ApiKey}&v=5.74";
                    await GetAsync(uri);
                }
            });
        }

        private int GetUserId(string request)
        {
            var jsonData = JsonConvert.DeserializeObject<Dictionary<string, object>>(request);
            var info = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonData["object"].ToString());
            return int.Parse(info["user_id"].ToString());
        }

        public async Task<string> GetAsync(string uri)
        {
            var request = (HttpWebRequest)WebRequest.Create(uri);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            using (var response = (HttpWebResponse)await request.GetResponseAsync())
            using (var stream = response.GetResponseStream())
            using (var reader = new StreamReader(stream))
            {
                return await reader.ReadToEndAsync();
            }
        }
    }
}
