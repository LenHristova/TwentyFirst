namespace TwentyFirst.Data.EntitiesConfig
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    public class ArticleTagConfig : IEntityTypeConfiguration<ArticleTag>
    {
        public void Configure(EntityTypeBuilder<ArticleTag> builder)
        {
            builder
                .HasKey(at => new { at.ArticleId, at.TagId });

            builder
                .HasOne(at => at.Article)
                .WithMany(a => a.Tags)
                .HasForeignKey(a => a.ArticleId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(at => at.Tag)
                .WithMany(t => t.Articles)
                .HasForeignKey(at => at.TagId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
