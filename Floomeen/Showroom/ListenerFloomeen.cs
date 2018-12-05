using Floomeen.Meen;

namespace Floomeen.Showroom
{

    public class ListenerFloomeen : MeenBase
    {
        public struct State
        {
            public const string Unchanged = "NotChangedState";

            public const string Changed = "ChangedState";
        }

        public struct Command
        {
            public const string Change = "ChangeStateCommand";
        }

        public ListenerFloomeen()
        {

            Flow.AddSetting(State.Unchanged).IsStartState();

            Flow.AddTransition("ChangeStateTransition")
                .From(State.Unchanged)
                .On(Command.Change)
                .GoTo(State.Changed);
        }
    }
}
