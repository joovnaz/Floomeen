namespace Floomeen.Flow.Settings.State
{
    public interface IStateSetting
    {
        IStateEvent Then();

        IStateEvent IsStartState();

        IStateEvent IsEndState();

    }
}
