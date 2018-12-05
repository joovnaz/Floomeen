using System;
using Floomeen.Meen;

namespace Floomeen.Flow.FluentApi
{
    public interface IOn
    {
        IDo On(string command);

        IOn OnExit(Action<Context> onExit);

        IOn OnEnter(Action<Context> onEnter);
    }

}
