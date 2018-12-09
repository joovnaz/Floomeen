using Floomeen.Adapters;
using Floomeen.Adapters.MessageSender;
using SupportedTypes = Showroom.CustomerOrder.OrderManagementSystem.SupportedTypes;

namespace Showroom.CustomerOrder
{
    public class OrderSystemAdapter : IAdapter
    {
        public string[] AcceptedTypes()
        {
            return new []
            {
                SupportedTypes.CustomerOrder
            };
        }
    }
}
