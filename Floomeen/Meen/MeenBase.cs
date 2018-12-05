using System;
using System.Collections.Generic;
using System.Linq;
using Floomeen.Adapters;
using Floomeen.Attributes;
using Floomeen.Exceptions;
using Floomeen.Flow;
using Floomeen.Meen.Events;
using Floomeen.Meen.Interfaces;
using Floomeen.Utils;
using PubSub.Extension;

namespace Floomeen.Meen
{
    public class MeenBase
    {
        private readonly string _typename;

        public Floo Flow { get; }

        public Fellow BoundFellow { get; private set; }

        public bool IsBound => BoundFellow != null;

        protected List<IAdapter> _adapters = new List<IAdapter>();

        private readonly ContextInfo _contextData = new ContextInfo();

        private string _executingCommand;

        protected MeenBase(string typename)
        {
            _typename = string.IsNullOrEmpty(typename) ? GetType().FullName : typename;

            Flow = new Floo(_typename);
        }

        protected MeenBase() : this(string.Empty)
        {

        }

        #region Configuration

        public void InjectAdapter(string typeName)
        {
            var adapter = Factory.GetInstance(typeName);

            if (adapter is IAdapter castedAdapter)
            {
                _adapters.Add(castedAdapter);

            }
            else throw new FloomeenException("InvalidAdapterType.IAdapterExpected");
        }

        public void InjectAdapter<T>() where T : IAdapter
        {
            var adapter = Factory.GetInstance<T>();

            _adapters.Add(adapter);
        }

        protected TAdaper SelectAdapter<TAdaper>(string acceptedType) where TAdaper : IAdapter
        {
            return (TAdaper)_adapters.FirstOrDefault(a => a.AcceptedTypes().Contains(acceptedType));

        }

        public void AddContextData<T>(string key, T data)
        {
            _contextData.Add(key, data);
        }
        
        #endregion

        #region Validation

        public void CheckIfWorkflowIsValid()
        {
            Flow.CheckValidity();
        }

        public bool HasValidWorklow()
        {
            return Flow.IsValid();
        }

        #endregion

        #region Fellowship

        public void Plug(IFellow fellow)
        {
            DoBindingFellowPreliminaryChecks(fellow);

            BoundFellow = new Fellow(fellow);
            
            BoundFellow.Initialize(_typename, Flow.StartState);
        }

        private void CheckIfBindingFellowIdIsNotNullOrEmpty(IFellow fellow)
        {
            var id = fellow.Id();

            if (id == null || string.IsNullOrEmpty(id.ToString()))
                RaiseException("BindingFellowIdCannotBeNullOrEmpty");
        }

        public void Unbind()
        {
            BoundFellow = null;
        }

        private void DoBindingFellowPreliminaryChecks(IFellow fellow, bool isBinding = false)
        {
            CheckIfBound();

            CheckIfWorkflowIsValid();

            if (isBinding)
                CheckIfBindingFellowIsAligned(fellow);
            else
                CheckIfPluggingFellowIsSet(fellow);

            CheckIfBindingFellowHasMandatoryAttributes(fellow);

            CheckIfBindingFellowIdIsNotNullOrEmpty(fellow);
        }

        public void Bind(IFellow fellow)
        {
            DoBindingFellowPreliminaryChecks(fellow, true);

            BoundFellow = new Fellow(fellow);
        }

        private void CheckIfBindingFellowHasMandatoryAttributes(IFellow fellow)
        {
            var idPropName = fellow.GetPropNameByAttribute<FloomeenId>();

            if (string.IsNullOrEmpty(idPropName))
                RaiseException($"MissingDefinitionOfIdAttribute");

            var statePropName = fellow.GetPropNameByAttribute<FloomeenState>();

            if (string.IsNullOrEmpty(statePropName))
                RaiseException($"MissingDefinitionOfStateAttribute");

            var statePropType = fellow.GetPropTypeByAttribute<FloomeenState>();

            if (!IsString(statePropType))
                RaiseException($"FellowStatePropertyMustBeAString");

            var machinePropName = fellow.GetPropNameByAttribute<FloomeenMachine>();

            if (string.IsNullOrEmpty(machinePropName))
                RaiseException($"MissingDefinitionOfMachineAttribute");

            var machinePropType = fellow.GetPropTypeByAttribute<FloomeenMachine>();

            if (!IsString(machinePropType))
                RaiseException($"FellowMachinePropertyMustBeAString");

            var stateDataPropName = fellow.GetPropNameByAttribute<FloomeenStateData>();

            if (string.IsNullOrEmpty(stateDataPropName))
                RaiseException($"MissingDefinitionOfStateDataAttribute");

            var stateDataPropType = fellow.GetPropTypeByAttribute<FloomeenStateData>();

            if (!IsString(stateDataPropType))
                RaiseException($"FellowStateDataPropertyMustBeAString");

            var changedOnPropName = fellow.GetPropNameByAttribute<FloomeenChangedOn>();

            if (string.IsNullOrEmpty(changedOnPropName))
                RaiseException($"MissingDefinitionOfChangedOnAttribute");

            var changedOnPropType = fellow.GetPropTypeByAttribute<FloomeenChangedOn>();

            if (!IsDateTime(changedOnPropType))
                RaiseException($"FellowChangedOnPropertyMustBeADateTime");
        }

        private static bool IsDateTime(Type obj)
        {
            return obj == typeof(DateTime);
        }

        private static bool IsString(Type obj)
        {
            return obj == typeof(string);
        }

        public object CheckIfNotBoundAndGetId()
        {
            CheckIfNotBound();

            return BoundFellow.Id();
        }

        public string[] AvailableCommands()
        {
            CheckIfNotBound();

            var state = BoundFellow.State();

            return Flow.AvailableCommands(state).ToArray();
        }

        private void CheckIfPluggingFellowIsSet(IFellow fellow)
        {
            var state = fellow.State();

            var machine = fellow.Machine();

            if (IsSet(state) || IsSet(machine))

                RaiseException("CannotBindBecauseFellowIsSet");
        }

        private bool IsSet(string value)
        {
            return !string.IsNullOrEmpty(value);
        }


        private void CheckIfBound()
        {
            if (IsBound)
                RaiseException("AlreadyBind");
        }

        private void CheckIfNotBound()
        {
            if (!IsBound)
                RaiseException("NotBound");
        }

        private void CheckIfBindingFellowIsAligned(IFellow fellow)
        {
            CheckBindingFellowMachineType(fellow);

            CheckBindingFellowState(fellow);
        }

        private void CheckBindingFellowMachineType(IFellow fellow)
        {
            var fellowType = fellow.Machine();

            if (fellowType != GetType().FullName)

                RaiseException($"WrongFellowMachineType[{fellowType}]");
        }

        private void CheckBindingFellowState(IFellow fellow)
        {
            var fellowState = fellow.State();

            if (Flow.FromStatesList.Contains(fellowState)) return;

            if (Flow.ToStatesList.Contains(fellowState)) return;

            RaiseException($"WrongFellowState[{fellowState}]");
        }

        #endregion

        #region State


        private Context CreateContext()
        {
            var currentState = GetState();

            var context = new Context
            {
                State = currentState,

                StateData = BoundFellow.TempStateData,

                Command = _executingCommand,

                Fellow = BoundFellow,

                Data = _contextData
            };

            return context;
        }

        public void Execute(string command)
        {
            _executingCommand = command;

            var fromState = GetState();

            var rule = Flow.RetrieveApplicableRule(fromState, _executingCommand);

            if (rule.IsShort)
            {
                SwapState(fromState, rule.ToState);
            }
            else
            {
                ProcessRuleThenSwapState(fromState, rule);
            }
        }

        private void ProcessRuleThenSwapState(string fromState, Rule rule)
        {
            var context = CreateContext();

            var result = rule.DoFunc.Invoke(context);

            var conditions = rule.ConditionElements.Reverse();

            foreach (var condition in conditions)
            {
                var conditionFunc = condition.ConditionFunc;

                if (conditionFunc == null || condition.ConditionFunc.Invoke(result, context))
                {
                    SwapState(fromState, condition.EndState);

                    break;
                }
            }
        }

        private void SwapState(string fromState, string toState)
        {
            BoundFellow.SerializeTempStateData();

            BoundFellow.ChangedOn(DateTime.UtcNow);

            BoundFellow.State(toState);

            if (toState == fromState) return;
            
            ExitCurrentState(fromState, CreateContext());
            
            EnterNewState(toState, CreateContext());

            RaiseEvent(new ChangedStateEvent(this, BoundFellow.Id(), fromState, toState));
        }

        private void ExitCurrentState(string state, Context context)
        {
            RetrieveSetting(state)?.OnExitEventHandler?.Invoke(context);

            RaiseEvent(new ExitedStateEvent(this, BoundFellow.Id(), state));
        }

        private void RaiseEvent<TEvent>(TEvent @event) where TEvent : MeenEventBase
        {
            this.Publish(@event);
        }

        private void EnterNewState(string state, Context context)
        {
            RetrieveSetting(state)?.OnEnterEventHandler?.Invoke(context);

            RaiseEvent(new EnteredStateEvent(this, BoundFellow.Id(), state));
        }

        private Setting RetrieveSetting(string state)
        {
            return Flow.RetrieveSetting(state);
        }

        public string GetState()
        {
            CheckIfNotBound();

            var state = BoundFellow.State();

            return state;
        }

        public string GetStateData()
        {
            CheckIfNotBound();

            var stateData = BoundFellow.StateData();

            return stateData;

        }

        public string GetMachine()
        {
            CheckIfNotBound();

            var machine = BoundFellow.Machine();

            return machine;
        }

        public DateTime GetChangeOn()
        {
            CheckIfNotBound();

            var changedOn = BoundFellow.ChangedOn();

            return changedOn;
        }

        #endregion

        private void RaiseException(string message)
        {
            throw new FloomeenException($"[{_typename}] {message}");
        }

    }
}
