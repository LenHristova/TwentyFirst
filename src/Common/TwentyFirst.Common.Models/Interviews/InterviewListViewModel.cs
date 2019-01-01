namespace TwentyFirst.Common.Models.Interviews
{
    using Data.Models;
    using Images;
    using Mapping.Contracts;
    using System;
    using Extensions;

    public class InterviewListViewModel : IMapFrom<Interview>
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Author { get; set; }

        public string Interviewed { get; set; }

        public DateTime PublishedOn { get; set; }

        public ImageThumbBaseViewModel Image { get; set; }

        public string PublishedOnString
            => this.PublishedOn.UtcToEst().ToFormattedString();
    }
}
