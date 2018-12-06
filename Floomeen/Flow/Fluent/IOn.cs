namespace Floomeen.Flow.Fluent
{
    public interface IOn
    {
        IDo On(string command);

        //IOn OnExit(Action<Context> onExit);

        //IOn OnEnter(Action<Context> onEnter);
    }

}
