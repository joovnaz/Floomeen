using Floomeen.Utils;

namespace Floomeen.Meen
{
    public class Factory<TMachine> where TMachine : MeenBase
    {
        public static TMachine Create()
        {
            var m = FactoryExtensions.GetInstance<TMachine>();

            m.Flow.Check();

            return m;
        }
    }
}