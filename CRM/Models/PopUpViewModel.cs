using System.ComponentModel.DataAnnotations;

namespace CRM.Models
{
    public sealed class PopUpViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Phone number")]
        public PhoneViewModel PhoneNumber { get; set; }

        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Message")]
        public string Message { get; set; }
    }
}