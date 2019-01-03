namespace TwentyFirst.Web.Components
{
    using Common.Models.Polls;
    using Microsoft.AspNetCore.Mvc;
    using Services.DataServices.Contracts;
    using System.Linq;
    using System.Threading.Tasks;

    [ViewComponent(Name = "active-poll")]
    public class ActivePollViewComponent : ViewComponent
    {
        private readonly IPollService pollService;

        public ActivePollViewComponent(IPollService pollService)
        {
            this.pollService = pollService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var poll = await this.pollService.GetActiveAsync<ActivePollVoteInputModel>();
            if (poll != null)
            {
                poll.Options = poll.Options.OrderBy(o => o.Id).ToList();
            }
            return this.View(poll);
        }
    }
}
