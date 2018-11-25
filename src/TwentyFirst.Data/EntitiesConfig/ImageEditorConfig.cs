namespace TwentyFirst.Data.EntitiesConfig
{
    using Domain;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class ImageEditorConfig : IEntityTypeConfiguration<ImageEditor>
    {
        public void Configure(EntityTypeBuilder<ImageEditor> builder)
        {
            builder
                .HasKey(ie => new { ie.ImageId, ie.EditorId });

            builder
                .HasOne(ie => ie.Image)
                .WithMany(a => a.Editors)
                .HasForeignKey(ie => ie.ImageId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(ie => ie.Editor)
                .WithMany(e => e.EditedImages)
                .HasForeignKey(ie => ie.EditorId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
