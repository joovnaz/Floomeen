using System;
using Floomeen.Meen;
using Showroom;

namespace RadioExample
{
    class Program
    {

        private string[] Commands = {

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

            var poco = new RadioPOCO {RadioId = 1};

            radio.Plug(poco);

            Console.WriteLine($"Start State '{radio.CurrentState}'");

            Console.ReadKey();
        }
    }
}
