#pragma warning disable CS8618
#pragma warning disable CS8600
#pragma warning disable CS8602
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WeddingPlanner.Models;
public class LoginUser 
{
    [Required]
    [NotMapped]
    [Display(Name = "Email")]
    public string LoginEmail {get; set;}

    [Required]
    [NotMapped]
    [Display(Name = "Password")]
    [DataType(DataType.Password)]
    public string LoginPassword {get; set;}
}