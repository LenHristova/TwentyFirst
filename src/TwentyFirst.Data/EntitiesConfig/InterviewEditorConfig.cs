namespace TwentyFirst.Data.EntitiesConfig
{
    using Domain;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class InterviewEditorConfig : IEntityTypeConfiguration<InterviewEditor>
    {
        public void Configure(EntityTypeBuilder<InterviewEditor> builder)
        {
            builder
                .HasKey(ie => new { ie.InterviewId, ie.EditorId });

            builder
                .HasOne(ie => ie.Interview)
                .WithMany(a => a.Editors)
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
