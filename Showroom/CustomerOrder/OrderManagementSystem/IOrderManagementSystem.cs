using System;
using System.Collections.Generic;
using System.Text;
using Floomeen.Adapters;

namespace Showroom.CustomerOrder.OrderManagementSystem
{
    interface IOrderManagementSystem : IAdapter
    {
        bool CheckProductsAvailabilityByOrderId(object orderId);
    }
}
