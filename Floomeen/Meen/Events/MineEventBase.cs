namespace Floomeen.Meen.Events
{
    public class MineEventBase
    {
        public MineBase Floomine;

        public object Id;

        public MineEventBase(MineBase floomine, object id)
        {
            Floomine = floomine;

            Id = id;
        }
    }
}
