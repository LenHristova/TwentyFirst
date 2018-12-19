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
                .To<TModel>()
                .ToListAsync();

        //public async Task<IEnumerable<TModel>> All<TModel>()
        //    => await this.db.Categories
        //        .Where(c => c.IsDeleted == false)
        //        .To<TModel>()
        //        .ToListAsync();

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
                .Where(c => c.Id == id && c.IsDeleted == false)
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
                .SingleOrDefaultAsync(c => c.Id == id && c.IsDeleted == false);

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
                .Select(a => new SelectListItem
                {
                    Value = a.Id,
                    Text = a.Name
                })
                .ToListAsync();
    }
}
