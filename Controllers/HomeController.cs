using Microsoft.AspNetCore.Mvc;

namespace GuardianAngel.Controllers;

public class HomeController : Controller
{
    [Route("")]
    public IActionResult Index() => View();

    [Route("about")]
    public IActionResult About() => View();

    [Route("ourgoals")]
    public IActionResult OurGoals() => View();

    [Route("project")]
    public IActionResult Project() => View();

    [Route("blog")]
    public IActionResult Blog() => View();

    [Route("gallery")]
    public IActionResult Gallery() => View();

    [Route("faq")]
    public IActionResult Faq() => View();

    [Route("contact")]
    public IActionResult Contact() => View();
}
