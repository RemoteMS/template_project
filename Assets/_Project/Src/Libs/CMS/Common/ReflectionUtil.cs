using System;
using System.Linq;
using System.Reflection;

namespace Common
{
    public static class ReflectionUtil
    {
        public static Type[] FindAllSubclasses<T>()
        {
            var baseType = typeof(T);
            var assembly = Assembly.GetAssembly(baseType);

            var types = assembly.GetTypes();
            var subclasses = types.Where(type => type.IsSubclassOf(baseType) && !type.IsAbstract).ToArray();

            return subclasses;
        }
    }
}