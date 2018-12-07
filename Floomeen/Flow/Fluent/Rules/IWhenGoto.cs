namespace Floomeen.Flow.Fluent.Rules
{
    public interface IWhenGoto
    {
        IConditional GoTo(string state);

        IConditional ReturnTo(string state);

        IConditional StayAt(string state);
    }
}
