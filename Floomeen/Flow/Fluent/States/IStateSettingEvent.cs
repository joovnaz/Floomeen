using System;
using Floomeen.Meen;

namespace Floomeen.Flow.Fluent.States
{
    public interface IStateSettingEvent
    {
        IStateSettingEvent OnEnterEvent(Action<Context> handler);

        IStateSettingEvent OnExitEvent(Action<Context> handler);
    }
}
