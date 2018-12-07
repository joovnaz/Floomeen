using System.Collections.Generic;
using Floomeen.Exceptions;
using Floomeen.Flow.Fluent.Rules;
using Floomeen.Flow.Fluent.States;
using Floomeen.Flow.Settings.Rules;
using Floomeen.Flow.Settings.States;

namespace Floomeen.Flow
{
    public class Floo
    {
        private readonly string _machineName;

        private readonly RulesValidator _rulesValidator;

        private readonly StateSettingsValidator _stateSettingsValidator;

        public Floo(string machineName)
        {
            _machineName = machineName;

            _rulesValidator = new RulesValidator(machineName);

            _stateSettingsValidator = new StateSettingsValidator(machineName);

        }

        public Floo()
        {
            _rulesValidator = new RulesValidator(string.Empty);

            _stateSettingsValidator = new StateSettingsValidator(string.Empty);
        }

        #region Fluent

        public IFrom AddTransition()
        {
            return AddTransition(string.Empty);
        }

        public IFrom AddTransition(string rulename)
        {
            var rule = new Rule(rulename);

            _rulesValidator.AddRule(rule);

            return rule;
        }

        public IStateSetting AddStateSetting(string state)
        {
            var stateSetting = new StateSetting(state);

            _stateSettingsValidator.AddSetting(stateSetting);

            return stateSetting;
        }

        #endregion

        public void Check()
        {
            _rulesValidator.Check();

            _stateSettingsValidator.Check(_rulesValidator.FromStates, _rulesValidator.ToStates());
        }

        public bool IsValid()
        {
            try
            {
                Check();

                return true;
            }
            catch (FloomeenException)
            {
                return false;
            }
        }
        
        public bool StatesContains(string state)
        {
            return _rulesValidator.IsExistingState(state);
        }

        public List<string> AvailableCommands(string state)
        {
            return _rulesValidator.AvailableCommands(state);
        }

        public string StartState()
        {
            return _stateSettingsValidator.StartState();
        } 

        public Rule RetrieveApplicableRule(string state, string command)
        {
            return _rulesValidator.RetrieveApplicableRule(state, command);
        }

        public StateSetting RetrieveSetting(string state)
        {
            return _stateSettingsValidator.RetrieveSetting(state);
        }

    }
}
