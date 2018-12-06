using Floomeen.Attributes;
using Floomeen.Meen.Interfaces;

namespace CoordinatorExample.POCOs
{
    public class MessagePOCO : IFellow
    {
        [FloomeenId]
        public string MessageId { get; set; }

        [FloomeenState]
        public string State { get; set; }
        
        
        [FloomeenStateData]
        public string __FlooData { get; set; }
    }
}