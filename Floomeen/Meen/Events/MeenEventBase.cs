namespace Floomeen.Meen.Events
{
    public class MeenEventBase
    {
        public MeenBase Floomeen;

        public object Id;

        public MeenEventBase(MeenBase floomeen, object id)
        {
            Floomeen = floomeen;

            Id = id;
        }
    }
}
