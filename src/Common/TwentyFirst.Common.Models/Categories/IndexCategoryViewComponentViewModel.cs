namespace TwentyFirst.Common.Models.Categories
{
    using System.Collections.Generic;
    using System.Linq;
    using Constants;

    public class IndexCategoryViewComponentViewModel
    {
        private const int MainCategoriesCount = GlobalConstants.MainCategoriesCount;

        public IEnumerable<CategoryViewModel> Categories { get; set; } = new List<CategoryViewModel>();

        public IList<CategoryViewModel> MainCategories 
            => this.Categories.Take(MainCategoriesCount).ToList();

        public IList<CategoryViewModel> AdditionalCategories 
            => this.Categories.Skip(MainCategoriesCount).ToList();
    }
}
