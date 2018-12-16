namespace TwentyFirst.Services.DataServices
{
    using AutoMapper;
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

                if (articleCreateInputModel.ConnectedArticlesIds != null)
                {
                    foreach (var connectedArticleId in articleCreateInputModel.ConnectedArticlesIds)
                    {
                        var connectedArticleExists = this.Exists(connectedArticleId);
                        if (!connectedArticleExists)
                        {
                            return null;
                        }

                        article.ConnectedTo.Add(new ArticleToArticle { ConnectedToId = connectedArticleId });
                    }
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

        public bool Edit(ArticleEditInputModel articleUpdateInputModel, string editorId)
        {
            try
            {
                var article = this.db.Articles
                    .Include(c => c.Categories)
                    .Include(c => c.ConnectedTo)
                    .SingleOrDefault(a => a.Id == articleUpdateInputModel.Id);

                if (article == null)
                {
                    return false;
                }

                article.Title = articleUpdateInputModel.Title;
                article.Lead = articleUpdateInputModel.Lead;
                article.Content = articleUpdateInputModel.Content;
                article.Author = articleUpdateInputModel.Author;
                article.ImageId = articleUpdateInputModel.ImageId;
                article.IsTop = articleUpdateInputModel.IsTop;
                article.IsImportant = articleUpdateInputModel.IsImportant;
                article.Edits.Add(new ArticleEdit { EditorId = editorId, EditDateTime = DateTime.UtcNow });

                var missingCategories = article.Categories
                    .Where(c => !articleUpdateInputModel.CategoriesIds.Contains(c.CategoryId))
                    .ToList();

                if (missingCategories.Any())
                {
                    this.db.ArticlesCategories.RemoveRange(missingCategories);
                }

                var newCategories = articleUpdateInputModel.CategoriesIds
                    .Where(c => !article.Categories.Select(ac => ac.CategoryId).Contains(c))
                    .ToList();

                foreach (var categoryId in newCategories)
                {
                    var categoryExists = this.categoryService.Exists(categoryId);
                    if (!categoryExists)
                    {
                        return false;
                    }

                    article.Categories.Add(new ArticleCategory { CategoryId = categoryId });
                }

                if (articleUpdateInputModel.ConnectedArticlesIds != null)
                {
                    var missingConnectedArticles = article.ConnectedTo
                        .Where(id => !articleUpdateInputModel.ConnectedArticlesIds.Contains(id.ConnectedToId))
                        .ToList();

                    if (missingConnectedArticles.Any())
                    {
                        this.db.ArticlesToArticles.RemoveRange(missingConnectedArticles);
                    }

                    var newConnectedArticles = articleUpdateInputModel.ConnectedArticlesIds
                        .Where(id => !article.ConnectedTo.Select(aa => aa.ConnectedToId).Contains(id));

                    foreach (var connectedArticleId in newConnectedArticles)
                    {
                        var connectedArticleExists = this.Exists(connectedArticleId);
                        if (!connectedArticleExists)
                        {
                            return false;
                        }

                        article.ConnectedTo.Add(new ArticleToArticle { ConnectedToId = connectedArticleId });
                    }
                }
                else if (article.ConnectedTo.Any())
                {
                    this.db.ArticlesToArticles.RemoveRange(article.ConnectedTo);
                }

                this.db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                //TODO process
                return false;
            }
        }

        public bool Exists(string id)
            => this.db.Articles.Any(a => a.Id == id && !a.IsDeleted);

        public bool Delete(string articleId, string editorId)
        {
            var article = this.db.Articles.Find(articleId);
            article.IsDeleted = true;
            article.Edits.Add(new ArticleEdit { EditorId = editorId, EditDateTime = DateTime.UtcNow });
            try
            {
                this.db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                //TODO process
                return false;
            }
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

        public async Task<TModel> GetAsync<TModel>(string id)
            => await this.db.Articles
                .Where(c => c.Id == id && !c.IsDeleted)
                .To<TModel>()
                .SingleOrDefaultAsync();

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
    }
}
