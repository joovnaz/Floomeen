using System.Collections.Generic;
using System.Linq;
using Floomeen.Exceptions;

namespace Floomeen.Flow.Settings.State
{
    public class StateSettingsValidator
    {
        private readonly string _machineName;

        private List<StateSettingSetting> StateSettings { get; }


        public StateSettingsValidator(string machineName)
        {
            _machineName = machineName;

            StateSettings = new List<StateSettingSetting>();
        }

        public string StartState()
        {
            var startState = StateSettings?.FirstOrDefault(s => s.IsStart);

            return startState?.State;
        }
        
        public void Check(List<string> fromStates, List<string> toStates)
        {
            if (StateSettings == null)

                RaiseException("UndefinedStateSetting");

            StateSettings.ForEach(ss => CheckSetting(ss, fromStates, toStates));

            if (StartState() == null)

                RaiseException("UndefinedStartState");
        }

        private void CheckSetting(StateSettingSetting stateSettingSetting, List<string> fromStates, List<string> toStates)
        {
            if (string.IsNullOrEmpty(stateSettingSetting.State))

                RaiseException("InvalidState");

            if (stateSettingSetting.IsStart && !fromStates.Contains(stateSettingSetting.State))

                RaiseException("InvalidSettingStartStateNotPresentInAnyRule");

            if (stateSettingSetting.IsEnd && !toStates.Contains(stateSettingSetting.State))

                RaiseException("InvalidSettingEndStateNotPresentInAnyRule");
        }

        public void Add(StateSettingSetting stateSettingSetting)
        {
            StateSettings.Add(stateSettingSetting);
        }

        public StateSettingSetting RetrieveSetting(string state)
        {
            return StateSettings.FirstOrDefault(r => r.State == state);
        }

        private void RaiseException(string message)
        {
            throw new FloomeenException($"[{_machineName}] {message}");
        }

    }
}
