using System;
using Floomeen.Meen;
using Showroom;

namespace RadioExample
{
    class Program
    {

        private static string[] Commands = {

            RadioFloomeen.Command.Play,
            RadioFloomeen.Command.Stop,
            RadioFloomeen.Command.Play,
            RadioFloomeen.Command.Pause,
            RadioFloomeen.Command.Play,
            RadioFloomeen.Command.Stop,
            RadioFloomeen.Command.Play,
            RadioFloomeen.Command.Pause,
            RadioFloomeen.Command.Stop,

        };


        static void Main(string[] args)
        {
            var radio = Factory<RadioFloomeen>.Create();

            var poco = new RadioPOCO { RadioId = 1 };

            radio.Plug(poco);


            foreach (var command in Commands)
            {
                Console.WriteLine($"===============================");

                Console.WriteLine($"State '{radio.CurrentState}'");

                Console.WriteLine($"Available Commands [{string.Join(",", radio.AvailableCommands())}]");

                Console.WriteLine($"Executing {command}");

                Console.ReadKey();

                radio.Execute(command);

            }


        }
    }
}
