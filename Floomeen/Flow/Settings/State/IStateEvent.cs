using System;
using Floomeen.Meen;

namespace Floomeen.Flow.Settings.State
{
    public interface IStateEvent
    {
        IStateEvent OnEnterEvent(Action<Context> handler);

        IStateEvent OnExitEvent(Action<Context> handler);
    }
}
