using System;
using Floomeen.Meen;
using Floomeen.Meen.Events;

namespace Showroom
{
    public class MessagingFlipperCoordinator : CoordinatorBase<MessagingFloomeen, FlipperFloomeen>
    {
        private readonly string FlipCommand = FlipperFloomeen.Command.Flip;

        public MessagingFlipperCoordinator(MessagingFloomeen m, FlipperFloomeen s)  : base(m, s)
        {
            OnEvent<ChangedStateEvent>((@event, master, slave) => 
            {
                if (!FellowStatesAreCompatible(master.BoundFellow, slave.BoundFellow)) return false;

                slave.Execute(FlipCommand);

                return true;

            });
            
            OnEvent<ExitedStateEvent>((@event, master, slave) =>
            {
                Console.WriteLine($"[COORDINATOR] ExitedStateEvent '{@event.State}' received from message Id='{@event.Id.ToString()}'");
                
                return true;
            });

            OnEvent<EnteredStateEvent>((@event, master, slave) =>
            {
                Console.WriteLine($"[COORDINATOR] EnteredStateEvent '{@event.State}' received from message Id='{@event.Id.ToString()}'");

                return true;
            });
        }

        private static bool FellowStatesAreCompatible(Fellow master, Fellow slave)
        {
            return 
                   master.State == MessagingFloomeen.State.Retrying &&

                   slave.State == FlipperFloomeen.State.Unchanged;
        }

    }
}
