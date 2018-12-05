using System;
using Floomeen.Attributes;
using Floomeen.Meen.Interfaces;

namespace ConsoleWithConnectedMines
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
        public string __FlooType { get; set; }

        [FloomineChangedOn]
        public DateTime __FlooUtcStateChangedOn { get; set; }

        [FloomineStateData]
        public string __FlooData { get; set; }
    }
}