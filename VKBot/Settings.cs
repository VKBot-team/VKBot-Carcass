using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VKBot
{
    public static class Settings
    {
        public static string ApiKey = "aa6784bf1a3d4e733acc22858605fcb709ae2e5a251e0e836893ae51b22854c47f33a8794d57c4404a0dd";
        public static string HandlerPath = "/handle/for_callback_vk_api";
        public static string Response = "c2842cb9";
        public static string MethodUri = $"https://api.vk.com/method/messages.send?access_token={ApiKey}&v=5.74";
    }
}
