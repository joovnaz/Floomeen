using System;
using Floomeen.Attributes;
using Floomeen.Meen.Interfaces;

namespace Floomeen.Tests
{
    public class POCO : IFellow
    {
        [FloomineId]
        public string Id { get; set; }

        public string Username { get; set; }

        [FloomineMachine]
        public string FlooMachine { get; set; }

        [FloomineState]
        public string FlooState { get; set; }

        [FloomineStateData]
        public string FlooStateData { get; set; }

        [FloomineChangedOn]
        public DateTime FlooUtcChangedOn { get; set; }

    }
}