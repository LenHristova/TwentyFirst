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

        public DbSet<ArticleEdit> ArticlesEdits { get; set; }

        public DbSet<ArticleToArticle> ArticlesToArticles { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Image> Images { get; set; }

        public DbSet<Interview> Interviews { get; set; }

        public DbSet<InterviewEdit> InterviewsEdits { get; set; }

        public DbSet<Log> Logs { get; set; }

        public DbSet<Poll> Polls { get; set; }

        public DbSet<PollAnswer> PollAnswers { get; set; }

        public DbSet<Subscriber> Subscribers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new ArticleCategoryConfig());
            builder.ApplyConfiguration(new ArticleConfig());
            builder.ApplyConfiguration(new ArticleEditConfig());
            builder.ApplyConfiguration(new ArticleToArticleConfig());
            builder.ApplyConfiguration(new ImageConfig());
            builder.ApplyConfiguration(new InterviewConfig());
            builder.ApplyConfiguration(new InterviewEditConfig());
            builder.ApplyConfiguration(new PollConfig());

            base.OnModelCreating(builder);
        }
    }
}
