using Floomeen.Meen;

namespace Floomeen.Showroom
{

    public class ListenerFloomine : MineBase
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

        public ListenerFloomine()
        {

            Floo.AddSetting(State.Unchanged).IsStartState();

            Floo.AddTransition("ChangeStateTransition")
                .From(State.Unchanged)
                .On(Command.Change)
                .GoTo(State.Changed);
        }
    }
}
