using System;
using Floomeen.Meen;

namespace Floomeen.Flow.FluentApi
{
    public interface IConditional
    {
        IIfGoto When(Func<Result, Context, bool> condition);

        IGoTo Otherwise();
    }
}
