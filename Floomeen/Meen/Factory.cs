using System;
using System.Reflection;

namespace Floomeen.Meen
{
    public class Factory<TMachine> where TMachine : MeenBase
    {
        public static TMachine Create()
        {
            var m = Factory.GetInstance<TMachine>();

            return m;
        }
    }

    public static class Factory
    {
        public static T Create<T>(string typeName)
        {
            var machine = GetInstance(typeName);

            if (machine is T) return (T) machine;

            throw new Exception($"ClassTypeNotExists[{typeName}]");
        }

        public static T GetInstance<T>()
        {
            var t = (T) Activator.CreateInstance(typeof(T));

            return t;
        }

        public static object GetInstance(string settingsType)
        {
            Type t = SearchType(settingsType);

            if (t == null)
                throw new Exception($"ClassTypeNotExists[{settingsType}]");

            return Activator.CreateInstance(t);
        }

        public static Type SearchType(string settingsType)
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (Assembly assembly in assemblies)
            {
                Type type = assembly.GetType(settingsType);

                if (type != null) return type;
            }

            return null;
        }


    }
}
