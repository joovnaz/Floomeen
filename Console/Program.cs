using Floomeen.Adapters.MessageSender;
using Floomeen.Showroom;

namespace Console
{
    class Program
    {

        public static string MessageTemplate = "Dear {0}, <br/> please get in contact with us by clicking on <a href=\"{1}\">this link</a>";
        
        static void Main(string[] args)
        {
            var poco = new POCO
            {
                Id = "someId",
                Name = "Pietro",
                Email = "p@p.it",
                Url = "https://www.google.com"
            };

            var message = new FlooMessage
            {
                To = poco.Email,

                Content = string.Format(MessageTemplate, poco.Name, poco.Url)
            };

            //var machine = Factory.Create("Floomeen.Showroom.MessagingFloomeen");
            var machine = new MessagingFloomeen();

            machine.InjectAdapter<SendgridAdapter>();

            machine.AddContextData(MessagingFloomeen.ContextKey.Message, message);

            //machine.AddContextData(MessagingFloomeen.ContextKey.MaxRetries, new FlooInt(10));

            const string send = MessagingFloomeen.Command.Send;

            machine.Plug(poco);

            PrintAvailableCommands(machine);
            machine.Execute(send);
            machine.Unbind();

            //
            machine.Bind(poco);
            machine.Execute(send);
            machine.Unbind();

            //
            machine.Bind(poco);
            machine.Execute(send);
            machine.Unbind();

            //
            machine.Bind(poco);
            machine.Execute(send);
            machine.Unbind();

            machine.Bind(poco);
            PrintAvailableCommands(machine);
            machine.Execute(send);
            machine.Unbind();

            System.Console.WriteLine("Done");
            System.Console.ReadKey();
        }

        private static void PrintAvailableCommands(MessagingFloomeen machine)
        {
            System.Console.WriteLine($"AvailableCommands '{string.Join(',', machine.AvailableCommands())}'");

        }

    }
}
