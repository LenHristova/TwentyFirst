namespace TwentyFirst.Web.Controllers
{
    using Common.Models.Interviews;
    using Filters;
    using Microsoft.AspNetCore.Mvc;
    using Services.DataServices.Contracts;
    using System.Threading.Tasks;

    [TypeFilter(typeof(ErrorPageExceptionFilterAttribute))]
    public class InterviewsController : BaseController
    {
        private readonly IInterviewService interviewService;

        public InterviewsController(IInterviewService interviewService)
        {
            this.interviewService = interviewService;
        }

        public async Task<IActionResult> Details(string id)
        {
            var interview = await this.interviewService.GetAsync<InterviewDetailsViewModel>(id);
            return this.View(interview);
        }
    }
}
