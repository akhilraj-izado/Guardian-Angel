using System.ComponentModel.DataAnnotations;

namespace GuardianAngel.Models;

public class ContactForm
{
    [Required(ErrorMessage = "Please enter your name.")]
    [StringLength(100)]
    public string Name { get; set; } = "";

    [Required(ErrorMessage = "Please enter your email.")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
    [StringLength(200)]
    public string Email { get; set; } = "";

    [Required(ErrorMessage = "Please enter a subject.")]
    [StringLength(200)]
    public string Subject { get; set; } = "";

    [Required(ErrorMessage = "Please enter your message.")]
    [StringLength(5000)]
    public string Message { get; set; } = "";

    [Required(ErrorMessage = "Please enter the code shown in the image.")]
    public string Captcha { get; set; } = "";
}
