using System;
using Floomeen.Meen;

namespace Floomeen.Showroom
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

        public RadioFloomeen(string typename) : base(typename)
        {
            Flow.AddTransition()
                .From(State.StandBy)
                .OnEnter(OnEnterAction)
                .OnExit(OnExitAction)
                .On(Command.Play)
                    .GoTo(State.Playing);

            Flow.AddTransition()
                .From(State.Playing)
                .On(Command.Stop)
                .ReturnTo(State.StandBy);

            Flow.AddTransition()
                .From(State.Playing)
                .OnEnter(OnEnterAction)
                .On(Command.Pause)
                    .GoTo(State.Paused);

            Flow.AddTransition()
                .From(State.Paused)
                    .On(Command.Stop).ReturnTo(State.StandBy);

            Flow.AddTransition()
                .From(State.Paused)
                    .On(Command.Play).ReturnTo(State.Playing);
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
