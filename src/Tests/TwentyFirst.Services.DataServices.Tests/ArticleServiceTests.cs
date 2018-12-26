namespace TwentyFirst.Services.DataServices.Tests
{
    using Common.Exceptions;
    using Common.Models.Articles;
    using Common.Models.Images;
    using Contracts;
    using Data.Models;
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Xunit;

    public class ArticleServiceTests : DataServiceTests
    {
        [Fact]
        public async Task LatestAsync_ShouldReturnsCorrectCountOfArticles()
        {
            await this.dbContext.Articles.AddRangeAsync(new List<Article>
            {
                new Article { IsDeleted = true },
                new Article { IsDeleted = false },
                new Article { IsDeleted = true },
                new Article { IsDeleted = false },
                new Article { IsDeleted = false },
            });

            await this.dbContext.SaveChangesAsync();

            var articleService = new ArticleService(this.dbContext, null);

            var givenCount = 2;
            var result = await articleService.LatestAsync<FakeArticle>(givenCount);

            var expectedCount = givenCount;
            var actualCount = result.Count();
            Assert.Equal(expectedCount, actualCount);
        }

        [Fact]
        public async Task LatestAsync_ShouldReturnsAll_WhenGivenCountIsGreaterThenAvailable()
        {
            await this.dbContext.Articles.AddAsync(new Article { IsDeleted = true });
            await this.dbContext.Articles.AddAsync(new Article { IsDeleted = false });
            await this.dbContext.SaveChangesAsync();

            var articleService = new ArticleService(this.dbContext, null);

            var givenCount = 2;
            var result = await articleService.LatestAsync<FakeArticle>(givenCount);

            var expectedCount = 1;
            var actualCount = result.Count();
            Assert.Equal(expectedCount, actualCount);
        }

        [Fact]
        public async Task LatestAsync_ShouldReturnsLatest()
        {
            await this.dbContext.Articles.AddRangeAsync(new List<Article>
            {
                new Article { Id = "1", PublishedOn = DateTime.UtcNow.AddHours(-2) },
                new Article { Id = "2", PublishedOn = DateTime.UtcNow },
                new Article { Id = "3", PublishedOn = DateTime.UtcNow.AddHours(-20) },
                new Article { Id = "4", PublishedOn = DateTime.UtcNow.AddHours(-12) },
                new Article { Id = "5", PublishedOn = DateTime.UtcNow.AddHours(-8) },
            });

            await this.dbContext.SaveChangesAsync();

            var articleService = new ArticleService(this.dbContext, null);
            var result = await articleService.LatestAsync<FakeArticle>(3);
            var expectedIds = new List<string> { "2", "1", "5" };

            Assert.True(result.All(a => expectedIds.Contains(a.Id)));
        }

        [Fact]
        public async Task LatestAsync_ShouldReturnsOrderedByPublishedOnDescending()
        {
            await this.dbContext.Articles.AddRangeAsync(new List<Article>
            {
                new Article { Id = "1", IsDeleted = true, PublishedOn = DateTime.UtcNow },
                new Article { Id = "2", IsDeleted = false, PublishedOn = DateTime.UtcNow.AddHours(-2) },
                new Article { Id = "3", IsDeleted = false, PublishedOn = DateTime.UtcNow.AddHours(-20) },
                new Article { Id = "4", IsDeleted = false, PublishedOn = DateTime.UtcNow.AddHours(-12) },
                new Article { Id = "5", IsDeleted = false, PublishedOn = DateTime.UtcNow.AddHours(-8) },
            });

            await this.dbContext.SaveChangesAsync();

            var articleService = new ArticleService(this.dbContext, null);
            var result = await articleService.LatestAsync<FakeArticle>(3);
            var expectedIds = new List<string> { "2", "5", "4" };
            var actualArticles = result.ToList();

            for (int i = 0; i < expectedIds.Count; i++)
            {
                Assert.Equal(expectedIds[i], actualArticles[i].Id);
            }
        }

        [Fact]
        public async Task LatestTopAsync_ShouldReturnsCorrectCountOfTopArticles()
        {
            await this.dbContext.Articles.AddRangeAsync(new List<Article>
            {
                new Article { IsDeleted = false, IsTop = true },
                new Article { IsDeleted = true, IsTop = true },
                new Article { IsDeleted = false, IsTop = true },
                new Article { IsDeleted = false, IsTop = false },
                new Article { IsDeleted = false, IsTop = true },
                new Article { IsDeleted = false, IsTop = true },
                new Article { IsDeleted = false, IsTop = true },
            });

            await this.dbContext.SaveChangesAsync();

            var wantedArticlesCount = 3;
            var expectedCount = wantedArticlesCount;

            var articleService = new ArticleService(this.dbContext, null);
            var actual = await articleService.LatestTopAsync<FakeArticle>(wantedArticlesCount);

            Assert.Equal(expectedCount, actual.Count());
        }

        [Fact]
        public async Task LatestTopAsync_ShouldReturnsAllTop_WhenGivenCountIsGreaterThenAvailable()
        {
            await this.dbContext.Articles.AddRangeAsync(new List<Article>
            {
                new Article { IsDeleted = false, IsTop = true },
                new Article { IsDeleted = true, IsTop = true },
                new Article { IsDeleted = false, IsTop = false },
            });

            await this.dbContext.SaveChangesAsync();

            var wantedArticlesCount = 3;
            var expectedCount = 1;

            var articleService = new ArticleService(this.dbContext, null);
            var actual = await articleService.LatestTopAsync<FakeArticle>(wantedArticlesCount);

            Assert.Equal(expectedCount, actual.Count());
        }

        [Fact]
        public async Task LatestTopAsync_ShouldReturnsLatestTop()
        {
            await this.dbContext.Articles.AddRangeAsync(new List<Article>
            {
                new Article { Id = "1", IsTop = true, PublishedOn = DateTime.UtcNow.AddHours(-2) },
                new Article { Id = "2", IsTop = true, PublishedOn = DateTime.UtcNow },
                new Article { Id = "3", IsTop = true, PublishedOn = DateTime.UtcNow.AddHours(-20) },
                new Article { Id = "4", IsTop = true, PublishedOn = DateTime.UtcNow.AddHours(-12) },
                new Article { Id = "5", IsTop = true, PublishedOn = DateTime.UtcNow.AddHours(-8) },
                new Article { Id = "6", IsTop = false, PublishedOn = DateTime.UtcNow.AddHours(-1) },
                new Article { Id = "7", IsTop = false, PublishedOn = DateTime.UtcNow.AddHours(-3) },
            });

            await this.dbContext.SaveChangesAsync();

            var articleService = new ArticleService(this.dbContext, null);
            var result = await articleService.LatestTopAsync<FakeArticle>(3);
            var expectedIds = new List<string> { "2", "1", "5" };

            Assert.True(result.All(a => expectedIds.Contains(a.Id)));
        }

        [Fact]
        public async Task LatestTopAsync_ShouldReturnsTopOrderedByPublishedOnDescending()
        {
            await this.dbContext.Articles.AddRangeAsync(new List<Article>
            {
                new Article { Id = "1", IsTop = true, PublishedOn = DateTime.UtcNow.AddHours(-2) },
                new Article { Id = "2", IsTop = true, PublishedOn = DateTime.UtcNow },
                new Article { Id = "3", IsTop = true, PublishedOn = DateTime.UtcNow.AddHours(-20) },
                new Article { Id = "4", IsTop = true, PublishedOn = DateTime.UtcNow.AddHours(-12) },
                new Article { Id = "5", IsTop = true, PublishedOn = DateTime.UtcNow.AddHours(-8) },
                new Article { Id = "6", IsTop = false, PublishedOn = DateTime.UtcNow.AddHours(-1) },
                new Article { Id = "7", IsTop = false, PublishedOn = DateTime.UtcNow.AddHours(-3) },
            });

            await this.dbContext.SaveChangesAsync();

            var articleService = new ArticleService(this.dbContext, null);
            var result = await articleService.LatestTopAsync<FakeArticle>(3);
            var expectedIds = new List<string> { "2", "1", "5" };
            var actualArticles = result.ToList();

            for (int i = 0; i < expectedIds.Count; i++)
            {
                Assert.Equal(expectedIds[i], actualArticles[i].Id);
            }
        }

        [Fact]
        public async Task LatestFromCategoryAsync_ShouldReturnsCorrectCountOfArticlesFromGivenCategory_WhenCategoryExists()
        {
            await this.dbContext.Articles.AddRangeAsync(new List<Article>
            {
                new Article { IsDeleted = false, Categories = new List<ArticleCategory>
                {
                    new ArticleCategory{CategoryId = "1"},
                    new ArticleCategory{CategoryId = "2"}
                }},
                new Article { IsDeleted = false, Categories = new List<ArticleCategory>
                {
                    new ArticleCategory{CategoryId = "2"}
                }},
                new Article { IsDeleted = false, Categories = new List<ArticleCategory>
                {
                    new ArticleCategory{CategoryId = "1"},
                    new ArticleCategory{CategoryId = "3"},
                    new ArticleCategory{CategoryId = "4"}
                }},
                new Article { IsDeleted = true, Categories = new List<ArticleCategory>
                {
                    new ArticleCategory{CategoryId = "1"},
                    new ArticleCategory{CategoryId = "3"},
                    new ArticleCategory{CategoryId = "2"}
                }},
                new Article { IsDeleted = false, Categories = new List<ArticleCategory>
                {
                    new ArticleCategory{CategoryId = "1"},
                    new ArticleCategory{CategoryId = "3"},
                    new ArticleCategory{CategoryId = "2"}
                }},
                new Article { IsDeleted = false, Categories = new List<ArticleCategory>
                {
                    new ArticleCategory{CategoryId = "1"},
                    new ArticleCategory{CategoryId = "3"},
                    new ArticleCategory{CategoryId = "2"}
                }},
            });

            await this.dbContext.SaveChangesAsync();

            var wantedCategoryId = "2";
            var expectedCount = 2;

            var mock = new Mock<ICategoryService>();
            mock.Setup(m => m.VerifyExistent(wantedCategoryId));
            var articleService = new ArticleService(this.dbContext, mock.Object);
            var actual = await articleService.LatestFromCategoryAsync<FakeArticle>(wantedCategoryId, 2);

            Assert.Equal(expectedCount, actual.Count());
        }

        [Fact]
        public async Task LatestFromCategoryAsync_ShouldReturnsAllArticlesFromGivenCategory_WhenGivenCountIsGreaterThenAvailableAndCategoryExists()
        {
            await this.dbContext.Articles.AddRangeAsync(new List<Article>
            {
                new Article { IsDeleted = false, Categories = new List<ArticleCategory>
                {
                    new ArticleCategory{CategoryId = "1"},
                    new ArticleCategory{CategoryId = "2"}
                }},
                new Article { IsDeleted = false, Categories = new List<ArticleCategory>
                {
                    new ArticleCategory{CategoryId = "2"}
                }},
                new Article { IsDeleted = false, Categories = new List<ArticleCategory>
                {
                    new ArticleCategory{CategoryId = "1"},
                    new ArticleCategory{CategoryId = "3"},
                    new ArticleCategory{CategoryId = "4"}
                }},
                new Article { IsDeleted = true, Categories = new List<ArticleCategory>
                {
                    new ArticleCategory{CategoryId = "1"},
                    new ArticleCategory{CategoryId = "3"},
                    new ArticleCategory{CategoryId = "2"}
                }},
            });

            await this.dbContext.SaveChangesAsync();

            var wantedCategoryId = "2";
            var expectedCount = 2;

            var mock = new Mock<ICategoryService>();
            var articleService = new ArticleService(this.dbContext, mock.Object);
            var actual = await articleService.LatestFromCategoryAsync<FakeArticle>(wantedCategoryId, 20);

            Assert.Equal(expectedCount, actual.Count());
        }

        [Fact]
        public async Task LatestFromCategoryAsync_ShouldReturnsLatestFromCategory_WhenCategoryExists()
        {
            await this.dbContext.Articles.AddRangeAsync(new List<Article>
            {
                new Article { Id = "1", PublishedOn = DateTime.UtcNow, Categories = new List<ArticleCategory>
                {
                    new ArticleCategory{CategoryId = "1"},
                    new ArticleCategory{CategoryId = "2"},
                    new ArticleCategory{CategoryId = "4"}
                }},
                new Article { Id = "2", PublishedOn = DateTime.UtcNow.AddHours(-20), Categories = new List<ArticleCategory>
                {
                    new ArticleCategory{CategoryId = "2"}
                }},
                new Article { Id = "3", PublishedOn = DateTime.UtcNow.AddHours(-12), Categories = new List<ArticleCategory>
                {
                    new ArticleCategory{CategoryId = "1"},
                    new ArticleCategory{CategoryId = "2"}
                }},
                new Article { Id = "4", PublishedOn = DateTime.UtcNow.AddHours(-3), Categories = new List<ArticleCategory>
                {
                    new ArticleCategory{CategoryId = "1"},
                    new ArticleCategory{CategoryId = "3"},
                    new ArticleCategory{CategoryId = "2"}
                }},
            });

            await this.dbContext.SaveChangesAsync();

            var wantedCategoryId = "2";

            var mock = new Mock<ICategoryService>();
            var articleService = new ArticleService(this.dbContext, mock.Object);
            var result = await articleService.LatestFromCategoryAsync<FakeArticle>(wantedCategoryId, 3);

            var expectedIds = new List<string> { "1", "4", "3" };
            Assert.True(result.All(a => expectedIds.Contains(a.Id)));
        }

        [Fact]
        public async Task LatestFromCategoryAsync_ShouldReturnsArticlesFromCategoryOrderedByPublishedOnDescending_WhenCategoryExists()
        {
            await this.dbContext.Articles.AddRangeAsync(new List<Article>
            {
                new Article { Id = "1", PublishedOn = DateTime.UtcNow, Categories = new List<ArticleCategory>
                {
                    new ArticleCategory{CategoryId = "1"},
                    new ArticleCategory{CategoryId = "2"},
                    new ArticleCategory{CategoryId = "4"}
                }},
                new Article { Id = "2", PublishedOn = DateTime.UtcNow.AddHours(-20), Categories = new List<ArticleCategory>
                {
                    new ArticleCategory{CategoryId = "2"}
                }},
                new Article { Id = "3", PublishedOn = DateTime.UtcNow.AddHours(-12), Categories = new List<ArticleCategory>
                {
                    new ArticleCategory{CategoryId = "1"},
                    new ArticleCategory{CategoryId = "2"}
                }},
                new Article { Id = "4", PublishedOn = DateTime.UtcNow.AddHours(-3), Categories = new List<ArticleCategory>
                {
                    new ArticleCategory{CategoryId = "1"},
                    new ArticleCategory{CategoryId = "3"},
                    new ArticleCategory{CategoryId = "2"}
                }},
            });

            await this.dbContext.SaveChangesAsync();

            var wantedCategoryId = "2";

            var mock = new Mock<ICategoryService>();
            var articleService = new ArticleService(this.dbContext, mock.Object);
            var result = await articleService.LatestFromCategoryAsync<FakeArticle>(wantedCategoryId, 3);

            var expectedIds = new List<string> { "1", "4", "3" };
            var actualArticles = result.ToList();

            for (int i = 0; i < expectedIds.Count; i++)
            {
                Assert.Equal(expectedIds[i], actualArticles[i].Id);
            }
        }

        [Fact]
        public void LatestFromCategoryAsync_ShouldThrowInvalidCategoryException_WhenCategoryNotExists()
        {
            var wantedCategoryId = "2";

            var mock = new Mock<ICategoryService>();
            mock.Setup(m => m.VerifyExistent(wantedCategoryId)).Throws<InvalidCategoryException>();
            var articleService = new ArticleService(this.dbContext, mock.Object);

            Assert.Throws<InvalidCategoryException>(
                () => articleService.LatestFromCategoryAsync<FakeArticle>(wantedCategoryId, 2).GetAwaiter().GetResult());
        }

        [Fact]
        public async Task LatestImportantFromCategoriesAsync_ShouldReturnsCorrectCountOfImportantArticlesFromCategories()
        {
            await this.dbContext.Articles.AddRangeAsync(new List<Article>
            {
                new Article { IsDeleted = false, IsImportant = true, Categories = new List<ArticleCategory>
                {
                    new ArticleCategory{ Category = new Category{Id = "1", IsDeleted = false } },
                    new ArticleCategory{Category = new Category{Id = "2", IsDeleted = false } }
                }},
                new Article { IsDeleted = false, IsImportant = false, Categories = new List<ArticleCategory>
                {
                    new ArticleCategory{Category = new Category{Id = "2", IsDeleted = false } }
                }},
                new Article { IsDeleted = false, IsImportant = true, Categories = new List<ArticleCategory>
                {
                    new ArticleCategory{Category = new Category{Id = "1", IsDeleted = false } },
                    new ArticleCategory{Category = new Category{Id = "3", IsDeleted = false } },
                    new ArticleCategory{Category = new Category{Id = "4", IsDeleted = false } }
                }},
                new Article { IsDeleted = true, IsImportant = true, Categories = new List<ArticleCategory>
                {
                    new ArticleCategory{Category = new Category{Id = "1", IsDeleted = false } },
                    new ArticleCategory{Category = new Category{Id = "3", IsDeleted = true } },
                    new ArticleCategory{Category = new Category{Id = "2", IsDeleted = false } }
                }},
                new Article { IsDeleted = false, IsImportant = true, Categories = new List<ArticleCategory>
                {
                    new ArticleCategory{Category = new Category{Id = "1", IsDeleted = false } },
                    new ArticleCategory{Category = new Category{Id = "3", IsDeleted = true } },
                    new ArticleCategory{Category = new Category{Id = "2", IsDeleted = true } }
                }},
                new Article { IsDeleted = false, IsImportant = true, Categories = new List<ArticleCategory>
                {
                    new ArticleCategory{Category = new Category{Id = "1", IsDeleted = false } },
                    new ArticleCategory{Category = new Category{Id = "3", IsDeleted = false } },
                    new ArticleCategory{Category = new Category{Id = "2", IsDeleted = false } }
                }},
            });

            await this.dbContext.SaveChangesAsync();

            var wantedCategoriesIds = new List<string> { "2", "3" };
            var expectedCount = 2;

            var articleService = new ArticleService(this.dbContext, null);
            var actual = await articleService.LatestImportantFromCategoriesAsync<FakeArticle>(wantedCategoriesIds, 2);

            Assert.Equal(expectedCount, actual.Count());
        }

        [Fact]
        public async Task LatestImportantFromCategoriesAsync_ShouldReturnsAllImportantFromCategories_WhenGivenCountIsGreaterThenAvailable()
        {
            await this.dbContext.Articles.AddRangeAsync(new List<Article>
            {
                new Article { IsDeleted = false, IsImportant = true, Categories = new List<ArticleCategory>
                {
                    new ArticleCategory{ Category = new Category{Id = "1", IsDeleted = false } },
                    new ArticleCategory{Category = new Category{Id = "2", IsDeleted = false } }
                }},
                new Article { IsDeleted = false, IsImportant = false, Categories = new List<ArticleCategory>
                {
                    new ArticleCategory{Category = new Category{Id = "2", IsDeleted = false } }
                }},
                new Article { IsDeleted = false, IsImportant = true, Categories = new List<ArticleCategory>
                {
                    new ArticleCategory{Category = new Category{Id = "1", IsDeleted = false } },
                    new ArticleCategory{Category = new Category{Id = "3", IsDeleted = false } },
                    new ArticleCategory{Category = new Category{Id = "4", IsDeleted = false } }
                }},
                new Article { IsDeleted = true, IsImportant = true, Categories = new List<ArticleCategory>
                {
                    new ArticleCategory{Category = new Category{Id = "1", IsDeleted = false } },
                    new ArticleCategory{Category = new Category{Id = "3", IsDeleted = true } },
                    new ArticleCategory{Category = new Category{Id = "2", IsDeleted = false } }
                }},
                new Article { IsDeleted = false, IsImportant = true, Categories = new List<ArticleCategory>
                {
                    new ArticleCategory{Category = new Category{Id = "1", IsDeleted = false } },
                    new ArticleCategory{Category = new Category{Id = "3", IsDeleted = true } },
                    new ArticleCategory{Category = new Category{Id = "2", IsDeleted = true } }
                }},
                new Article { IsDeleted = false, IsImportant = true, Categories = new List<ArticleCategory>
                {
                    new ArticleCategory{Category = new Category{Id = "1", IsDeleted = false } },
                    new ArticleCategory{Category = new Category{Id = "3", IsDeleted = false } },
                    new ArticleCategory{Category = new Category{Id = "2", IsDeleted = false } }
                }},
            });

            await this.dbContext.SaveChangesAsync();

            var wantedCategoriesIds = new List<string> { "2", "3" };
            var expectedCount = 4;

            var articleService = new ArticleService(this.dbContext, null);
            var actual = await articleService.LatestImportantFromCategoriesAsync<FakeArticle>(wantedCategoriesIds, 20);

            Assert.Equal(expectedCount, actual.Count());
        }

        [Fact]
        public async Task LatestImportantFromCategoriesAsync_ShouldReturnsLatestFromCategories()
        {
            await this.dbContext.Articles.AddRangeAsync(new List<Article>
            {
                new Article { Id = "1", PublishedOn = DateTime.UtcNow.AddHours(-15), IsImportant = true, Categories = new List<ArticleCategory>
                {
                    new ArticleCategory{ Category = new Category{Id = "1" } },
                    new ArticleCategory{ Category = new Category{Id = "2" } }
                }},
                new Article { Id = "2", PublishedOn = DateTime.UtcNow.AddHours(-1), IsImportant = true, Categories = new List<ArticleCategory>
                {
                    new ArticleCategory{Category = new Category{Id = "2" } }
                }},
                new Article { Id = "3", PublishedOn = DateTime.UtcNow.AddHours(-10), IsImportant = true, Categories = new List<ArticleCategory>
                {
                    new ArticleCategory{Category = new Category{Id = "1"} },
                    new ArticleCategory{Category = new Category{Id = "3"} },
                    new ArticleCategory{Category = new Category{Id = "4"} }
                }},
                new Article { Id = "4", PublishedOn = DateTime.UtcNow.AddHours(-13), IsImportant = true, Categories = new List<ArticleCategory>
                {
                    new ArticleCategory{Category = new Category{Id = "1" } },
                    new ArticleCategory{Category = new Category{Id = "3" } },
                    new ArticleCategory{Category = new Category{Id = "2" } }
                }},
                new Article {Id = "5", PublishedOn = DateTime.UtcNow.AddHours(-5), IsImportant = true, Categories = new List<ArticleCategory>
                {
                    new ArticleCategory{Category = new Category{Id = "1" } },
                    new ArticleCategory{Category = new Category{Id = "3" } },
                    new ArticleCategory{Category = new Category{Id = "2" } }
                }},
            });

            await this.dbContext.SaveChangesAsync();

            var wantedCategoriesIds = new List<string> { "2", "3" };

            var articleService = new ArticleService(this.dbContext, null);
            var result = await articleService.LatestImportantFromCategoriesAsync<FakeArticle>(wantedCategoriesIds, 3);

            var expectedIds = new List<string> { "2", "5", "3" };
            Assert.True(result.All(a => expectedIds.Contains(a.Id)));
        }

        [Fact]
        public async Task LatestImportantFromCategoriesAsync_ShouldReturnsImportantFromCategoriesOrderedByPublishedOnDescending()
        {
            await this.dbContext.Articles.AddRangeAsync(new List<Article>
            {
                new Article { Id = "1", PublishedOn = DateTime.UtcNow.AddHours(-15), IsImportant = true, Categories = new List<ArticleCategory>
                {
                    new ArticleCategory{ Category = new Category{Id = "1" } },
                    new ArticleCategory{ Category = new Category{Id = "2" } }
                }},
                new Article { Id = "2", PublishedOn = DateTime.UtcNow.AddHours(-1), IsImportant = true, Categories = new List<ArticleCategory>
                {
                    new ArticleCategory{Category = new Category{Id = "2" } }
                }},
                new Article { Id = "3", PublishedOn = DateTime.UtcNow.AddHours(-10), IsImportant = true, Categories = new List<ArticleCategory>
                {
                    new ArticleCategory{Category = new Category{Id = "1"} },
                    new ArticleCategory{Category = new Category{Id = "3"} },
                    new ArticleCategory{Category = new Category{Id = "4"} }
                }},
                new Article { Id = "4", PublishedOn = DateTime.UtcNow.AddHours(-13), IsImportant = true, Categories = new List<ArticleCategory>
                {
                    new ArticleCategory{Category = new Category{Id = "1" } },
                    new ArticleCategory{Category = new Category{Id = "3" } },
                    new ArticleCategory{Category = new Category{Id = "2" } }
                }},
                new Article {Id = "5", PublishedOn = DateTime.UtcNow.AddHours(-5), IsImportant = true, Categories = new List<ArticleCategory>
                {
                    new ArticleCategory{Category = new Category{Id = "1" } },
                    new ArticleCategory{Category = new Category{Id = "3" } },
                    new ArticleCategory{Category = new Category{Id = "2" } }
                }},
            });

            await this.dbContext.SaveChangesAsync();

            var wantedCategoriesIds = new List<string> { "2", "3" };

            var articleService = new ArticleService(this.dbContext, null);
            var result = await articleService.LatestImportantFromCategoriesAsync<FakeArticle>(wantedCategoriesIds, 3);
            var actualArticles = result.ToList();

            var expectedIds = new List<string> { "2", "5", "3" };
            for (int i = 0; i < expectedIds.Count; i++)
            {
                Assert.Equal(expectedIds[i], actualArticles[i].Id);
            }
        }

        [Fact]
        public async Task AllImportantForTheDayAsync_ShouldReturnsAllArticles_ThatAreImportantAndArePublishedCurrentDay()
        {
            await this.dbContext.Articles.AddAsync(new Article { IsImportant = true, PublishedOn = DateTime.UtcNow });
            await this.dbContext.Articles.AddAsync(new Article { IsImportant = true, PublishedOn = DateTime.UtcNow.AddDays(-1) });
            await this.dbContext.Articles.AddAsync(new Article { IsImportant = false, PublishedOn = DateTime.UtcNow.AddDays(-10) });
            await this.dbContext.Articles.AddAsync(new Article { IsImportant = false, PublishedOn = DateTime.UtcNow.AddHours(2) });
            await this.dbContext.Articles.AddAsync(new Article { IsImportant = true, PublishedOn = DateTime.UtcNow.AddHours(2) });
            await this.dbContext.Articles.AddAsync(new Article { IsImportant = true, PublishedOn = DateTime.UtcNow.AddHours(-2) });
            await this.dbContext.Articles.AddAsync(new Article { IsImportant = false, PublishedOn = DateTime.UtcNow.AddDays(-3) });
            await this.dbContext.Articles.AddAsync(new Article { IsImportant = true, PublishedOn = DateTime.UtcNow.AddMinutes(5) });
            await this.dbContext.Articles.AddAsync(new Article { IsImportant = false, PublishedOn = DateTime.UtcNow.AddMinutes(5) });
            await this.dbContext.Articles.AddAsync(new Article { IsImportant = true, PublishedOn = DateTime.UtcNow.AddMinutes(-5) });
            await this.dbContext.SaveChangesAsync();

            var currentDayStart = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day, 0, 0, 0);
            var expectedCount = this.dbContext.Articles.Count(a => a.IsImportant && a.PublishedOn >= currentDayStart);

            var articleService = new ArticleService(this.dbContext, null);
            var actual = await articleService.AllImportantForTheDayAsync<FakeArticle>();

            Assert.Equal(expectedCount, actual.Count());
        }

        [Fact]
        public async Task GetAsync_ShouldGetCorrectArticle_WhenArticleExists()
        {
            await this.dbContext.Articles.AddRangeAsync(new List<Article>()
            {
                new Article {Id = "1"},
                new Article {Id = "2"},
                new Article {Id = "3"},
            });
            await this.dbContext.SaveChangesAsync();

            var articleService = new ArticleService(this.dbContext, null);

            var wantedArticleId = "2";
            var expectedArticleId = wantedArticleId;
            var article = await articleService.GetAsync(wantedArticleId);

            Assert.Equal(expectedArticleId, article.Id);
        }

        [Fact]
        public void GetAsync_ShouldThrowInvalidArticleException_WhenArticleNotExists()
        {
            var articleService = new ArticleService(this.dbContext, null);
            Assert.Throws<InvalidArticleException>(
                () => articleService.GetAsync(Guid.NewGuid().ToString()).GetAwaiter().GetResult());
        }

        [Fact]
        public async Task GetAsync_Projection_ShouldGetCorrectArticle_WhenArticleExists()
        {
            await this.dbContext.Articles.AddRangeAsync(new List<Article>()
            {
                new Article {Id = "1"},
                new Article {Id = "2"},
                new Article {Id = "3"},
            });
            await this.dbContext.SaveChangesAsync();

            var articleService = new ArticleService(this.dbContext, null);

            var wantedArticleId = "2";
            var expectedArticleId = wantedArticleId;
            var articleProjected = await articleService.GetAsync<FakeArticle>(wantedArticleId);

            Assert.Equal(expectedArticleId, articleProjected.Id);
        }

        [Fact]
        public void GetAsync_Projection_ShouldThrowInvalidArticleException_WhenArticleNotExists()
        {
            var articleService = new ArticleService(this.dbContext, null);
            Assert.Throws<InvalidArticleException>(
                () => articleService.GetAsync<FakeArticle>(Guid.NewGuid().ToString()).GetAwaiter().GetResult());
        }

        [Fact]
        public async Task CreateAsync_ShouldAddNewArticleToDb()
        {
            var mock = new Mock<ICategoryService>();
            var articleService = new ArticleService(this.dbContext, mock.Object);

            var creatorId = Guid.NewGuid().ToString();
            await articleService.CreateAsync(new ArticleCreateInputModel(), creatorId);
            const int expected = 1;
            var actual = this.dbContext.Articles.Count();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task EditAsync_ShouldSaveChangesToDb_WithExistentId()
        {
            var articleToEditId = Guid.NewGuid().ToString();
            var articleToDb = new Article
            {
                Id = articleToEditId,
                Title = "CurrentTitle",
                Lead = "CurrentLead",
                Author = "CurrentAuthor",
                Content = "CurrentContent",
                Image = new Image { Id = "1" },
                IsTop = false,
                IsImportant = true,
            };

            await this.dbContext.Articles.AddAsync(articleToDb);
            await this.dbContext.SaveChangesAsync();

            var articleWithEdits = new ArticleEditInputModel
            {
                Id = articleToEditId,
                Title = "NewTitle",
                Lead = "NewLead",
                Author = "NewAuthor",
                Content = "NewContent",
                Image = new ImageForArticleInputModel { Id = "2" },
                IsTop = true,
                IsImportant = false
            };

            var mock = new Mock<ICategoryService>();
            var articleService = new ArticleService(this.dbContext, mock.Object);

            var editorId = Guid.NewGuid().ToString();
            await articleService.EditAsync(articleWithEdits, editorId);

            var actualArticle = this.dbContext.Articles.First();

            Assert.Equal(articleWithEdits.Id, actualArticle.Id);
            Assert.Equal(articleWithEdits.Title, actualArticle.Title);
            Assert.Equal(articleWithEdits.Lead, actualArticle.Lead);
            Assert.Equal(articleWithEdits.Author, actualArticle.Author);
            Assert.Equal(articleWithEdits.Content, actualArticle.Content);
            Assert.Equal(articleWithEdits.Content, actualArticle.Content);
            Assert.Equal(articleWithEdits.Image.Id, actualArticle.ImageId);
            Assert.Equal(articleWithEdits.IsTop, actualArticle.IsTop);
            Assert.Equal(articleWithEdits.IsImportant, actualArticle.IsImportant);
        }

        [Fact]
        public async Task EditAsync_ShouldAddEditsToArticle_WithExistentId()
        {
            var articleToEditId = Guid.NewGuid().ToString();
            var articleToDb = new Article { Id = articleToEditId };

            await this.dbContext.Articles.AddAsync(articleToDb);
            await this.dbContext.SaveChangesAsync();

            var articleWithEdits = new ArticleEditInputModel { Id = articleToEditId };

            var mock = new Mock<ICategoryService>();
            var articleService = new ArticleService(this.dbContext, mock.Object);

            var editorId = Guid.NewGuid().ToString();
            await articleService.EditAsync(articleWithEdits, editorId);

            var actualArticle = this.dbContext.Articles.First();

            Assert.Equal(editorId, actualArticle.Edits.FirstOrDefault()?.EditorId);
        }

        [Fact]
        public void EditAsync_ShouldThrowInvalidArticleException_WithNonExistentId()
        {
            var articleWithEdits = new ArticleEditInputModel { Id = Guid.NewGuid().ToString() };

            var mock = new Mock<ICategoryService>();
            var articleService = new ArticleService(this.dbContext, mock.Object);

            Assert.Throws<InvalidArticleException>(
                () => articleService.EditAsync(articleWithEdits, Guid.NewGuid().ToString()).GetAwaiter().GetResult());
        }

        [Fact]
        public async Task EditAsync_ShouldThrowInvalidArticleException_WhenIdExistsButArticleIsSoftDeleted()
        {
            var articleId = Guid.NewGuid().ToString();
            await this.dbContext.Articles.AddAsync(new Article { Id = articleId, IsDeleted = true });
            await this.dbContext.SaveChangesAsync();

            var articleService = new ArticleService(this.dbContext, null);
            var articleWithEdits = new ArticleEditInputModel { Id = articleId };
            Assert.Throws<InvalidArticleException>(
                () => articleService.EditAsync(articleWithEdits, Guid.NewGuid().ToString()).GetAwaiter().GetResult());
        }

        [Fact]
        public async Task DeleteAsync_ShouldSoftDeleteArticle_WithExistentId()
        {
            var articleId = Guid.NewGuid().ToString();
            await this.dbContext.Articles.AddAsync(new Article { Id = articleId, IsDeleted = false });
            await this.dbContext.SaveChangesAsync();

            var articleService = new ArticleService(this.dbContext, null);
            var editorId = Guid.NewGuid().ToString();
            await articleService.DeleteAsync(articleId, editorId);

            var resultArticle = await this.dbContext.Articles.FindAsync(articleId);
            Assert.True(resultArticle.IsDeleted);

            var resultEditorId = resultArticle.Edits.FirstOrDefault(e => e.EditorId == editorId);
            Assert.NotNull(resultEditorId);
        }

        [Fact]
        public void DeleteAsync_ShouldThrowInvalidArticleException_WithNonExistentId()
        {
            var articleService = new ArticleService(this.dbContext, null);

            Assert.Throws<InvalidArticleException>(
                () => articleService.DeleteAsync(Guid.NewGuid().ToString(), Guid.NewGuid().ToString()).GetAwaiter().GetResult());
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrowInvalidArticleException_WhenIdExistsButArticleIsSoftDeleted()
        {
            var articleId = Guid.NewGuid().ToString();
            await this.dbContext.Articles.AddAsync(new Article { Id = articleId, IsDeleted = true });
            await this.dbContext.SaveChangesAsync();

            var articleService = new ArticleService(this.dbContext, null);

            Assert.Throws<InvalidArticleException>(
                () => articleService.DeleteAsync(articleId, Guid.NewGuid().ToString()).GetAwaiter().GetResult());
        }

        [Fact]
        public async Task DeleteAsync_ShouldAddEditsToArticle_WithExistentId()
        {
            var articleToDeleteId = Guid.NewGuid().ToString();
            var articleToDb = new Article { Id = articleToDeleteId };

            await this.dbContext.Articles.AddAsync(articleToDb);
            await this.dbContext.SaveChangesAsync();

            var mock = new Mock<ICategoryService>();
            var articleService = new ArticleService(this.dbContext, mock.Object);

            var editorId = Guid.NewGuid().ToString();
            await articleService.DeleteAsync(articleToDeleteId, editorId);

            var actualArticle = this.dbContext.Articles.First();

            Assert.Equal(editorId, actualArticle.Edits.FirstOrDefault()?.EditorId);
        }

        internal class FakeArticle
        {
            public string Id { get; set; }
        }
    }
}
