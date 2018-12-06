using Floomeen.Attributes;
using Floomeen.Meen.Interfaces;

namespace CoordinatorExample.POCOs
{
    public class CustomerPOCO : IFellow
    {
        [FloomeenId]
        public string CustomerId { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Url { get; set; }

        [FloomeenState]
        public string Status { get; set; }

    }
}
