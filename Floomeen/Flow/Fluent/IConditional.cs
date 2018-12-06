using System;
using Floomeen.Meen;

namespace Floomeen.Flow.Fluent
{
    public interface IConditional
    {
        IIfGoto When(Func<Result, Context, bool> condition);

        IGoTo Otherwise();
    }
}
