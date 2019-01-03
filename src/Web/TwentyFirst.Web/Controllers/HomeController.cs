namespace TwentyFirst.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : Controller
    {
        public IActionResult Index() => this.View();

        public IActionResult Contact() => this.View();

        public IActionResult Privacy() => this.View();
    }
}
