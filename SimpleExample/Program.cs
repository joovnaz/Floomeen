using Floomeen.Adapters.MessageSender;
using Floomeen.Meen;
using Showroom;

namespace SimpleExample
{
    class Program
    {
        public const string Send = MessagingFloomeen.Command.Send;

        public static string MessageTemplate = "Dear {0}, <br/> please get in contact with us by clicking on <a href=\"{1}\">this link</a>";
        
        static void Main(string[] args)
        {
            var poco = CreatPOCO();

            var message = CreateMessage(poco);

            var machine = Factory.Create<MessagingFloomeen>("Showroom.MessagingFloomeen");

            //var machine = new MessagingFloomeen();

            machine.InjectAdapter<EmailAdapter>();

            machine.AddContextData(MessagingFloomeen.ContextKey.Message, message);

            //machine.AddContextData(MessagingFloomeen.ContextKey.MaxRetries, new FlooInt(10));

            machine.Plug(poco);

            System.Console.WriteLine($"Current State is '{machine.CurrentState}'");

            PrintAvailableCommands(machine);
            
            machine.Execute(Send);

            machine.Unbind();

            const int attempts = 3;

            // Retries
            for (var i = 0; i < attempts; i++)
            {
                machine.Bind(poco);

                System.Console.WriteLine($"Current State is '{machine.CurrentState}'");

                PrintAvailableCommands(machine);

                machine.Execute(Send);

                machine.Unbind();
            }

            System.Console.WriteLine($"After {attempts} attempts, final State is '{poco.State}'");

            System.Console.ReadKey();
        }

        private static void PrintAvailableCommands(MessagingFloomeen machine)
        {
            System.Console.WriteLine($"AvailableCommands '{string.Join(',', machine.AvailableCommands())}'");
        }

        public static POCO CreatPOCO()
        {
            return new POCO
            {
                Id = "someId",

                Name = "Myname",

                Email = "hello@hello.it",

                Url = "https://www.hello.com"
            };
        }

        public static FlooMessage CreateMessage(POCO poco)
        {
            return new FlooMessage
            {
                To = poco.Email,

                Content = string.Format(MessageTemplate, poco.Name, poco.Url),

                Type = SupportedTypes.Email
            };
        }

    }
}
