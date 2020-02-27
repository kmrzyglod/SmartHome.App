using System;
using System.Linq;
using System.Runtime.Serialization;

namespace SmartHome.Infrastructure.Helpers
{
    public static class EnumHelpers
    {
        public static string ToEnumString<T>(T type)
        {
            var enumType = typeof (T);
            var name = Enum.GetName(enumType, type);
            var enumMemberAttribute = ((EnumMemberAttribute[])enumType.GetField(name).GetCustomAttributes(typeof(EnumMemberAttribute), true)).Single();
            return enumMemberAttribute.Value;
        }

        public static T ToEnum<T>(string str)
        {
            var enumType = typeof(T);
            foreach (var name in Enum.GetNames(enumType))
            {
                var enumMemberAttribute = ((EnumMemberAttribute[])enumType.GetField(name).GetCustomAttributes(typeof(EnumMemberAttribute), true)).FirstOrDefault();
                if (enumMemberAttribute!= null && enumMemberAttribute.Value == str) return (T)Enum.Parse(enumType, name);
            }
            return default(T);
        }
    }
}
