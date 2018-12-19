namespace TwentyFirst.Services.DataServices.Contracts
{
    using Common.Models.Interviews;
    using Data.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Common.Exceptions;

    public interface IInterviewService
    {
        Task<IEnumerable<TModel>> AllAsync<TModel>();

        Task<Interview> CreateAsync(InterviewCreateInputModel interviewCreateInputModel, string creatorId);

        Task<Interview> Edit(InterviewEditInputModel interviewEditInputModel, string editorId);

        Task Delete(string articleId, string editorId);

        /// <summary>
        /// Gets interview by id and project it to given model.
        /// Throw InvalidInterviewIdException if id is not present.
        /// </summary>
        /// <exception cref="InvalidInterviewIdException"></exception>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TModel> GetAsync<TModel>(string id);

        /// <summary>
        /// Gets interview by id.
        /// Throw InvalidInterviewIdException if id is not present.
        /// </summary>
        /// <exception cref="InvalidInterviewIdException"></exception>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Interview> GetAsync(string id);
    }
}
