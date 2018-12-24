namespace TwentyFirst.Web.Components
{
    using Common.Models.Categories;
    using Microsoft.AspNetCore.Mvc;
    using Services.DataServices.Contracts;
    using System.Threading.Tasks;

    [ViewComponent(Name = "categories")]
    public class CategoriesViewComponent : ViewComponent
    {
        private readonly ICategoryService categoryService;

        public CategoriesViewComponent(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string activeCategoryId)
        {
            var allCategories = await this.categoryService.All<CategoryViewModel>();
            var categoriesToShow = new IndexCategoryViewComponentViewModel { Categories = allCategories };
            return this.View(categoriesToShow);
        }
    }
}
