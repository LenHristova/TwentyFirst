namespace TwentyFirst.Data.EntitiesConfig
{
    using Domain;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class InterviewConfig : IEntityTypeConfiguration<Interview>
    {
        public void Configure(EntityTypeBuilder<Interview> builder)
        {
            builder
                .HasOne(i => i.Image)
                .WithMany()
                .HasForeignKey(i => i.ImageId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(i => i.Creator)
                .WithMany(c => c.CreatedInterviews)
                .HasForeignKey(a => a.CreatorId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
