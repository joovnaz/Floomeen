using Floomeen.Meen;

namespace Floomeen.Tests.FloomeenShould
{
    public class CustomerOrderFloomeen : MeenBase
    {
        public struct State
        {
            public const string New = "New";
            public const string Shipping = "Shipping";
            public const string Delivered = "Shipping";
        }

        public struct Command
        {
            public const string Cargo = "Cargo";
            public const string Hand = "Hand";
        }

        public CustomerOrderFloomeen()
        {
            Flow.AddSetting(State.New)
                .IsStartState();

            Flow.AddSetting(State.Delivered)
                .IsEndState();
            
            Flow.AddTransition("CargoTransition")
                .From(State.New)
                .On(Command.Cargo)
                .GoTo(State.Shipping);

            Flow.AddTransition("HandTransition")
                .From(State.Shipping)
                .On(Command.Hand)
                .GoTo(State.Delivered);
        }
    }
}
