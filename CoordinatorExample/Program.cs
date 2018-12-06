using System;
using System.Collections.Generic;
using System.Linq;
using CoordinatorExample.POCOs;
using Floomeen.Adapters.MessageSender;
using Floomeen.Meen;
using Floomeen.Meen.Interfaces;
using Showroom;

namespace CoordinatorExample
{
    class Program
    {
        public static string MessageTemplate = "Dear {0}, <br/> please get in contact with us by clicking on <a href=\"{1}\">this link</a>";

        public static List<CustomerPOCO> Customers = new List<CustomerPOCO>
        {
            new CustomerPOCO { CustomerId = "c1", Name = "C1", Email = "c1@c1.me", Url="https://www.customer1.com"},
            new CustomerPOCO { CustomerId = "c2", Name = "C2", Email = "c2@c2.me", Url="https://www.customer2.com"},
            new CustomerPOCO { CustomerId = "c3", Name = "C3", Email = "c3@c3.me", Url="https://www.customer3.com"},
        };

        public static List<MessagePOCO> Messages = new List<MessagePOCO>();


        static void Main(string[] args)
        {
            foreach (var customer in Customers)
            {

                PlugFlipperFloomeen(out FlipperFloomeen flipperSlave, customer);

                var message = Messages.FirstOrDefault( m => m.MessageId == MessageId(customer.CustomerId)) ??
                    
                              new MessagePOCO {MessageId = MessageId(customer.CustomerId) };

                var email = MessageToSend(customer);

                PlugMessagingFloomeen(out MessagingFloomeen messagingMaster, message, email);
                
                var coordinator = new MessagingFlipperCoordinator(messagingMaster, flipperSlave);

                Console.WriteLine("==========================================");

                Console.WriteLine($"[Before] Customer {customer.CustomerId} is '{ flipperSlave.CurrentState }'");

                messagingMaster.Execute(MessagingFloomeen.Command.Send);

                Console.WriteLine($"[After] Customer {customer.CustomerId} is '{ flipperSlave.CurrentState }'");

                messagingMaster.Unbind();

                flipperSlave.Unbind();

                Console.WriteLine("==========================================");
            }

            Console.ReadKey();

        }

        public static string MessageId(string customerId)
        {
            return $"{customerId}_M";
        }

        static void PlugFlipperFloomeen(out FlipperFloomeen flipper, IFellow customer)
        {
            flipper = new FlipperFloomeen();

            flipper.Plug(customer);
        }

        static void PlugMessagingFloomeen(out MessagingFloomeen floomeen, MessagePOCO message, FlooMessage email)
        {
            floomeen = new MessagingFloomeen();

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
