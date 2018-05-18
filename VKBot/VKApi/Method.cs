using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace VKBot.VKApi
{
    public enum Method
    {
        [EnumString("messages.send")]
        MessageSend
    }

    public class EnumStringAttribute : Attribute
    {
        public EnumStringAttribute(string stringValue)
        {
            StringValue = stringValue;
        }

        public string StringValue { get; set; }
    }

    public static class ExtenstionClass
    {
        public static string GetStringValue(this Enum value)
        {
            var type = value.GetType();
            var fieldInfo = type.GetField(value.ToString());
            if (fieldInfo.GetCustomAttribute(
                typeof(EnumStringAttribute), false) is EnumStringAttribute attribute) return attribute.StringValue;
            return string.Empty;
        }
    }
}
