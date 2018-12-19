namespace TwentyFirst.Common.Models.Interviews
{
    using System;
    using Constants;
    using Data.Models;
    using Mapping.Contracts;

    public class InterviewDetailsViewModel : IMapFrom<Interview>
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public string HtmlContent
            => this.Content.Insert(0, GlobalConstants.HtmlTab)
                .Replace("\n", $"{GlobalConstants.HtmlNewLine}{GlobalConstants.HtmlTab}");

        public string Author { get; set; }

        public string Interviewed { get; set; }

        public DateTime PublishedOn { get; set; }

        public string ImageUrl { get; set; }
    }
}
