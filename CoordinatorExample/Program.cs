using System;
using System.Collections.Generic;
using System.Linq;
using CoordinatorExample.POCOs;
using Floomeen.Adapters.MessageSender;
using Floomeen.Meen;
using Floomeen.Meen.Interfaces;
using Showroom;
using Showroom.Adapters;

namespace CoordinatorExample
{
    class Program
    {
        public static string MessageTemplate = "Dear {0}, <br/> please get in contact with us by clicking on <a href=\"{1}\">this link</a>";

        public static readonly string Send = MessagingFloomeen.Command.Send;

        public static List<CustomerPOCO> Customers = new List<CustomerPOCO>
        {
            new CustomerPOCO { CustomerId = "c1", Name = "C1", Email = "c1@c1.me", Url="https://www.customer1.com"},
        };

        public static List<MessagePOCO> Messages = new List<MessagePOCO>();


        static void Main(string[] args)
        {
            foreach (var customer in Customers)
            {
                Setup(customer, out MessagingFloomeen messagingMaster, out FlipperFloomeen flipperSlave);

                Console.WriteLine("==========================================");

                Console.WriteLine($"[BeforeCommand] Customer '{customer.CustomerId}' has state '{ flipperSlave.CurrentState }'");

                messagingMaster.Execute(Send);

                Console.WriteLine($"[AfterCommand] Customer '{customer.CustomerId}' has state '{ flipperSlave.CurrentState }'");

                messagingMaster.Unbind();

                flipperSlave.Unbind();

                Console.WriteLine("==========================================");
            }

            Console.ReadKey();

        }
        

        public static void Setup(CustomerPOCO customer, out MessagingFloomeen master, out FlipperFloomeen slave)
        {
            PlugFlipperFloomeen(out FlipperFloomeen flipperSlave, customer);

            slave = flipperSlave;

            var message = Messages.FirstOrDefault(m => m.MessageId == MessageId(customer.CustomerId)) ??

                          new MessagePOCO { MessageId = MessageId(customer.CustomerId) };

            var email = MessageToSend(customer);

            PlugMessagingFloomeen(out MessagingFloomeen messagingMaster, message, email);

            master = messagingMaster;

            var coordinator = new MessagingFlipperCoordinator(messagingMaster, flipperSlave);
        }

        public static string MessageId(string customerId)
        {
            return $"{customerId}_message";
        }

        static void PlugFlipperFloomeen(out FlipperFloomeen flipper, IFellow customer)
        {
            flipper = Factory<FlipperFloomeen>.Create();

            flipper.Plug(customer);
        }

        static void PlugMessagingFloomeen(out MessagingFloomeen floomeen, MessagePOCO message, FlooMessage email)
        {
            floomeen = Factory<MessagingFloomeen>.Create();

            floomeen.InjectAdapter<EmailAdapter>();

            floomeen.AddContextData(MessagingFloomeen.ContextKey.Message, email);

            floomeen.AddContextData(MessagingFloomeen.ContextKey.MaxRetries, new FlooInt(3));

            floomeen.Plug(message);
        }

        static FlooMessage MessageToSend(CustomerPOCO customer)
        {
            return new FlooMessage
            {
                To = customer.Email,

                From = "system@example.com",

                Content = string.Format(MessageTemplate, customer.Name, customer.Url),

                Type = SupportedTypes.Email
            };
        }
    }
}
