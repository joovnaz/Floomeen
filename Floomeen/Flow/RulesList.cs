using System.Collections.Generic;
using System.Linq;

namespace Floomeen.Flow
{
    public class RulesList
    {
        public List<Rule> Rules { get; private set; }

        public RulesList(List<Rule> rules)
        {
            Rules = rules;
        }

        public RulesList()
        {
            Rules = new List<Rule>();
        }

        public List<string> FromStates() => Rules
            .Where(r => !string.IsNullOrEmpty(r.FromState))
            .Select(r => r.FromState)
            .Distinct().ToList();

        public List<string> Commands() => Rules
            .Where(r => !string.IsNullOrEmpty(r.OnCommand))
            .Select(r => r.OnCommand)
            .Distinct().ToList();

        public List<string> ToStates()
        {
            var endStates = Rules
                .Where(r => !string.IsNullOrEmpty(r.ToState))
                .Select(r => r.ToState)
                .Distinct()
                .ToList();

            var rulesWithCondition = Rules.Where(r => r.ConditionElements != null);

            foreach (var rule in rulesWithCondition)
            {
                var moreEndStates = rule.ConditionElements
                                .Where(c => !string.IsNullOrEmpty(c.EndState))
                                .Select(c => c.EndState)
                                .Distinct()
                                .ToList();

                endStates.AddRange(moreEndStates);
            }

            return endStates.Distinct().ToList();
        }

    }
}
