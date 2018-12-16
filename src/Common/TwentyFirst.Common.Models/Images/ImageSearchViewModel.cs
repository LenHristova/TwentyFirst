namespace TwentyFirst.Common.Models.Images
{
    using System.Collections.Generic;

    public class ImageSearchViewModel
    {
        public string SearchTerm { get; set; }

        public IEnumerable<ImageSearchListViewModel> SearchResultImages { get; set; }
    }
}
