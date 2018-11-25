namespace TwentyFirst.Data.EntitiesConfig
{
    using Domain;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class ArticleCategoryConfig : IEntityTypeConfiguration<ArticleCategory>
    {
        public void Configure(EntityTypeBuilder<ArticleCategory> builder)
        {
            builder
                .HasKey(ac => new { ac.ArticleId, ac.CategoryId });

            builder
                .HasOne(ac => ac.Article)
                .WithMany(a => a.Categories)
                .HasForeignKey(ac => ac.ArticleId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(ac => ac.Category)
                .WithMany(c => c.Articles)
                .HasForeignKey(ac => ac.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
