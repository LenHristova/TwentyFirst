namespace TwentyFirst.Web.Areas.Administration.Controllers
{
    using Common.Constants;
    using Common.Models.Images;
    using Data.Models;
    using Filters;
    using Infrastructure.Extensions;
    using Logging;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Services.DataServices.Contracts;
    using System.Linq;
    using System.Threading.Tasks;

    public class ImagesController : AdministrationController
    {
        private readonly IImageService imageService;
        private readonly UserManager<User> userManager;
        private readonly ILogger<ImagesController> logger;

        public ImagesController(
            IImageService imageService,
            UserManager<User> userManager,
            ILogger<ImagesController> logger)
        {
            this.imageService = imageService;
            this.userManager = userManager;
            this.logger = logger;
        }

        public IActionResult Index() => this.View();

        [TypeFilter(typeof(ErrorPageExceptionFilterAttribute))]
        public IActionResult Upload() => this.View();

        [HttpPost]
        [TypeFilter(typeof(ErrorPageExceptionFilterAttribute))]
        public async Task<IActionResult> Upload(ImagesCreateInputModel imageCreateInputModel)
        {
            if (!ModelState.IsValid)
            {
                return this.View(imageCreateInputModel);
            }

            var userId = this.userManager.GetUserId(this.User);
            var corruptedImages = await this.imageService.UploadAsync(imageCreateInputModel, userId);
            if (corruptedImages > 0)
            {
                //TODO tempData[message]
            }

            //TODO tempData[success message]

            return this.RedirectToAction(nameof(Upload));
        }

        [HttpPost]
        [Authorize(Roles = GlobalConstants.MasterAdministratorRoleName)]
        public async Task<IActionResult> Delete(string id)
        {
            await this.imageService.DeleteAsync(id);
            //TODO tempData[success message]
            var message = $"Успешно беше изтрита снимка с ИД: {id}";
            this.logger.LogInformation((int)LoggingEvents.DeleteItem, message);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Authorize(Roles = GlobalConstants.MasterAdministratorRoleName)]
        public async Task<IActionResult> Recover(string id)
        {
            await this.imageService.RecoverAsync(id);
            //TODO tempData[success message]
            var message = $"Успешно беше възстановена снимка с ИД: {id}";
            this.logger.LogInformation((int)LoggingEvents.RecoverItem, message);
            return RedirectToAction(nameof(Index));
        }

        [TypeFilter(typeof(ErrorAlertExceptionFilterAttribute))]
        public async Task<IActionResult> Search(string search, int? pageNumber)
        {
            search = search ?? string.Empty;

            var images = this.User.IsInRole(GlobalConstants.MasterAdministratorRoleName)
                ? await this.imageService.GetBySearchTermWithDeletedAsync<ImageSearchListViewModel>(search)
                : await this.imageService.GetBySearchTermAsync<ImageSearchListViewModel>(search);

            var onePageOfImages = await images.ToList()
                .PaginateAsync(pageNumber, GlobalConstants.ImagesOnSearchPageCount);

            var imageSearchViewModel = new ImageSearchViewModel
            {
                SearchTerm = search,
                SearchResultImages = onePageOfImages
            };

            return PartialView("_ImageSearchListPartial", imageSearchViewModel);
        }
    }
}
