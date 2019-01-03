namespace TwentyFirst.Web.Areas.Administration.Controllers
{
    using System;
    using Common.Constants;
    using Common.Exceptions;
    using Common.Models.Categories;
    using Common.Models.Enums;
    using Filters;
    using Infrastructure.Extensions;
    using Logging;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Services.DataServices.Contracts;
    using System.Linq;
    using System.Threading.Tasks;

    [Authorize(Roles = GlobalConstants.MasterAdministratorRoleName)]
    public class CategoriesController : AdministrationController
    {
        private readonly ICategoryService categoryService;
        private readonly ILogger logger;

        public CategoriesController(
            ICategoryService categoryService,
            ILogger<CategoriesController> logger)
        {
            this.categoryService = categoryService;
            this.logger = logger;
        }

        public async Task<IActionResult> Index(int? pageNumber)
        {
            var categories = await this.categoryService.AllWithArchivedAsync<CategoryListViewModel>();

            var onePageOfCategories = await categories.ToList()
                .PaginateAsync(pageNumber, GlobalConstants.AdministrationCategoriesOnPageCount);

            return this.View(onePageOfCategories);
        }

        [TypeFilter(typeof(ErrorAlertExceptionFilterAttribute), Order = 1)]
        public IActionResult Create() => this.PartialView("_CategoryCreateFormPartial");

        [HttpPost]
        public async Task<IActionResult> Create(CategoryCreateInputModel categoryCreateInputModel)
        {
            throw new Exception("Test");
            if (!this.ModelState.IsValid)
            {
                this.SetAlertMessage(AlertMessageLevel.Error, this.GetModelStateErrorMessages());
                return RedirectToAction(nameof(Index));
            }

            var createdCategory = await this.categoryService
                .CreateAsync(categoryCreateInputModel);

            var message = $"Категорията \"{createdCategory.Name}\" беше добавена успешно.";
            this.logger.LogInformation((int)LoggingEvents.InsertItem, message);
            this.SetAlertMessage(AlertMessageLevel.Success, message);

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
        public async Task<IActionResult> Edit(CategoryUpdateInputModel categoryUpdateInputModel)
        {
            if (!this.ModelState.IsValid)
            {
                this.SetAlertMessage(AlertMessageLevel.Error, this.GetModelStateErrorMessages());
                return RedirectToAction(nameof(Index));
            }

            var categoryToEdit = await this.categoryService.EditAsync(categoryUpdateInputModel);

            var message = $"Категорията \"{categoryToEdit.Name}\" беше променена успешно.";
            this.logger.LogInformation((int)LoggingEvents.UpdateItem, message);
            this.SetAlertMessage(AlertMessageLevel.Success, message);

            return RedirectToAction(nameof(Index));
        }

        [TypeFilter(typeof(ErrorAlertExceptionFilterAttribute), Order = 1)]
        public async Task<IActionResult> Archive(string id)
        {
            var categoryToArchive = await this.categoryService
                .GetAsync<CategoryUpdateInputModel>(id);

            return this.PartialView("_CategoryArchiveFormPartial", categoryToArchive);
        }

        [HttpPost]
        public async Task<IActionResult> Archive(CategoryUpdateInputModel categoryUpdateInputModel)
        {
            var archivedCategory = await this.categoryService
                .ArchiveAsync(categoryUpdateInputModel.Id);

            var message = $"Категорията \"{archivedCategory.Name}\" беше архивирана успешно.";
            this.logger.LogInformation((int)LoggingEvents.DeleteItem, message);
            this.SetAlertMessage(AlertMessageLevel.Success, message);

            return RedirectToAction(nameof(Index));
        }

        [TypeFilter(typeof(ErrorAlertExceptionFilterAttribute), Order = 1)]
        public async Task<IActionResult> Recover(string id)
        {
            var categoryToRecover = await this.categoryService
                .GetArchivedAsync<CategoryUpdateInputModel>(id);

            return this.PartialView("_CategoryRecoverFormPartial", categoryToRecover);
        }

        [HttpPost]
        public async Task<IActionResult> Recover(CategoryUpdateInputModel categoryUpdateInputModel)
        {
            var archivedCategory = await this.categoryService
                .RecoverAsync(categoryUpdateInputModel.Id);

            var message = $"Категорията \"{archivedCategory.Name}\" беше възстановена успешно.";
            this.logger.LogInformation((int)LoggingEvents.RecoverItem, message);
            this.SetAlertMessage(AlertMessageLevel.Success, message);

            return RedirectToAction(nameof(Index));
        }

        [TypeFilter(typeof(ErrorAlertExceptionFilterAttribute), Order = 1)]
        public async Task<IActionResult> Order(string id, bool up, bool down)
        {
            if ((up && down) || (!up && !down))
            {
                throw new InvalidCategoryOrderException();
            }

            var category = up
                ? await this.categoryService.ReorderUpAsync(id)
                : await this.categoryService.ReorderDownAsync(id);

            var direction = up ? "предна" : "задна";
            var message = $"Категорията \"{category.Name}\" беше поставена на по-{direction} позиция.";
            this.logger.LogInformation((int)LoggingEvents.UpdateItem, message);
            this.SetAlertMessage(AlertMessageLevel.Success, message);

            return RedirectToAction(nameof(Index));
        }
    }
}
