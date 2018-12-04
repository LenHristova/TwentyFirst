namespace TwentyFirst.Data.EntitiesConfig
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    public class ImageConfig : IEntityTypeConfiguration<Image>
    {
        public void Configure(EntityTypeBuilder<Image> builder)
        {
            builder
                .HasOne(i => i.Creator)
                .WithMany(a => a.CreatedImages)
                .HasForeignKey(ae => ae.CreatorId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
