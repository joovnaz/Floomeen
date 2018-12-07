using System;
using Floomeen.Meen;

namespace Floomeen.Flow.Settings.Rules
{
    public class Condition
    {
        public Func<Result, Context, bool> ConditionFunc { get; set; }

        public string EndState { get; set; }
    }
}