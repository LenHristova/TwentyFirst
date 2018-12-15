namespace TwentyFirst.Web.Areas.Administration.Controllers
{
    using Common.Constants;
    using Common.Models.Categories;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Services.DataServices.Contracts;
    using System.Threading.Tasks;
    using Common.Models.Enums;

    [Authorize(Roles = GlobalConstants.MasterAdministratorRoleName)]
    public class CategoriesController : AdministrationController
    {
        private readonly ICategoryService categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
            => this.View(await this.categoryService.AllWithArchived<CategoryListViewModel>());

        public IActionResult Create() => this.PartialView("_CategoryCreateFormPartial");

        [HttpPost]
        public async Task<IActionResult> Create(CategoryCreateInputModel categoryCreateInputModel)
        {
            this.SetAlertMessage(AlertMessageLevel.Error, "Error");
            if (this.ModelState.IsValid)
            {
                var success = await this.categoryService.CreateAsync(categoryCreateInputModel);

                if (success)
                {
                    this.SetAlertMessage(AlertMessageLevel.Success, "Success");
                }
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(string id)
        {
            var categoryToEdit = await this.categoryService.GetAsync<CategoryUpdateInputModel>(id);
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

        public async Task<IActionResult> Archive(string id)
        {
            var categoryToDelete = await this.categoryService.GetAsync<CategoryUpdateInputModel>(id);
            if (categoryToDelete == null)
            {
                //TODO TempData
            }

            return this.PartialView("_CategoryArchiveFormPartial", categoryToDelete);
        }

        [HttpPost]
        public async Task<IActionResult> Archive(CategoryUpdateInputModel categoryUpdateInputModel)
        {
            var idExists = this.categoryService.Exists(categoryUpdateInputModel.Id);

            if (!idExists)
            {
                //TODO TempData
            }

            var success = await this.categoryService.ArchiveAsync(categoryUpdateInputModel.Id);

            if (!success)
            {
                //TODO TempData
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Recover(string id)
        {
            var categoryToRecover = await this.categoryService.GetArchivedAsync<CategoryUpdateInputModel>(id);
            if (categoryToRecover == null)
            {
                //TODO TempData
            }

            return this.PartialView("_CategoryRecoverFormPartial", categoryToRecover);
        }

        [HttpPost]
        public async Task<IActionResult> Recover(CategoryUpdateInputModel categoryUpdateInputModel)
        {
            var idExists = this.categoryService.Exists(categoryUpdateInputModel.Id);

            if (!idExists)
            {
                //TODO TempData
            }

            var success = await this.categoryService.RecoverAsync(categoryUpdateInputModel.Id);

            if (!success)
            {
                //TODO TempData
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
