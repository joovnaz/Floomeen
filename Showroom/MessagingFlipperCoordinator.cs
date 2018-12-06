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
                if (!MachinesAreCompatible(master, slave)) return false;

                slave.Execute(FlipCommand);

                return true;

            });
            
            OnEvent<ExitedStateEvent>((@event, master, slave) =>
            {
                Console.WriteLine($"[COORDINATOR] Event '{@event.State}' received from Id={@event.Id.ToString()}");
                
                return true;
            });

            OnEvent<EnteredStateEvent>((@event, master, slave) =>
            {
                Console.WriteLine($"[COORDINATOR] Event '{@event.State}' received from Id={@event.Id.ToString()}");

                return true;
            });
        }

        private static bool MachinesAreCompatible(MeenBase master, MeenBase slave)
        {
            return 
                   master.BoundFellow.State == MessagingFloomeen.State.Retrying &&

                   slave.BoundFellow.State == FlipperFloomeen.State.Unchanged;
        }

    }
}
