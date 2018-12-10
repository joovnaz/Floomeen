using Floomeen.Meen;
using Showroom.CustomerOrder.OrderManagementSystem;

namespace Showroom.CustomerOrder
{
    public class CustomerOrderFloomeen : MeenBase
    {
        public struct State
        {
            public const string New = "New";
            public const string Shipping = "Shipping";
            public const string Delivered = "Delivered";
            public const string Waiting = "Waiting";
        }

        public struct Command
        {
            public const string Cargo = "Cargo";
            public const string Hand = "Hand";
        }

        public CustomerOrderFloomeen()
        {
            Flow.AddStateSetting(State.New)
                .IsStartState();

            Flow.AddStateSetting(State.Delivered)
                .IsEndState();

            Flow.AddTransition("CargoTransition")
                .From(State.New)
                .On(Command.Cargo)
                .Do(CheckOrderedProductsAvailability)
                    .When(AreReadyForShipping)
                        .GoTo(State.Shipping)
                    .Otherwise()
                        .GoTo(State.Waiting);

            Flow.AddTransition("HandTransition")
                .From(State.Shipping)
                .On(Command.Hand)
                    .GoTo(State.Delivered);
        }

        public Result CheckOrderedProductsAvailability(Context context)
        {
            var orderId = context.Fellow.Id;

            var externalOrderSystem = SelectAdapter<IOrderManagementSystem>(SupportedTypes.CustomerOrder);

            var areProductsAvailable = externalOrderSystem.CheckProductsAvailabilityByOrderId(orderId);

            return new Result(areProductsAvailable);
        }

        public bool AreReadyForShipping(Result result, Context context)
        {
            return result.Success;
        }
    }
}

