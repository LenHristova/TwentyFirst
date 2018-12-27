namespace TwentyFirst.Services.DataServices
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

        public async Task<IEnumerable<TModel>> LatestAsync<TModel>(int count)
        {
            var interviews = await this.db.Interviews
                .Where(a => !a.IsDeleted)
                .OrderByDescending(a => a.PublishedOn)
                .Take(count)
                .To<TModel>()
                .ToListAsync();

            return interviews;
        }

        /// <inheritdoc />
        /// <summary>
        /// Gets interview by id and project it to given model.
        /// Throw InvalidInterviewException if id is not present.
        /// </summary>
        /// <exception cref="InvalidInterviewException"></exception>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<TModel> GetAsync<TModel>(string id)
        {
            var interview = await this.db.Interviews
                .Where(c => c.Id == id && !c.IsDeleted)
                .To<TModel>()
                .SingleOrDefaultAsync();

            CoreValidator.ThrowIfNull(interview, new InvalidInterviewException());
            return interview;
        }

        /// <inheritdoc />
        /// <summary>
        /// Gets interview by id.
        /// Throw InvalidInterviewException if id is not present.
        /// </summary>
        /// <exception cref="InvalidInterviewException"></exception>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Interview> GetAsync(string id)
        {
            var interview = await this.db.Interviews
                .SingleOrDefaultAsync(c => c.Id == id && !c.IsDeleted);

            CoreValidator.ThrowIfNull(interview, new InvalidInterviewException());
            return interview;
        }

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

        public async Task<Interview> EditAsync(InterviewEditInputModel interviewEditInputModel, string editorId)
        {
            var interview = await this.GetAsync(interviewEditInputModel.Id);

            interview.Title = interviewEditInputModel.Title;
            interview.Interviewed = interviewEditInputModel.Interviewed;
            interview.Content = interviewEditInputModel.Content;
            interview.Author = interviewEditInputModel.Author;
            interview.ImageId = interviewEditInputModel.Image?.Id;
            interview.Edits.Add(new InterviewEdit { EditorId = editorId, EditDateTime = DateTime.UtcNow });

            await this.db.SaveChangesAsync();
            return interview;
        }

        public async Task DeleteAsync(string interviewId, string editorId)
        {
            var interview = await this.GetAsync(interviewId);
            interview.IsDeleted = true;
            interview.Edits.Add(new InterviewEdit { EditorId = editorId, EditDateTime = DateTime.UtcNow });
            await this.db.SaveChangesAsync();
        }
    }
}
