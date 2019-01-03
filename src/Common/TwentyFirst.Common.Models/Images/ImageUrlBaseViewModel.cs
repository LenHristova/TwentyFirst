namespace TwentyFirst.Common.Models.Images
{
    using Data.Models;
    using Mapping.Contracts;

    public class ImageUrlBaseViewModel : IMapFrom<Image>
    {
        public string Id { get; set; }

        public string Url { get; set; }
    }
}
