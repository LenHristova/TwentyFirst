namespace TwentyFirst.Data.Models
{
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Identity;

    public class User : IdentityUser
    {
        public bool IsDeleted { get; set; }

        public virtual ICollection<Article> CreatedArticles { get; set; } = new HashSet<Article>();

        public virtual ICollection<ArticleEdit> EditedArticles { get; set; } = new HashSet<ArticleEdit>();

        public virtual ICollection<Interview> CreatedInterviews { get; set; } = new HashSet<Interview>();

        public virtual ICollection<InterviewEdit> EditedInterviews { get; set; } = new HashSet<InterviewEdit>();

        public virtual ICollection<Image> CreatedImages { get; set; } = new HashSet<Image>();

        public virtual ICollection<Poll> CreatedPolls { get; set; } = new HashSet<Poll>();
    }
}
