using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Models;

namespace Portfolio.Controllers
{
    public class ContactController : Controller
    {
        private readonly IConfiguration _configuration;

        public ContactController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(ContactForm model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var emailSettings = _configuration.GetSection("EmailSettings");
                    
                    var smtpServer = emailSettings["SmtpServer"];
                    var smtpPort = int.Parse(emailSettings["SmtpPort"] ?? "587");
                    var senderEmail = emailSettings["SenderEmail"];
                    var senderPassword = emailSettings["SenderPassword"];
                    var receiverEmail = emailSettings["ReceiverEmail"];

                    using (var client = new SmtpClient(smtpServer, smtpPort))
                    {
                        client.Credentials = new NetworkCredential(senderEmail, senderPassword);
                        client.EnableSsl = true;

                        var mailMessage = new MailMessage
                        {
                            From = new MailAddress(senderEmail!),
                            Subject = $"Portfolio Contact: {model.Name}",
                            Body = $"From: {model.Name} ({model.Email})\n\nMessage:\n{model.Message}",
                            IsBodyHtml = false
                        };
                        mailMessage.To.Add(receiverEmail!);

                        await client.SendMailAsync(mailMessage);
                    }

                    TempData["Success"] = "Your message has been sent successfully! I will get back to you as soon as possible.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while sending your message. Please try again later.");
                    // Log the error (ex) if needed
                }
            }

            return View(model);
        }
    }
}
