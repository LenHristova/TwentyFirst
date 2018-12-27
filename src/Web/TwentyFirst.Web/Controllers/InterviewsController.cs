namespace TwentyFirst.Web.Controllers
{
    using System.Linq;
    using Common.Models.Interviews;
    using Filters;
    using Microsoft.AspNetCore.Mvc;
    using Services.DataServices.Contracts;
    using System.Threading.Tasks;
    using Common.Constants;
    using Common.Models.Articles;
    using Infrastructure.Extensions;

    [TypeFilter(typeof(ErrorPageExceptionFilterAttribute))]
    public class InterviewsController : BaseController
    {
        private readonly IInterviewService interviewService;

        public InterviewsController(IInterviewService interviewService)
        {
            this.interviewService = interviewService;
        }

        public async Task<IActionResult> Index(int? pageNumber, string categoryId = null)
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
