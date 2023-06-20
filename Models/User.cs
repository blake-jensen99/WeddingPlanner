#pragma warning disable CS8618
#pragma warning disable CS8600
#pragma warning disable CS8602
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WeddingPlanner.Models;
public class User 
{
    [Key]
    public int UserId { get; set; }

    [Required(ErrorMessage = "First name is Required")]
    [Display(Name =  "First Name")]
    public string FirstName { get; set; } 

    [Required(ErrorMessage = "Last name is Required")]
    [Display(Name =  "Last Name")]
    public string LastName { get; set; }
    
    [Required]
    [EmailAddress]
    [UniqueEmail]
    public string Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    [MinLength(8, ErrorMessage ="Password must be at least 8 character")]
    public string Password { get; set; }

    [Required]
    [NotMapped]
    [Display(Name =  "Confirm Password")]
    [Compare("Password", ErrorMessage = "Passwords must match")]
    [DataType(DataType.Password)]
    public string Confirm { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    public List<Wedding> MyWeddings {get; set;} = new List<Wedding>();

    public List<Association> Attending {get; set;} = new List<Association>();
}

public class UniqueEmailAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if(value == null)
        {
            return new ValidationResult("Email is required!");
        }
        MyContext _context = (MyContext)validationContext.GetService(typeof(MyContext));
    	if(_context.Users.Any(e => e.Email == value.ToString()))
        {
            return new ValidationResult("Email must be unique!");
        } else {
            return ValidationResult.Success;
        }
    }
}

