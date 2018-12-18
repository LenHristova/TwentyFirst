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

        public async Task<Article> CreateAsync(ArticleCreateInputModel articleCreateInputModel, string creatorId)
        {
            var article = Mapper.Map<Article>(articleCreateInputModel);

            await AddCategories(article, articleCreateInputModel.CategoriesIds);
            await AddConnectedArticles(article, articleCreateInputModel.ConnectedArticlesIds);
            article.CreatorId = creatorId;
            article.PublishedOn = DateTime.UtcNow;
            article.IsDeleted = false;

            await this.db.Articles.AddAsync(article);
            await this.db.SaveChangesAsync();
            return article;
        }

        public async Task<Article> Edit(ArticleEditInputModel articleUpdateInputModel, string editorId)
        {
            var article = await this.GetAsync(articleUpdateInputModel.Id);
            if (articleUpdateInputModel.CategoriesIds != null)
            {
                this.RemoveMissingCategories(articleUpdateInputModel, article);
                await AddNewCategories(articleUpdateInputModel, article);
            }
            else if (article.ConnectedTo.Any())
            {
                this.db.ArticlesCategories.RemoveRange(article.Categories);
            }

            if (articleUpdateInputModel.ConnectedArticlesIds != null)
            {
                this.RemoveMissingConnectedArticles(articleUpdateInputModel, article);

                await AddNewConnectedArticles(articleUpdateInputModel, article);
            }
            else if (article.ConnectedTo.Any())
            {
                this.db.ArticlesToArticles.RemoveRange(article.ConnectedTo);
            }

            article.Title = articleUpdateInputModel.Title;
            article.Lead = articleUpdateInputModel.Lead;
            article.Content = articleUpdateInputModel.Content;
            article.Author = articleUpdateInputModel.Author;
            article.ImageId = articleUpdateInputModel.Image.Id;
            article.IsTop = articleUpdateInputModel.IsTop;
            article.IsImportant = articleUpdateInputModel.IsImportant;
            article.Edits.Add(new ArticleEdit { EditorId = editorId, EditDateTime = DateTime.UtcNow });

            await this.db.SaveChangesAsync();
            return article;
        }

        public async Task Delete(string articleId, string editorId)
        {
            var article = await this.GetAsync(articleId);
            article.IsDeleted = true;
            article.Edits.Add(new ArticleEdit { EditorId = editorId, EditDateTime = DateTime.UtcNow });
            await this.db.SaveChangesAsync();
        }

        public async Task<IEnumerable<SelectListItem>> AllToSelectListItemsAsync()
            => await this.db.Articles
                .Where(a => !a.IsDeleted)
                .Select(a => new SelectListItem
                {
                    Value = a.Id,
                    Text = a.Title
                })
                .ToListAsync();

        /// <inheritdoc />
        /// <summary>
        /// Gets article by id and project it to given model.
        /// Throw InvalidArticleIdException if id is not present.
        /// </summary>
        /// <exception cref="InvalidArticleIdException"></exception>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<TModel> GetAsync<TModel>(string id)
        {
            var article = await this.db.Articles
                .Where(c => c.Id == id && !c.IsDeleted)
                .To<TModel>()
                .SingleOrDefaultAsync();

            CoreValidator.ThrowIfNull(article, new InvalidArticleIdException(id));
            return article;
        }

        /// <inheritdoc />
        /// <summary>
        /// Gets article by id.
        /// Throw InvalidArticleIdException if id is not present.
        /// </summary>
        /// <exception cref="InvalidArticleIdException"></exception>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Article> GetAsync(string id)
        {
            var article = await this.db.Articles
                .Include(c => c.Categories)
                .Include(c => c.ConnectedTo)
                .SingleOrDefaultAsync(c => c.Id == id && !c.IsDeleted);

            CoreValidator.ThrowIfNull(article, new InvalidArticleIdException(id));
            return article;
        }

        public async Task<IEnumerable<TModel>> GetLastAddedFromCategoriesAsync<TModel>(IEnumerable<string> ids, int count)
            => await this.db.Articles
                .Where(a => !a.IsDeleted &&
                            a.Categories.Any(c => !c.Category.IsDeleted && ids.Contains(c.Category.Id)))
                .OrderByDescending(a => a.PublishedOn)
                .Take(count)
                .To<TModel>()
                .ToListAsync();

        public async Task<IEnumerable<TModel>> AllAsync<TModel>()
            => await this.db.Articles
                .Where(a => !a.IsDeleted)
                .OrderByDescending(a => a.PublishedOn)
                .To<TModel>()
                .ToListAsync();

        private async Task AddNewConnectedArticles(ArticleEditInputModel articleUpdateInputModel, Article article)
        {
            var newConnectedArticles = articleUpdateInputModel.ConnectedArticlesIds
                .Where(id => !article.ConnectedTo.Select(aa => aa.ConnectedToId).Contains(id))
                .ToList();

            if (newConnectedArticles.Any())
            {
                await AddConnectedArticles(article, newConnectedArticles);
            }
        }

        private void RemoveMissingConnectedArticles(ArticleEditInputModel articleUpdateInputModel, Article article)
        {
            var missingConnectedArticles = article.ConnectedTo
                .Where(id => !articleUpdateInputModel.ConnectedArticlesIds.Contains(id.ConnectedToId))
                .ToList();

            if (missingConnectedArticles.Any())
            {
                this.db.ArticlesToArticles.RemoveRange(missingConnectedArticles);
            }
        }

        private async Task AddNewCategories(ArticleEditInputModel articleUpdateInputModel, Article article)
        {
            var newCategories = articleUpdateInputModel.CategoriesIds
                .Where(c => !article.Categories.Select(ac => ac.CategoryId).Contains(c))
                .ToList();

            if (newCategories.Any())
            {
                await AddCategories(article, newCategories);
            }
        }

        private void RemoveMissingCategories(ArticleEditInputModel articleUpdateInputModel, Article article)
        {
            var missingCategories = article.Categories
                .Where(c => !articleUpdateInputModel.CategoriesIds.Contains(c.CategoryId))
                .ToList();

            if (missingCategories.Any())
            {
                this.db.ArticlesCategories.RemoveRange(missingCategories);
            }
        }

        private async Task AddConnectedArticles(Article article, IEnumerable<string> newConnectedArticles)
        {
            if (newConnectedArticles != null)
            {
                foreach (var connectedArticleId in newConnectedArticles)
                {
                    var connectedArticle = await this.GetAsync(connectedArticleId);
                    article.ConnectedTo.Add(new ArticleToArticle { ConnectedTo = connectedArticle });
                }
            }
        }

        private async Task AddCategories(Article article, IEnumerable<string> newCategories)
        {
            if (newCategories != null)
            {
                foreach (var categoryId in newCategories)
                {
                    var category = await this.categoryService.GetAsync(categoryId);
                    article.Categories.Add(new ArticleCategory { Category = category });
                }
            }
        }
    }
}
