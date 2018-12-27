namespace TwentyFirst.Services.DataServices.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Common.Exceptions;
    using Data.Models;
    using Xunit;

    public class SubscriberServiceTests : DataServiceTests
    {
        [Fact]
        public async Task AllConfirmedAsync_ShouldReturnsCorrectCountOfConfirmedSubscribers()
        {
            await this.dbContext.Subscribers.AddRangeAsync(new List<Subscriber>
            {
                new Subscriber { IsConfirmed = true },
                new Subscriber { IsConfirmed = false },
                new Subscriber { IsConfirmed = true },
                new Subscriber { IsConfirmed = true },
                new Subscriber { IsConfirmed = false },
            });

            await this.dbContext.SaveChangesAsync();

            var subscriberService = new SubscriberService(this.dbContext);

            var result = await subscriberService.AllConfirmedAsync<FakeSubscriber>();

            var expectedCount = 3;
            var actualCount = result.Count();
            Assert.Equal(expectedCount, actualCount);
        }

        [Fact]
        public async Task ExistsAsync_ShouldReturnsTrue_WhenSubscriberEmailExists()
        {
            var existingEmail = "exist@test.ts";
            await this.dbContext.Subscribers.AddRangeAsync(new List<Subscriber>
            {
                new Subscriber { Email = "1@test.ts" },
                new Subscriber { Email = "2@test.ts" },
                new Subscriber { Email = existingEmail },
                new Subscriber { Email = "4@test.ts" },
                new Subscriber { Email = "5@test.ts" },
            });

            await this.dbContext.SaveChangesAsync();

            var subscriberService = new SubscriberService(this.dbContext);

            var result = await subscriberService.ExistsAsync(existingEmail);

            Assert.True(result);
        }

        [Fact]
        public async Task ExistsAsync_ShouldReturnsFalse_WhenSubscriberEmailNotExists()
        {
            await this.dbContext.Subscribers.AddRangeAsync(new List<Subscriber>
            {
                new Subscriber { Email = "1@test.ts" },
                new Subscriber { Email = "2@test.ts" },
                new Subscriber { Email = "2@test.ts" },
                new Subscriber { Email = "3@test.ts" },
                new Subscriber { Email = "5@test.ts" },
            });

            await this.dbContext.SaveChangesAsync();

            var subscriberService = new SubscriberService(this.dbContext);

            var notExistingEmail = "notExist@test.ts";
            var result = await subscriberService.ExistsAsync(notExistingEmail);

            Assert.False(result);
        }

        [Fact]
        public async Task CreateAsync_ShouldAddNewSubscriberToDb()
        {
            var subscriberService = new SubscriberService(this.dbContext);

            var newSubscriberEmail = "new@test.ts";
            await subscriberService.CreateAsync(newSubscriberEmail);
            const int expected = 1;
            var actual = this.dbContext.Subscribers.Count();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task CreateAsync_ShouldGenerateConfirmationCodeToNewSubscriber()
        {
            var subscriberService = new SubscriberService(this.dbContext);

            var newSubscriberEmail = "new@test.ts";
            var newSubscriber = await subscriberService.CreateAsync(newSubscriberEmail);
            var newSubscriberFromDb = await this.dbContext.Subscribers.FindAsync(newSubscriber.Id);

            Assert.NotNull(newSubscriberFromDb.ConfirmationCode);
        }

        [Fact]
        public async Task CreateAsync_ShouldAddNewSubscriber_WithNotConfirmedEmail()
        {
            var subscriberService = new SubscriberService(this.dbContext);

            var newSubscriberEmail = "new@test.ts";
            var newSubscriber = await subscriberService.CreateAsync(newSubscriberEmail);

            var newSubscriberFromDb = await this.dbContext.Subscribers.FindAsync(newSubscriber.Id);

            Assert.False(newSubscriberFromDb.IsConfirmed);
        }

        [Fact]
        public async Task SubscribeAsync_ShouldSubscribe_WhenSubscriberExistsAndConfirmationCodeIsCorrect()
        {
            var subscriberId = Guid.NewGuid().ToString();
            var subscriberConfirmationCode = Guid.NewGuid().ToString();
            await this.dbContext.Subscribers.AddAsync(new Subscriber { Id = subscriberId, ConfirmationCode = subscriberConfirmationCode});
            await this.dbContext.SaveChangesAsync();

            var subscriberService = new SubscriberService(this.dbContext);
            await subscriberService.SubscribeAsync(subscriberId, subscriberConfirmationCode);

            var subscriber = await this.dbContext.Subscribers.FindAsync(subscriberId);
            Assert.True(subscriber.IsConfirmed);
        }

        [Fact]
        public async Task SubscribeAsync_ShouldThrowInvalidSubscriberException_WhenSubscriberExistsButConfirmationCodeIsIncorrect()
        {
            var subscriberId = Guid.NewGuid().ToString();
            var subscriberConfirmationCode = Guid.NewGuid().ToString();
            await this.dbContext.Subscribers.AddAsync(new Subscriber { Id = subscriberId, ConfirmationCode = subscriberConfirmationCode });
            await this.dbContext.SaveChangesAsync();

            var subscriberService = new SubscriberService(this.dbContext);

            var incorrectConfirmationCode = Guid.NewGuid().ToString();

            Assert.Throws<InvalidSubscriberException>(() => 
                subscriberService.SubscribeAsync(subscriberId, incorrectConfirmationCode).GetAwaiter().GetResult());
        }

        [Fact]
        public async Task SubscribeAsync_ShouldThrowInvalidSubscriberException_WhenSubscriberNotExists()
        {
            var subscriberId = Guid.NewGuid().ToString();
            var subscriberConfirmationCode = Guid.NewGuid().ToString();
            await this.dbContext.Subscribers.AddAsync(new Subscriber { Id = subscriberId, ConfirmationCode = subscriberConfirmationCode });
            await this.dbContext.SaveChangesAsync();

            var subscriberService = new SubscriberService(this.dbContext);

            var incorrectId = Guid.NewGuid().ToString();

            Assert.Throws<InvalidSubscriberException>(() =>
                subscriberService.SubscribeAsync(incorrectId, subscriberConfirmationCode).GetAwaiter().GetResult());
        }

        [Fact]
        public async Task UnsubscribeAsync_ShouldUnsubscribe_WhenSubscriberExistsAndConfirmationCodeIsCorrect()
        {
            var subscriberId = Guid.NewGuid().ToString();
            var subscriberConfirmationCode = Guid.NewGuid().ToString();
            await this.dbContext.Subscribers.AddAsync(new Subscriber
            {
                Id = subscriberId,
                ConfirmationCode = subscriberConfirmationCode,
                IsConfirmed = true
            });

            await this.dbContext.SaveChangesAsync();

            var subscriberService = new SubscriberService(this.dbContext);
            await subscriberService.UnsubscribeAsync(subscriberId, subscriberConfirmationCode);

            var subscriber = await this.dbContext.Subscribers.FindAsync(subscriberId);
            Assert.False(subscriber.IsConfirmed);
        }

        [Fact]
        public async Task UnsubscribeAsync_ShouldThrowInvalidSubscriberException_WhenSubscriberExistsButConfirmationCodeIsIncorrect()
        {
            var subscriberId = Guid.NewGuid().ToString();
            var subscriberConfirmationCode = Guid.NewGuid().ToString();
            await this.dbContext.Subscribers.AddAsync(new Subscriber { Id = subscriberId, ConfirmationCode = subscriberConfirmationCode });
            await this.dbContext.SaveChangesAsync();

            var subscriberService = new SubscriberService(this.dbContext);

            var incorrectConfirmationCode = Guid.NewGuid().ToString();

            Assert.Throws<InvalidSubscriberException>(() =>
                subscriberService.UnsubscribeAsync(subscriberId, incorrectConfirmationCode).GetAwaiter().GetResult());
        }

        [Fact]
        public async Task UnsubscribeAsync_ShouldThrowInvalidSubscriberException_WhenSubscriberNotExists()
        {
            var subscriberId = Guid.NewGuid().ToString();
            var subscriberConfirmationCode = Guid.NewGuid().ToString();
            await this.dbContext.Subscribers.AddAsync(new Subscriber { Id = subscriberId, ConfirmationCode = subscriberConfirmationCode });
            await this.dbContext.SaveChangesAsync();

            var subscriberService = new SubscriberService(this.dbContext);

            var incorrectId = Guid.NewGuid().ToString();

            Assert.Throws<InvalidSubscriberException>(() =>
                subscriberService.UnsubscribeAsync(incorrectId, subscriberConfirmationCode).GetAwaiter().GetResult());
        }

        [Fact]
        public async Task GetAsync_ShouldGetCorrectSubscriber_WhenCategoryExists()
        {
            var wantedSubscriberId = Guid.NewGuid().ToString();
            var wantedSubscriberConfirmationCode = Guid.NewGuid().ToString();

            await this.dbContext.Subscribers.AddRangeAsync(new List<Subscriber>()
            {
                new Subscriber {Id = "1", ConfirmationCode = "1"},
                new Subscriber {Id = wantedSubscriberId, ConfirmationCode = wantedSubscriberConfirmationCode},
                new Subscriber {Id = "3", ConfirmationCode = "3"},
            });

            await this.dbContext.SaveChangesAsync();

            var subscriberService = new SubscriberService(this.dbContext);

            var expectedSubscriberId = wantedSubscriberId;
            var subscriber = await subscriberService.GetAsync(wantedSubscriberId, wantedSubscriberConfirmationCode);

            Assert.Equal(expectedSubscriberId, subscriber.Id);
        }

        [Fact]
        public async Task GetAsync_ShouldThrowInvalidSubscriberException_WhenSubscriberExistsButConfirmationCodeIsIncorrect()
        {
            var wantedSubscriberId = Guid.NewGuid().ToString();

            await this.dbContext.Subscribers.AddRangeAsync(new List<Subscriber>()
            {
                new Subscriber {Id = "1", ConfirmationCode = "1"},
                new Subscriber {Id = wantedSubscriberId, ConfirmationCode = Guid.NewGuid().ToString()},
                new Subscriber {Id = "3", ConfirmationCode = "3"},
            });

            await this.dbContext.SaveChangesAsync();

            var subscriberService = new SubscriberService(this.dbContext);

            var incorrectConfirmationCode = Guid.NewGuid().ToString();

            Assert.Throws<InvalidSubscriberException>(() =>
                subscriberService.GetAsync(wantedSubscriberId, incorrectConfirmationCode).GetAwaiter().GetResult());
        }

        [Fact]
        public async Task GetAsync_ShouldThrowInvalidSubscriberException_WhenSubscriberNotExists()
        {
            var correctConfirmationCode = Guid.NewGuid().ToString();
            await this.dbContext.Subscribers.AddRangeAsync(new List<Subscriber>()
            {
                new Subscriber {Id = "1", ConfirmationCode = "1"},
                new Subscriber {Id = "2", ConfirmationCode = correctConfirmationCode },
                new Subscriber {Id = "3", ConfirmationCode = "3"},
            });

            await this.dbContext.SaveChangesAsync();

            var subscriberService = new SubscriberService(this.dbContext);

            var incorrectId = Guid.NewGuid().ToString();

            Assert.Throws<InvalidSubscriberException>(() =>
                subscriberService.GetAsync(incorrectId, correctConfirmationCode).GetAwaiter().GetResult());
        }

        [Fact]
        public async Task GetAsync_Projection_ShouldGetCorrectSubscriber_WhenCategoryExists()
        {
            var wantedSubscriberId = Guid.NewGuid().ToString();
            var wantedSubscriberConfirmationCode = Guid.NewGuid().ToString();

            await this.dbContext.Subscribers.AddRangeAsync(new List<Subscriber>()
            {
                new Subscriber {Id = "1", ConfirmationCode = "1"},
                new Subscriber {Id = wantedSubscriberId, ConfirmationCode = wantedSubscriberConfirmationCode},
                new Subscriber {Id = "3", ConfirmationCode = "3"},
            });

            await this.dbContext.SaveChangesAsync();

            var subscriberService = new SubscriberService(this.dbContext);

            var expectedSubscriberId = wantedSubscriberId;
            var subscriber = await subscriberService.GetAsync<FakeSubscriber>(wantedSubscriberId, wantedSubscriberConfirmationCode);

            Assert.Equal(expectedSubscriberId, subscriber.Id);
        }

        [Fact]
        public async Task GetAsync_Projection_ShouldThrowInvalidSubscriberException_WhenSubscriberExistsButConfirmationCodeIsIncorrect()
        {
            var wantedSubscriberId = Guid.NewGuid().ToString();

            await this.dbContext.Subscribers.AddRangeAsync(new List<Subscriber>()
            {
                new Subscriber {Id = "1", ConfirmationCode = "1"},
                new Subscriber {Id = wantedSubscriberId, ConfirmationCode = Guid.NewGuid().ToString()},
                new Subscriber {Id = "3", ConfirmationCode = "3"},
            });

            await this.dbContext.SaveChangesAsync();

            var subscriberService = new SubscriberService(this.dbContext);

            var incorrectConfirmationCode = Guid.NewGuid().ToString();

            Assert.Throws<InvalidSubscriberException>(() =>
                subscriberService.GetAsync<FakeSubscriber>(wantedSubscriberId, incorrectConfirmationCode).GetAwaiter().GetResult());
        }

        [Fact]
        public async Task GetAsync_Projection_ShouldThrowInvalidSubscriberException_WhenSubscriberNotExists()
        {
            var correctConfirmationCode = Guid.NewGuid().ToString();
            await this.dbContext.Subscribers.AddRangeAsync(new List<Subscriber>()
            {
                new Subscriber {Id = "1", ConfirmationCode = "1"},
                new Subscriber {Id = "2", ConfirmationCode = correctConfirmationCode },
                new Subscriber {Id = "3", ConfirmationCode = "3"},
            });

            await this.dbContext.SaveChangesAsync();

            var subscriberService = new SubscriberService(this.dbContext);

            var incorrectId = Guid.NewGuid().ToString();

            Assert.Throws<InvalidSubscriberException>(() =>
                subscriberService.GetAsync<FakeSubscriber>(incorrectId, correctConfirmationCode).GetAwaiter().GetResult());
        }

        [Fact]
        public async Task GetAsync_ShouldGetCorrectSubscriber_WhenEmailExists()
        {
            var wantedSubscriberId = Guid.NewGuid().ToString();
            var wantedSubscriberEmail = Guid.NewGuid().ToString();

            await this.dbContext.Subscribers.AddRangeAsync(new List<Subscriber>()
            {
                new Subscriber { Id = "1", Email = "1@test.ts"},
                new Subscriber { Id = wantedSubscriberId, Email = wantedSubscriberEmail},
                new Subscriber { Id = "3", Email = "3@test.ts"},
            });

            await this.dbContext.SaveChangesAsync();

            var subscriberService = new SubscriberService(this.dbContext);

            var expectedSubscriberId = wantedSubscriberId;
            var subscriber = await subscriberService.GetAsync(wantedSubscriberEmail);

            Assert.Equal(expectedSubscriberId, subscriber.Id);
        }

        [Fact]
        public async Task GetAsync_ShouldThrowInvalidSubscriberException_WhenEmailNotExists()
        {
            await this.dbContext.Subscribers.AddRangeAsync(new List<Subscriber>()
            {
                new Subscriber {Id = "1", Email = "1@test.ts"},
                new Subscriber {Id = "2", Email = "2@test.ts"},
            });

            await this.dbContext.SaveChangesAsync();

            var subscriberService = new SubscriberService(this.dbContext);

            var incorrectEmail = "3@test.ts";

            Assert.Throws<InvalidSubscriberException>(() =>
                subscriberService.GetAsync(incorrectEmail).GetAwaiter().GetResult());
        }

        internal class FakeSubscriber
        {
            public string Id { get; set; }
        }
    }
}
