using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CounselVoting.Infrastructure.Helper
{
    public static class AssemblyHelper
    {
        public static IEnumerable<Type> GetAllTypesThatImplementInterface<TInterface>()
        {
            return Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(type => typeof(TInterface).IsAssignableFrom(type) && !type.IsInterface);
        }
    }
}
