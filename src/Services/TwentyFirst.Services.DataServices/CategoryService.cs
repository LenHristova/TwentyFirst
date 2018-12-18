namespace TwentyFirst.Services.DataServices
{
    using AutoMapper;
    using Common.Mapping;
    using Common.Models.Categories;
    using Contracts;
    using Data;
    using Data.Models;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Common;
    using Common.Exceptions;

    public class CategoryService : ICategoryService
    {
        private readonly TwentyFirstDbContext db;

        public CategoryService(TwentyFirstDbContext db)
        {
            this.db = db;
        }

        public async Task<bool> CreateAsync(CategoryCreateInputModel categoryCreateInputModel)
        {
            var category = Mapper.Map<Category>(categoryCreateInputModel);
            category.IsDeleted = false;

            try
            {
                await this.db.Categories.AddAsync(category);
                await this.db.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                //TODO log
                return false;
            }
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

        public async Task<bool> ArchiveAsync(string id)
        {
            var category = await this.db.Categories.FindAsync(id);

            if (category == null)
            {
                return false;
            }

            category.IsDeleted = true;

            try
            {
                await this.db.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                //TODO log
                return false;
            }
        }

        public async Task<bool> RecoverAsync(string id)
        {
            var category = await this.db.Categories.FindAsync(id);

            if (category == null)
            {
                return false;
            }

            category.IsDeleted = false;

            try
            {
                await this.db.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                //TODO log
                return false;
            }
        }

        public async Task<bool> EditAsync(CategoryUpdateInputModel categoryUpdateInputModel)
        {
            var category = await this.db.Categories.FindAsync(categoryUpdateInputModel.Id);

            if (category == null)
            {
                return false;
            }

            try
            {
                category.Name = categoryUpdateInputModel.Name;
                await this.db.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                //TODO log
                return false;
            }
        }

        public bool Exists(string id)
            => this.db.Categories.Any(c => c.Id == id && c.IsDeleted == false);

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

        public async Task<TModel> GetArchivedAsync<TModel>(string id)
            => await this.db.Categories
                .Where(c => c.Id == id && c.IsDeleted)
                .To<TModel>()
                .SingleOrDefaultAsync();

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
