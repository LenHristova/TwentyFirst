namespace TwentyFirst.Data.EntitiesConfig
{
    using Domain;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class InterviewTagConfig : IEntityTypeConfiguration<InterviewTag>
    {
        public void Configure(EntityTypeBuilder<InterviewTag> builder)
        {
            builder
                .HasKey(it => new { it.InterviewId, it.TagId });

            builder
                .HasOne(it => it.Interview)
                .WithMany(i => i.Tags)
                .HasForeignKey(it => it.InterviewId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(it => it.Tag)
                .WithMany(t => t.Interviews)
                .HasForeignKey(it => it.InterviewId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
