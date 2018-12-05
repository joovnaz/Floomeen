using System;
using Floomeen.Adapters.MessageSender;
using Floomeen.Meen;

namespace Floomeen.Showroom
{

    public class MessagingFloomeen : MeenBase
    {
        public struct State
        {
            public const string Ready = "Ready";
            public const string Retrying = "Retrying";
            public const string Sent = "Sent";
            public const string Error = "Error";
        }

        public struct Command
        {
            public const string Send = "Send";
        }

        public struct ContextKey
        {
            public const string Message = "Message";
            public const string MaxRetries = "MaxRetries";
            public const string Retries = "Retries";
        }

        // Adapter
        public static readonly int DefaultMaxRetries = 3;

        public static readonly string AdapterAcceptedType = SupportedTypes.Email;

        public MessagingFloomeen()
        {
            Flow.AddSetting(State.Ready)
                .IsStartState()
                .OnEnterEvent(ManageOnEnterEvent)
                .OnExitEvent(ManageOnExitEvent);

            Flow.AddSetting(State.Retrying)
                .IsStartState()
                .OnEnterEvent(ManageOnEnterEvent)
                .OnExitEvent(ManageOnExitEvent);
            
            Flow.AddSetting(State.Error)
                .IsEndState();

            Flow.AddSetting(State.Sent)
                .IsEndState();

            Flow.AddTransition("SendingFlow")
                .From(State.Ready)
                .On(Command.Send)
                .Do(SendEmail)
                    .When(Success)
                        .GoTo(State.Sent)
                    .Otherwise()
                        .GoTo(State.Retrying);

            Flow.AddTransition("RetryingFlow")
                .From(State.Retrying)
                .On(Command.Send)
                .Do(SendEmail)
                    .When(Success)
                        .GoTo(State.Sent)
                    .When(BelowMaxRetries)
                        .StayAt(State.Retrying)
                    .Otherwise()
                        .GoTo(State.Error);

        }



        public Result SendEmail(Context context)
        {
            var messageAdapter = SelectAdapter<IMessageSenderAdapter>(AdapterAcceptedType);

            var message = context.Data.Get<FlooMessage>(ContextKey.Message);
            
            return messageAdapter.Send(message);
        }

        private FlooInt IncrementRetries(Context context)
        {
            var retries = context.StateData.Get(ContextKey.Retries, new FlooInt(0)) + 1;

            context.StateData.Save(ContextKey.Retries, retries);

            return retries;
        }

        public bool Success(Result result,Context context)
        {
            return result.Success;
        }

        public bool BelowMaxRetries(Result result, Context context)
        {

            var MaxRetries = context.Data.Get<FlooInt>(ContextKey.MaxRetries, new FlooInt(DefaultMaxRetries));

            var retries = IncrementRetries(context);

            if (retries < MaxRetries.Value) return true;

            context.StateData.Save("MyData", "Hello!");

            return false;
        }

        public void ManageOnEnterEvent(Context context)
        {
            Console.WriteLine($"Entered [State={context.State}]");
        }

        public void ManageOnExitEvent(Context context)
        {
            Console.WriteLine($"Exited [State={context.State}]");
        }
    }
}
