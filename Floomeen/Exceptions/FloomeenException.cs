using System;

namespace Floomeen.Exceptions
{
    public class FloomeenException : Exception
    {
        public FloomeenException(string message) : base(message)
        {
            
        }

        public static void Raise(string machineTypename, string message)
        {
            throw new FloomeenException($"[{machineTypename}] {message}");
        }
    }
}
