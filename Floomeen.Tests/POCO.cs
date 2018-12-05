using System;
using Floomeen.Attributes;
using Floomeen.Meen.Interfaces;

namespace Floomeen.Tests
{
    public class POCO : IFellow
    {
        [FloomeenId]
        public string Id { get; set; }

        public string Username { get; set; }

        [FloomeenMachine]
        public string FlooMachine { get; set; }

        [FloomeenState]
        public string FlooState { get; set; }

        [FloomeenStateData]
        public string FlooStateData { get; set; }

        [FloomeenChangedOn]
        public DateTime FlooUtcChangedOn { get; set; }

    }
}