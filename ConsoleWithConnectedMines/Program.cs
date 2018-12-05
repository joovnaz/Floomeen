using System;
using System.Collections.Generic;
using System.Linq;
using Floomeen.Adapters.MessageSender;
using Floomeen.Meen;
using Floomeen.Showroom;

namespace ConsoleWithConnectedMines
{
    class Program
    {
        public static string MessageTemplate = "Dear {0}, <br/> please get in contact with us by clicking on <a href=\"{1}\">this link</a>";

        public static List<POCO> messages = new List<POCO>
        {
            new POCO {Id = "1", Name = "P1", Email = "p1@p1.it"},
            new POCO {Id = "2", Name = "P2", Email = "p2@p2.it"},
            new POCO {Id = "3", Name = "P3", Email = "p3@p3.it"},
        };

        public static List<POCO> listeners = new List<POCO>
        {
            new POCO {Id = "1-1"},
            new POCO {Id = "2-1"},
            new POCO {Id = "3-1"},
        };

        static void Main(string[] args)
        {
            foreach (var message in messages)
            {
                BuildMessaging(out MessagingFloomine master, message);

                var listener = listeners.First(l => l.Id == message.Id + "-1");

                BuildListener(out ListenerFloomine slave, listener);

                var coordinator = new MessagingListenerFloominesCoordinator(master, slave);

                // ----------------------------------------------------------------

                Console.WriteLine($"Before {listener.Id} {slave.GetState()}");

                master.Execute(MessagingFloomine.Command.Send);

                master.Unbind();

                Console.WriteLine($"After {listener.Id} {slave.GetState()}");

            }

             Console.ReadKey();

        }

        static void BuildListener(out ListenerFloomine listener, POCO poco)
        {
            listener = new ListenerFloomine();

            listener.Plug(poco);
        }

        static void BuildMessaging(out MessagingFloomine floomine, POCO poco)
        {
            var messageToSend = new FlooMessage
            {
                To = poco.Email,

                Content = string.Format(MessageTemplate, poco.Name, poco.Url)
            };

            var maxretries = new FlooInt(3);

            //var machine = Factory.Create("Floomine.Showroom.MessagingFloomine");
            floomine = new MessagingFloomine();

            floomine.InjectAdapter<SendgridAdapter>();

            floomine.AddContextData(MessagingFloomine.ContextKey.Message, messageToSend);

            floomine.AddContextData(MessagingFloomine.ContextKey.MaxRetries, maxretries);

            floomine.Plug(poco);
        }
    }
}
