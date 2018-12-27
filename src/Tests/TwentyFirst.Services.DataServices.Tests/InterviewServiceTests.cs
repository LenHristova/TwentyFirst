namespace TwentyFirst.Services.DataServices.Tests
{
    using Common.Exceptions;
    using Common.Models.Images;
    using Common.Models.Interviews;
    using Data.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Xunit;

    public class InterviewServiceTests : DataServiceTests
    {
        [Fact]
        public async Task LatestAsync_ShouldReturnsCorrectCountOfInterviews()
        {
            await this.dbContext.Interviews.AddRangeAsync(new List<Interview>
            {
                new Interview { IsDeleted = true },
                new Interview { IsDeleted = false },
                new Interview { IsDeleted = true },
                new Interview { IsDeleted = false },
                new Interview { IsDeleted = false },
            });

            await this.dbContext.SaveChangesAsync();

            var interviewService = new InterviewService(this.dbContext);

            var givenCount = 2;
            var result = await interviewService.LatestAsync<FakeInterview>(givenCount);

            var expectedCount = givenCount;
            var actualCount = result.Count();
            Assert.Equal(expectedCount, actualCount);
        }

        [Fact]
        public async Task LatestAsync_ShouldReturnsAll_WhenGivenCountIsGreaterThenAvailable()
        {
            await this.dbContext.Interviews.AddRangeAsync(new List<Interview>
            {
                new Interview { IsDeleted = true },
                new Interview { IsDeleted = false },
            });

            await this.dbContext.SaveChangesAsync();

            var interviewService = new InterviewService(this.dbContext);

            var givenCount = 2;
            var result = await interviewService.LatestAsync<FakeInterview>(givenCount);

            var expectedCount = 1;
            var actualCount = result.Count();
            Assert.Equal(expectedCount, actualCount);
        }

        [Fact]
        public async Task LatestAsync_ShouldReturnsLatest()
        {
            await this.dbContext.Interviews.AddRangeAsync(new List<Interview>
            {
                new Interview { Id = "1", PublishedOn = DateTime.UtcNow.AddHours(-2) },
                new Interview { Id = "2", PublishedOn = DateTime.UtcNow },
                new Interview { Id = "3", PublishedOn = DateTime.UtcNow.AddHours(-20) },
                new Interview { Id = "4", PublishedOn = DateTime.UtcNow.AddHours(-12) },
                new Interview { Id = "5", PublishedOn = DateTime.UtcNow.AddHours(-8) },
            });

            await this.dbContext.SaveChangesAsync();

            var interviewService = new InterviewService(this.dbContext);
            var result = await interviewService.LatestAsync<FakeInterview>(3);
            var expectedIds = new List<string> { "2", "1", "5" };

            Assert.True(result.All(a => expectedIds.Contains(a.Id)));
        }

        [Fact]
        public async Task LatestAsync_ShouldReturnsOrderedByPublishedOnDescending()
        {
            await this.dbContext.Interviews.AddRangeAsync(new List<Interview>
            {
                new Interview { Id = "1", PublishedOn = DateTime.UtcNow.AddHours(-2) },
                new Interview { Id = "2", PublishedOn = DateTime.UtcNow },
                new Interview { Id = "3", PublishedOn = DateTime.UtcNow.AddHours(-20) },
                new Interview { Id = "4", PublishedOn = DateTime.UtcNow.AddHours(-12) },
                new Interview { Id = "5", PublishedOn = DateTime.UtcNow.AddHours(-8) },
            });

            await this.dbContext.SaveChangesAsync();

            var interviewService = new InterviewService(this.dbContext);
            var result = await interviewService.LatestAsync<FakeInterview>(3);
            var expectedIds = new List<string> { "2", "1", "5" };
            var actualInterviews = result.ToList();

            for (int i = 0; i < expectedIds.Count; i++)
            {
                Assert.Equal(expectedIds[i], actualInterviews[i].Id);
            }
        }

        [Fact]
        public async Task GetAsync_ShouldGetCorrectInterview_WhenInterviewExists()
        {
            await this.dbContext.Interviews.AddRangeAsync(new List<Interview>()
            {
                new Interview {Id = "1"},
                new Interview {Id = "2"},
                new Interview {Id = "3"},
            });

            await this.dbContext.SaveChangesAsync();

            var interviewService = new InterviewService(this.dbContext);

            var wantedInterviewId = "2";
            var expectedInterviewId = wantedInterviewId;
            var interview = await interviewService.GetAsync(wantedInterviewId);

            Assert.Equal(expectedInterviewId, interview.Id);
        }

        [Fact]
        public void GetAsync_ShouldThrowInvalidInterviewException_WhenInterviewNotExists()
        {
            var interviewService = new InterviewService(this.dbContext);
            Assert.Throws<InvalidInterviewException>(
                () => interviewService.GetAsync(Guid.NewGuid().ToString()).GetAwaiter().GetResult());
        }

        [Fact]
        public async Task GetAsync_Projection_ShouldGetCorrectInterview_WhenInterviewExists()
        {
            await this.dbContext.Interviews.AddRangeAsync(new List<Interview>()
            {
                new Interview {Id = "1"},
                new Interview {Id = "2"},
                new Interview {Id = "3"},
            });

            await this.dbContext.SaveChangesAsync();

            var interviewService = new InterviewService(this.dbContext);

            var wantedInterviewId = "2";
            var expectedInterviewId = wantedInterviewId;
            var interview = await interviewService.GetAsync<FakeInterview>(wantedInterviewId);

            Assert.Equal(expectedInterviewId, interview.Id);
        }

        [Fact]
        public void GetAsync_Projection_ShouldThrowInvalidInterviewException_WhenInterviewNotExists()
        {
            var interviewService = new InterviewService(this.dbContext);
            Assert.Throws<InvalidInterviewException>(
                () => interviewService.GetAsync<FakeInterview>(Guid.NewGuid().ToString()).GetAwaiter().GetResult());
        }

        [Fact]
        public async Task CreateAsync_ShouldAddNewInterviewToDb()
        {
            var interviewService = new InterviewService(this.dbContext);

            var creatorId = Guid.NewGuid().ToString();
            await interviewService.CreateAsync(new InterviewCreateInputModel(), creatorId);
            const int expected = 1;
            var actual = this.dbContext.Interviews.Count();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task CreateAsync_ShouldAddCorrectCreatorToNewInterview()
        {
            var interviewService = new InterviewService(this.dbContext);

            var creatorId = Guid.NewGuid().ToString();
            await interviewService.CreateAsync(new InterviewCreateInputModel(), creatorId);
            var interviewFromDb = this.dbContext.Interviews.FirstOrDefault();

            Assert.Equal(creatorId, interviewFromDb?.CreatorId);
        }

        [Fact]
        public async Task EditAsync_ShouldSaveChangesToDb_WhenInterviewExists()
        {
            var interviewToEditId = Guid.NewGuid().ToString();
            var interview = new Interview
            {
                Id = interviewToEditId,
                Title = "CurrentTitle",
                Interviewed = "CurrentInterviewed",
                Author = "CurrentAuthor",
                Content = "CurrentContent",
                Image = new Image { Id = "1" },
            };

            await this.dbContext.Interviews.AddAsync(interview);
            await this.dbContext.SaveChangesAsync();

            var interviewWithEdits = new InterviewEditInputModel()
            {
                Id = interviewToEditId,
                Title = "NewTitle",
                Interviewed = "NewInterviewed",
                Author = "NewAuthor",
                Content = "NewContent",
                Image = new ImageBaseInputModel { Id = "2" },
            };

            var interviewService = new InterviewService(this.dbContext);

            var editorId = Guid.NewGuid().ToString();
            await interviewService.EditAsync(interviewWithEdits, editorId);

            var actualInterview = this.dbContext.Interviews.First();

            Assert.Equal(interviewWithEdits.Id, actualInterview.Id);
            Assert.Equal(interviewWithEdits.Title, actualInterview.Title);
            Assert.Equal(interviewWithEdits.Interviewed, actualInterview.Interviewed);
            Assert.Equal(interviewWithEdits.Author, actualInterview.Author);
            Assert.Equal(interviewWithEdits.Content, actualInterview.Content);
            Assert.Equal(interviewWithEdits.Content, actualInterview.Content);
            Assert.Equal(interviewWithEdits.Image.Id, actualInterview.ImageId);
        }

        [Fact]
        public async Task EditAsync_ShouldAddCorrectEditToInterview_WhenInterviewExists()
        {
            var interviewToEditId = Guid.NewGuid().ToString();
            var interview = new Interview() { Id = interviewToEditId };

            await this.dbContext.Interviews.AddAsync(interview);
            await this.dbContext.SaveChangesAsync();

            var interviewWithEdits = new InterviewEditInputModel { Id = interviewToEditId };

            var interviewService = new InterviewService(this.dbContext);

            var editorId = Guid.NewGuid().ToString();
            await interviewService.EditAsync(interviewWithEdits, editorId);

            var actualInterview = this.dbContext.Interviews.First();

            Assert.Equal(editorId, actualInterview.Edits.FirstOrDefault()?.EditorId);
        }

        [Fact]
        public void EditAsync_ShouldThrowInvalidInterviewException_WhenInterviewNotExists()
        {
            var interviewWithEdits = new InterviewEditInputModel { Id = Guid.NewGuid().ToString() };

            var interviewService = new InterviewService(this.dbContext);

            Assert.Throws<InvalidInterviewException>(
                () => interviewService.EditAsync(interviewWithEdits, Guid.NewGuid().ToString()).GetAwaiter().GetResult());
        }

        [Fact]
        public async Task EditAsync_ShouldThrowInvalidInterviewException_WhenIdExistsButInterviewIsSoftDeleted()
        {
            var interviewId = Guid.NewGuid().ToString();
            await this.dbContext.Interviews.AddAsync(new Interview { Id = interviewId, IsDeleted = true });
            await this.dbContext.SaveChangesAsync();

            var interviewService = new InterviewService(this.dbContext);
            var interviewWithEdits = new InterviewEditInputModel { Id = interviewId };
            Assert.Throws<InvalidInterviewException>(
                () => interviewService.EditAsync(interviewWithEdits, Guid.NewGuid().ToString()).GetAwaiter().GetResult());
        }

        [Fact]
        public async Task DeleteAsync_ShouldSoftDeleteInterview_WhenInterviewExists()
        {
            var interviewId = Guid.NewGuid().ToString();
            await this.dbContext.Interviews.AddAsync(new Interview { Id = interviewId, IsDeleted = false });
            await this.dbContext.SaveChangesAsync();

            var interviewService = new InterviewService(this.dbContext);
            var editorId = Guid.NewGuid().ToString();
            await interviewService.DeleteAsync(interviewId, editorId);

            var resultInterview = await this.dbContext.Interviews.FindAsync(interviewId);
            Assert.True(resultInterview.IsDeleted);
        }

        [Fact]
        public async Task DeleteAsync_ShouldAddCorrectEditToInterview_WhenInterviewExists()
        {
            var interviewId = Guid.NewGuid().ToString();
            await this.dbContext.Interviews.AddAsync(new Interview { Id = interviewId, IsDeleted = false });
            await this.dbContext.SaveChangesAsync();

            var interviewService = new InterviewService(this.dbContext);
            var editorId = Guid.NewGuid().ToString();
            await interviewService.DeleteAsync(interviewId, editorId);

            var resultInterview = await this.dbContext.Interviews.FindAsync(interviewId);

            Assert.Equal(editorId, resultInterview.Edits.FirstOrDefault()?.EditorId);
        }

        [Fact]
        public void DeleteAsync_ShouldThrowInvalidInterviewException_WhenInterviewNotExists()
        {
            var interviewService = new InterviewService(this.dbContext);

            Assert.Throws<InvalidInterviewException>(
                () => interviewService.DeleteAsync(Guid.NewGuid().ToString(), Guid.NewGuid().ToString()).GetAwaiter().GetResult());
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrowInvalidInterviewException_WhenIdExistsButInterviewIsSoftDeleted()
        {
            var interviewToDeleteId = Guid.NewGuid().ToString();
            await this.dbContext.Interviews.AddAsync(new Interview { Id = interviewToDeleteId, IsDeleted = true });
            await this.dbContext.SaveChangesAsync();

            var interviewService = new InterviewService(this.dbContext);

            Assert.Throws<InvalidInterviewException>(
                () => interviewService.DeleteAsync(interviewToDeleteId, Guid.NewGuid().ToString()).GetAwaiter().GetResult());
        }

        internal class FakeInterview
        {
            public string Id { get; set; }
        }
    }
}
