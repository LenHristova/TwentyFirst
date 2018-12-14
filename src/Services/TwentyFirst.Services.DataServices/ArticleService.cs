namespace TwentyFirst.Services.DataServices
{
    using AutoMapper;
    using Common.Models.Articles;
    using Contracts;
    using Data;
    using Data.Models;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class ArticleService : IArticleService
    {
        private readonly TwentyFirstDbContext db;
        private readonly ICategoryService categoryService;

        public ArticleService(
            TwentyFirstDbContext db,
            ICategoryService categoryService)
        {
            this.db = db;
            this.categoryService = categoryService;
        }

        public async Task<string> CreateAsync(ArticleCreateInputModel articleCreateInputModel, string creatorId)
        {
            try
            {
                var article = Mapper.Map<Article>(articleCreateInputModel);
                article.CreatorId = creatorId;
                article.PublishedOn = DateTime.UtcNow;
                article.IsDeleted = false;

                foreach (var categoriesId in articleCreateInputModel.CategoriesIds)
                {
                    var categoryExists = this.categoryService.Exists(categoriesId);
                    if (!categoryExists)
                    {
                        return null;
                    }

                    article.Categories.Add(new ArticleCategory { CategoryId = categoriesId });
                }

                foreach (var connectedArticleId in articleCreateInputModel.ConnectedArticlesIds)
                {
                    var connectedArticleExists = this.Exists(connectedArticleId);
                    if (!connectedArticleExists)
                    {
                        return null;
                    }

                    article.ConnectedTo.Add(new ArticleToArticle { ConnectedToId = connectedArticleId });
                }

                await this.db.Articles.AddAsync(article);
                await this.db.SaveChangesAsync();
                return article.Id;
            }
            catch (Exception e)
            {
                //TODO process
                throw;
            }
        }

        private bool Exists(string id)
            => this.db.Articles.Any(a => a.Id == id);

        public async Task<IEnumerable<SelectListItem>> AllToSelectListItemsAsync()
            => await this.db.Articles
                .Select(a => new SelectListItem
                {
                    Value = a.Id,
                    Text = a.Title
                })
                .ToListAsync();
    }
}
