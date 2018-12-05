using System;
using Floomeen.Exceptions;
using Floomeen.Flow.FluentApi;
using Floomeen.Meen;

namespace Floomeen.Flow
{
    public class Setting : ISetting, IEvent
    {
        public string Element { get; private set; }

        public bool AsStartElement { get; private set; }

        public bool AsEndElement { get; private set; }
        
        public Action<Context> OnEnterEventHandler { get; private set; }

        public Action<Context> OnExitEventHandler { get; private set; }

        public Setting(string element)
        {
            Element = element;
        }

        public IEvent IsState()
        {
            throw new NotImplementedException();
        }

        public IEvent IsStartState()
        {
            if (AsEndElement) throw new FloomeenException("ElementAlreadySet");

            AsStartElement = true;

            return this;
        }

        public IEvent IsEndState()
        {
            if (AsStartElement) throw new FloomeenException("ElementAlreadySet");

            AsEndElement = true;

            return this;
        }

        public IEvent OnEnterEvent(Action<Context> handler)
        {
            OnEnterEventHandler = handler;

            return this;
        }

        public IEvent OnExitEvent(Action<Context> handler)
        {
            OnExitEventHandler = handler;

            return this;
        }
    }
}
