using Floomeen.Adapters.MessageSender;
using Floomeen.Meen;

namespace Showroom.Adapters
{
    public class EmailAdapter : IMessageSenderAdapter
    {
        public string[] AcceptedTypes()
        {
            return new []
            {
                SupportedTypes.Email
            };
        }

        public Result Send(FlooMessage message)
        {
            System.Console.WriteLine($"Sending Message ({message.Type}) =======");

            System.Console.WriteLine($"_");

            System.Console.WriteLine($"{MessageToString(message)}");

            System.Console.WriteLine($"_");

            System.Console.WriteLine($"==================================");

            return new Result
            {
                Success = false
            };
        }

        public string MessageToString(FlooMessage message)
        {
            return $"From: {message.FromAlias} [{message.From}]\r\n" +
                   $"To: {message.ToAlias} [{message.To}]\r\n" +
                   $"Content:-----------------------\r\n" +
                   $"{message.Content}\r\n" +
                   $"--------------------------------\r\n";
        }
    }
}
