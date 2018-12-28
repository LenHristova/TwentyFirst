namespace TwentyFirst.Services.DataServices.Tests
{
    using System;
    using Data.Models;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Common.Exceptions;
    using Common.Models.Categories;
    using Common.Models.Images;
    using Common.Models.Interviews;
    using Xunit;

    public class CategoryServiceTests : DataServiceTests
    {
        [Fact]
        public async Task AllWithArchivedAsync_ShouldReturnsCorrectCountOfCategories()
        {
            await this.dbContext.Categories.AddRangeAsync(new List<Category>
            {
                new Category { IsDeleted = true },
                new Category { IsDeleted = false },
                new Category { IsDeleted = true },
                new Category { IsDeleted = false },
                new Category { IsDeleted = false },
            });

            await this.dbContext.SaveChangesAsync();

            var categoryService = new CategoryService(this.dbContext);

            var result = await categoryService.AllWithArchivedAsync<FakeCategory>();

            var expectedCount = 5;
            var actualCount = result.Count();
            Assert.Equal(expectedCount, actualCount);
        }

        [Fact]
        public async Task AllWithArchivedAsync_ShouldOrderByIsDeletedThenByOrder()
        {
            await this.dbContext.Categories.AddRangeAsync(new List<Category>
            {
                new Category { Id = "1", IsDeleted = true, Order = 3},
                new Category { Id = "2", IsDeleted = false, Order = 1},
                new Category { Id = "3", IsDeleted = true, Order = 2},
                new Category { Id = "4", IsDeleted = false, Order = 5},
                new Category { Id = "5", IsDeleted = false, Order = 4},
            });

            await this.dbContext.SaveChangesAsync();

            var categoryService = new CategoryService(this.dbContext);

            var result = await categoryService.AllWithArchivedAsync<FakeCategory>();

            var expectedIdsOrder = new List<string>{"2", "5", "4", "3", "1"};
            var actualCategories = result.ToList();
            for (int i = 0; i < expectedIdsOrder.Count; i++)
            {
                Assert.Equal(expectedIdsOrder[i], actualCategories[i].Id);
            }
        }

        [Fact]
        public async Task AllAsync_ShouldReturnsCorrectCountOfCategories()
        {
            await this.dbContext.Categories.AddRangeAsync(new List<Category>
            {
                new Category { IsDeleted = true },
                new Category { IsDeleted = false },
                new Category { IsDeleted = true },
                new Category { IsDeleted = false },
                new Category { IsDeleted = false },
            });

            await this.dbContext.SaveChangesAsync();

            var categoryService = new CategoryService(this.dbContext);

            var result = await categoryService.AllAsync<FakeCategory>();

            var expectedCount = 3;
            var actualCount = result.Count();
            Assert.Equal(expectedCount, actualCount);
        }

        [Fact]
        public async Task AllAsync_ShouldOrderByIsDeletedThenByOrder()
        {
            await this.dbContext.Categories.AddRangeAsync(new List<Category>
            {
                new Category { Id = "1", IsDeleted = false, Order = 3},
                new Category { Id = "2", IsDeleted = false, Order = 1},
                new Category { Id = "3", IsDeleted = false, Order = 2},
                new Category { Id = "4", IsDeleted = false, Order = 5},
                new Category { Id = "5", IsDeleted = false, Order = 4},
            });

            await this.dbContext.SaveChangesAsync();

            var categoryService = new CategoryService(this.dbContext);

            var result = await categoryService.AllAsync<FakeCategory>();

            var expectedIdsOrder = new List<string> { "2", "3", "1", "5", "4" };
            var actualCategories = result.ToList();
            for (int i = 0; i < expectedIdsOrder.Count; i++)
            {
                Assert.Equal(expectedIdsOrder[i], actualCategories[i].Id);
            }
        }

        [Fact]
        public async Task GetAsync_ShouldGetCorrectCategory_WhenCategoryExists()
        {
            await this.dbContext.Categories.AddRangeAsync(new List<Category>()
            {
                new Category {Id = "1"},
                new Category {Id = "2"},
                new Category {Id = "3"},
            });

            await this.dbContext.SaveChangesAsync();

            var categoryService = new CategoryService(this.dbContext);

            var wantedCategoryId = "2";
            var expectedCategoryId = wantedCategoryId;
            var category = await categoryService.GetAsync(wantedCategoryId);

            Assert.Equal(expectedCategoryId, category.Id);
        }

        [Fact]
        public void GetAsync_ShouldThrowInvalidCategoryException_WhenCategoryNotExists()
        {
            var categoryService = new CategoryService(this.dbContext);
            Assert.Throws<InvalidCategoryException>(
                () => categoryService.GetAsync(Guid.NewGuid().ToString()).GetAwaiter().GetResult());
        }

        [Fact]
        public async Task GetAsync_Projection_ShouldGetCorrectCategory_WhenCategoryExists()
        {
            await this.dbContext.Categories.AddRangeAsync(new List<Category>()
            {
                new Category {Id = "1"},
                new Category {Id = "2"},
                new Category {Id = "3"},
            });

            await this.dbContext.SaveChangesAsync();

            var categoryService = new CategoryService(this.dbContext);

            var wantedCategoryId = "2";
            var expectedCategoryId = wantedCategoryId;
            var category = await categoryService.GetAsync<FakeCategory>(wantedCategoryId);

            Assert.Equal(expectedCategoryId, category.Id);
        }

        [Fact]
        public void GetAsync_Projection_ShouldThrowInvalidCategoryException_WhenCategoryNotExists()
        {
            var categoryService = new CategoryService(this.dbContext);
            Assert.Throws<InvalidCategoryException>(
                () => categoryService.GetAsync<FakeCategory>(Guid.NewGuid().ToString()).GetAwaiter().GetResult());
        }

        [Fact]
        public async Task GetArchivedAsync_ShouldGetCorrectCategory_WhenCategoryExistsAndIsSoftDeleted()
        {
            await this.dbContext.Categories.AddRangeAsync(new List<Category>()
            {
                new Category {Id = "1", IsDeleted = true },
                new Category {Id = "2", IsDeleted = true },
                new Category {Id = "3", IsDeleted = true },
            });

            await this.dbContext.SaveChangesAsync();

            var categoryService = new CategoryService(this.dbContext);

            var wantedCategoryId = "2";
            var expectedCategoryId = wantedCategoryId;
            var category = await categoryService.GetArchivedAsync(wantedCategoryId);

            Assert.Equal(expectedCategoryId, category.Id);
        }

        [Fact]
        public void GetArchivedAsync_ShouldThrowInvalidCategoryException_WhenCategoryNotExists()
        {
            var categoryService = new CategoryService(this.dbContext);
            Assert.Throws<InvalidCategoryException>(
                () => categoryService.GetArchivedAsync(Guid.NewGuid().ToString()).GetAwaiter().GetResult());
        }

        [Fact]
        public async Task GetArchivedAsync_ShouldThrowInvalidCategoryException_WhenCategoryExistsButIsNotSoftDeleted()
        {
            var categoryId = Guid.NewGuid().ToString();
            await this.dbContext.Categories.AddAsync(new Category { Id = categoryId, IsDeleted = false });
            await this.dbContext.SaveChangesAsync();

            var categoryService = new CategoryService(this.dbContext);
            Assert.Throws<InvalidCategoryException>(
                () => categoryService.GetArchivedAsync(categoryId).GetAwaiter().GetResult());
        }

        [Fact]
        public async Task GetArchivedAsync_Projection_ShouldGetCorrectCategory_WhenCategoryExistsAndIsSoftDeleted()
        {
            await this.dbContext.Categories.AddRangeAsync(new List<Category>()
            {
                new Category {Id = "1", IsDeleted = true },
                new Category {Id = "2", IsDeleted = true },
                new Category {Id = "3", IsDeleted = true },
            });

            await this.dbContext.SaveChangesAsync();

            var categoryService = new CategoryService(this.dbContext);

            var wantedCategoryId = "2";
            var expectedCategoryId = wantedCategoryId;
            var category = await categoryService.GetArchivedAsync<FakeCategory>(wantedCategoryId);

            Assert.Equal(expectedCategoryId, category.Id);
        }

        [Fact]
        public void GetArchivedAsync_Projection_ShouldThrowInvalidCategoryException_WhenCategoryNotExists()
        {
            var categoryService = new CategoryService(this.dbContext);
            Assert.Throws<InvalidCategoryException>(
                () => categoryService.GetArchivedAsync<FakeCategory>(Guid.NewGuid().ToString()).GetAwaiter().GetResult());
        }

        [Fact]
        public async Task GetArchivedAsync_Projection_ShouldThrowInvalidCategoryException_WhenCategoryExistsButIsNotSoftDeleted()
        {
            var categoryId = Guid.NewGuid().ToString();
            await this.dbContext.Categories.AddAsync(new Category { Id = categoryId, IsDeleted = false });
            await this.dbContext.SaveChangesAsync();

            var categoryService = new CategoryService(this.dbContext);
            Assert.Throws<InvalidCategoryException>(
                () => categoryService.GetArchivedAsync<FakeCategory>(categoryId).GetAwaiter().GetResult());
        }

        [Fact]
        public async Task AllOrderedByNameAsync_ShouldReturnsCorrectCountOfCategories()
        {
            await this.dbContext.Categories.AddRangeAsync(new List<Category>
            {
                new Category { IsDeleted = true },
                new Category { IsDeleted = false },
                new Category { IsDeleted = true },
                new Category { IsDeleted = false },
                new Category { IsDeleted = false },
            });

            await this.dbContext.SaveChangesAsync();

            var categoryService = new CategoryService(this.dbContext);

            var result = await categoryService.AllOrderedByNameAsync<FakeCategory>();

            var expectedCount = 3;
            var actualCount = result.Count();
            Assert.Equal(expectedCount, actualCount);
        }

        [Fact]
        public async Task AllOrderedByNameAsync_ShouldOrderByName()
        {
            await this.dbContext.Categories.AddRangeAsync(new List<Category>
            {
                new Category { Id = "1", IsDeleted = false, Name = "b"},
                new Category { Id = "2", IsDeleted = false, Name = "c"},
                new Category { Id = "3", IsDeleted = false, Name = "e"},
                new Category { Id = "4", IsDeleted = false, Name = "a"},
                new Category { Id = "5", IsDeleted = false, Name = "d"},
            });

            await this.dbContext.SaveChangesAsync();

            var categoryService = new CategoryService(this.dbContext);

            var result = await categoryService.AllOrderedByNameAsync<FakeCategory>();

            var expectedIdsOrder = new List<string> { "4", "1", "2", "5", "3" };
            var actualCategories = result.ToList();
            for (int i = 0; i < expectedIdsOrder.Count; i++)
            {
                Assert.Equal(expectedIdsOrder[i], actualCategories[i].Id);
            }
        }

        [Fact]
        public async Task ReorderUpAsync_ShouldSwitchOrdersWithPreviousNotArchivedCategory()
        {
            await this.dbContext.Categories.AddRangeAsync(new List<Category>
            {
                new Category { Id = "1", IsDeleted = false,  Order = 1 },
                new Category { Id = "2", IsDeleted = true,  Order = 2 },
                new Category { Id = "3", IsDeleted = false,  Order = 3 },
                new Category { Id = "4", IsDeleted = false,  Order = 4 },
                new Category { Id = "5", IsDeleted = false,  Order = 5 },
            });

            await this.dbContext.SaveChangesAsync();

            var categoryService = new CategoryService(this.dbContext);

            var categoryToUpId = "3";
            var categoryToSwitchWithId = "1";

            await categoryService.ReorderUpAsync(categoryToUpId);

            var categoryToUp = await this.dbContext.Categories.FindAsync(categoryToUpId);
            var categoryToSwitchWith = await this.dbContext.Categories.FindAsync(categoryToSwitchWithId);
            var expectedCategoryToUpOrder = 1;
            var expectedCategoryToSwitchOrder = 3;
            Assert.True(categoryToUp.Order == expectedCategoryToUpOrder && 
                        categoryToSwitchWith.Order == expectedCategoryToSwitchOrder);
        }

        [Fact]
        public void ReorderUpAsync_ShouldThrowInvalidCategoryException_WhenCategoryNotExists()
        {
            var categoryService = new CategoryService(this.dbContext);

            Assert.Throws<InvalidCategoryException>(() => categoryService.ReorderUpAsync(Guid.NewGuid().ToString()).GetAwaiter().GetResult());
        }

        [Fact]
        public async Task ReorderUpAsync_ShouldThrowInvalidCategoryException_WhenCategoryExistsButIsSoftDeleted()
        {
            var categoryId = Guid.NewGuid().ToString();
            await this.dbContext.Categories.AddAsync(new Category {Id = categoryId, IsDeleted = true});
            await this.dbContext.SaveChangesAsync();

            var categoryService = new CategoryService(this.dbContext);

            Assert.Throws<InvalidCategoryException>(() => categoryService.ReorderUpAsync(categoryId).GetAwaiter().GetResult());
        }

        [Fact]
        public async Task ReorderUpAsync_ShouldThrowInvalidCategoryOrderException_WhenPreviousCategoryNotExists()
        {
            var categoryId = Guid.NewGuid().ToString();
            await this.dbContext.Categories.AddAsync(new Category { Id = categoryId, Order = 1 });
            await this.dbContext.SaveChangesAsync();

            var categoryService = new CategoryService(this.dbContext);

            Assert.Throws<InvalidCategoryOrderException>(() => categoryService.ReorderUpAsync(categoryId).GetAwaiter().GetResult());
        }

        [Fact]
        public async Task ReorderDownAsync_ShouldSwitchOrdersWithNextNotArchivedCategory()
        {
            await this.dbContext.Categories.AddRangeAsync(new List<Category>
            {
                new Category { Id = "1", IsDeleted = false,  Order = 1 },
                new Category { Id = "2", IsDeleted = false,  Order = 2 },
                new Category { Id = "3", IsDeleted = false,  Order = 3 },
                new Category { Id = "4", IsDeleted = true,  Order = 4 },
                new Category { Id = "5", IsDeleted = false,  Order = 5 },
            });

            await this.dbContext.SaveChangesAsync();

            var categoryService = new CategoryService(this.dbContext);

            var categoryToUpId = "3";
            var categoryToSwitchWithId = "5";

            await categoryService.ReorderDownAsync(categoryToUpId);

            var categoryToUp = await this.dbContext.Categories.FindAsync(categoryToUpId);
            var categoryToSwitchWith = await this.dbContext.Categories.FindAsync(categoryToSwitchWithId);
            var expectedCategoryToUpOrder = 5;
            var expectedCategoryToSwitchOrder = 3;
            Assert.True(categoryToUp.Order == expectedCategoryToUpOrder &&
                        categoryToSwitchWith.Order == expectedCategoryToSwitchOrder);
        }

        [Fact]
        public void ReorderDownAsync_ShouldThrowInvalidCategoryException_WhenCategoryNotExists()
        {
            var categoryService = new CategoryService(this.dbContext);

            Assert.Throws<InvalidCategoryException>(() => 
                categoryService.ReorderDownAsync(Guid.NewGuid().ToString()).GetAwaiter().GetResult());
        }

        [Fact]
        public async Task ReorderDownAsync_ShouldThrowInvalidCategoryException_WhenCategoryExistsButIsSoftDeleted()
        {
            var categoryId = Guid.NewGuid().ToString();
            await this.dbContext.Categories.AddAsync(new Category { Id = categoryId, IsDeleted = true });
            await this.dbContext.SaveChangesAsync();

            var categoryService = new CategoryService(this.dbContext);

            Assert.Throws<InvalidCategoryException>(() => categoryService.ReorderDownAsync(categoryId).GetAwaiter().GetResult());
        }

        [Fact]
        public async Task ReorderDownAsync_ShouldThrowInvalidCategoryOrderException_WhenNextCategoryNotExists()
        {
            var categoryId = Guid.NewGuid().ToString();
            await this.dbContext.Categories.AddAsync(new Category { Id = categoryId, Order = 1 });
            await this.dbContext.SaveChangesAsync();

            var categoryService = new CategoryService(this.dbContext);

            Assert.Throws<InvalidCategoryOrderException>(() => categoryService.ReorderDownAsync(categoryId).GetAwaiter().GetResult());
        }

        [Fact]
        public async Task ThrowIfAnyNotExist_ShouldPassCheck_WhenCategoriesExist()
        {
            await this.dbContext.Categories.AddRangeAsync(new List<Category>()
            {
                new Category {Id = "1"},
                new Category {Id = "2"},
                new Category {Id = "3"},
            });
            await this.dbContext.SaveChangesAsync();

            var categoryService = new CategoryService(this.dbContext);

            var wantedCategoriesIds = new List<string> { "1", "2", "3" };
            categoryService.ThrowIfAnyNotExist(wantedCategoriesIds);
            var ex = Record.Exception(() => categoryService.ThrowIfAnyNotExist(wantedCategoriesIds));
            Assert.Null(ex);
        }

        [Fact]
        public async Task ThrowIfAnyNotExist_ShouldThrowInvalidCategoryException_WhenAnyCategoryNotExist()
        {
            await this.dbContext.Categories.AddRangeAsync(new List<Category>()
            {
                new Category {Id = "1"},
                new Category {Id = "2"},
            });
            await this.dbContext.SaveChangesAsync();

            var categoryService = new CategoryService(this.dbContext);

            var wantedCategoriesIds = new List<string> { "1", "2", "3" };
            Assert.Throws<InvalidCategoryException>(() => categoryService.ThrowIfAnyNotExist(wantedCategoriesIds));
        }

        [Fact]
        public void ThrowIfAnyNotExist_ShouldThrowInvalidCategoryException_WhenAllCategoryNotExist()
        {
            var categoryService = new CategoryService(this.dbContext);

            var wantedCategoriesIds = new List<string> { "1", "2", "3" };
            Assert.Throws<InvalidCategoryException>(() => categoryService.ThrowIfAnyNotExist(wantedCategoriesIds));
        }

        [Fact]
        public async Task ThrowIfNotExists_ShouldPassCheck_WhenCategoryExist()
        {
            await this.dbContext.Categories.AddRangeAsync(new List<Category>()
            {
                new Category {Id = "1"},
                new Category {Id = "2"},
                new Category {Id = "3"},
            });
            await this.dbContext.SaveChangesAsync();

            var categoryService = new CategoryService(this.dbContext);

            var wantedCategoryId = "2";
            categoryService.ThrowIfNotExists(wantedCategoryId);
            var ex = Record.Exception(() => categoryService.ThrowIfNotExists(wantedCategoryId));
            Assert.Null(ex);
        }

        [Fact]
        public void ThrowIfNotExists_ShouldThrowInvalidCategoryException_WhenCategoryNotExists()
        {
            var categoryService = new CategoryService(this.dbContext);
            Assert.Throws<InvalidCategoryException>(() => categoryService.ThrowIfNotExists(Guid.NewGuid().ToString()));
        }

        [Fact]
        public async Task CreateAsync_ShouldAddNewCategoryToDb()
        {
            var categoryService = new CategoryService(this.dbContext);

            await categoryService.CreateAsync(new CategoryCreateInputModel());
            const int expected = 1;
            var actual = this.dbContext.Categories.Count();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task CreateAsync_ShouldAddCorrectCategoryOrder_WhenDbHasNoCategories()
        {
            var categoryService = new CategoryService(this.dbContext);

            var newCategory = await categoryService.CreateAsync(new CategoryCreateInputModel());
            var newCategoryFromDb = await this.dbContext.Categories.FindAsync(newCategory.Id);

            var expectedOrder = 1;
            var actualOrder = newCategoryFromDb.Order;
            Assert.Equal(expectedOrder, actualOrder);
        }

        [Fact]
        public async Task CreateAsync_ShouldAddCorrectCategoryOrder_WhenDbAlreadyHasCategories()
        {
            await this.dbContext.Categories.AddRangeAsync(new List<Category>()
            {
                new Category { Order = 1 },
                new Category { Order = 2 },
                new Category { Order = 3 },
            });

            await this.dbContext.SaveChangesAsync();

            var categoryService = new CategoryService(this.dbContext);

            var newCategory = await categoryService.CreateAsync(new CategoryCreateInputModel());
            var newCategoryFromDb = await this.dbContext.Categories.FindAsync(newCategory.Id);

            var expectedOrder = 4;
            var actualOrder = newCategoryFromDb.Order;
            Assert.Equal(expectedOrder, actualOrder);
        }

        [Fact]
        public async Task EditAsync_ShouldSaveChangesToDb_WhenCategoryExists()
        {
            var categoryToEditId = Guid.NewGuid().ToString();
            var category = new Category
            {
                Id = categoryToEditId,
                Name = "CurrentName",
            };

            await this.dbContext.Categories.AddAsync(category);
            await this.dbContext.SaveChangesAsync();

            var categoryWithEdits = new CategoryUpdateInputModel()
            {
                Id = categoryToEditId,
                Name = "NewName"
            };

            var categoryService = new CategoryService(this.dbContext);

            await categoryService.EditAsync(categoryWithEdits);

            var actualInterview = this.dbContext.Categories.First();

            Assert.Equal(categoryWithEdits.Id, actualInterview.Id);
            Assert.Equal(categoryWithEdits.Name, actualInterview.Name);
        }

        [Fact]
        public void EditAsync_ShouldThrowInvalidCategoryException_WhenCategoryNotExists()
        {
            var categoryWithEdits = new CategoryUpdateInputModel { Id = Guid.NewGuid().ToString() };

            var categoryService = new CategoryService(this.dbContext);

            Assert.Throws<InvalidCategoryException>(
                () => categoryService.EditAsync(categoryWithEdits).GetAwaiter().GetResult());
        }

        [Fact]
        public async Task EditAsync_ShouldThrowInvalidCategoryException_WhenIdExistsButCategoryIsSoftDeleted()
        {
            var categoryId = Guid.NewGuid().ToString();
            await this.dbContext.Categories.AddAsync(new Category { Id = categoryId, IsDeleted = true });
            await this.dbContext.SaveChangesAsync();

            var categoryWithEdits = new CategoryUpdateInputModel { Id = categoryId };
            var categoryService = new CategoryService(this.dbContext);

            Assert.Throws<InvalidCategoryException>(
                () => categoryService.EditAsync(categoryWithEdits).GetAwaiter().GetResult());
        }

        [Fact]
        public async Task ArchiveAsync_ShouldSoftDeleteCategory_WhenCategoryExists()
        {
            var categoryId = Guid.NewGuid().ToString();
            await this.dbContext.Categories.AddAsync(new Category { Id = categoryId, IsDeleted = false });
            await this.dbContext.SaveChangesAsync();

            var categoryService = new CategoryService(this.dbContext);
            await categoryService.ArchiveAsync(categoryId);

            var resultCategory = await this.dbContext.Categories.FindAsync(categoryId);
            Assert.True(resultCategory.IsDeleted);
        }

        [Fact]
        public void ArchiveAsync_ShouldThrowInvalidCategoryException_WhenCategoryNotExists()
        {
            var categoryService = new CategoryService(this.dbContext);

            Assert.Throws<InvalidCategoryException>(
                () => categoryService.ArchiveAsync(Guid.NewGuid().ToString()).GetAwaiter().GetResult());
        }

        [Fact]
        public async Task ArchiveAsync_ShouldThrowInvalidCategoryException_WhenIdExistsButCategoryIsSoftDeleted()
        {
            var categoryId = Guid.NewGuid().ToString();
            await this.dbContext.Categories.AddAsync(new Category { Id = categoryId, IsDeleted = true });
            await this.dbContext.SaveChangesAsync();
            var categoryService = new CategoryService(this.dbContext);

            Assert.Throws<InvalidCategoryException>(
                () => categoryService.ArchiveAsync(categoryId).GetAwaiter().GetResult());
        }

        [Fact]
        public async Task RecoverAsync_ShouldRecoverCategory_WhenCategoryExistsAndIsSoftDeleted()
        {
            var categoryId = Guid.NewGuid().ToString();
            await this.dbContext.Categories.AddAsync(new Category { Id = categoryId, IsDeleted = true });
            await this.dbContext.SaveChangesAsync();

            var categoryService = new CategoryService(this.dbContext);
            await categoryService.RecoverAsync(categoryId);

            var resultCategory = await this.dbContext.Categories.FindAsync(categoryId);
            Assert.False(resultCategory.IsDeleted);
        }

        [Fact]
        public void RecoverAsync_ShouldThrowInvalidCategoryException_WhenCategoryNotExists()
        {
            var categoryService = new CategoryService(this.dbContext);

            Assert.Throws<InvalidCategoryException>(
                () => categoryService.RecoverAsync(Guid.NewGuid().ToString()).GetAwaiter().GetResult());
        }

        [Fact]
        public async Task RecoverAsync_ShouldThrowInvalidCategoryException_WhenIdExistsButCategoryIsNotSoftDeleted()
        {
            var categoryId = Guid.NewGuid().ToString();
            await this.dbContext.Categories.AddAsync(new Category { Id = categoryId, IsDeleted = false });
            await this.dbContext.SaveChangesAsync();
            var categoryService = new CategoryService(this.dbContext);

            Assert.Throws<InvalidCategoryException>(
                () => categoryService.RecoverAsync(categoryId).GetAwaiter().GetResult());
        }

        internal class FakeCategory
        {
            public string Id { get; set; }
        }
    }
}
