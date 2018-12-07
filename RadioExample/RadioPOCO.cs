using System;
using Floomeen.Attributes;
using Floomeen.Meen.Interfaces;

namespace RadioExample
{
    public class RadioPOCO : IFellow
    {
        [FloomeenId]
        public long RadioId { get; set; }

        public int AudioVolume { get; set; }

        [FloomeenState]
        public string State { get; set; }

        [FloomeenMachine]
        public string Machine { get; set; }

        [FloomeenChangedOn]
        public DateTime ChangedOn { get; set; }
    }
}
