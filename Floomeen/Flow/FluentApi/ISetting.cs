namespace Floomeen.Flow.FluentApi
{
    public interface ISetting
    {
        IEvent IsState();

        IEvent IsStartState();

        IEvent IsEndState();

    }
}
