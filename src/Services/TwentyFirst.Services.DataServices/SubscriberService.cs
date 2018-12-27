namespace TwentyFirst.Services.DataServices
{
    using Common;
    using Common.Exceptions;
    using Common.Mapping;
    using Contracts;
    using Data;
    using Data.Models;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class SubscriberService : ISubscriberService
    {
        private readonly TwentyFirstDbContext db;

        public SubscriberService(TwentyFirstDbContext db)
        {
            this.db = db;
        }

        public async Task<IEnumerable<TModel>> AllConfirmedAsync<TModel>()
            => await this.db.Subscribers
                .Where(s => s.IsConfirmed)
                .To<TModel>()
                .ToListAsync();

        public async Task<bool> ExistsAsync(string email)
            => await this.db.Subscribers.AnyAsync(s => s.Email == email);

        public async Task<Subscriber> CreateAsync(string email)
        {
            var newSubscriber = new Subscriber
            {
                Email = email,
                ConfirmationCode = Guid.NewGuid().ToString(),
                IsConfirmed = false
            };

            await this.db.Subscribers.AddAsync(newSubscriber);
            await this.db.SaveChangesAsync();

            return newSubscriber;
        }

        public async Task SubscribeAsync(string id, string confirmationCode)
        {
            var subscriber = await this.GetAsync(id, confirmationCode);
            subscriber.IsConfirmed = true;
            await this.db.SaveChangesAsync();
        }

        public async Task UnsubscribeAsync(string id, string confirmationCode)
        {
            var subscriber = await this.GetAsync(id, confirmationCode);
            subscriber.IsConfirmed = false;
            await this.db.SaveChangesAsync();
        }

        /// <inheritdoc />
        /// <summary>
        /// Gets subscriber and project it to given model.
        /// Throw InvalidSubscriberException if not present. 
        /// </summary>
        /// <exception cref="InvalidSubscriberException"></exception>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="id"></param>
        /// <param name="confirmationCode"></param>
        /// <returns></returns>
        public async Task<TModel> GetAsync<TModel>(string id, string confirmationCode)
        {
            var subscriber = await this.db.Subscribers
                .Where(s => s.Id == id && s.ConfirmationCode == confirmationCode)
                .To<TModel>()
                .SingleOrDefaultAsync();

            CoreValidator.ThrowIfNull(subscriber, new InvalidSubscriberException());
            return subscriber;
        }

        /// <inheritdoc />
        /// <summary>
        /// Gets subscriber.
        /// Throw InvalidSubscriberException if not present. 
        /// </summary>
        /// <exception cref="InvalidSubscriberException"></exception>
        /// <param name="id"></param>
        /// <param name="confirmationCode"></param>
        /// <returns></returns>
        public async Task<Subscriber> GetAsync(string id, string confirmationCode)
        {
            var subscriber = await this.db.Subscribers
                .SingleOrDefaultAsync(s => s.Id == id && s.ConfirmationCode == confirmationCode);

            CoreValidator.ThrowIfNull(subscriber, new InvalidSubscriberException());
            return subscriber;
        }

        /// <inheritdoc />
        /// <summary>
        /// Gets subscriber.
        /// Throw InvalidSubscriberException if not present. 
        /// </summary>
        /// <exception cref="InvalidSubscriberException"></exception>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<Subscriber> GetAsync(string email)
        {
            var subscriber = await this.db.Subscribers
                .SingleOrDefaultAsync(s => s.Email == email);

            CoreValidator.ThrowIfNull(subscriber, new InvalidSubscriberException());
            return subscriber;
        }
    }
}
