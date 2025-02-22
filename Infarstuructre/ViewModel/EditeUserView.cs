using System.ComponentModel.DataAnnotations;

namespace Infarstuructre.ViewModel
{
    public class EditeUserView
    {
        public string Id { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
    }

}
