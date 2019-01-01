namespace TwentyFirst.Data.EntitiesConfig
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    public class PollConfig : IEntityTypeConfiguration<Poll>
    {
        public void Configure(EntityTypeBuilder<Poll> builder)
        {
            builder
                .HasOne(p => p.Creator)
                .WithMany(c => c.CreatedPolls)
                .HasForeignKey(p => p.CreatorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasMany(p => p.Options)
                .WithOne(a => a.Poll)
                .HasForeignKey(a => a.PollId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasMany(p => p.Votes)
                .WithOne(a => a.Poll)
                .HasForeignKey(a => a.PollId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
