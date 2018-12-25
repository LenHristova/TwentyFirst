namespace TwentyFirst.Services.DataServices
{
    using AutoMapper;
    using Common;
    using Common.Exceptions;
    using Common.Mapping;
    using Common.Models.Categories;
    using Contracts;
    using Data;
    using Data.Models;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class CategoryService : ICategoryService
    {
        private readonly TwentyFirstDbContext db;

        public CategoryService(TwentyFirstDbContext db)
        {
            this.db = db;
        }

        public async Task<Category> CreateAsync(CategoryCreateInputModel categoryCreateInputModel)
        {
            var category = Mapper.Map<Category>(categoryCreateInputModel);
            category.IsDeleted = false;

            var lastCategoryOrder = this.db.Categories.OrderByDescending(c => c.Order).FirstOrDefault()?.Order ?? 0;
            category.Order = lastCategoryOrder + 1;

            await this.db.Categories.AddAsync(category);
            await this.db.SaveChangesAsync();
            return category;
        }

        public async Task<Category> EditAsync(CategoryUpdateInputModel categoryUpdateInputModel)
        {
            var category = await this.GetAsync(categoryUpdateInputModel.Id);
            category.Name = categoryUpdateInputModel.Name;

            await this.db.SaveChangesAsync();
            return category;
        }

        public async Task<IEnumerable<TModel>> AllWithArchived<TModel>()
            => await this.db.Categories
                .OrderBy(c => c.IsDeleted)
                .ThenBy(c => c.Order)
                .To<TModel>()
                .ToListAsync();

        public async Task<IEnumerable<TModel>> All<TModel>()
            => await this.db.Categories
                .Where(c => !c.IsDeleted)
                .OrderBy(c => c.Order)
                .To<TModel>()
                .ToListAsync();

        public async Task<Category> ArchiveAsync(string id)
        {
            var category = await this.GetAsync(id);
            category.IsDeleted = true;

            await this.db.SaveChangesAsync();
            return category;
        }

        public async Task<Category> RecoverAsync(string id)
        {
            var category = await this.GetArchivedAsync(id);
            category.IsDeleted = false;

            await this.db.SaveChangesAsync();
            return category;
        }

        /// <inheritdoc />
        /// <summary>
        /// Get category by id and project it to given model.
        /// Throw InvalidCategoryIdException if id is not present.
        /// </summary>
        /// <exception cref="InvalidCategoryIdException"></exception>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<TModel> GetAsync<TModel>(string id)
        {
            var result = await this.db.Categories
                .Where(c => c.Id == id && !c.IsDeleted)
                .To<TModel>()
                .SingleOrDefaultAsync();

            CoreValidator.ThrowIfNull(result, new InvalidCategoryIdException(id));
            return result;
        }

        /// <inheritdoc />
        /// <summary>
        /// Get category by id
        /// Throw InvalidCategoryIdException if id is not present.
        /// </summary>
        /// <exception cref="InvalidCategoryIdException"></exception>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Category> GetAsync(string id)
        {
            var result = await this.db.Categories
                .SingleOrDefaultAsync(c => c.Id == id && !c.IsDeleted);

            CoreValidator.ThrowIfNull(result, new InvalidCategoryIdException(id));
            return result;
        }

        /// <inheritdoc />
        /// <summary>
        /// Get archived category by id and project it to given model.
        /// Throw InvalidCategoryIdException if id is not present.
        /// </summary>
        /// <exception cref="InvalidCategoryIdException"></exception>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<TModel> GetArchivedAsync<TModel>(string id)
        {
            var result = await this.db.Categories
                 .Where(c => c.Id == id && c.IsDeleted)
                 .To<TModel>()
                 .SingleOrDefaultAsync();

            CoreValidator.ThrowIfNull(result, new InvalidCategoryIdException(id));
            return result;
        }

        /// <inheritdoc />
        /// <summary>
        /// Get archived category by id.
        /// Throw InvalidCategoryIdException if id is not present.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Category> GetArchivedAsync(string id)
        {
            var result = await this.db.Categories
                .SingleOrDefaultAsync(c => c.Id == id && c.IsDeleted);

            CoreValidator.ThrowIfNull(result, new InvalidCategoryIdException(id));
            return result;
        }

        public async Task<IEnumerable<SelectListItem>> AllToSelectListItemsAsync()
            => await this.db.Categories
                .Where(c => c.IsDeleted == false)
                .OrderBy(c => c.Name)
                .Select(a => new SelectListItem
                {
                    Value = a.Id,
                    Text = a.Name
                })
                .ToListAsync();

        public async Task<Category> OrderAsync(string id, bool isUp, bool isDown)
        {
            if ((isUp && isDown) || (!isUp && !isDown))
            {
                throw new InvalidCategoryOrderException();
            }

            var category = await this.GetAsync(id);
            var categoryToSwitch = isUp
                ? await this.GetPreviousToSwitchAsync(category.Order) 
                : await this.GetNextToSwitchAsync(category.Order);

            if (categoryToSwitch == null)
            {
                throw new InvalidCategoryOrderException();
            }

            var currentCategoryNewOrder = categoryToSwitch.Order;
            categoryToSwitch.Order = category.Order;
            category.Order = currentCategoryNewOrder;

            await this.db.SaveChangesAsync();

            return category;
        }

        private async Task<Category> GetPreviousToSwitchAsync(int order)
            => await this.db.Categories
                .Where(c => !c.IsDeleted && c.Order < order)
                .OrderByDescending(c => c.Order)
                .FirstOrDefaultAsync();

        private async Task<Category> GetNextToSwitchAsync(int order)
            => await this.db.Categories
                .Where(c => !c.IsDeleted && c.Order > order)
                .OrderBy(c => c.Order)
                .FirstOrDefaultAsync();
    }
}
