namespace TwentyFirst.Data.EntitiesConfig
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    public class InterviewEditConfig : IEntityTypeConfiguration<InterviewEdit>
    {
        public void Configure(EntityTypeBuilder<InterviewEdit> builder)
        {
            builder
                .HasOne(ie => ie.Interview)
                .WithMany(a => a.Edits)
                .HasForeignKey(ie => ie.InterviewId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(ie => ie.Editor)
                .WithMany(e => e.EditedInterviews)
                .HasForeignKey(ie => ie.EditorId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
