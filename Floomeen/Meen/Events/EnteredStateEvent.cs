namespace Floomeen.Meen.Events
{
    public class EnteredStateEvent : MeenEventBase
    {
        public string State { get; }

        public EnteredStateEvent(MeenBase floomeen, object id, string state) : base(floomeen, id)
        {
            State = state;
        }

        public override string ToString()
        {
            return $"[EnterStateEvent] Floomeen={Floomeen.GetType().Name}, FellowId ={Id}, State={State}";
        }

    }
}
