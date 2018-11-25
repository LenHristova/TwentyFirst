namespace TwentyFirst.Data.EntitiesConfig
{
    using Domain;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class ArticleConfig : IEntityTypeConfiguration<Article>
    {
        public void Configure(EntityTypeBuilder<Article> builder)
        {
            builder
                .HasOne(a => a.Image)
                .WithMany()
                .HasForeignKey(a => a.ImageId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(a => a.Creator)
                .WithMany(e => e.CreatedArticles)
                .HasForeignKey(a => a.CreatorId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
