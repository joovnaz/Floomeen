using System;
using System.Collections.Generic;
using System.Linq;
using Floomeen.Adapters.MessageSender;
using Floomeen.Meen;
using Showroom;

namespace ConsoleWithConnectedMines
{
    class Program
    {
        public static string MessageTemplate = "Dear {0}, <br/> please get in contact with us by clicking on <a href=\"{1}\">this link</a>";

        public static List<POCO> messages = new List<POCO>
        {
            new POCO {Id = "1", Name = "P1", Email = "p1@p1.me"},
            new POCO {Id = "2", Name = "P2", Email = "p2@p2.me"},
            new POCO {Id = "3", Name = "P3", Email = "p3@p3.me"},
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
                BuildMessaging(out MessagingFloomeen master, message);

                var listener = listeners.First(l => l.Id == message.Id + "-1");

                BuildListener(out ListenerFloomeen slave, listener);

                var coordinator = new MessagingListenerFloomeensCoordinator(master, slave);

                // ----------------------------------------------------------------

                Console.WriteLine($"Before {listener.Id} {slave.CurrentState}");

                master.Execute(MessagingFloomeen.Command.Send);

                master.Execute(MessagingFloomeen.Command.Send);

                master.Unbind();

                Console.WriteLine($"After {listener.Id} {slave.CurrentState}");

            }

             Console.ReadKey();

        }

        static void BuildListener(out ListenerFloomeen listener, POCO poco)
        {
            listener = new ListenerFloomeen();

            listener.Plug(poco);
        }

        static void BuildMessaging(out MessagingFloomeen Floomeen, POCO poco)
        {
            var messageToSend = new FlooMessage
            {
                To = poco.Email,

                Content = string.Format(MessageTemplate, poco.Name, poco.Url)
            };

            var maxretries = new FlooInt(3);

            //var machine = Factory.Create("Floomeen.Showroom.MessagingFloomeen");
            Floomeen = new MessagingFloomeen();

            Floomeen.InjectAdapter<SendgridAdapter>();

            Floomeen.AddContextData(MessagingFloomeen.ContextKey.Message, messageToSend);

            Floomeen.AddContextData(MessagingFloomeen.ContextKey.MaxRetries, maxretries);

            Floomeen.Plug(poco);
        }
    }
}
