namespace TwentyFirst.Web.Components
{
    using System.Linq;
    using System.Threading.Tasks;
    using Common.Constants;
    using Common.Models.Interviews;
    using Microsoft.AspNetCore.Mvc;
    using Services.DataServices.Contracts;

    [ViewComponent(Name = "latest-interviews")]
    public class LatestInterviewsViewComponent : ViewComponent
    {
        private readonly IInterviewService interviewService;

        public LatestInterviewsViewComponent(IInterviewService interviewService)
        {
            this.interviewService = interviewService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var connectedInterviewsViewModels = await this.interviewService
                .LatestAsync<InterviewListViewModel>(
                    GlobalConstants.InterviewsCountForLatestSection);
            return this.View(connectedInterviewsViewModels.ToList());
        }
    }
}
