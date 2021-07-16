using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SmartHome.Application.Shared.Helpers
{
    public static class TypeHelpers
    {
        public static IEnumerable<Type> GetImplementations<T>() where T : class
        {
            return Assembly.GetExecutingAssembly().GetTypes()
                .Where(mytype => mytype.GetInterfaces().Contains(typeof(T)) && !mytype.IsInterface && !mytype.IsAbstract);
        }
    }
}