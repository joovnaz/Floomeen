namespace Floomeen.Meen.Events
{
    public class ExitedStateEvent : MeenEventBase
    {
        public string State { get; }

        public ExitedStateEvent(MeenBase floomeen, object id, string state) : base(floomeen, id)
        {
            State = state;
        }

        public override string ToString()
        {
            return $"[ExitStateEvent] Floomeen={Floomeen.GetType().Name}, FellowId ={Id}, State={State}";
        }

    }
}
