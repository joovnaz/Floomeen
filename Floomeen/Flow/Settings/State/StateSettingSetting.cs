using System;
using Floomeen.Exceptions;
using Floomeen.Meen;

namespace Floomeen.Flow.Settings.State
{
    public class StateSettingSetting : IStateSetting, IStateSettingEvent
    {
        public string State { get; private set; }

        public bool IsStart { get; private set; }

        public bool IsEnd { get; private set; }
       
        public Action<Context> OnEnterEventHandler { get; private set; }

        public Action<Context> OnExitEventHandler { get; private set; }

        public StateSettingSetting(string state)
        {
            State = state;
        }

        public IStateSettingEvent Then()
        {
            return this;
        }

        public IStateSettingEvent IsStartState()
        {
            if (IsEnd) throw new FloomeenException("ElementAlreadySet");

            IsStart = true;

            return this;
        }

        public IStateSettingEvent IsEndState()
        {
            if (IsStart) throw new FloomeenException("ElementAlreadySet");

            IsEnd = true;

            return this;
        }

        public IStateSettingEvent OnEnterEvent(Action<Context> handler)
        {
            OnEnterEventHandler = handler;

            return this;
        }

        public IStateSettingEvent OnExitEvent(Action<Context> handler)
        {
            OnExitEventHandler = handler;

            return this;
        }
    }
}
