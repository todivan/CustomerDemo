using System.ComponentModel.DataAnnotations;

namespace CustomerDemo.Dto
{
    public class CustomerCreateDto
    {
        [Required(ErrorMessage = "Firstname is required")]
        [StringLength(60, ErrorMessage = "Firstname can't be longer than 60 characters")]
        public string? Firstname { get; set; }

        [Required(ErrorMessage = "Surename is required")]
        [StringLength(60, ErrorMessage = "Surename can't be longer than 60 characters")]
        public string? Surename { get; set; }

    }
}
