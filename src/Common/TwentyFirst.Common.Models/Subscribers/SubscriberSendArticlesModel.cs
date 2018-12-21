namespace TwentyFirst.Common.Models.Subscribers
{
    using Data.Models;
    using Mapping.Contracts;

    public class SubscriberSendArticlesModel : IMapFrom<Subscriber>
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public string ConfirmationCode { get; set; }
    }
}
