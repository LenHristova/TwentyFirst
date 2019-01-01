namespace TwentyFirst.Services.DataServices.Tests
{
    using Common.Exceptions;
    using Common.Models.Polls;
    using Data.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Xunit;

    public class PollServiceTests : DataServiceTests
    {
        [Fact]
        public async Task AllAsync_ShouldReturnsCorrectCountOfPolls()
        {
            await this.dbContext.Polls.AddRangeAsync(new List<Poll>
            {
                new Poll { IsDeleted = true },
                new Poll { IsDeleted = false },
                new Poll { IsDeleted = true },
                new Poll { IsDeleted = false },
                new Poll { IsDeleted = false },
            });

            await this.dbContext.SaveChangesAsync();

            var pollService = new PollService(this.dbContext);
            var result = await pollService.AllAsync<FakePoll>();

            var expectedCount = 3;
            var actualCount = result.Count();
            Assert.Equal(expectedCount, actualCount);
        }

        [Fact]
        public async Task AllAsync_ShouldReturnsOrderedByCreatedOnDescending()
        {
            await this.dbContext.Polls.AddRangeAsync(new List<Poll>
            {
                new Poll { Id = "1", CreatedOn = DateTime.UtcNow.AddHours(-2) },
                new Poll { Id = "2", CreatedOn = DateTime.UtcNow },
                new Poll { Id = "3", CreatedOn = DateTime.UtcNow.AddHours(-20) },
                new Poll { Id = "4", CreatedOn = DateTime.UtcNow.AddHours(-12) },
                new Poll { Id = "5", CreatedOn = DateTime.UtcNow.AddHours(-8) },
            });

            await this.dbContext.SaveChangesAsync();

            var pollService = new PollService(this.dbContext);
            var result = await pollService.AllAsync<FakePoll>();
            var expectedIds = new List<string> { "2", "1", "5", "4", "3" };
            var actualPolls = result.ToList();

            for (int i = 0; i < expectedIds.Count; i++)
            {
                Assert.Equal(expectedIds[i], actualPolls[i].Id);
            }
        }

        [Fact]
        public async Task GetAsync_ShouldGetCorrectPoll_WhenPollExists()
        {
            await this.dbContext.Polls.AddRangeAsync(new List<Poll>()
            {
                new Poll {Id = "1"},
                new Poll {Id = "2"},
                new Poll {Id = "3"},
            });

            await this.dbContext.SaveChangesAsync();

            var pollService = new PollService(this.dbContext);

            var wantedPollId = "2";
            var expectedIPollId = wantedPollId;
            var poll = await pollService.GetAsync(wantedPollId);

            Assert.Equal(expectedIPollId, poll.Id);
        }

        [Fact]
        public void GetAsync_ShouldThrowInvalidPollException_WhenPollNotExists()
        {
            var pollService = new PollService(this.dbContext);
            Assert.Throws<InvalidPollException>(
                () => pollService.GetAsync(Guid.NewGuid().ToString()).GetAwaiter().GetResult());
        }

        [Fact]
        public async Task GetAsync_Projection_ShouldGetCorrectPoll_WhenPollExists()
        {
            await this.dbContext.Polls.AddRangeAsync(new List<Poll>()
            {
                new Poll {Id = "1"},
                new Poll {Id = "2"},
                new Poll {Id = "3"},
            });

            await this.dbContext.SaveChangesAsync();

            var pollService = new PollService(this.dbContext);

            var wantedPollId = "2";
            var expectedIPollId = wantedPollId;
            var poll = await pollService.GetAsync<FakePoll>(wantedPollId);

            Assert.Equal(expectedIPollId, poll.Id);
        }

        [Fact]
        public void GetAsync_Projection_ShouldThrowInvalidPollException_WhenPollNotExists()
        {
            var pollService = new PollService(this.dbContext);
            Assert.Throws<InvalidPollException>(
                () => pollService.GetAsync<FakePoll>(Guid.NewGuid().ToString()).GetAwaiter().GetResult());
        }

        [Fact]
        public async Task CreateAsync_ShouldAddNewPollToDb()
        {
            var pollService = new PollService(this.dbContext);

            var creatorId = Guid.NewGuid().ToString();
            await pollService.CreateAsync(new PollCreateInputModel(), creatorId);
            const int expected = 1;
            var actual = this.dbContext.Polls.Count();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task CreateAsync_ShouldAddCorrectCreatorToNewPoll()
        {
            var pollService = new PollService(this.dbContext);

            var creatorId = Guid.NewGuid().ToString();
            await pollService.CreateAsync(new PollCreateInputModel(), creatorId);
            var pollFromDb = this.dbContext.Polls.FirstOrDefault();

            Assert.Equal(creatorId, pollFromDb?.CreatorId);
        }

        [Fact]
        public async Task CreateAsync_ShouldAddActivePoll()
        {
            var pollService = new PollService(this.dbContext);

            var creatorId = Guid.NewGuid().ToString();
            await pollService.CreateAsync(new PollCreateInputModel(), creatorId);
            var pollFromDb = this.dbContext.Polls.FirstOrDefault();

            Assert.True(pollFromDb?.IsActive);
        }

        [Fact]
        public async Task CreateAsync_ShouldBeTheOnlyOneActive()
        {
            await this.dbContext.Polls.AddRangeAsync(new List<Poll>
            {
                new Poll { IsActive = false },
                new Poll { IsActive = false },
                new Poll { IsActive = true },
                new Poll { IsActive = false },
                new Poll { IsActive = false },
            });

            await this.dbContext.SaveChangesAsync();

            var pollService = new PollService(this.dbContext);

            var creatorId = Guid.NewGuid().ToString();
            var newPoll = await pollService.CreateAsync(new PollCreateInputModel(), creatorId);
            var pollFromDb = this.dbContext.Polls.SingleOrDefault(p => p.IsActive);

            Assert.NotNull(pollFromDb);
            Assert.True(pollFromDb?.Id == newPoll.Id);
        }

        [Fact]
        public async Task CreateAsync_ShouldAddAllOptions()
        {
            var pollService = new PollService(this.dbContext);

            var creatorId = Guid.NewGuid().ToString();
            var pollToCreate = new PollCreateInputModel { Options = new List<string>() { "1", "2", "3" } };


            await pollService.CreateAsync(pollToCreate, creatorId);
            var pollFromDb = this.dbContext.Polls.FirstOrDefault();

            Assert.True(pollFromDb?.Options.Count == pollToCreate.Options.Count);
        }

        [Fact]
        public async Task DeleteAsync_ShouldSoftDeletePoll_WhenPollExists()
        {
            var pollId = Guid.NewGuid().ToString();
            await this.dbContext.Polls.AddAsync(new Poll { Id = pollId, IsDeleted = false });
            await this.dbContext.SaveChangesAsync();

            var pollService = new PollService(this.dbContext);
            await pollService.DeleteAsync(pollId);

            var resultPoll = await this.dbContext.Polls.FindAsync(pollId);
            Assert.True(resultPoll.IsDeleted);
        }

        [Fact]
        public async Task DeleteAsync_ShouldBeInactive()
        {
            var pollId = Guid.NewGuid().ToString();
            await this.dbContext.Polls.AddAsync(new Poll { Id = pollId, IsDeleted = false });
            await this.dbContext.SaveChangesAsync();

            var pollService = new PollService(this.dbContext);
            await pollService.DeleteAsync(pollId);

            var resultPoll = await this.dbContext.Polls.FindAsync(pollId);
            Assert.False(resultPoll.IsActive);
        }

        [Fact]
        public void DeleteAsync_ShouldThrowInvalidPollException_WhenPollNotExists()
        {
            var pollService = new PollService(this.dbContext);

            Assert.Throws<InvalidPollException>(
                () => pollService.DeleteAsync(Guid.NewGuid().ToString()).GetAwaiter().GetResult());
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrowInvalidPollException_WhenIdExistsButPollIsSoftDeleted()
        {
            var pollToDeleteId = Guid.NewGuid().ToString();
            await this.dbContext.Polls.AddAsync(new Poll { Id = pollToDeleteId, IsDeleted = true });
            await this.dbContext.SaveChangesAsync();

            var pollService = new PollService(this.dbContext);

            Assert.Throws<InvalidPollException>(
                () => pollService.DeleteAsync(pollToDeleteId).GetAwaiter().GetResult());
        }

        [Fact]
        public async Task GetActiveAsync_ShouldGetCorrectPoll_WhenHasOneActive()
        {
            await this.dbContext.Polls.AddRangeAsync(new List<Poll>()
            {
                new Poll {Id = "1", IsActive = false },
                new Poll {Id = "2", IsActive = true },
                new Poll {Id = "3", IsActive = false },
            });

            await this.dbContext.SaveChangesAsync();

            var pollService = new PollService(this.dbContext);

            var expectedIPollId = "2";
            var poll = await pollService.GetActiveAsync<FakePoll>();

            Assert.Equal(expectedIPollId, poll.Id);
        }

        [Fact]
        public async Task GetActiveAsync_ShouldReturnsNull_WhenHasNoActive()
        {
            await this.dbContext.Polls.AddRangeAsync(new List<Poll>()
            {
                new Poll {Id = "1", IsActive = false },
                new Poll {Id = "2", IsActive = false },
                new Poll {Id = "3", IsActive = false },
            });

            await this.dbContext.SaveChangesAsync();

            var pollService = new PollService(this.dbContext);

            var poll = await pollService.GetActiveAsync<FakePoll>();

            Assert.Null(poll);
        }

        [Fact]
        public async Task GetActiveAsync_ShouldThrowInvalidOperationException_WhenHasManyActive()
        {
            await this.dbContext.Polls.AddRangeAsync(new List<Poll>()
            {
                new Poll {Id = "1", IsActive = true },
                new Poll {Id = "2", IsActive = true },
                new Poll {Id = "3", IsActive = true },
            });

            await this.dbContext.SaveChangesAsync();

            var pollService = new PollService(this.dbContext);

            Assert.Throws<InvalidOperationException>(
                () => pollService.GetActiveAsync<FakePoll>().GetAwaiter().GetResult());
        }

        [Fact]
        public async Task VoteAsync_ShouldAddNewVoteToCorrectOption_WhenPollAndSelectedOptionExists()
        {
            await this.dbContext.Polls.AddAsync(new Poll
            {
                Id = "1",
                IsActive = true,
                Options = new List<PollOption>
                {
                    new PollOption{Id = 1, Votes = 0},
                    new PollOption{Id = 2, Votes = 0},
                    new PollOption{Id = 3, Votes = 0},
                }
            });

            await this.dbContext.SaveChangesAsync();

            var pollService = new PollService(this.dbContext);

            var optionIdToVote = 2;
            var activePollVoteInputModel = new ActivePollVoteInputModel
            {
                Id = "1",
                SelectedOptionId = optionIdToVote,
                VoteIp = Guid.NewGuid().ToString()
            };

            await pollService.VoteAsync(activePollVoteInputModel);

            var pollFromDb = this.dbContext.Polls.First();

            var optionToVote = pollFromDb.Options.First(o => o.Id == optionIdToVote);

            var expectedVotesCount = 1;
            var actualVotesCount = optionToVote.Votes;
            Assert.Equal(expectedVotesCount, actualVotesCount);
        }

        [Fact]
        public async Task VoteAsync_ShouldNotAddNewVoteToOtherOptions_WhenPollAndSelectedOptionExists()
        {
            await this.dbContext.Polls.AddAsync(new Poll
            {
                Id = "1",
                IsActive = true,
                Options = new List<PollOption>
                {
                    new PollOption{Id = 1, Votes = 0},
                    new PollOption{Id = 2, Votes = 0},
                    new PollOption{Id = 3, Votes = 0},
                }
            });

            await this.dbContext.SaveChangesAsync();

            var pollService = new PollService(this.dbContext);

            var optionIdToVote = 2;
            var activePollVoteInputModel = new ActivePollVoteInputModel
            {
                Id = "1",
                SelectedOptionId = optionIdToVote,
                VoteIp = Guid.NewGuid().ToString()
            };

            await pollService.VoteAsync(activePollVoteInputModel);

            var pollFromDb = this.dbContext.Polls.First();

            var notVotedOptions = pollFromDb.Options.Where(o => o.Id != optionIdToVote);

            Assert.True(notVotedOptions.All(o => o.Votes == 0));
        }

        [Fact]
        public async Task VoteAsync_ShouldAssignIpToNewVote_WhenPollAndSelectedOptionExists()
        {
            await this.dbContext.Polls.AddAsync(new Poll
            {
                Id = "1",
                IsActive = true,
                Options = new List<PollOption>
                {
                    new PollOption{Id = 1, Votes = 0},
                    new PollOption{Id = 2, Votes = 0},
                    new PollOption{Id = 3, Votes = 0},
                }
            });

            await this.dbContext.SaveChangesAsync();

            var pollService = new PollService(this.dbContext);

            var voteIp = Guid.NewGuid().ToString();

            var activePollVoteInputModel = new ActivePollVoteInputModel
            {
                Id = "1",
                SelectedOptionId = 1,
                VoteIp = voteIp
            };

            await pollService.VoteAsync(activePollVoteInputModel);

            var pollFromDb = this.dbContext.Polls.First();
            var vote = pollFromDb.Votes.SingleOrDefault(v => v.Ip == voteIp);
            Assert.NotNull(vote);
        }


        [Fact]
        public async Task VoteAsync_ShouldThrowNullIpException_WhenVoteIsNull()
        {
            await this.dbContext.Polls.AddAsync(new Poll
            {
                Id = "1",
                IsActive = true,
                Options = new List<PollOption>
                {
                    new PollOption{Id = 1, Votes = 0},
                    new PollOption{Id = 2, Votes = 0},
                    new PollOption{Id = 3, Votes = 0},
                }
            });

            await this.dbContext.SaveChangesAsync();

            var pollService = new PollService(this.dbContext);

            var activePollVoteInputModel = new ActivePollVoteInputModel
            {
                Id = "1",
                SelectedOptionId = 1,
                VoteIp = null
            };

            Assert.Throws<NullIpException>(() => pollService.VoteAsync(activePollVoteInputModel).GetAwaiter().GetResult());
        }

        [Fact]
        public async Task VoteAsync_ShouldThrowAlreadyVotedException_WhenVoteIpIsAlreadyAddedToThePoll()
        {
            var voteIp = Guid.NewGuid().ToString();

            await this.dbContext.Polls.AddAsync(new Poll
            {
                Id = "1",
                IsActive = true,
                Options = new List<PollOption>
                {
                    new PollOption{Id = 1, Votes = 0},
                    new PollOption{Id = 2, Votes = 0},
                    new PollOption{Id = 3, Votes = 0},
                },
                Votes = new List<PollVote>
                {
                    new PollVote{Ip = voteIp}
                }
            });

            await this.dbContext.SaveChangesAsync();

            var pollService = new PollService(this.dbContext);

            var activePollVoteInputModel = new ActivePollVoteInputModel
            {
                Id = "1",
                SelectedOptionId = 1,
                VoteIp = voteIp
            };

            Assert.Throws<AlreadyVotedException>(() => pollService.VoteAsync(activePollVoteInputModel).GetAwaiter().GetResult());
        }

        [Fact]
        public void VoteAsync_ShouldThrowInvalidPollException_WhenPollNotExists()
        {
            var pollService = new PollService(this.dbContext);

            var activePollVoteInputModel = new ActivePollVoteInputModel
            {
                Id = "1",
                SelectedOptionId = 1,
                VoteIp = Guid.NewGuid().ToString()
            };

            Assert.Throws<InvalidPollException>(() => pollService.VoteAsync(activePollVoteInputModel).GetAwaiter().GetResult());
        }

        [Fact]
        public async Task VoteAsync_ShouldThrowInvalidPollOptionException_WhenSelectedOptionNotExists()
        {
            await this.dbContext.Polls.AddAsync(new Poll
            {
                Id = "1",
                IsActive = true,
                Options = new List<PollOption>
                {
                    new PollOption{Id = 1, Votes = 0},
                    new PollOption{Id = 2, Votes = 0},
                    new PollOption{Id = 3, Votes = 0},
                }
            });

            await this.dbContext.SaveChangesAsync();

            var pollService = new PollService(this.dbContext);


            var activePollVoteInputModel = new ActivePollVoteInputModel
            {
                Id = "1",
                SelectedOptionId = 5,
                VoteIp = Guid.NewGuid().ToString()
            };

            Assert.Throws<InvalidPollOptionException>(() => pollService.VoteAsync(activePollVoteInputModel).GetAwaiter().GetResult());
        }

        internal class FakePoll
        {
            public string Id { get; set; }
        }
    }
}
