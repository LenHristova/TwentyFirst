namespace TwentyFirst.Services.DataServices.Contracts
{
    using System.Collections.Generic;
    using Common.Exceptions;
    using Data.Models;
    using System.Threading.Tasks;

    public interface ISubscriberService
    {
        Task<bool> ExistsAsync(string email);

        Task<Subscriber> CreateAsync(string email);

        Task SubscribeAsync(string id, string confirmationCode);

        Task UnsubscribeAsync(string id, string confirmationCode);

        /// <summary>
        /// Gets subscriber and project it to given model.
        /// Throw InvalidSubscriberException if not present. 
        /// </summary>
        /// <exception cref="InvalidSubscriberException"></exception>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="id"></param>
        /// <param name="confirmationCode"></param>
        /// <returns></returns>
        Task<TModel> GetAsync<TModel>(string id, string confirmationCode);

        /// <summary>
        /// Gets subscriber.
        /// Throw InvalidSubscriberException if not present. 
        /// </summary>
        /// <exception cref="InvalidSubscriberException"></exception>
        /// <param name="id"></param>
        /// <param name="confirmationCode"></param>
        /// <returns></returns>
        Task<Subscriber> GetAsync(string id, string confirmationCode);

        /// <summary>
        /// Gets subscriber.
        /// Throw InvalidSubscriberException if not present. 
        /// </summary>
        /// <exception cref="InvalidSubscriberException"></exception>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<Subscriber> GetAsync(string email);

        Task<IEnumerable<TModel>> AllSubscribersEmailsAsync<TModel>();
    }
}
