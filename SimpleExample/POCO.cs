using System;
using Floomeen.Attributes;
using Floomeen.Meen.Interfaces;

namespace Console
{
    public class POCO : IFellow
    {
        [FloomeenId]
        public string Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Url { get; set; }

        [FloomeenState]
        public string State { get; set; }

        [FloomeenMachine]
        public string Machine { get; set; }

        [FloomeenChangedOn]
        public DateTime UtChangedOn { get; set; }

        [FloomeenStateData]
        public string StateData { get; set; }
    }
}