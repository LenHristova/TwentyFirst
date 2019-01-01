namespace TwentyFirst.Common.Models.Polls
{
    using Data.Models;
    using Mapping.Contracts;

    public class PollOptionViewModel : IMapFrom<PollOption>
    {
        public string Value { get; set; }

        public int Votes { get; set; }

        public double GetVotesPercentage(int allVotes)
        {
            allVotes = allVotes == 0 ? 1 : allVotes;

            return this.Votes * 100.0 / allVotes;
        }
    }
}
