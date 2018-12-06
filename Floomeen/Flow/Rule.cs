using System;
using System.Collections.Generic;
using Floomeen.Flow.Fluent;
using Floomeen.Meen;

namespace Floomeen.Flow
{
    public class ConditionElement
    {
        public Func<Result, Context, bool> ConditionFunc { get; set; }

        public string EndState { get; set; }
    }

    public class Rule : IFrom, IOn, IDo, IConditional, IIfGoto, IGoTo
    {
        public string Rulename { get; private set; }

        public string FromState { get; private set; }

        //public Action<Context> OnEnterAction { get; private set; }

        //public Action<Context> OnExitAction { get; private set; }

        public string OnCommand { get; private set; }

        public Func<Context, Result> DoFunc { get; private set; }
        
        public bool IsShort => DoFunc == null;

        public Stack<ConditionElement> ConditionElements { get; private set; }

        public string ToState { get; private set; }

        public Rule(string rulename)
        {
            Rulename = rulename;
        }

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

        //public IOn OnExit(Action<Context> onExit)
        //{
        //    if (OnExitAction != null)
        //        throw new FloomeenException($"RepeatedOnExitDeclaration {Rulename}");

        //    OnExitAction = onExit;

        //    return this;
        //}

        //public IOn OnEnter(Action<Context> onEnter)
        //{
        //    if (OnEnterAction != null)
        //        throw new FloomeenException($"RepeatedOnEnterDeclaration {Rulename}");

        //    OnEnterAction = onEnter;

        //    return this;
        //}

        public IConditional Do(Func<Context, Result> action)
        {
            DoFunc = action;

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
            var condition = ConditionElements.Peek();

            condition.EndState = state;
        }

        IConditional IIfGoto.ReturnTo(string state)
        {
            UpdateLastConditionEndState(state);

            return this;
        }

        IConditional IIfGoto.StayAt(string state)
        {
            UpdateLastConditionEndState(state);

            return this;
        }
        
        IConditional IIfGoto.GoTo(string state)
        {
            UpdateLastConditionEndState(state);

            return this;
        }

        public IIfGoto When(Func<Result, Context, bool> condition)
        {
            if (ConditionElements == null) ConditionElements = new Stack<ConditionElement>();

            ConditionElements.Push(new ConditionElement
            {
                ConditionFunc = condition
            });

            return this;
        }

        public IGoTo Otherwise()
        {
            if (ConditionElements == null) ConditionElements = new Stack<ConditionElement>();

            ConditionElements.Push(new ConditionElement());

            return this;
        }
    }
}
