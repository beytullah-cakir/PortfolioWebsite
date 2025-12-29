using System.ComponentModel.DataAnnotations;

namespace Portfolio.Models
{
    public class ContactForm
    {
        [Required(ErrorMessage = "Lütfen adınızı giriniz.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Lütfen e-posta adresinizi giriniz.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Lütfen mesajınızı giriniz.")]
        public string Message { get; set; } = string.Empty;
    }
}
