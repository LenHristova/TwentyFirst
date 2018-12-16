namespace TwentyFirst.Common.Models.Images
{
    using System;
    using Constants;
    using Data.Models;
    using Mapping.Contracts;

    public class ImageSearchListViewModel : IMapFrom<Image>
    {
        public string Id { get; set; }

        public string ThumbUrl { get; set; }

        public string Description { get; set; }

        public string ShortDescription
        {
            get
            {
                var description = this.Description ?? string.Empty;
                var symbolsToGet = Math.Min(description.Length, GlobalConstants.ImageShortDescriptionMaxLength);
                return this.Description?.Substring(0, symbolsToGet) + "...";
            }
        }

        public string Author { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
