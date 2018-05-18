using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VKBot
{
    public class Message
    {
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("object")]
        public MessageObject Object { get; set; }
        [JsonProperty("group_id")]
        public int GroupId { get; set; }
    }
}
