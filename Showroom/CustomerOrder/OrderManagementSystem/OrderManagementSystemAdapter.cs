using System;
using System.Collections.Generic;
using System.Text;

namespace Showroom.CustomerOrder.OrderManagementSystem
{
    public class OrderManagementSystemAdapter : IOrderManagementSystem
    {
        public bool CheckAvailabilityById(object orderId)
        {
            return true;
        }

        public string[] AcceptedTypes()
        {
            throw new NotImplementedException();
        }
    }
}
