namespace TwentyFirst.Web.Areas.Administration.Controllers
{
    using Common.Constants;
    using Common.Models.Categories;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Services.DataServices.Contracts;
    using System.Threading.Tasks;

    [Authorize(Roles = GlobalConstants.MasterAdministratorRoleName)]
    public class CategoriesController : AdministrationController
    {
        private readonly ICategoryService categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
            => this.View(await this.categoryService.All<CategoryListViewModel>());

        public IActionResult Create() => this.PartialView("_CategoryCreateFormPartial");

        [HttpPost]
        public async Task<IActionResult> Create(CategoryCreateInputModel categoryCreateInputModel)
        {
            //TODO Configure TempData in hole project
            //this.TempData[nameof(AlertMessage)] = new AlertMessage(AlertMessageLevel.Error, "Error");

            if (this.ModelState.IsValid)
            {
                var success = await this.categoryService.CreateAsync(categoryCreateInputModel);

                if (success)
                {
                    //this.TempData[nameof(AlertMessage)] = new AlertMessage(AlertMessageLevel.Success, "Success");
                }
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(string id)
        {
            var categoryToEdit = this.categoryService.Get<CategoryUpdateInputModel>(id);
            if (categoryToEdit == null)
            {
                //TODO TempData
            }

            return this.PartialView("_CategoryEditFormPartial", categoryToEdit);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CategoryUpdateInputModel categoryUpdateInputModel)
        {
            var idExists = this.categoryService.Exists(categoryUpdateInputModel.Id);

            if (!idExists)
            {
                //TODO TempData
                return RedirectToAction(nameof(Index));
            }

            if (!this.ModelState.IsValid)
            {
                //TODO TempData
                return this.View(nameof(Index));
            }

            var success = await this.categoryService.EditAsync(categoryUpdateInputModel);

            if (!success)
            {
                //TODO TempData
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(string id)
        {
            var categoryToDelete = this.categoryService.Get<CategoryUpdateInputModel>(id);
            if (categoryToDelete == null)
            {
                //TODO TempData
            }

            return this.PartialView("_CategoryDeleteFormPartial", categoryToDelete);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(CategoryUpdateInputModel categoryEditInputModel)
        {
            var idExists = this.categoryService.Exists(categoryEditInputModel.Id);

            if (!idExists)
            {
                //TODO TempData
            }

            var success = await this.categoryService.DeleteAsync(categoryEditInputModel.Id);

            if (!success)
            {
                //TODO TempData
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
