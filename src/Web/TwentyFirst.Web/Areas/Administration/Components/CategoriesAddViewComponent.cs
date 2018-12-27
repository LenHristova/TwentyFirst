namespace TwentyFirst.Web.Areas.Administration.Components
{
    using Common.Models.Categories;
    using Microsoft.AspNetCore.Mvc;
    using Services.DataServices.Contracts;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    [ViewComponent(Name = "categories-add")]
    public class CategoriesAddViewComponent : ViewComponent
    {
        private readonly ICategoryService categoryService;

        public CategoriesAddViewComponent(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        public async Task<IViewComponentResult> InvokeAsync(IEnumerable<string> ids)
            => View(new CategoriesChooseInputModel
            {
                CategoriesIds = ids,
                CategoryBaseViewModels = await this.categoryService.AllOrderedByNameAsync<CategoryBaseViewModel>()
            });
    }
}
