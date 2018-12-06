using System.Collections.Generic;
using System.Linq;
using Floomeen.Exceptions;

namespace Floomeen.Flow
{
    public class RulesValidator
    {
        private readonly string _machineName;

        public List<Rule> Rules { get; }

        public RulesValidator(string machineName)
        {
            _machineName = machineName;

            Rules = new List<Rule>();
        }

        #region Rules

        public List<string> FromStates => Rules
            .Where(r => !string.IsNullOrEmpty(r.FromState))
            .Select(r => r.FromState)
            .Distinct().ToList();

        public List<string> Commands => Rules
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

        public bool IsExistingState(string state)
        {
            return FromStates.Contains(state) || ToStates().Contains(state);
        }

        public Rule RetrieveApplicableRule(string state, string command)
        {
            var rule = Rules.FirstOrDefault(r => r.FromState == state && r.OnCommand == command);

            if (rule == null) RaiseException($"UnsupportedCommand '{command}'");

            return rule;
        }

        public List<string> AvailableCommands(string state)
        {
            var filtered = Rules.Where(r => r.FromState == state);

            return filtered.Select(r => r.OnCommand).ToList();
        }

        #endregion

        #region Validation

        public void Check()
        {
            CheckRules();

            CheckRepeatedRules();
        }

        private void CheckRules()
        {
            if (Rules == null)
                RaiseException("UndefinedRuleset");

            Rules.ForEach(CheckRule);

            foreach (var fromState in FromStates)
            {
                if (Rules.Count(r => r.FromState == fromState) > 1)
                    RaiseException($"DoubleOnEnterActionDefinedFromState [{fromState}]");

                if (Rules.Count(r => r.FromState == fromState) > 1)
                    RaiseException($"DoubleOnExitActionDefinedFromState [{fromState}]");

            }
        }

        private void CheckRule(Rule rule)
        {
            if (string.IsNullOrEmpty(rule.FromState))

                RaiseException($"UndefinedFromState [{rule.Rulename}]");

            if (string.IsNullOrEmpty(rule.OnCommand))

                RaiseException($"UndefinedCommand [{rule.Rulename}]");

            if (IsAShortRuleWithEndState(rule)) return;

            if (rule.ConditionElements == null)

                RaiseException($"MissingConditions [{rule.Rulename}]");

            var conditions = rule.ConditionElements.ToList();

            if (conditions.Count == 0) RaiseException($"MissingConditions [{rule.Rulename}]");

            if (ThereAreNullOrEmptyEndStatesIn(conditions))

                RaiseException($"UndefinedEndState [{rule.Rulename}]");
        }

        private bool IsAShortRuleWithEndState(Rule rule)
        {
            return rule.ToState != null && rule.DoFunc == null;
        }

        private bool ThereAreNullOrEmptyEndStatesIn(List<ConditionElement> elements)
        {
            return elements.Count(el => string.IsNullOrEmpty(el.EndState)) > 0;
        }

        private void CheckRepeatedRules()
        {
            foreach (var state in FromStates)
            {
                foreach (var command in Commands)
                {
                    if (Rules.Count(r => r.FromState == state && r.OnCommand == command) > 1)

                        RaiseException($"DuplicateStateCommandDefinition [{state},{command}]");
                }
            }
        }

        #endregion

        private void RaiseException(string message)
        {
            throw new FloomeenException($"[{_machineName}] {message}");
        }
    }
}
