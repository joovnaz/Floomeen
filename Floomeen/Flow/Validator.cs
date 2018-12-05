using System.Collections.Generic;
using System.Linq;
using Floomeen.Exceptions;

namespace Floomeen.Flow
{
    public class Validator
    {
        private readonly string _typename;

        private readonly RulesList _rulesList;

        private readonly SettingsList _settingsList;

        public Validator(string typename, RulesList rules, SettingsList settings)
        {
            _typename = typename;

            _rulesList = rules;

            _settingsList = settings;
        }

        public void CheckValidity()
        {
            CheckRules();

            CheckRepeatedRules();

            CheckSettings();
        }

        public bool IsValid()
        {
            try
            {
                CheckValidity();

                return true;
            }
            catch (FloomineException)
            {
                return false;
            }
        }

        public void CheckRules()
        {
            if (_rulesList?.Rules == null)
                RaiseException("UndefinedRuleset");

            _rulesList.Rules.ForEach(CheckRule);

            // Check 
            var fromStates = _rulesList.Rules.Select(r => r.FromState).Distinct();

            foreach (var fromState in fromStates)
            {
                if (_rulesList.Rules.Count(r => r.FromState == fromState && r.OnEnterAction != null) > 1)
                    RaiseException($"DoubleOnEnterActionDefinedFromState [{fromState}]");

                if (_rulesList.Rules.Count(r => r.FromState == fromState && r.OnExitAction != null) > 1)
                    RaiseException($"DoubleOnExitActionDefinedFromState [{fromState}]");

            }
        }

        public void CheckSettings()
        {
            if (_settingsList?.Sets == null)
                RaiseException("UndefinedElements.SetStartAndEndState");

            _settingsList.Sets.ForEach(CheckSetting);
        }

        public void CheckSetting(Setting set)
        {
            if (string.IsNullOrEmpty(set.Element))

                RaiseException("InvalidElement");

            if (!set.AsEndElement && !set.AsStartElement)

                RaiseException("InvalidSetting");

            var fromStates = _rulesList.FromStates();

            if (set.AsStartElement && !fromStates.Contains(set.Element))

                RaiseException("InvalidSettingNotFromState");

            var toStates = _rulesList.ToStates();

            if (set.AsEndElement && !toStates.Contains(set.Element))

                RaiseException("InvalidSettingNotEndState");
        }

        public void CheckRule(Rule rule)
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

        public void CheckRepeatedRules()
        {
            var fromStates = _rulesList.FromStates();

            var commands = _rulesList.Commands();

            foreach (var state in fromStates)
            {
                foreach (var command in commands)
                {
                    if (_rulesList.Rules.Count(r => r.FromState == state && r.OnCommand == command) > 1)

                        RaiseException($"DuplicateStateCommandDefinition [{state},{command}]");
                }
            }
        }

        private bool ThereAreNullOrEmptyEndStatesIn(List<ConditionElement> elements)
        {
            return elements.Count(el => string.IsNullOrEmpty(el.EndState)) > 0;
        }


        private void RaiseException(string message)
        {
            throw new FloomineException($"[{_typename}] {message}");
        }
    }
}
