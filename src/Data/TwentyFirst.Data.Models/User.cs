namespace TwentyFirst.Data.Models
{
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Identity;

    public class User : IdentityUser
    {
        public bool IsDeleted { get; set; }

        public virtual ICollection<Article> CreatedArticles { get; set; } = new HashSet<Article>();

        public virtual ICollection<ArticleEditor> EditedArticles { get; set; } = new HashSet<ArticleEditor>();

        public virtual ICollection<Interview> CreatedInterviews { get; set; } = new HashSet<Interview>();

        public virtual ICollection<InterviewEditor> EditedInterviews { get; set; } = new HashSet<InterviewEditor>();

        public virtual ICollection<Image> CreatedImages { get; set; } = new HashSet<Image>();

        public virtual ICollection<ImageEditor> EditedImages { get; set; } = new HashSet<ImageEditor>();

        public virtual ICollection<Poll> CreatedPolls { get; set; } = new HashSet<Poll>();
    }
}
