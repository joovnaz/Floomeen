using Floomeen.Meen;

namespace Floomeen.Adapters.MessageSender
{
    public interface IMessageSenderAdapter : IAdapter
    {
        Result Send(FlooMessage message);
    }
}
