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

            //var machine = Factory.Create("Floomine.Showroom.MessagingFloomine");
            var machine = new MessagingFloomine();

            machine.InjectAdapter<SendgridAdapter>();

            machine.AddContextData(MessagingFloomine.ContextKey.Message, message);

            //machine.AddContextData(MessagingFloomine.ContextKey.MaxRetries, new FlooInt(10));

            const string send = MessagingFloomine.Command.Send;

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

        private static void PrintAvailableCommands(MessagingFloomine machine)
        {
            System.Console.WriteLine($"AvailableCommands '{string.Join(',', machine.AvailableCommands())}'");

        }

    }
}
