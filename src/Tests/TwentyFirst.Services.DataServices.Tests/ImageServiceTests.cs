namespace TwentyFirst.Services.DataServices.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Common.Exceptions;
    using Common.Models.Images;
    using Data.Models;
    using Xunit;

    public class ImageServiceTests : DataServiceTests
    {
        [Fact]
        public async Task GetBySearchTermAsync_ShouldReturnsCorrectCountOfImages_WhenIncludeSearchTermInDescription()
        {
            var searchTerm = "test";
            await this.dbContext.Images.AddRangeAsync(new List<Image>
            {
                new Image { IsDeleted = true, Description = searchTerm },
                new Image { IsDeleted = false, Description = searchTerm },
                new Image { IsDeleted = false, Description = "abc"},
                new Image { IsDeleted = false, Description = $"{searchTerm} " },
                new Image { IsDeleted = false, Description = $"abc {searchTerm}" },
                new Image { IsDeleted = false, Description = "not"},
                new Image { IsDeleted = false, Description = $"{searchTerm} abc" },
                new Image { IsDeleted = false, Description = $"a{searchTerm}z" },
                new Image { IsDeleted = false, Description = $"abc a{searchTerm}z" },
                new Image { IsDeleted = false, Description = $"a{searchTerm}z abc" },
                new Image { IsDeleted = false, Description = ""},
            });

            await this.dbContext.SaveChangesAsync();

            var imageService = new ImageService(this.dbContext);

            var result = await imageService.GetBySearchTermAsync<FakeImage>(searchTerm);

            var expectedCount = 7;
            var actualCount = result.Count();
            Assert.Equal(expectedCount, actualCount);
        }

        [Fact]
        public async Task GetBySearchTermAsync_ShouldSearchCaseInsensitive()
        {
            var searchTerm = "TeSt";
            await this.dbContext.Images.AddRangeAsync(new List<Image>
            {
                new Image { Description = searchTerm.ToLower() },
                new Image { Description = searchTerm.ToUpper() },
                new Image { Description = "abc"},
                new Image { Description = $"{searchTerm.ToLower()} " },
                new Image { Description = $"abc {searchTerm.ToUpper()}" },
                new Image { Description = "not"},
                new Image { Description = "tEst abc" },
                new Image { Description = "teSTz" },
                new Image { Description = "newteSTz" },
                new Image { Description = ""},
            });

            await this.dbContext.SaveChangesAsync();

            var imageService = new ImageService(this.dbContext);

            var result = await imageService.GetBySearchTermAsync<FakeImage>(searchTerm);

            var expectedCount = 7;
            var actualCount = result.Count();
            Assert.Equal(expectedCount, actualCount);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public async Task GetBySearchTermAsync_ShouldReturnsAllImages_WhenSearchTermIsNullOrEmpty(string searchTerm)
        {            await this.dbContext.Images.AddRangeAsync(new List<Image>
            {
                new Image { IsDeleted = true, Description = "abc" },
                new Image { IsDeleted = false, Description = "abc"},
                new Image { IsDeleted = false, Description = "abc test" },
                new Image { IsDeleted = false, Description = ""},
            });

            await this.dbContext.SaveChangesAsync();

            var imageService = new ImageService(this.dbContext);

            var result = await imageService.GetBySearchTermAsync<FakeImage>(searchTerm);

            var expectedCount = 3;
            var actualCount = result.Count();
            Assert.Equal(expectedCount, actualCount);
        }

        [Fact]
        public async Task GetBySearchTermAsync_ShouldReturnsOrderedByCreatedOnDescending()
        {
            var searchTerm = "test";
            await this.dbContext.Images.AddRangeAsync(new List<Image>
            {
                new Image { Id = "1", CreatedOn = DateTime.UtcNow.AddDays(-10), Description = $"{searchTerm} abc" },
                new Image { Id = "2", CreatedOn = DateTime.UtcNow.AddDays(-1), Description = $"a{searchTerm}z" },
                new Image { Id = "3", CreatedOn = DateTime.UtcNow.AddDays(-13), Description = $"abc a{searchTerm}z" },
                new Image { Id = "4", CreatedOn = DateTime.UtcNow.AddDays(-3), Description = $"a{searchTerm}z abc" },
                new Image { Id = "5", CreatedOn = DateTime.UtcNow.AddDays(-20), Description = $"a abc{searchTerm}z abc"},
            });

            await this.dbContext.SaveChangesAsync();

            var imageService = new ImageService(this.dbContext);

            var result = await imageService.GetBySearchTermAsync<FakeImage>(searchTerm);

            var expectedIds = new List<string> { "2","4", "1", "3", "5" };
            var actualImages = result.ToList();

            for (int i = 0; i < expectedIds.Count; i++)
            {
                Assert.Equal(expectedIds[i], actualImages[i].Id);
            }

        }

        [Fact]
        public async Task GetBySearchTermWithDeletedAsync_ShouldReturnsCorrectCountOfImages_WhenIncludeSearchTermInDescription()
        {
            var searchTerm = "test";
            await this.dbContext.Images.AddRangeAsync(new List<Image>
            {
                new Image { IsDeleted = true, Description = searchTerm },
                new Image { IsDeleted = false, Description = searchTerm },
                new Image { IsDeleted = false, Description = "abc"},
                new Image { IsDeleted = true, Description = $"{searchTerm} " },
                new Image { IsDeleted = false, Description = $"abc {searchTerm}" },
                new Image { IsDeleted = false, Description = "not"},
                new Image { IsDeleted = false, Description = $"{searchTerm} abc" },
                new Image { IsDeleted = true, Description = $"a{searchTerm}z" },
                new Image { IsDeleted = false, Description = $"abc a{searchTerm}z" },
                new Image { IsDeleted = false, Description = $"a{searchTerm}z abc" },
                new Image { IsDeleted = false, Description = ""},
            });

            await this.dbContext.SaveChangesAsync();

            var imageService = new ImageService(this.dbContext);

            var result = await imageService.GetBySearchTermWithDeletedAsync<FakeImage>(searchTerm);

            var expectedCount = 8;
            var actualCount = result.Count();
            Assert.Equal(expectedCount, actualCount);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public async Task GetBySearchTermWithDeletedAsync_ShouldReturnsAllImages_WhenSearchTermIsNullOrEmpty(string searchTerm)
        {
            await this.dbContext.Images.AddRangeAsync(new List<Image>
            {
                new Image { IsDeleted = true, Description = "abc" },
                new Image { IsDeleted = false, Description = "abc"},
                new Image { IsDeleted = true, Description = "abc test" },
                new Image { IsDeleted = false, Description = ""},
            });

            await this.dbContext.SaveChangesAsync();

            var imageService = new ImageService(this.dbContext);

            var result = await imageService.GetBySearchTermWithDeletedAsync<FakeImage>(searchTerm);

            var expectedCount = 4;
            var actualCount = result.Count();
            Assert.Equal(expectedCount, actualCount);
        }

        [Fact]
        public async Task GetBySearchTermWithDeletedAsync_ShouldReturnsOrderedByCreatedOnDescending()
        {
            var searchTerm = "test";
            await this.dbContext.Images.AddRangeAsync(new List<Image>
            {
                new Image { Id = "1", IsDeleted = true, CreatedOn = DateTime.UtcNow.AddDays(-10), Description = $"{searchTerm} abc" },
                new Image { Id = "2", IsDeleted = false, CreatedOn = DateTime.UtcNow.AddDays(-1), Description = $"a{searchTerm}z" },
                new Image { Id = "3", IsDeleted = true, CreatedOn = DateTime.UtcNow.AddDays(-13), Description = $"abc a{searchTerm}z" },
                new Image { Id = "4", IsDeleted = false, CreatedOn = DateTime.UtcNow.AddDays(-3), Description = $"a{searchTerm}z abc" },
                new Image { Id = "5", IsDeleted = true, CreatedOn = DateTime.UtcNow.AddDays(-20), Description = $"a abc{searchTerm}z abc"},
            });

            await this.dbContext.SaveChangesAsync();

            var imageService = new ImageService(this.dbContext);

            var result = await imageService.GetBySearchTermWithDeletedAsync<FakeImage>(searchTerm);

            var expectedIds = new List<string> { "2", "4", "1", "3", "5" };
            var actualImages = result.ToList();

            for (int i = 0; i < expectedIds.Count; i++)
            {
                Assert.Equal(expectedIds[i], actualImages[i].Id);
            }

        }

        [Fact]
        public async Task GetBySearchTermWithDeletedAsync_ShouldSearchCaseInsensitive()
        {
            var searchTerm = "TeSt";
            await this.dbContext.Images.AddRangeAsync(new List<Image>
            {
                new Image { Description = searchTerm.ToLower() },
                new Image { Description = searchTerm.ToUpper() },
                new Image { Description = "abc"},
                new Image { Description = $"{searchTerm.ToLower()} " },
                new Image { Description = $"abc {searchTerm.ToUpper()}" },
                new Image { Description = "not"},
                new Image { Description = "tEst abc" },
                new Image { Description = "teSTz" },
                new Image { Description = "newteSTz" },
                new Image { Description = ""},
            });

            await this.dbContext.SaveChangesAsync();

            var imageService = new ImageService(this.dbContext);

            var result = await imageService.GetBySearchTermWithDeletedAsync<FakeImage>(searchTerm);

            var expectedCount = 7;
            var actualCount = result.Count();
            Assert.Equal(expectedCount, actualCount);
        }

        [Fact]
        public async Task GetAsync_ShouldGetCorrectImage_WhenImageExists()
        {
            await this.dbContext.Images.AddRangeAsync(new List<Image>()
            {
                new Image {Id = "1"},
                new Image {Id = "2"},
                new Image {Id = "3"},
            });

            await this.dbContext.SaveChangesAsync();

            var imageService = new ImageService(this.dbContext);

            var wantedImageId = "2";
            var expectedIImageId = wantedImageId;
            var image = await imageService.GetAsync(wantedImageId);

            Assert.Equal(expectedIImageId, image.Id);
        }

        [Fact]
        public void GetAsync_ShouldThrowInvalidImageException_WhenImageNotExists()
        {
            var imageService = new ImageService(this.dbContext);
            Assert.Throws<InvalidImageException>(
                () => imageService.GetAsync(Guid.NewGuid().ToString()).GetAwaiter().GetResult());
        }

        [Fact]
        public async Task GetAsync_Projection_ShouldGetCorrectImage_WhenImageExists()
        {
            await this.dbContext.Images.AddRangeAsync(new List<Image>()
            {
                new Image {Id = "1"},
                new Image {Id = "2"},
                new Image {Id = "3"},
            });

            await this.dbContext.SaveChangesAsync();

            var imageService = new ImageService(this.dbContext);

            var wantedImageId = "2";
            var expectedIImageId = wantedImageId;
            var image = await imageService.GetAsync<FakeImage>(wantedImageId);

            Assert.Equal(expectedIImageId, image.Id);
        }

        [Fact]
        public void GetAsync_Projection_ShouldThrowInvalidImageException_WhenImageNotExists()
        {
            var imageService = new ImageService(this.dbContext);
            Assert.Throws<InvalidImageException>(
                () => imageService.GetAsync<FakeImage>(Guid.NewGuid().ToString()).GetAwaiter().GetResult());
        }

        [Fact]
        public async Task GetDeletedAsync_ShouldGetCorrectImage_WhenImageExistsAndIsSoftDeleted()
        {
            await this.dbContext.Images.AddRangeAsync(new List<Image>()
            {
                new Image {Id = "1", IsDeleted = true },
                new Image {Id = "2", IsDeleted = true },
                new Image {Id = "3", IsDeleted = true },
            });

            await this.dbContext.SaveChangesAsync();

            var imageService = new ImageService(this.dbContext);

            var wantedImageId = "2";
            var expectedImageId = wantedImageId;
            var image = await imageService.GetDeletedAsync(wantedImageId);

            Assert.Equal(expectedImageId, image.Id);
        }

        [Fact]
        public void GetDeletedAsync_ShouldThrowInvalidImageException_WhenImageNotExists()
        {
            var imageService = new ImageService(this.dbContext);
            Assert.Throws<InvalidImageException>(
                () => imageService.GetDeletedAsync(Guid.NewGuid().ToString()).GetAwaiter().GetResult());
        }

        [Fact]
        public async Task GetDeletedAsync_Projection_ShouldGetCorrectImage_WhenImageExistsAndIsSoftDeleted()
        {
            await this.dbContext.Images.AddRangeAsync(new List<Image>()
            {
                new Image {Id = "1", IsDeleted = true },
                new Image {Id = "2", IsDeleted = true },
                new Image {Id = "3", IsDeleted = true },
            });

            await this.dbContext.SaveChangesAsync();

            var imageService = new ImageService(this.dbContext);

            var wantedImageId = "2";
            var expectedImageId = wantedImageId;
            var image = await imageService.GetDeletedAsync<FakeImage>(wantedImageId);

            Assert.Equal(expectedImageId, image.Id);
        }

        [Fact]
        public void GetDeletedAsync_Projection_ShouldThrowInvalidImageException_WhenImageNotExists()
        {
            var imageService = new ImageService(this.dbContext);
            Assert.Throws<InvalidImageException>(
                () => imageService.GetDeletedAsync<FakeImage>(Guid.NewGuid().ToString()).GetAwaiter().GetResult());
        }

        [Fact]
        public async Task GetDeletedAsync_ShouldThrowInvalidImageException_WhenImageExistsButIsNotSoftDeleted()
        {
            var imageId = Guid.NewGuid().ToString();
            await this.dbContext.Images.AddAsync(new Image { Id = imageId, IsDeleted = false });
            await this.dbContext.SaveChangesAsync();

            var imageService = new ImageService(this.dbContext);
            Assert.Throws<InvalidImageException>(
                () => imageService.GetDeletedAsync(imageId).GetAwaiter().GetResult());
        }

        [Fact]
        public async Task CreateAsync_ShouldAddNewImageToDb()
        {
            var imageService = new ImageService(this.dbContext);

            var creatorId = Guid.NewGuid().ToString();
            var url = Guid.NewGuid().ToString();
            var thumbUrl = Guid.NewGuid().ToString();
            await imageService.CreateAsync(new ImagesCreateInputModel(), creatorId, url, thumbUrl);
            const int expected = 1;
            var actual = this.dbContext.Images.Count();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task CreateAsync_ShouldAddCorrectCreatorToNewImage()
        {
            var imageService = new ImageService(this.dbContext);

            var creatorId = Guid.NewGuid().ToString();
            var url = Guid.NewGuid().ToString();
            var thumbUrl = Guid.NewGuid().ToString();
            await imageService.CreateAsync(new ImagesCreateInputModel(), creatorId, url, thumbUrl);
            var imageFromDb = this.dbContext.Images.FirstOrDefault();

            Assert.Equal(creatorId, imageFromDb?.CreatorId);
        }

        [Fact]
        public async Task DeleteAsync_ShouldSoftDeleteImage_WhenImageExists()
        {
            var imageId = Guid.NewGuid().ToString();
            await this.dbContext.Images.AddAsync(new Image { Id = imageId, IsDeleted = false });
            await this.dbContext.SaveChangesAsync();

            var imageService = new ImageService(this.dbContext);
            await imageService.DeleteAsync(imageId);

            var resultImage = await this.dbContext.Images.FindAsync(imageId);
            Assert.True(resultImage.IsDeleted);
        }

        [Fact]
        public void DeleteAsync_ShouldThrowInvalidImageException_WhenImageNotExists()
        {
            var imageService = new ImageService(this.dbContext);

            Assert.Throws<InvalidImageException>(
                () => imageService.DeleteAsync(Guid.NewGuid().ToString()).GetAwaiter().GetResult());
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrowInvalidCImageException_WhenIdExistsButImageIsSoftDeleted()
        {
            var imageId = Guid.NewGuid().ToString();
            await this.dbContext.Images.AddAsync(new Image { Id = imageId, IsDeleted = true });
            await this.dbContext.SaveChangesAsync();
            var imageService = new ImageService(this.dbContext);

            Assert.Throws<InvalidImageException>(
                () => imageService.DeleteAsync(imageId).GetAwaiter().GetResult());
        }

        [Fact]
        public async Task RecoverAsync_ShouldRecoverImage_WhenImageExistsAndIsSoftDeleted()
        {
            var imageId = Guid.NewGuid().ToString();
            await this.dbContext.Images.AddAsync(new Image { Id = imageId, IsDeleted = true });
            await this.dbContext.SaveChangesAsync();

            var imageService = new ImageService(this.dbContext);
            await imageService.RecoverAsync(imageId);

            var resultImage = await this.dbContext.Images.FindAsync(imageId);
            Assert.False(resultImage.IsDeleted);
        }

        [Fact]
        public void RecoverAsync_ShouldThrowInvalidImageException_WhenImageNotExists()
        {
            var imageService = new ImageService(this.dbContext);

            Assert.Throws<InvalidImageException>(
                () => imageService.RecoverAsync(Guid.NewGuid().ToString()).GetAwaiter().GetResult());
        }

        [Fact]
        public async Task RecoverAsync_ShouldThrowInvalidImageException_WhenIdExistsButImageIsNotSoftDeleted()
        {
            var imageId = Guid.NewGuid().ToString();
            await this.dbContext.Images.AddAsync(new Image { Id = imageId, IsDeleted = false });
            await this.dbContext.SaveChangesAsync();
            var imageService = new ImageService(this.dbContext);

            Assert.Throws<InvalidImageException>(
                () => imageService.RecoverAsync(imageId).GetAwaiter().GetResult());
        }

        internal class FakeImage
        {
            public string Id { get; set; }
        }
    }
}
