using System;
using Floomeen.Exceptions;
using Floomeen.Meen.Events;
using PubSub.Extension;

namespace Floomeen.Meen
{
    public class CoordinatorBase<TMaster, TSlave> where TMaster : MineBase where TSlave : MineBase
    {

        public TMaster MasterFloomine { get; }

        public TSlave SlaveFloomine { get; }

        public object MasterId { get; }

        protected CoordinatorBase(TMaster master, TSlave slave)
        {
            if (!master.IsBound)
                throw new FloomineException("MasterFloomineIsNotBound");

            if (!slave.IsBound)
                throw new FloomineException("SlaveFloomineIsNotBound");

            MasterFloomine = master;

            SlaveFloomine = slave;
            
            MasterId = MasterFloomine.CheckIfNotBoundAndGetId();
        }

        public void OnEvent<TEvent>(Func<TEvent, TMaster, TSlave, bool> handler) where TEvent : MineEventBase
        {
            this.Subscribe<TEvent>(@event =>
            {
                Console.WriteLine($">> {@event}");

                if (!IsRightForMe(@event)) return;

                if (handler(@event, MasterFloomine, SlaveFloomine))

                    this.Unsubscribe<TEvent>();

            });
        }

        private bool IsRightForMe(MineEventBase @event)
        {
            return @event.Id.Equals(MasterId) && @event.Floomine.GetType() == MasterFloomine.GetType();
        }
    }
}
