using System;

namespace Floomeen.Meen
{
    public class Factory<TMachine> where TMachine : MeenBase
    {
        public static TMachine Create()
        {
            var m = Factory.GetInstance<TMachine>();

            m.CheckIfWorkflowIsValid();

            return m;
        }
    }

    public static class Factory
    {
        public static MeenBase Create(string typeName)
        {
            var machine = GetInstance(typeName);

            var m = machine as MeenBase;

            m.CheckIfWorkflowIsValid();

            return m;
        }

        public static T GetInstance<T>()
        {
            var t = (T) Activator.CreateInstance(typeof(T));

            return t;
        }

        public static object GetInstance(string settingsType)
        {
            Type t = Type.GetType(settingsType);

            if (t == null)
                throw new Exception($"MissingFloomeenClassType[{settingsType}]");

            return Activator.CreateInstance(t);
        }
    }
}
