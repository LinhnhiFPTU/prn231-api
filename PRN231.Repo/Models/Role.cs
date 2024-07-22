using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PRN231.Repo.Models
{
    public partial class Role
    {
        public Role()
        {
            Accounts = new HashSet<Account>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }

        [JsonIgnore] public virtual ICollection<Account> Accounts { get; set; }
    }
}
