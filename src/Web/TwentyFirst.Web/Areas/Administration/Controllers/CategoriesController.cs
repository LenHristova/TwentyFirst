namespace TwentyFirst.Web.Areas.Administration.Controllers
{
    using Common.Constants;
    using Common.Models.Categories;
    using Common.Models.Enums;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Services.DataServices.Contracts;
    using System.Linq;
    using System.Threading.Tasks;
    using Filters;

    [Authorize(Roles = GlobalConstants.MasterAdministratorRoleName)]
    public class CategoriesController : AdministrationController
    {
        private readonly ICategoryService categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        [TypeFilter(typeof(ErrorPageExceptionFilterAttribute))]
        public async Task<IActionResult> Index(int? pageNumber)
        {
            var categories = await this.categoryService
                .AllWithArchived<CategoryListViewModel>();

            var onePageOfCategories = await categories.ToList()
                .PaginateAsync(pageNumber, GlobalConstants.AdministrationCategoriesOnPageCount);

            return this.View(onePageOfCategories);
        }

        [TypeFilter(typeof(ErrorAlertExceptionFilterAttribute))]
        public IActionResult Create() => this.PartialView("_CategoryCreateFormPartial");

        [HttpPost]
        [TypeFilter(typeof(ErrorPageExceptionFilterAttribute))]
        public async Task<IActionResult> Create(CategoryCreateInputModel categoryCreateInputModel)
        {
            if (!this.ModelState.IsValid)
            {
                var errors = string.Join(GlobalConstants.HtmlNewLine, this.ModelState.Values);
                this.SetAlertMessage(AlertMessageLevel.Error, errors);

                return RedirectToAction(nameof(Index));
            }

           var createdCategory = await this.categoryService
               .CreateAsync(categoryCreateInputModel);

            var alertMessage = $"Категорията \"{createdCategory.Name}\" беше добавена успешно.";
            this.SetAlertMessage(AlertMessageLevel.Success, alertMessage);

            return RedirectToAction(nameof(Index));
        }

        [TypeFilter(typeof(ErrorAlertExceptionFilterAttribute))]
        public async Task<IActionResult> Edit(string id)
        {
            var categoryToEdit = await this.categoryService
                .GetAsync<CategoryUpdateInputModel>(id);

            return this.PartialView("_CategoryEditFormPartial", categoryToEdit);
        }

        [HttpPost]
        [TypeFilter(typeof(ErrorPageExceptionFilterAttribute))]
        public async Task<IActionResult> Edit(CategoryUpdateInputModel categoryUpdateInputModel)
        {
            if (!this.ModelState.IsValid)
            {
                var errors = string.Join(GlobalConstants.HtmlNewLine, this.ModelState.Values);
                this.SetAlertMessage(AlertMessageLevel.Error, errors);

                return RedirectToAction(nameof(Index));
            }

            var categoryToEdit = await this.categoryService
                .EditAsync(categoryUpdateInputModel);

            var alertMessage = $"Категорията \"{categoryToEdit.Name}\" беше успешно променена успешно.";
            this.SetAlertMessage(AlertMessageLevel.Success, alertMessage);

            return RedirectToAction(nameof(Index));
        }

        [TypeFilter(typeof(ErrorAlertExceptionFilterAttribute))]
        public async Task<IActionResult> Archive(string id)
        {
            var categoryToArchive = await this.categoryService
                .GetAsync<CategoryUpdateInputModel>(id);

            return this.PartialView("_CategoryArchiveFormPartial", categoryToArchive);
        }

        [HttpPost]
        [TypeFilter(typeof(ErrorPageExceptionFilterAttribute))]
        public async Task<IActionResult> Archive(CategoryUpdateInputModel categoryUpdateInputModel)
        {
            var archivedCategory = await this.categoryService
                .ArchiveAsync(categoryUpdateInputModel.Id);

            var alertMessage = $"Категорията \"{archivedCategory.Name}\" беше архивирана успешно.";
            this.SetAlertMessage(AlertMessageLevel.Success, alertMessage);

            return RedirectToAction(nameof(Index));
        }

        [TypeFilter(typeof(ErrorAlertExceptionFilterAttribute))]
        public async Task<IActionResult> Recover(string id)
        {
            var categoryToRecover = await this.categoryService
                .GetArchivedAsync<CategoryUpdateInputModel>(id);

            return this.PartialView("_CategoryRecoverFormPartial", categoryToRecover);
        }

        [HttpPost]
        [TypeFilter(typeof(ErrorPageExceptionFilterAttribute))]
        public async Task<IActionResult> Recover(CategoryUpdateInputModel categoryUpdateInputModel)
        {
            var archivedCategory = await this.categoryService
                .RecoverAsync(categoryUpdateInputModel.Id);

            var alertMessage = $"Категорията \"{archivedCategory.Name}\" беше възстановена успешно.";
            this.SetAlertMessage(AlertMessageLevel.Success, alertMessage);

            return RedirectToAction(nameof(Index));
        }
    }
}
