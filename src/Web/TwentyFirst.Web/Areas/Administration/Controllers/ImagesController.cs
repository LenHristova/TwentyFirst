namespace TwentyFirst.Web.Areas.Administration.Controllers
{
    using System.Linq;
    using Common.Models.Images;
    using Data.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Services.DataServices.Contracts;
    using System.Threading.Tasks;
    using Common.Constants;
    using Filters;

    public class ImagesController : AdministrationController
    {
        private readonly IImageService imageService;
        private readonly UserManager<User> userManager;

        public ImagesController(IImageService imageService, UserManager<User> userManager)
        {
            this.imageService = imageService;
            this.userManager = userManager;
        }

        //public IActionResult Index()
        //{
        //    return this.View();
        //}

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

        //public IActionResult Delete()
        //{

        //}

        [TypeFilter(typeof(ErrorAlertExceptionFilterAttribute))]
        public async Task<IActionResult> Search(string search, int? pageNumber)
        {
            search = search ?? string.Empty;
            var images = this.imageService.GetBySearchTerm<ImageSearchListViewModel>(search);

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
