using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace VKBot.VKApi
{
    public static class VkApi
    {
        private const string UriTamplate = "https://api.vk.com/method/{0}?{1}&access_token={2}&v={3}";

        public static async Task ExecMethod(Method method, Dictionary<string, string> attributes)
        {
            var stringAttributes = string.Join("&", attributes.Select((e, i) => $"{e.Key}={e.Value}"));

            var uri = string.Format(UriTamplate, method.GetStringValue(), stringAttributes, Settings.ApiKey,
                Settings.VkApiVersion);

            await GetAsync(uri);
        }

        private static async Task<string> GetAsync(string uri)
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
