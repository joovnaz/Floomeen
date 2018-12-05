using System;
using Floomeen.Attributes;
using Floomeen.Meen.Interfaces;

namespace Console
{
    public class POCO : IFellow
    {
        [FloomineId]
        public string Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Url { get; set; }

        [FloomineState]
        public string State { get; set; }

        [FloomineMachine]
        public string Machine { get; set; }

        [FloomineChangedOn]
        public DateTime UtChangedOn { get; set; }

        [FloomineStateData]
        public string StateData { get; set; }
    }
}