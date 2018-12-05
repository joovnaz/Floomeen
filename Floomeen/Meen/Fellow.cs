using System;
using System.Collections.Generic;
using System.Text;
using Floomeen.Attributes;
using Floomeen.Exceptions;
using Floomeen.Meen.Interfaces;
using Floomeen.Utils;

namespace Floomeen.Meen
{
    public class Fellow
    {
        private readonly IFellow _fellow;

        private readonly string _machineTypename;

        public ContextInfo ContextStateData = new ContextInfo();

        public bool SupportStateData = false;

        public bool SupportMachine = false;

        public bool SupportChangedOn = false;

        public Fellow(IFellow fellow, string machineTypename)
        {
            _fellow = fellow;

            _machineTypename = machineTypename;

            var stateData = _fellow.StateData();

            if (stateData != null)
                ContextStateData = ContextInfo.Deserialize(stateData);

            CheckAttributes();
        }

        public void Plug(string machineType, string state)
        {
            _fellow.SetPropValueByAttribute<FloomeenState>(state);

            if (SupportMachine)
                _fellow.SetPropValueByAttribute<FloomeenMachine>(machineType);
            
            if (SupportChangedOn)
                _fellow.SetPropValueByAttribute<FloomeenChangedOn>(DateTime.UtcNow);
        }

        #region Pseudo-Properties: Id, State, StateData, Machine, ChangedOn

        public object Id
        {
            get => _fellow.Id();

            set => _fellow.Id(value);
        }

        public string State
        {
            get => _fellow.State();

            set
            {
                _fellow.State(value);

                if (SupportChangedOn)
                    _fellow.ChangedOn(DateTime.UtcNow);
            } 
        }

        public string StateData
        {
            get => _fellow.StateData();

            set => _fellow.StateData(value);
        }

        public string Machine
        {
            get => _fellow.Machine();

            set => _fellow.Machine(value);
        }
        
        public DateTime ChangedOn
        {
            get => _fellow.ChangedOn();

            set => _fellow.ChangedOn(value);
        }

        #endregion

        public void SerializeContextStateData()
        {
            if (ContextStateData.IsEmpty()) return;

            _fellow.StateData(ContextStateData.Serialize());
        }

        public void CheckAttributes()
        {
            // Id (mandatory)
            var idPropName = _fellow.GetPropNameByAttribute<FloomeenId>();

            if (string.IsNullOrEmpty(idPropName))
                RaiseException($"MissingMandatoryAttribute(Id)");

            var id = _fellow.Id();

            if (id == null || string.IsNullOrEmpty(id.ToString()))
                RaiseException("FellowIdPropertyIsNullOrEmpty");

            // State (mandatory)
            var statePropName = _fellow.GetPropNameByAttribute<FloomeenState>();

            if (string.IsNullOrEmpty(statePropName))
                RaiseException($"MissingMandatoryAttribute(State)");

            var statePropType = _fellow.GetPropTypeByAttribute<FloomeenState>();
            
            if (!IsString(statePropType))
                RaiseException($"FellowStatePropertyMustBeAString");
            
            // Machine (opt)
            var machinePropName = _fellow.GetPropNameByAttribute<FloomeenMachine>();

            SupportMachine = !string.IsNullOrEmpty(machinePropName);

            if (SupportMachine)
            {
                var machinePropType = _fellow.GetPropTypeByAttribute<FloomeenMachine>();

                if (!IsString(machinePropType))

                    RaiseException($"FellowMachinePropertyMustBeAString");
            }

            // StateData (opt)
            var stateDataPropName = _fellow.GetPropNameByAttribute<FloomeenStateData>();

            SupportStateData = !string.IsNullOrEmpty(stateDataPropName);

            if (SupportStateData)
            {
                var stateDataPropType = _fellow.GetPropTypeByAttribute<FloomeenStateData>();

                if (!IsString(stateDataPropType))
                    RaiseException($"FellowStateDataPropertyMustBeAString");
            }

            // ChangedOn (opt)
            var changedOnPropName = _fellow.GetPropNameByAttribute<FloomeenChangedOn>();

            SupportChangedOn = !string.IsNullOrEmpty(changedOnPropName);

            if (SupportChangedOn)
            {
                var changedOnPropType = _fellow.GetPropTypeByAttribute<FloomeenChangedOn>();

                if (!IsDateTime(changedOnPropType))
                    RaiseException($"FellowChangedOnPropertyMustBeADateTime");

            }
        }

        private void RaiseException(string message)
        {
            FloomeenException.Raise(_machineTypename, message);
        }

        private static bool IsDateTime(Type obj)
        {
            return obj == typeof(DateTime);
        }

        private static bool IsString(Type obj)
        {
            return obj == typeof(string);
        }

        public bool IsSet()
        {
            var state = _fellow.State();

            var machine = _fellow.Machine();

            return IsSet(state) || IsSet(machine);

        }

        private bool IsSet(string value)
        {
            return !string.IsNullOrEmpty(value);
        }

        public void CheckMachineType()
        {
            var fellowType = _fellow.Machine();

            if (fellowType != _machineTypename)

                FloomeenException.Raise(_machineTypename, $"WrongFellowMachineType[{fellowType}]");
        }

    }
}
