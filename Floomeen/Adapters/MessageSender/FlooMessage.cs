namespace Floomeen.Adapters.MessageSender
{
    public class FlooMessage
    {
       
        public string Id { get; set; }

        public string Type { get; set; }

        public string FromAlias { get; set; }

        public string From { get; set; }

        public string To { get; set; }

        public string ToAlias { get; set; }

        public string Content { get; set; }

    }
}
