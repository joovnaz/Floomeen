using System;
using System.Collections.Generic;
using System.Text;
using Floomeen.Attributes;
using Floomeen.Meen.Interfaces;
using Floomeen.Utils;

namespace Floomeen.Meen
{
    public class Fellow
    {
        private readonly IFellow _fellow;

        public ContextInfo TempStateData = new ContextInfo();

        public Fellow(IFellow fellow)
        {
            _fellow = fellow;

            var stateData = _fellow.StateData();

            if (stateData != null)
                TempStateData = ContextInfo.Deserialize(stateData);
        }

        public void Initialize(string machineType, string state)
        {
            _fellow.SetPropValueByAttribute<FloomeenMachine>(machineType);

            _fellow.SetPropValueByAttribute<FloomeenState>(state);

            _fellow.SetPropValueByAttribute<FloomeenChangedOn>(DateTime.UtcNow);
        }

        public object Id()
        {
            return _fellow.Id();
        }

        public string StateData()
        {
            return _fellow.StateData();
        }

        public void StateData(string stateData)
        {
            _fellow.StateData(stateData);
        }

        public void State(string state)
        {
            _fellow.State(state);
        }

        public string State()
        {
            return _fellow.State();
        }

        public void Machine(string machine)
        {
            _fellow.Machine(machine);
        }

        public string Machine()
        {
            return _fellow.Machine();
        }

        public void ChangedOn(DateTime changedOn)
        {
            _fellow.ChangedOn(changedOn);
        }

        public DateTime ChangedOn()
        {
            return _fellow.ChangedOn();
        }

        public void SerializeTempStateData()
        {
            _fellow.StateData(TempStateData.Serialize());
        }
    }
}
