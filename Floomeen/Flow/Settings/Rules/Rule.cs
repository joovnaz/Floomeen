using System;
using System.Collections.Generic;
using Floomeen.Flow.Fluent.Rules;
using Floomeen.Meen;

namespace Floomeen.Flow.Settings.Rules
{
    public class Rule : IFrom, IOn, IDo, IConditional, IWhenGoto, IGoTo
    {
        public string Rulename { get; }

        public string FromState { get; private set; }

        public string OnCommand { get; private set; }

        public string ToState { get; private set; }

        public Func<Context, Result> DoFunc { get; private set; }

        public bool IsShort => DoFunc == null;

        public Stack<Condition> Conditions { get; private set; }

        public Rule(string rulename)
        {
            Rulename = rulename;
        }

        #region Fluent

        public IOn From(string state)
        {
            FromState = state;

            return this;
        }

        public IDo On(string command)
        {
            OnCommand = command;

            return this;
        }

        public void GoTo(string state)
        {
            UpdateEndState(state);
        }

        public void StayAt(string state)
        {
            UpdateEndState(state);
        }

        public void ReturnTo(string state)
        {
            UpdateEndState(state);
        }

        public IConditional Do(Func<Context, Result> action)
        {
            DoFunc = action;

            return this;
        }

        public IWhenGoto When(Func<Result, Context, bool> condition)
        {
            if (Conditions == null) Conditions = new Stack<Condition>();

            Conditions.Push(new Condition
            {
                ConditionFunc = condition
            });

            return this;
        }

        public IGoTo Otherwise()
        {
            if (Conditions == null) Conditions = new Stack<Condition>();

            Conditions.Push(new Condition());

            return this;
        }

        IConditional IWhenGoto.ReturnTo(string state)
        {
            UpdateLastConditionEndState(state);

            return this;
        }

        IConditional IWhenGoto.StayAt(string state)
        {
            UpdateLastConditionEndState(state);

            return this;
        }

        IConditional IWhenGoto.GoTo(string state)
        {
            UpdateLastConditionEndState(state);

            return this;
        }

        // private

        private void UpdateEndState(string state)
        {
            if (DoFunc != null)
            {
                UpdateLastConditionEndState(state);
            }
            else
            {
                ToState = state;
            }
        }

        private void UpdateLastConditionEndState(string state)
        {
            var condition = Conditions.Peek();

            condition.EndState = state;
        }

        #endregion
    }
}
