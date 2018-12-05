using System;
using Floomeen.Meen;

namespace Floomeen.Flow.FluentApi
{
    public interface IEvent
    {
        IEvent OnEnterEvent(Action<Context> handler);

        IEvent OnExitEvent(Action<Context> handler);
    }
}
