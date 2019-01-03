namespace TwentyFirst.Web.Controllers
{
    using Common.Constants;
    using Common.Models.Interviews;
    using Infrastructure.Extensions;
    using Microsoft.AspNetCore.Mvc;
    using Services.DataServices.Contracts;
    using System.Linq;
    using System.Threading.Tasks;

    public class InterviewsController : BaseController
    {
        private readonly IInterviewService interviewService;

        public InterviewsController(IInterviewService interviewService)
        {
            this.interviewService = interviewService;
        }

        public async Task<IActionResult> Index(int? pageNumber)
        {
            var interviews = await this.interviewService
                .LatestAsync<InterviewListViewModel>(GlobalConstants.MaxInterviewsCountToGet);

            var onePageOfInterviews = await interviews.ToList()
                .PaginateAsync(pageNumber, GlobalConstants.InterviewsOnPageCount);

            return this.View(onePageOfInterviews);
        }

        public async Task<IActionResult> Details(string id)
        {
            var interview = await this.interviewService.GetAsync<InterviewDetailsViewModel>(id);
            return this.View(interview);
        }
    }
}
