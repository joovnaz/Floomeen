using System;
using Floomeen.Meen;

namespace Floomeen.Flow.Fluent.Rules
{
    public interface IConditional
    {
        IWhenGoto When(Func<Result, Context, bool> condition);

        IGoTo Otherwise();
    }
}
