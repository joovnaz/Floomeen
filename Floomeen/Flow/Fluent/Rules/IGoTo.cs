namespace Floomeen.Flow.Fluent.Rules
{
    public interface IGoTo
    {
        void GoTo(string state);

        void ReturnTo(string state);

        void StayAt(string state);

    }
}
