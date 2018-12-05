﻿using System;
using Floomeen.Exceptions;
using Floomeen.Meen.Events;
using PubSub.Extension;

namespace Floomeen.Meen
{
    public class CoordinatorBase<TMaster, TSlave> where TMaster : MeenBase where TSlave : MeenBase
    {

        public TMaster MasterFloomeen { get; }

        public TSlave SlaveFloomeen { get; }

        public object MasterId { get; }

        protected CoordinatorBase(TMaster master, TSlave slave)
        {
            if (!master.IsBound)
                throw new FloomeenException("MasterFloomeenIsNotBound");

            if (!slave.IsBound)
                throw new FloomeenException("SlaveFloomeenIsNotBound");

            MasterFloomeen = master;

            SlaveFloomeen = slave;
            
            MasterId = MasterFloomeen.CheckIfNotBoundAndGetId();
        }

        public void OnEvent<TEvent>(Func<TEvent, TMaster, TSlave, bool> handler) where TEvent : MeenEventBase
        {
            this.Subscribe<TEvent>(@event =>
            {
                Console.WriteLine($">> {@event}");

                if (!IsRightForMe(@event)) return;

                if (handler(@event, MasterFloomeen, SlaveFloomeen))

                    this.Unsubscribe<TEvent>();

            });
        }

        private bool IsRightForMe(MeenEventBase @event)
        {
            return @event.Id.Equals(MasterId) && @event.Floomeen.GetType() == MasterFloomeen.GetType();
        }
    }
}
