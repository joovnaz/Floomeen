using System;
using Floomeen.Meen;
using Floomeen.Meen.Events;

namespace Floomeen.Showroom
{
    public class MessagingListenerFloominesCoordinator : CoordinatorBase<MessagingFloomine, ListenerFloomine>
    {
        public MessagingListenerFloominesCoordinator(MessagingFloomine m, ListenerFloomine s)  : base(m, s)
        {
            OnEvent<ChangedStateEvent>((@event, master, slave) => 
            {

                if (!MachinesAreCompatible(master, slave)) return false;

                slave.Execute(ListenerFloomine.Command.Change);

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

        private static bool MachinesAreCompatible(MineBase master, MineBase slave)
        {
            return 
                   master.GetState() == MessagingFloomine.State.Retrying &&
                   slave.GetState() == ListenerFloomine.State.Unchanged;
        }

    }
}
