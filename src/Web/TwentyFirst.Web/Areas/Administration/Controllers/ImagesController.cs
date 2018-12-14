namespace TwentyFirst.Web.Areas.Administration.Controllers
{
    using Common.Models.Images;
    using Data.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Services.DataServices.Contracts;
    using System.Threading.Tasks;

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

        public IActionResult Upload() => this.View();

        [HttpPost]
        [Authorize]
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

        //public IActionResult Edit()
        //{

        //}

        //public IActionResult Edit()
        //{

        //}

        //public IActionResult Delete()
        //{

        //}

        public IActionResult Search(string search)
        {
            var images = this.imageService.GetBySearchTerm<ImageSearchListViewModel>(search);

            return PartialView("_ImageSearchListPartial", images);
        }

        //public IActionResult Details()
        //{

        //}
    }
}
