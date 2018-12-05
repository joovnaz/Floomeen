using Floomeen.Meen;

namespace Floomeen.Tests.FloomeenShould
{
    public class TestingOnOffMachine : MineBase
    {
        public struct State
        {
            public const string On = "On";

            public const string Off = "Off";
        }

        public struct Command
        {
            public const string SwitchOn = "SwitchOn";

            public const string SwitchOff = "SwitchOff";
        }


        public TestingOnOffMachine()
        {

            Floo.AddSetting(State.On)
              .IsStartState();

            Floo.AddTransition("SwitchOnTransition")
                .From(State.On)
                .On(Command.SwitchOff)
                .GoTo(State.Off);

            Floo.AddTransition("SwitchOffTransition")
                .From(State.Off)
                .On(Command.SwitchOn)
                .GoTo(State.On);

        }

    }
}
