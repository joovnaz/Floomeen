using Floomeen.Meen;

namespace Showroom
{
    public class FlipperFloomeen : MeenBase
    {
        public struct State
        {
            public const string Unchanged = "Unchanged";

            public const string Changed = "Changed";
        }

        public struct Command
        {
            public const string Flip = "FlipCommand";
        }

        public FlipperFloomeen()
        {

            Flow.AddSetting(State.Unchanged).IsStartState();

            Flow.AddTransition("FlipTransition")
                .From(State.Unchanged)
                .On(Command.Flip)
                .GoTo(State.Changed);
        }
    }
}
