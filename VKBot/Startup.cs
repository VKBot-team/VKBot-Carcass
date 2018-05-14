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
                await context.Response.WriteAsync("Hello, VKBOT!");
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

                var jsonData = JsonConvert.DeserializeObject<Dictionary<string, string>>(data);

                switch (jsonData["type"])
                {
                    case "confirmation":
                        data = Settings.ConfiramtionKey;
                        break;
                    case "message_new":
                        data = "ok";
                        break;
                }

                await context.Response.WriteAsync(data);
                var uri = $"https://api.vk.com/method/messages.send?user_id=84069595&message={data}&access_token={Settings.ApiKey}&v=5.74";
                await GetAsync(uri);
            });
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
