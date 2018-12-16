namespace TwentyFirst.Data.EntitiesConfig
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    public class ArticleEditConfig : IEntityTypeConfiguration<ArticleEdit>
    {
        public void Configure(EntityTypeBuilder<ArticleEdit> builder)
        {
            builder
                .HasOne(ae => ae.Article)
                .WithMany(a => a.Edits)
                .HasForeignKey(ae => ae.ArticleId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(ae => ae.Editor)
                .WithMany(e => e.EditedArticles)
                .HasForeignKey(ae => ae.EditorId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
