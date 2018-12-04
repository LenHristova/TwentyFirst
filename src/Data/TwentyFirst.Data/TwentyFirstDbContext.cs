namespace TwentyFirst.Data
{
    using EntitiesConfig;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class TwentyFirstDbContext : IdentityDbContext<User>
    {
        public TwentyFirstDbContext(DbContextOptions<TwentyFirstDbContext> options)
            : base(options) { }

        public DbSet<Article> Articles { get; set; }

        public DbSet<ArticleCategory> ArticlesCategories { get; set; }

        public DbSet<ArticleEditor> ArticlesEditors { get; set; }

        public DbSet<ArticleTag> ArticlesTags { get; set; }

        public DbSet<ArticleToArticle> ArticlesToArticles { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Image> Images { get; set; }

        public DbSet<ImageEditor> ImagesEditors { get; set; }

        public DbSet<Interview> Interviews { get; set; }

        public DbSet<InterviewEditor> InterviewsEditors { get; set; }

        public DbSet<InterviewTag> InterviewsTags { get; set; }

        public DbSet<Log> Logs { get; set; }

        public DbSet<Poll> Polls { get; set; }

        public DbSet<PollAnswer> PollAnswers { get; set; }

        public DbSet<Subscriber> Subscribers { get; set; }

        public DbSet<Tag> Tags { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new ArticleCategoryConfig());
            builder.ApplyConfiguration(new ArticleConfig());
            builder.ApplyConfiguration(new ArticleEditorConfig());
            builder.ApplyConfiguration(new ArticleTagConfig());
            builder.ApplyConfiguration(new ArticleToArticleConfig());
            builder.ApplyConfiguration(new ImageConfig());
            builder.ApplyConfiguration(new ImageEditorConfig());
            builder.ApplyConfiguration(new InterviewConfig());
            builder.ApplyConfiguration(new InterviewEditorConfig());
            builder.ApplyConfiguration(new InterviewTagConfig());
            builder.ApplyConfiguration(new PollConfig());

            base.OnModelCreating(builder);
        }
    }
}
