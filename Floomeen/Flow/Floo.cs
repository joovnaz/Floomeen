using System.Collections.Generic;
using System.Linq;
using Floomeen.Exceptions;
using Floomeen.Flow.FluentApi;

namespace Floomeen.Flow
{
    public class Floo
    {
        private readonly string _typename;

        private readonly RulesList _rulesList;

        private readonly SettingsList _settingsList;

        public Floo(string typename) : this()
        {
            _typename = typename;
        }

        public Floo()
        {
            _rulesList = new RulesList();

            _settingsList = new SettingsList();
        }

        public IFrom AddTransition()
        {
            return AddTransition(string.Empty);
        }

        public IFrom AddTransition(string rulename)
        {
            var wfx = new Rule(rulename);

            _rulesList.Rules.Add(wfx);

            return wfx;
        }

        public ISetting AddSetting(string element)
        {
            var setx = new Setting(element);

            _settingsList.Sets.Add(setx);

            return setx;
        }

        public void CheckValidity()
        {
            var validator = new Validator(_typename, _rulesList, _settingsList);

            validator.CheckValidity();
        }

        public bool IsValid()
        {
            try
            {
                CheckValidity();

                return true;
            }
            catch (FloomeenException)
            {
                return false;
            }
        }

        private List<string> FromStatesList => _rulesList.FromStates();

        private List<string> ToStatesList => _rulesList.ToStates();

        public bool CheckState(string state)
        {
            return FromStatesList.Contains(state) ||
                   ToStatesList.Contains(state);
        }


        public List<string> AvailableCommands(string state)
        {
            var filtered = _rulesList.Rules.Where(r => r.FromState == state);

            return filtered.Select(r => r.OnCommand).ToList();
        }

        public string StartState => _settingsList.StartState;

        public Rule RetrieveApplicableRule(string state, string command)
        {
            var rule = _rulesList.Rules.FirstOrDefault(r => r.FromState == state && r.OnCommand == command);

            if (rule == null) RaiseException($"UnsupportedCommand '{command}'");

            return rule;
        }

        public Setting RetrieveSetting(string state)
        {
            return _settingsList.Sets.FirstOrDefault(r => r.Element == state);
        }

        private void RaiseException(string message)
        {
            throw new FloomeenException($"[{_typename}] {message}");
        }
    }
}
