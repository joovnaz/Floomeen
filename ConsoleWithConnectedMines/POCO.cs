using System;
using Floomeen.Attributes;
using Floomeen.Meen.Interfaces;

namespace ConsoleWithConnectedMines
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
        
        //[FloomeenMachine]
        public string __FlooType { get; set; }

        //[FloomeenChangedOn]
        public DateTime __FlooUtcStateChangedOn { get; set; }

        //[FloomeenStateData]
        public string __FlooData { get; set; }
    }
}