using System;
using Floomeen.Meen;

namespace Floomeen.Flow.FluentApi
{
    public interface IDo
    {
        IConditional Do(Func<Context, Result> action);

        void GoTo(string state);

        void StayAt(string state);

        void ReturnTo(string state);
    }
}
