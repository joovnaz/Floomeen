using System.Collections.Generic;
using Floomeen.Exceptions;
using Floomeen.Flow.Fluent;
using Floomeen.Flow.Settings.State;

namespace Floomeen.Flow
{
    public class Floo
    {
        private readonly string _machineName;

        private readonly RulesValidator _rulesValidator;

        private readonly StateSettingsValidator _stateSettingsValidator;

        public Floo(string machineName) : this()
        {
            _machineName = machineName;
        }

        public Floo()
        {
            _rulesValidator = new RulesValidator(_machineName);

            _stateSettingsValidator = new StateSettingsValidator(_machineName);
        }

        #region Fluent

        public IFrom AddTransition()
        {
            return AddTransition(string.Empty);
        }

        public IFrom AddTransition(string rulename)
        {
            var wfx = new Rule(rulename);

            _rulesValidator.Rules.Add(wfx);

            return wfx;
        }

        public IStateSetting AddStateSetting(string state)
        {
            var stateSetting = new StateSetting(state);

            _stateSettingsValidator.Add(stateSetting);

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
        
        public bool IsExistingState(string state)
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
