namespace Floomeen.Meen.Events
{
    public class EnteredStateEvent : MineEventBase
    {
        public string State { get; }

        public EnteredStateEvent(MineBase floomine, object id, string state) : base(floomine, id)
        {
            State = state;
        }

        public override string ToString()
        {
            return $"[EnterStateEvent] Floomine={Floomine.GetType().Name}, FellowId ={Id}, State={State}";
        }

    }
}
