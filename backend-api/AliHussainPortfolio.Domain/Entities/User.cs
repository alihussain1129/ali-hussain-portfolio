using Microsoft.AspNetCore.Identity;

namespace AliHussainPortfolio.Domain.Entities;

public class User : IdentityUser<int>
{
    public string Name { get; set; } = string.Empty;
    public string Role { get; set; } = "Admin";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; } = true;
}