using System;
using Floomeen.Exceptions;
using Floomeen.Meen;

namespace Floomeen.Flow.Settings.State
{
    public class StateSetting : IStateSetting, IStateEvent
    {
        public string State { get; private set; }

        public bool IsStart { get; private set; }

        public bool IsEnd { get; private set; }
       
        public Action<Context> OnEnterEventHandler { get; private set; }

        public Action<Context> OnExitEventHandler { get; private set; }

        public StateSetting(string state)
        {
            State = state;
        }

        public IStateEvent Then()
        {
            return this;
        }

        public IStateEvent IsStartState()
        {
            if (IsEnd) throw new FloomeenException("ElementAlreadySet");

            IsStart = true;

            return this;
        }

        public IStateEvent IsEndState()
        {
            if (IsStart) throw new FloomeenException("ElementAlreadySet");

            IsEnd = true;

            return this;
        }

        public IStateEvent OnEnterEvent(Action<Context> handler)
        {
            OnEnterEventHandler = handler;

            return this;
        }

        public IStateEvent OnExitEvent(Action<Context> handler)
        {
            OnExitEventHandler = handler;

            return this;
        }
    }
}
