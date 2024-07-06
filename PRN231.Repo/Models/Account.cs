using System.Text.Json.Serialization;

namespace PRN231.Repo.Models;

public class Account
{
    public Guid Id { get; init; }
    public string Name { get; init; } = null!;
    public string Username { get; init; } = null!;
    public string Password { get; init; } = null!;
    public Guid RoleId { get; init; }
    public string Status { get; init; } = null!;

    [JsonIgnore] public virtual Role Role { get; set; } = null!;
}