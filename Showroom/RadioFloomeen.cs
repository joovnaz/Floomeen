using System;
using Floomeen.Meen;

namespace Showroom
{
    public class RadioFloomeen : MeenBase
    {
        public struct State
        {
            public const string StandBy = "StandBy";

            public const string Playing = "Playing";

            public const string Paused = "Paused";
        }

        public struct Command
        {
            public const string Play = "Play";

            public const string Stop = "Stop";

            public const string Pause = "Pause";
        }

        public RadioFloomeen()
        {
            Flow.AddStateSetting(State.Playing)
                .Then()
                    .OnEnterEvent(OnEnterAction)
                    .OnExitEvent(OnEnterAction);


            Flow.AddTransition()
                .From(State.StandBy)
                    .On(Command.Play)
                        .GoTo(State.Playing);

            Flow.AddTransition()
                .From(State.Playing)
                    .On(Command.Stop)
                        .ReturnTo(State.StandBy);

            Flow.AddTransition()
                .From(State.Playing)
                .On(Command.Pause)
                    .GoTo(State.Paused);

            Flow.AddTransition()
                .From(State.Paused)
                    .On(Command.Stop)
                        .ReturnTo(State.StandBy);

            Flow.AddTransition()
                .From(State.Paused)
                    .On(Command.Play)
                        .ReturnTo(State.Playing);
        }

        public void OnExitAction(Context context)
        {
            Log($"OnExit");
        }

        public void OnEnterAction(Context context)
        {
            Log($"OnEnter");
        }

        public static void Log(string msg)
        {
            Console.WriteLine(msg);
        }
    }
}
