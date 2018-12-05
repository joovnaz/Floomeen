using Floomeen.Meen.Interfaces;

namespace Floomeen.Meen
{
    public class Context
    {
        public string State { get; set; }

        public ContextInfo StateData { get; set; }

        public string Command { get; set; }

        public Fellow Fellow { get; set; }

        public ContextInfo Data { get; set; }
    }
}
