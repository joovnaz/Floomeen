using Floomeen.Meen;

namespace Floomeen.Tests.FloomeenShould
{
    public class CustomerOrderFloomine : MineBase
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

        public CustomerOrderFloomine()
        {
            Floo.AddSetting(State.New)
                .IsStartState();

            Floo.AddSetting(State.Delivered)
                .IsEndState();
            
            Floo.AddTransition("CargoTransition")
                .From(State.New)
                .On(Command.Cargo)
                .GoTo(State.Shipping);

            Floo.AddTransition("HandTransition")
                .From(State.Shipping)
                .On(Command.Hand)
                .GoTo(State.Delivered);
        }
    }
}
