using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;

namespace Chat.Domain.Entities;

[Index(nameof(Email), IsUnique = true)]
[Index(nameof(Username), IsUnique = true)]
public class User : BaseEntity
{
    [Column]
    [MaxLength(320)]
    [Required(ErrorMessage = "EmailIsRequired")]
    public string Email { get; set; } = null!;

    [Column]
    [MaxLength(100)]
    [Required(ErrorMessage = "UsernameIsRequired")]
    public string Username { get; set; } = null!;

    [Column]
    [MaxLength(100)]
    [Required(ErrorMessage = "PasswordIsRequired")]
    public string Password { get; set; } = null!;

    [Column]
    [MaxLength(100)]
    public string? AvatarUrl { get; set; } = null!;

    public ICollection<UserServers> UserServers { get; set; } = new List<UserServers>();
    public ICollection<UserMessages> UserMessages { get; set; } = new List<UserMessages>();

    public ICollection<Friends> Friends { get; set; } = new List<Friends>();

    public bool EmailIsValid()
    {
        string regex = @"^[^@\s]+@[^@\s]+\.(com|net|org|gov)$";

        return Regex.IsMatch(Email, regex, RegexOptions.IgnoreCase);
    }
}
