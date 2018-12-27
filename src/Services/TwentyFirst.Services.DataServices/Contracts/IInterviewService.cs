namespace TwentyFirst.Services.DataServices.Contracts
{
    using Common.Models.Interviews;
    using Data.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Common.Exceptions;

    public interface IInterviewService
    {
        Task<IEnumerable<TModel>> LatestAsync<TModel>(int count);

        Task<Interview> CreateAsync(InterviewCreateInputModel interviewCreateInputModel, string creatorId);

        Task<Interview> EditAsync(InterviewEditInputModel interviewEditInputModel, string editorId);

        Task DeleteAsync(string interviewId, string editorId);

        /// <summary>
        /// Gets interview by id and project it to given model.
        /// Throw InvalidInterviewException if id is not present.
        /// </summary>
        /// <exception cref="InvalidInterviewException"></exception>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TModel> GetAsync<TModel>(string id);

        /// <summary>
        /// Gets interview by id.
        /// Throw InvalidInterviewException if id is not present.
        /// </summary>
        /// <exception cref="InvalidInterviewException"></exception>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Interview> GetAsync(string id);
    }
}
