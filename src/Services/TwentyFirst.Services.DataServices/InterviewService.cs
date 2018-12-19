﻿namespace TwentyFirst.Services.DataServices
{
    using AutoMapper;
    using Common;
    using Common.Exceptions;
    using Common.Mapping;
    using Common.Models.Interviews;
    using Contracts;
    using Data;
    using Data.Models;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class InterviewService : IInterviewService
    {
        private readonly TwentyFirstDbContext db;

        public InterviewService(TwentyFirstDbContext db)
        {
            this.db = db;
        }

        public async Task<IEnumerable<TModel>> AllAsync<TModel>()
            => await this.db.Interviews
                .Where(a => !a.IsDeleted)
                .OrderByDescending(a => a.PublishedOn)
                .To<TModel>()
                .ToListAsync();

        public async Task<Interview> CreateAsync(InterviewCreateInputModel interviewCreateInputModel, string creatorId)
        {
            var interview = Mapper.Map<Interview>(interviewCreateInputModel);
            interview.CreatorId = creatorId;
            interview.PublishedOn = DateTime.UtcNow;
            interview.IsDeleted = false;

            await this.db.Interviews.AddAsync(interview);
            await this.db.SaveChangesAsync();
            return interview;
        }

        public async Task<Interview> Edit(InterviewEditInputModel interviewEditInputModel, string editorId)
        {
            var interview = await this.GetAsync(interviewEditInputModel.Id);

            interview.Title = interviewEditInputModel.Title;
            interview.Interviewed = interviewEditInputModel.Interviewed;
            interview.Content = interviewEditInputModel.Content;
            interview.Author = interviewEditInputModel.Author;
            interview.ImageId = interviewEditInputModel.Image.Id;
            interview.Edits.Add(new InterviewEdit { EditorId = editorId, EditDateTime = DateTime.UtcNow });

            await this.db.SaveChangesAsync();
            return interview;
        }

        public async Task Delete(string articleId, string editorId)
        {
            var article = await this.GetAsync(articleId);
            article.IsDeleted = true;
            article.Edits.Add(new InterviewEdit { EditorId = editorId, EditDateTime = DateTime.UtcNow });
            await this.db.SaveChangesAsync();
        }

        /// <inheritdoc />
        /// <summary>
        /// Gets interview by id and project it to given model.
        /// Throw InvalidInterviewIdException if id is not present.
        /// </summary>
        /// <exception cref="InvalidInterviewIdException"></exception>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<TModel> GetAsync<TModel>(string id)
        {
            var interview = await this.db.Interviews
                .Where(c => c.Id == id && !c.IsDeleted)
                .To<TModel>()
                .SingleOrDefaultAsync();

            CoreValidator.ThrowIfNull(interview, new InvalidArticleIdException(id));
            return interview;
        }

        /// <inheritdoc />
        /// <summary>
        /// Gets interview by id.
        /// Throw InvalidInterviewIdException if id is not present.
        /// </summary>
        /// <exception cref="InvalidInterviewIdException"></exception>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Interview> GetAsync(string id)
        {
            var interview = await this.db.Interviews
                .SingleOrDefaultAsync(c => c.Id == id && !c.IsDeleted);

            CoreValidator.ThrowIfNull(interview, new InvalidArticleIdException(id));
            return interview;
        }
    }
}
