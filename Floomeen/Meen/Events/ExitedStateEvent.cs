namespace Floomeen.Meen.Events
{
    public class ExitedStateEvent : MineEventBase
    {
        public string State { get; }

        public ExitedStateEvent(MineBase floomine, object id, string state) : base(floomine, id)
        {
            State = state;
        }

        public override string ToString()
        {
            return $"[ExitStateEvent] Floomine={Floomine.GetType().Name}, FellowId ={Id}, State={State}";
        }

    }
}
