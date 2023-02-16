using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AngularprojectApis.Models
{
    public class Users
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="Please enter Name")][MaxLength(50,ErrorMessage ="Enter only 50 Characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter Email")]
        [MaxLength(50,ErrorMessage ="Enter only 50 Characters")]
        [RegularExpression(@"^\S+@\S+\.\S+$",ErrorMessage ="Please enter Valid Email address")]
        public string Email { get; set; }



        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [Required(ErrorMessage = "Please enter Password")]
        [MaxLength(30, ErrorMessage = "Enter only 30 Characters")]
        public string Password { get; set; }

        [NotMapped]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }


        public string? OTP { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedAt { get; set; }
    }
}
