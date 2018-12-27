namespace TwentyFirst.Web.Areas.Administration.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;
    using Common.Constants;
    using Common.Models.Articles;
    using Common.Models.Categories;
    using Common.Models.Interviews;
    using Data.Models;
    using Filters;
    using Infrastructure.Extensions;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Services.DataServices.Contracts;

    [TypeFilter(typeof(ErrorPageExceptionFilterAttribute))]
    public class InterviewsController : AdministrationController
    {
        private readonly IInterviewService interviewService;
        private readonly UserManager<User> userManager;

        public InterviewsController(IInterviewService interviewService, UserManager<User> userManager)
        {
            this.interviewService = interviewService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index(int? pageNumber)
        {
            var interviews = await this.interviewService
                .LatestAsync<InterviewAdminListViewModel>(GlobalConstants.MaxInterviewsCountToGet);

            var onePageOfInterviews = await interviews.ToList()
                .PaginateAsync(pageNumber, GlobalConstants.AdministrationInterviewsOnPageCount);

            return this.View(onePageOfInterviews);
        }

        public IActionResult Create() => this.View();

        [HttpPost]
        public async Task<IActionResult> Create(InterviewCreateInputModel interviewCreateInputModel)
        {
            if (!ModelState.IsValid)
            {
                return this.View(interviewCreateInputModel);
            }

            var userId = this.userManager.GetUserId(this.User);
            var createdInterview = await this.interviewService.CreateAsync(interviewCreateInputModel, userId);

            return RedirectToAction("Details", "Interviews", new { createdInterview.Id });
        }

        public async Task<IActionResult> Edit(string id)
        {
            var interviewToEdit = await this.interviewService.GetAsync<InterviewEditInputModel>(id);
            return this.View(interviewToEdit);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(InterviewEditInputModel interviewUpdateInputModel)
        {
            if (!ModelState.IsValid)
            {
                return this.View(interviewUpdateInputModel);
            }

            var userId = this.userManager.GetUserId(this.User);
            var editedInterview = await this.interviewService.EditAsync(interviewUpdateInputModel, userId);
            return RedirectToAction("Details", "Interviews", new { editedInterview.Id });
        }

        public async Task<IActionResult> Delete(string id)
        {
            var interviewToDelete = await this.interviewService.GetAsync<InterviewDeleteViewModel>(id);
            return this.View(interviewToDelete);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id, string name)
        {
            var userId = this.userManager.GetUserId(this.User);
            await this.interviewService.DeleteAsync(id, userId);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> AllEdits(string id, int? pageNumber)
        {
            var interviewEdits = await this.interviewService.GetAsync<InterviewAllEditsViewModel>(id);

            if (interviewEdits.Edits != null && interviewEdits.Edits.Any())
            {
                interviewEdits.Edits = await interviewEdits.Edits
                    .OrderByDescending(ae => ae.EditDateTime)
                    .ToList()
                    .PaginateAsync(pageNumber, GlobalConstants.AdministrationArticleEditsOnPageCount);
            }

            return this.View(interviewEdits);
        }
    }
}
