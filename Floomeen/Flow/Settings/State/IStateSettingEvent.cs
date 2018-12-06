using System;
using Floomeen.Meen;

namespace Floomeen.Flow.Settings.State
{
    public interface IStateSettingEvent
    {
        IStateSettingEvent OnEnterEvent(Action<Context> handler);

        IStateSettingEvent OnExitEvent(Action<Context> handler);
    }
}
