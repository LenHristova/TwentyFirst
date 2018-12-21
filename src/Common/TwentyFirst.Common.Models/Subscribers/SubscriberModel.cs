namespace TwentyFirst.Common.Models.Subscribers
{
    using Data.Models;
    using Mapping.Contracts;

    public class SubscriberModel : IMapFrom<Subscriber>
    {
        public string Id { get; set; }

        public string ConfirmationCode { get; set; }
    }
}
