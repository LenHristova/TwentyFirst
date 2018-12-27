namespace TwentyFirst.Services.DataServices
{
    using AutoMapper;
    using Common;
    using Common.Exceptions;
    using Common.Mapping;
    using Common.Models.Articles;
    using Contracts;
    using Data;
    using Data.Models;
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

        public async Task<IEnumerable<TModel>> LatestAsync<TModel>(int count)
        {
            var articles = await this.db.Articles
                .Where(a => !a.IsDeleted)
                .OrderByDescending(a => a.PublishedOn)
                .Take(count)
                .To<TModel>()
                .ToListAsync();

            return articles;
        }

        public async Task<IEnumerable<TModel>> LatestTopAsync<TModel>(int count)
            => await this.db.Articles
                .Where(a => a.IsTop && !a.IsDeleted)
                .OrderByDescending(a => a.PublishedOn)
                .Take(count)
                .To<TModel>()
                .ToListAsync();

        public async Task<IEnumerable<TModel>> LatestFromCategoryAsync<TModel>(string categoryId, int count)
        {
            this.categoryService.ThrowIfNotExists(categoryId);

            return await this.db.Articles
                .Where(a => !a.IsDeleted && a.Categories.Any(c => c.CategoryId == categoryId))
                .OrderByDescending(a => a.PublishedOn)
                .Take(count)
                .To<TModel>()
                .ToListAsync();
        }

        public async Task<IEnumerable<TModel>> LatestImportantAsync<TModel>(int count)
        {
            var articles = await this.db.Articles
                .Where(a => !a.IsDeleted && a.IsImportant)
                .OrderByDescending(a => a.PublishedOn)
                .Take(count)
                .To<TModel>()
                .ToListAsync();

            return articles;
        }

        public async Task<IEnumerable<TModel>> LatestImportantFromCategoriesAsync<TModel>(
            IEnumerable<string> ids, int count)
        {
            if (ids == null || !ids.Any())
            {
                return new List<TModel>();
            }

            var articles = await this.db.Articles
                .Where(a => !a.IsDeleted &&
                            a.IsImportant &&
                            a.Categories.Any(c => !c.Category.IsDeleted && ids.Contains(c.Category.Id)))
                .OrderByDescending(a => a.PublishedOn)
                .Take(count)
                .To<TModel>()
                .ToListAsync();

            return articles;
        }

        public async Task<IEnumerable<TModel>> AllImportantForTheDayAsync<TModel>()
        {
            var now = DateTime.UtcNow;
            var untilDateTime = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);

            return await this.db.Articles
                 .Where(a => a.IsImportant && !a.IsDeleted && a.PublishedOn > untilDateTime)
                 .OrderByDescending(a => a.PublishedOn)
                 .To<TModel>()
                 .ToListAsync();
        }

        /// <inheritdoc />
        /// <summary>
        /// Gets article by id and project it to given model.
        /// Throw InvalidArticleException if id is not present.
        /// </summary>
        /// <exception cref="InvalidArticleException"></exception>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<TModel> GetAsync<TModel>(string id)
        {
            var article = await this.db.Articles
                .Where(c => c.Id == id && !c.IsDeleted)
                .To<TModel>()
                .SingleOrDefaultAsync();

            CoreValidator.ThrowIfNull(article, new InvalidArticleException());
            return article;
        }

        /// <inheritdoc />
        /// <summary>
        /// Gets article by id.
        /// Throw InvalidArticleException if id is not present.
        /// </summary>
        /// <exception cref="InvalidArticleException"></exception>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Article> GetAsync(string id)
        {
            var article = await this.db.Articles
                .Include(c => c.Categories)
                .Include(c => c.ConnectedTo)
                .SingleOrDefaultAsync(c => c.Id == id && !c.IsDeleted);

            CoreValidator.ThrowIfNull(article, new InvalidArticleException());
            return article;
        }

        public async Task<Article> CreateAsync(ArticleCreateInputModel articleCreateInputModel, string creatorId)
        {
            var article = Mapper.Map<Article>(articleCreateInputModel);

            this.categoryService.ThrowIfAnyNotExist(articleCreateInputModel.CategoriesIds);
            this.ThrowIfAnyNotExist(articleCreateInputModel.ConnectedArticlesIds);

            UpdateCategories(article.Categories, articleCreateInputModel.CategoriesIds?.ToList());
            UpdateConnectedArticles(article.ConnectedTo, articleCreateInputModel.ConnectedArticlesIds?.ToList());

            article.CreatorId = creatorId;
            article.PublishedOn = DateTime.UtcNow;
            article.IsDeleted = false;

            await this.db.Articles.AddAsync(article);
            await this.db.SaveChangesAsync();
            return article;
        }

        public async Task<Article> EditAsync(ArticleEditInputModel articleEditInputModel, string editorId)
        {
            var article = await this.GetAsync(articleEditInputModel.Id);

            this.categoryService.ThrowIfAnyNotExist(articleEditInputModel.CategoriesIds);
            this.ThrowIfAnyNotExist(articleEditInputModel.ConnectedArticlesIds);

            UpdateCategories(article.Categories, articleEditInputModel.CategoriesIds?.ToList());
            UpdateConnectedArticles(article.ConnectedTo, articleEditInputModel.ConnectedArticlesIds?.ToList());

            article.Title = articleEditInputModel.Title;
            article.Lead = articleEditInputModel.Lead;
            article.Content = articleEditInputModel.Content;
            article.Author = articleEditInputModel.Author;
            article.ImageId = articleEditInputModel.Image?.Id;
            article.IsTop = articleEditInputModel.IsTop;
            article.IsImportant = articleEditInputModel.IsImportant;
            article.Edits.Add(new ArticleEdit { EditorId = editorId, EditDateTime = DateTime.UtcNow });

            await this.db.SaveChangesAsync();
            return article;
        }

        public async Task DeleteAsync(string articleId, string editorId)
        {
            var article = await this.GetAsync(articleId);
            article.IsDeleted = true;
            article.Edits.Add(new ArticleEdit { EditorId = editorId, EditDateTime = DateTime.UtcNow });
            await this.db.SaveChangesAsync();
        }

        private void UpdateCategories(ICollection<ArticleCategory> currentCategories, ICollection<string> newCategoriesIds)
        {
            newCategoriesIds = newCategoriesIds ?? new List<string>();

            foreach (var category in currentCategories)
            {
                if (!newCategoriesIds.Contains(category.CategoryId))
                {
                    this.db.Remove(category);
                }
                else
                {
                    newCategoriesIds.Remove(category.CategoryId);
                }
            }

            foreach (var categoriesId in newCategoriesIds)
            {
                currentCategories.Add(new ArticleCategory { CategoryId = categoriesId });
            }
        }

        private void UpdateConnectedArticles(ICollection<ArticleToArticle> currentConnectedArticles, ICollection<string> newConnectedArticlesIds)
        {
            newConnectedArticlesIds = newConnectedArticlesIds ?? new List<string>();

            foreach (var connectedArticle in currentConnectedArticles)
            {
                if (!newConnectedArticlesIds.Contains(connectedArticle.ConnectedToId))
                {
                    this.db.Remove(connectedArticle);
                }
                else
                {
                    newConnectedArticlesIds.Remove(connectedArticle.ConnectedToId);
                }
            }

            foreach (var connectedArticleI in newConnectedArticlesIds)
            {
                currentConnectedArticles.Add(new ArticleToArticle { ConnectedToId = connectedArticleI });
            }
        }

        public void ThrowIfAnyNotExist(IEnumerable<string> ids)
        {
            if (ids != null)
            {
                var foundArticles = this.db.Articles.Where(a => ids.Contains(a.Id)).ToList();

                if (foundArticles.Count != ids.Count())
                {
                    throw new InvalidArticleException();
                }
            }
        }
    }
}