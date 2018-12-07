namespace Floomeen.Flow.Fluent.States
{
    public interface IStateSetting
    {
        IStateSettingEvent Then();

        IStateSettingEvent IsStartState();

        IStateSettingEvent IsEndState();
    }
}
