namespace TwentyFirst.Data.EntitiesConfig
{
    using Domain;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class ArticleEditorConfig : IEntityTypeConfiguration<ArticleEditor>
    {
        public void Configure(EntityTypeBuilder<ArticleEditor> builder)
        {
            builder
                .HasKey(ae => new { ae.ArticleId, ae.EditorId });

            builder
                .HasOne(ae => ae.Article)
                .WithMany(a => a.Editors)
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
