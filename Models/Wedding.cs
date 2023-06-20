#pragma warning disable CS8618
#pragma warning disable CS8603
#pragma warning disable CS8765
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WeddingPlanner.Models;
public class Wedding 
{
    [Key]
    public int WeddingId {get; set;}

    [Required(ErrorMessage = "Wedder one is required")]
    [Display(Name = "Wedder One")]
    public string WedderOne {get; set;}

    [Display(Name = "Wedder Two")]
    [Required(ErrorMessage = "Wedder two is required")]
    public string WedderTwo {get; set;}

    [Required(ErrorMessage = "Date required")]
    [NoPast]
    public DateTime? Date {get; set;}

    [Required(ErrorMessage = "Address is required")]
    public string Address {get; set;}

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    public int UserId {get; set;}
    public User? Creator {get; set;}

    public List<Association> Attendees {get; set;} = new List<Association>();
}


public class NoPastAttribute : ValidationAttribute
{  
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)    
    {    
        if (value == null)
        {
            return new ValidationResult("Date is required");
        }
        else if ((DateTime)value < DateTime.Now)
        {           
            return new ValidationResult("Date cannot be in the past");   
        } else {     
            return ValidationResult.Success;  
        }  
    }
}