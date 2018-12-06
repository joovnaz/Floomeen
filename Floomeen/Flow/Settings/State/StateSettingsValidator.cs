using System.Collections.Generic;
using System.Linq;
using Floomeen.Exceptions;

namespace Floomeen.Flow.Settings.State
{
    public class StateSettingsValidator
    {
        private readonly string _machineName;

        private List<StateSetting> StateSettings { get; }


        public StateSettingsValidator(string machineName)
        {
            _machineName = machineName;

            StateSettings = new List<StateSetting>();
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

        private void CheckSetting(StateSetting stateSetting, List<string> fromStates, List<string> toStates)
        {
            if (string.IsNullOrEmpty(stateSetting.State))

                RaiseException("InvalidState");

            if (stateSetting.IsStart && !fromStates.Contains(stateSetting.State))

                RaiseException("InvalidSettingStartStateNotPresentInAnyRule");

            if (stateSetting.IsEnd && !toStates.Contains(stateSetting.State))

                RaiseException("InvalidSettingEndStateNotPresentInAnyRule");
        }

        public void Add(StateSetting stateSetting)
        {
            StateSettings.Add(stateSetting);
        }

        public StateSetting RetrieveSetting(string state)
        {
            return StateSettings.FirstOrDefault(r => r.State == state);
        }

        private void RaiseException(string message)
        {
            throw new FloomeenException($"[{_machineName}] {message}");
        }

    }
}
