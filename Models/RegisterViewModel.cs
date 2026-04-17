using System.ComponentModel.DataAnnotations;

namespace AstanaFoodReviews.Models;

public class RegisterViewModel
{
    [Required, MaxLength(50)]
    public string UserName { get; set; } = string.Empty;
    [Required, DataType(DataType.EmailAddress)]
    public string Email { get; set; } = string.Empty;
    [Required, DataType(DataType.Password), MinLength(6)]
    public string Password { get; set; } = string.Empty;
    [Compare(nameof(Password)), DataType(DataType.Password)]
    public string ConfirmPassword { get; set; } = string.Empty;
}
