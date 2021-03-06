﻿namespace TwentyFirst.Services.DataServices
{
    using AutoMapper;
    using Common;
    using Common.Exceptions;
    using Common.Mapping;
    using Common.Models.Categories;
    using Contracts;
    using Data;
    using Data.Models;
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

        public async Task<IEnumerable<TModel>> AllWithArchivedAsync<TModel>()
            => await this.db.Categories
                .OrderBy(c => c.IsDeleted)
                .ThenBy(c => c.Order)
                .To<TModel>()
                .ToListAsync();

        public async Task<IEnumerable<TModel>> AllAsync<TModel>()
            => await this.db.Categories
                .Where(c => !c.IsDeleted)
                .OrderBy(c => c.Order)
                .To<TModel>()
                .ToListAsync();

        /// <inheritdoc />
        /// <summary>
        /// Get category by id and project it to given model.
        /// Throw InvalidCategoryIdException if id is not present.
        /// </summary>
        /// <exception cref="InvalidCategoryException"></exception>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<TModel> GetAsync<TModel>(string id)
        {
            var result = await this.db.Categories
                .Where(c => c.Id == id && !c.IsDeleted)
                .To<TModel>()
                .SingleOrDefaultAsync();

            CoreValidator.ThrowIfNull(result, new InvalidCategoryException());
            return result;
        }

        /// <inheritdoc />
        /// <summary>
        /// Get category by id
        /// Throw InvalidCategoryIdException if id is not present.
        /// </summary>
        /// <exception cref="InvalidCategoryException"></exception>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Category> GetAsync(string id)
        {
            var result = await this.db.Categories
                .SingleOrDefaultAsync(c => c.Id == id && !c.IsDeleted);

            CoreValidator.ThrowIfNull(result, new InvalidCategoryException());
            return result;
        }

        /// <inheritdoc />
        /// <summary>
        /// Get archived category by id and project it to given model.
        /// Throw InvalidCategoryIdException if id is not present.
        /// </summary>
        /// <exception cref="InvalidCategoryException"></exception>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<TModel> GetArchivedAsync<TModel>(string id)
        {
            var result = await this.db.Categories
                 .Where(c => c.Id == id && c.IsDeleted)
                 .To<TModel>()
                 .SingleOrDefaultAsync();

            CoreValidator.ThrowIfNull(result, new InvalidCategoryException());
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

            CoreValidator.ThrowIfNull(result, new InvalidCategoryException());
            return result;
        }

        public async Task<IEnumerable<TModel>> AllOrderedByNameAsync<TModel>()
            => await this.db.Categories
                .Where(c => c.IsDeleted == false)
                .OrderBy(c => c.Name)
                .To<TModel>()
                .ToListAsync();

        public async Task<Category> ReorderUpAsync(string id)
        {
            var category = await this.GetAsync(id);
            var categoryToSwitch = await this.db.Categories
                .Where(c => !c.IsDeleted && c.Order < category.Order)
                .OrderByDescending(c => c.Order)
                .FirstOrDefaultAsync();

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

        public async Task<Category> ReorderDownAsync(string id)
        {
            var category = await this.GetAsync(id);
            var categoryToSwitch = await this.db.Categories
                .Where(c => !c.IsDeleted && c.Order > category.Order)
                .OrderBy(c => c.Order)
                .FirstOrDefaultAsync();

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

        public void ThrowIfAnyNotExist(IEnumerable<string> ids)
        {
            if (ids != null)
            {
                var foundCategories = this.db.Categories.Where(c => !c.IsDeleted && ids.Contains(c.Id)).ToList();

                if (foundCategories.Count != ids.Count())
                {
                    throw new InvalidCategoryException();
                }
            }
        }

        public void ThrowIfNotExists(string id)
        {
            this.ThrowIfAnyNotExist(new List<string> { id });
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
    }
}
