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

        public async Task<IEnumerable<TModel>> All<TModel>()
            => await this.db.Categories
                .To<TModel>()
                .ToListAsync();

        public async Task<bool> DeleteAsync(string id)
        {
            var category = await this.db.Categories.FindAsync(id);

            if (category == null)
            {
                return false;
            }

            try
            {
                //TODO add softDelete
                this.db.Remove(category);
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
            => this.db.Categories.Any(c => c.Id == id);

        public TModel Get<TModel>(string id)
            => this.db.Categories
                .Where(c => c.Id == id)
                .To<TModel>()
                .FirstOrDefault();

        public async Task<IEnumerable<SelectListItem>> AllToSelectListItemsAsync()
            => await this.db.Categories
                .Select(a => new SelectListItem
                {
                    Value = a.Id,
                    Text = a.Name
                })
                .ToListAsync();
    }
}
