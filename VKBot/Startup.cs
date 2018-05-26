using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
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
            // Send message to BotAPI
        }
    }
}