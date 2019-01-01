namespace TwentyFirst.Web.Areas.Administration.Controllers
{
    using Common.Constants;
    using Common.Models.Enums;
    using Common.Models.Polls;
    using Data.Models;
    using Filters;
    using Infrastructure.Extensions;
    using Logging;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Services.DataServices.Contracts;
    using System.Linq;
    using System.Threading.Tasks;

    [TypeFilter(typeof(ErrorPageExceptionFilterAttribute))]
    public class PollsController : AdministrationController
    {
        private readonly IPollService pollService;
        private readonly UserManager<User> userManager;
        private readonly ILogger logger;

        public PollsController(
            IPollService pollService,
            UserManager<User> userManager,
            ILogger<PollsController> logger)
        {
            this.pollService = pollService;
            this.userManager = userManager;
            this.logger = logger;
        }

        public async Task<IActionResult> Index(int? pageNumber)
        {
            var polls = await this.pollService.AllAsync<PollListViewModel>();

            var onePageOfPolls = await polls.ToList()
                .PaginateAsync(pageNumber, GlobalConstants.PollsOnPageCount);

            return this.View(onePageOfPolls);
        }

        public async Task<IActionResult> Details(string id)
        {
            var poll = await this.pollService.GetAsync<PollDetailsViewModel>(id);

            return this.View(poll);
        }

        public IActionResult Create() => this.View();

        [HttpPost]
        public async Task<IActionResult> Create(PollCreateInputModel pollCreateInputModel)
        {
            if (!ModelState.IsValid)
            {
                return this.View(pollCreateInputModel);
            }

            var userId = this.userManager.GetUserId(this.User);
            var article = await this.pollService.CreateAsync(pollCreateInputModel, userId);

            return RedirectToAction("Details", "Polls", new { article.Id });
        }

        public async Task<IActionResult> Delete(string id)
        {
            var poll = await this.pollService.GetAsync<PollDeleteViewModel>(id);
            return this.View(poll);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id, string name)
        {
            await this.pollService.DeleteAsync(id);

            this.SetAlertMessage(AlertMessageLevel.Success, GlobalConstants.SuccessfulPollDelete);

            var logMessage = $"{GlobalConstants.SuccessfulPollDelete} ИД: {id}";
            this.logger.LogInformation((int)LoggingEvents.DeleteItem, logMessage);
            return RedirectToAction(nameof(Index));
        }
    }
}
