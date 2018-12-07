using System.Collections.Generic;
using System.Linq;
using Floomeen.Adapters;
using Floomeen.Exceptions;
using Floomeen.Flow;
using Floomeen.Flow.Settings.Rules;
using Floomeen.Meen.Events;
using Floomeen.Meen.Interfaces;
using Floomeen.Utils;
using PubSub.Extension;

namespace Floomeen.Meen
{
    public class MeenBase
    {
        private readonly string _machineName;

        protected List<IAdapter> Adapters = new List<IAdapter>();

        private readonly ContextInfo _contextData = new ContextInfo();

        private string _executingCommand;

        public Floo Flow { get; }

        public Fellow BoundFellow { get; private set; }

        public bool IsBound => BoundFellow != null;

        public string CurrentState => BoundFellow?.State;

        private string _previousState;

        private bool _isChecked = false;

        protected MeenBase(string typename)
        {
            _machineName = string.IsNullOrEmpty(typename) ? GetType().FullName : typename;

            Flow = new Floo(_machineName);
        }

        protected MeenBase() : this(string.Empty)
        {

        }

        #region Adapters

        public void InjectAdapter(string typeName)
        {
            var adapter = FactoryExtensions.GetInstance(typeName);

            if (adapter is IAdapter castedAdapter)
            {
                Adapters.Add(castedAdapter);

            }

            throw new FloomeenException("InvalidAdapterType.IAdapterExpected");
        }

        public void InjectAdapter<T>() where T : IAdapter
        {
            var adapter = FactoryExtensions.GetInstance<T>();

            Adapters.Add(adapter);
        }

        protected TAdaper SelectAdapter<TAdaper>(string acceptedType) where TAdaper : IAdapter
        {
            return (TAdaper)Adapters.FirstOrDefault(a => a.AcceptedTypes().Contains(acceptedType));
        }

        #endregion

        #region Fellowship

        public void Plug(IFellow fellow)
        {
            if (IsBound)
                FloomeenException.Raise(_machineName, "AlreadyBound");

            BoundFellow = new Fellow(fellow, _machineName);

            if (BoundFellow.IsSet())
                FloomeenException.Raise(_machineName, "CannotPlugFellowBecauseAlreadySet.TryBinding");

            var startState = Flow.StartState();

            BoundFellow.Plug(_machineName, startState);

            EnterNewState(startState, CreateContext());
        }

        public void Bind(IFellow fellow)
        {
            if (IsBound)
                FloomeenException.Raise(_machineName, "AlreadyBound");

            BoundFellow = new Fellow(fellow, _machineName);

            if (!BoundFellow.IsSet())
                FloomeenException.Raise(_machineName, "CannotBindFellowBecauseNotSet.TryPlugging");

            BoundFellow.CheckMachineType();

            Flow.IsExistingState(BoundFellow.State);
        }

        public void Unbind()
        {
            BoundFellow = null;
        }

        public string[] AvailableCommands()
        {
            var state = BoundFellow.State;

            return Flow.AvailableCommands(state).ToArray();
        }

        #endregion

        #region Engine

        public void Execute(string command)
        {
            _executingCommand = command;

            var fromState = BoundFellow.State;

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

            var conditions = rule.Conditions.Reverse();

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
            BoundFellow.SerializeContextStateData();

            BoundFellow.State = toState;

            _previousState = fromState;

            if (toState == fromState) return;
            
            ExitCurrentState(fromState, CreateContext());
            
            EnterNewState(toState, CreateContext());

            RaiseEvent(new ChangedStateEvent(this, BoundFellow.Id, fromState, toState));
        }

        private void ExitCurrentState(string state, Context context)
        {
            Flow.RetrieveSetting(state)?.OnExitEventHandler?.Invoke(context);

            RaiseEvent(new ExitedStateEvent(this, BoundFellow.Id, state));
        }

        private void RaiseEvent<TEvent>(TEvent @event) where TEvent : MeenEventBase
        {
            this.Publish(@event);
        }

        private void EnterNewState(string state, Context context)
        {
            Flow.RetrieveSetting(state)?.OnEnterEventHandler?.Invoke(context);

            RaiseEvent(new EnteredStateEvent(this, BoundFellow.Id, state));
        }

        #endregion
        
        #region Context

        private Context CreateContext()
        {
            return new Context
            {
                Data = _contextData,

                State = BoundFellow.State,

                PreviousState = _previousState,

                Command = _executingCommand,

                Fellow = BoundFellow,

                StateData = BoundFellow.ContextStateData,
            };
        }

        public void AddContextData<T>(string key, T data)
        {
            _contextData.Add(key, data);
        }

        #endregion
    }
}
