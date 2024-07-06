using System.Text.Json.Serialization;

namespace PRN231.Repo.Models;

public class Role
{
    public Role()
    {
        Accounts = new HashSet<Account>();
    }

    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Status { get; set; } = null!;

    [JsonIgnore] public virtual ICollection<Account> Accounts { get; set; }
}