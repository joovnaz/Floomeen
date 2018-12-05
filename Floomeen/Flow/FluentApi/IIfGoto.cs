namespace Floomeen.Flow.FluentApi
{
    public interface IIfGoto
    {
        IConditional GoTo(string state);

        IConditional ReturnTo(string state);

        IConditional StayAt(string state);
    }
}
