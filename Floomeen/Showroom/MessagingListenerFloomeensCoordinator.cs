using System;
using Floomeen.Meen;
using Floomeen.Meen.Events;

namespace Floomeen.Showroom
{
    public class MessagingListenerFloomeensCoordinator : CoordinatorBase<MessagingFloomeen, ListenerFloomeen>
    {
        public MessagingListenerFloomeensCoordinator(MessagingFloomeen m, ListenerFloomeen s)  : base(m, s)
        {
            OnEvent<ChangedStateEvent>((@event, master, slave) => 
            {

                if (!MachinesAreCompatible(master, slave)) return false;

                slave.Execute(ListenerFloomeen.Command.Change);

                return true;

            });
            
            OnEvent<ExitedStateEvent>((@event, master, slave) =>
            {
                Console.WriteLine($"[*] ExitState {@event.State} from Id={@event.Id.ToString()}");
                
                return true;
            });

            OnEvent<EnteredStateEvent>((@event, master, slave) =>
            {
                Console.WriteLine($"[*] EnterState {@event.State} from Id={@event.Id.ToString()}");

                return true;
            });
        }

        private static bool MachinesAreCompatible(MeenBase master, MeenBase slave)
        {
            return 
                   master.BoundFellow.State == MessagingFloomeen.State.Retrying &&
                   slave.BoundFellow.State == ListenerFloomeen.State.Unchanged;
        }

    }
}
