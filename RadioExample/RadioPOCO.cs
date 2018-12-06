using Floomeen.Attributes;
using Floomeen.Meen.Interfaces;

namespace RadioExample
{
    public class RadioPOCO : IFellow
    {
        [FloomeenId]
        public long RadioId { get; set; }

        [FloomeenState]
        public string State { get; set; }
    }
}
