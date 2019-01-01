namespace TwentyFirst.Common.Models.Polls
{
    using Data.Models;
    using Mapping.Contracts;

    public class PollOptionChooseModel : IMapTo<PollOption>
    {
        public int Id { get; set; }

        public string Value { get; set; }
    }
}
