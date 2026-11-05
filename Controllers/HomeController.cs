using GuardianAngel.Models;
using GuardianAngel.Services;
using Microsoft.AspNetCore.Mvc;

namespace GuardianAngel.Controllers;

public class HomeController(CaptchaService captcha) : Controller
{
    [Route("")]
    public IActionResult Index() => View();

    [Route("about")]
    public IActionResult About() => View();

    [Route("ourgoals")]
    public IActionResult OurGoals() => View();

    [Route("project")]
    public IActionResult Project() => View();

    // ================= PROJECTS / INSTITUTIONS ROUTES =================

    [Route("project/guardian-angel-retirement-home")]
    public IActionResult GuardianAngelRetirementHome() => View();

    [Route("project/yeldho-mar-baselios-boys-home")]
    public IActionResult YeldhoMarBaseliosBoysHome() => View();

    [Route("project/bethsada-geriatric-destitute-home")]
    public IActionResult BethsadaGeriatricDestituteHome() => View();

    [Route("project/bethesda-mental-health-center")]
    public IActionResult BethesdaMentalHealthCenter() => View();

    [Route("project/bethsada-public-school")]
    public IActionResult BethsadaPublicSchool() => View();

    [Route("project/mar-gabriel-prayer-tower")]
    public IActionResult MarGabrielPrayerTower() => View();

    [Route("project/hamara-auditorium")]
    public IActionResult HamaraAuditorium() => View();

    [Route("project/guardian-angel-peace-mission-center")]
    public IActionResult GuardianAngelPeaceMissionCenter() => View();

    [Route("project/the-hermits")]
    public IActionResult TheHermits() => View();

    [Route("project/bethsada-magazine")]
    public IActionResult BethsadaMagazine() => View();

    // ==================================================================

    [Route("blog")]
    public IActionResult Blog() => View();

    [Route("gallery")]
    public IActionResult Gallery() => View();

    [Route("faq")]
    public IActionResult Faq() => View();

    [HttpGet("contact")]
    public IActionResult Contact() => View(new ContactForm());

    [HttpPost("contact")]
    [ValidateAntiForgeryToken]
    public IActionResult Contact(ContactForm form)
    {
        // Validate always consumes the code, so a failed post always gets a fresh image
        var captchaOk = captcha.Validate(HttpContext.Session, form.Captcha);
        if (!captchaOk && !string.IsNullOrWhiteSpace(form.Captcha))
        {
            ModelState.AddModelError(nameof(ContactForm.Captcha), "The code didn't match. Please try the new one.");
        }

        if (!ModelState.IsValid)
            return View(form);

        // TODO: send the message by email (form.Name, form.Email, form.Subject, form.Message)
        TempData["ContactSuccess"] = "Thank you! Your message has been received. We will get back to you soon.";
        return Redirect("/contact#contact-form");
    }

    [HttpGet("captcha")]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public IActionResult Captcha() => File(captcha.CreateImage(HttpContext.Session), "image/png");
}