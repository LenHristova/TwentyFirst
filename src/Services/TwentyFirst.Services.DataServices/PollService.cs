namespace TwentyFirst.Services.DataServices
{
    using Common;
    using Common.Exceptions;
    using Common.Mapping;
    using Common.Models.Polls;
    using Contracts;
    using Data;
    using Data.Models;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class PollService : IPollService
    {
        private readonly TwentyFirstDbContext db;

        public PollService(TwentyFirstDbContext db)
        {
            this.db = db;
        }

        public async Task<IEnumerable<TModel>> AllAsync<TModel>()
            => await this.db.Polls
                .Where(p => !p.IsDeleted)
                .OrderByDescending(p => p.CreatedOn)
                .To<TModel>()
                .ToListAsync();

        /// <inheritdoc />
        /// <summary>
        /// Gets polls by id and project it to given model.
        /// Throw InvalidPollException if id is not present.
        /// </summary>
        /// <exception cref="InvalidPollException"></exception>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<TModel> GetAsync<TModel>(string id)
        {
            var poll = await this.db.Polls
                .Where(c => c.Id == id && !c.IsDeleted)
                .To<TModel>()
                .SingleOrDefaultAsync();

            CoreValidator.ThrowIfNull(poll, new InvalidPollException());
            return poll;
        }

        /// <inheritdoc />
        /// <summary>
        /// Gets article by id.
        /// Throw InvalidArticleException if id is not present.
        /// </summary>
        /// <exception cref="InvalidPollException"></exception>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Poll> GetAsync(string id)
        {
            var poll = await this.db.Polls
                .Include(p => p.Votes)
                .Include(p => p.Options)
                .SingleOrDefaultAsync(c => c.Id == id && !c.IsDeleted);

            CoreValidator.ThrowIfNull(poll, new InvalidPollException());
            return poll;
        }

        public async Task<Poll> CreateAsync(PollCreateInputModel pollCreateInputModel, string creatorId)
        {
            var lastActive = await this.db.Polls.SingleOrDefaultAsync(p => !p.IsDeleted && p.IsActive);
            if (lastActive != null)
            {
                lastActive.IsActive = false;
            }

            var poll = new Poll
            {
                Question = pollCreateInputModel.Question,
                CreatorId = creatorId,
                IsActive = true,
                IsDeleted = false,
                CreatedOn = DateTime.UtcNow,
                Options = pollCreateInputModel.Options?.Select(o => new PollOption { Value = o }).ToList()
            };

            await this.db.Polls.AddAsync(poll);

            await this.db.SaveChangesAsync();
            return poll;
        }

        public async Task DeleteAsync(string id)
        {
            var poll = await this.GetAsync(id);
            poll.IsDeleted = true;
            poll.IsActive = false;

            await this.db.SaveChangesAsync();
        }

        public async Task<TModel> GetActiveAsync<TModel>()
        => await this.db.Polls
            .Where(p => !p.IsDeleted && p.IsActive)
            .To<TModel>()
            .SingleOrDefaultAsync();

        public async Task VoteAsync(ActivePollVoteInputModel activePollVoteInputModel)
        {
            CoreValidator.ThrowIfNull(activePollVoteInputModel.VoteIp, new NullIpException());

            var poll = await this.GetAsync(activePollVoteInputModel.Id);

            var isAlreadyVoted = poll.Votes
                .Any(v => v.Ip == activePollVoteInputModel.VoteIp);

            if (isAlreadyVoted)
            {
                throw new AlreadyVotedException();
            }

            var option = poll.Options
                .SingleOrDefault(o => o.Id == activePollVoteInputModel.SelectedOptionId);

            CoreValidator.ThrowIfNull(option, new InvalidPollOptionException());
            
            poll.Votes.Add(new PollVote { Ip = activePollVoteInputModel.VoteIp });
            option.Votes++;
            await this.db.SaveChangesAsync();
        }
    }
}
