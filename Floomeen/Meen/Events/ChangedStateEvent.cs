namespace Floomeen.Meen.Events
{
    public class ChangedStateEvent : MeenEventBase
    {
        public string FromState { get; }

        public string ToState { get; }

        public ChangedStateEvent(MeenBase floomeen, object id, string fromState, string toState) :
            
            base(floomeen, id)
        {
            FromState = fromState;

            ToState = toState;
        }

        public override string ToString()
        {
            return $"[StateChangedEvent] Floomeen={Floomeen.GetType().Name}, FellowId ={Id}, From={FromState}, To={ToState}";
        }
    }
}
