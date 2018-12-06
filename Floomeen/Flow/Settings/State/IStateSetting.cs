namespace Floomeen.Flow.Settings.State
{
    public interface IStateSetting
    {
        IStateSettingEvent Then();

        IStateSettingEvent IsStartState();

        IStateSettingEvent IsEndState();

    }
}
