using Floomeen.Attributes;
using Floomeen.Meen.Interfaces;

namespace Floomeen.Tests.FloomeenShould
{
    public class POCO : IFellow
    {
        [FloomeenId]
        public string Id { get; set; }

        public string Username { get; set; }

        [FloomeenState]
        public string FlooState { get; set; }

    }
}