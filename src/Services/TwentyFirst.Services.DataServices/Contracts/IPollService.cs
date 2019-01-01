namespace TwentyFirst.Services.DataServices.Contracts
{
    using Common.Exceptions;
    using Common.Models.Polls;
    using Data.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IPollService
    {
        Task<IEnumerable<TModel>> AllAsync<TModel>();

        /// <summary>
        /// Gets polls by id and project it to given model.
        /// Throw InvalidPollException if id is not present.
        /// </summary>
        /// <exception cref="InvalidPollException"></exception>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TModel> GetAsync<TModel>(string id);

        /// <summary>
        /// Gets article by id.
        /// Throw InvalidArticleException if id is not present.
        /// </summary>
        /// <exception cref="InvalidPollException"></exception>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Poll> GetAsync(string id);

        Task<Poll> CreateAsync(PollCreateInputModel pollCreateInputModel, string creatorId);

        Task DeleteAsync(string id);

        Task<TModel> GetActiveAsync<TModel>();

        Task VoteAsync(ActivePollVoteInputModel activePollVoteInputModel);
    }
}
