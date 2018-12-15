namespace TwentyFirst.Common.Models.Images
{
    using Data.Models;
    using Mapping.Contracts;

    public class ImageThumbBaseViewModel : IMapFrom<Image>
    {
        public string Title { get; set; }

        public string ThumbUrl { get; set; }
    }
}
