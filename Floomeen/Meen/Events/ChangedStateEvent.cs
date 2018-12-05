namespace Floomeen.Meen.Events
{
    public class ChangedStateEvent : MineEventBase
    {
        public string FromState { get; }

        public string ToState { get; }

        public ChangedStateEvent(MineBase floomine, object id, string fromState, string toState) :
            
            base(floomine, id)
        {
            FromState = fromState;

            ToState = toState;
        }

        public override string ToString()
        {
            return $"[StateChangedEvent] Floomine={Floomine.GetType().Name}, FellowId ={Id}, From={FromState}, To={ToState}";
        }
    }
}
