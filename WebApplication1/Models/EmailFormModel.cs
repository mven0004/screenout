using System.ComponentModel.DataAnnotations;

namespace MVCEmail.Models
{
    public class EmailFormModel
    {
        [Required, Display(Name = "Your name")]
        public string CustomerName { get; set; }
        [Required, Display(Name = "Your email"), EmailAddress]
        public string FromEmail { get; set; }
        [Required]
        public string Message { get; set; }
        [Required]
        public string PhoneNum { get; set; }
    }
}