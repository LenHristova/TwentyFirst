namespace TwentyFirst.Web.Controllers
{
    using Common.Constants;
    using Common.Models.Polls;
    using Filters;
    using Microsoft.AspNetCore.Mvc;
    using Services.DataServices.Contracts;
    using System.Threading.Tasks;

    [TypeFilter(typeof(ErrorAlertExceptionFilterAttribute))]
    public class PollsController : BaseController
    {
        private readonly IPollService pollService;

        public PollsController(IPollService pollService)
        {
            this.pollService = pollService;
        }

        [HttpPost]
        public async Task<IActionResult> Vote(ActivePollVoteInputModel activePollViewModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.Content(GetModelStateErrorMessages());
            }

            await this.pollService.VoteAsync(activePollViewModel);

            return this.Content(GlobalConstants.PollVoteThanks);
        }
    }
}
