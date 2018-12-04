namespace TwentyFirst.Data.EntitiesConfig
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    public class ArticleToArticleConfig : IEntityTypeConfiguration<ArticleToArticle>
    {
        public void Configure(EntityTypeBuilder<ArticleToArticle> builder)
        {
            builder
                .HasKey(a => new { a.ConnectedToId, a.ConnectedFromId });

            builder
                .HasOne(a => a.ConnectedTo)
                .WithMany(a => a.ConnectedFrom)
                .HasForeignKey(a => a.ConnectedToId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(a => a.ConnectedFrom)
                .WithMany(a => a.ConnectedTo)
                .HasForeignKey(a => a.ConnectedFromId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
