using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

namespace SmartHome.Application.Extensions
{
    public static class EnumExtensions
    {
        public static T ParseEnum<T>(this string value)
        {
            var member = typeof(T)
                .GetTypeInfo()
                .DeclaredMembers
                .SingleOrDefault(x => string.Equals(x.GetCustomAttribute<EnumMemberAttribute>(false)?.Value, value, StringComparison.InvariantCultureIgnoreCase));


            return member is null ?
                (T)Enum.Parse(typeof(T), value, true)
                : (T)Enum.Parse(typeof(T), member.Name, true);
        }
    }
}
