namespace TwentyFirst.Web.Areas.Administration.Controllers
{
    using Common.Constants;
    using Common.Models.Enums;
    using Common.Models.Images;
    using Data.Models;
    using Filters;
    using Infrastructure.Extensions;
    using Logging;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Services.CloudFileUploader;
    using Services.DataServices.Contracts;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class ImagesController : AdministrationController
    {
        private readonly IImageService imageService;
        private readonly ICloudFileUploader cloudFileUploader;
        private readonly UserManager<User> userManager;
        private readonly ILogger<ImagesController> logger;

        public ImagesController(
            IImageService imageService,
            ICloudFileUploader cloudFileUploader,
            UserManager<User> userManager,
            ILogger<ImagesController> logger)
        {
            this.imageService = imageService;
            this.userManager = userManager;
            this.logger = logger;
            this.cloudFileUploader = cloudFileUploader;
        }

        public IActionResult Index() => this.View();

        [TypeFilter(typeof(ErrorPageExceptionFilterAttribute))]
        public IActionResult Upload() => this.View();

        [HttpPost]
        [TypeFilter(typeof(ErrorPageExceptionFilterAttribute))]
        public async Task<IActionResult> Upload(ImagesCreateInputModel imagesCreateInputModel)
        {
            if (!ModelState.IsValid)
            {
                return this.View(imagesCreateInputModel);
            }

            var images = imagesCreateInputModel.Images.ToList();

            var errors = new List<string>();
            foreach (var image in images)
            {
                var isEmpty = image.Length == 0;
                if (isEmpty)
                {
                    errors.Add(string.Format(GlobalConstants.CorruptedImage, image.FileName));
                    continue;
                }

                var fileUrls = this.cloudFileUploader.UploadImageAsync(image);
                if (fileUrls == null)
                {
                    errors.Add(string.Format(GlobalConstants.CorruptedImage, image.FileName));
                    continue;
                }

                var userId = this.userManager.GetUserId(this.User);
                await this.imageService.CreateAsync(imagesCreateInputModel, userId, fileUrls.Url, fileUrls.ThumbUrl);
            }

            if (errors.Any())
            {
                var formattedErrors = string.Join(GlobalConstants.HtmlNewLine, errors);
                this.SetAlertMessage(AlertMessageLevel.Error, formattedErrors);
            }
            else
            {
                this.SetAlertMessage(AlertMessageLevel.Success, GlobalConstants.SuccessfulImageUpload);
            }

            return this.RedirectToAction(nameof(Upload));
        }

        [HttpPost]
        [Authorize(Roles = GlobalConstants.MasterAdministratorOrAdministrator)]
        [TypeFilter(typeof(ErrorPageExceptionFilterAttribute))]
        public async Task<IActionResult> Delete(string id)
        {
            await this.imageService.DeleteAsync(id);

            this.SetAlertMessage(AlertMessageLevel.Success, GlobalConstants.SuccessfulImageDelete);

            var logMessage = $"{GlobalConstants.SuccessfulImageDelete} ИД: {id}";
            this.logger.LogInformation((int)LoggingEvents.DeleteItem, logMessage);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Authorize(Roles = GlobalConstants.MasterAdministratorOrAdministrator)]
        [TypeFilter(typeof(ErrorPageExceptionFilterAttribute))]
        public async Task<IActionResult> Recover(string id)
        {
            await this.imageService.RecoverAsync(id);

            this.SetAlertMessage(AlertMessageLevel.Success, GlobalConstants.SuccessfulImageRecover);

            var logMessage = $"{GlobalConstants.SuccessfulImageRecover} ИД: {id}";
            this.logger.LogInformation((int)LoggingEvents.RecoverItem, logMessage);
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
