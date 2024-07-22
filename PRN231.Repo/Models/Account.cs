using System;
using System.Collections.Generic;

namespace PRN231.Repo.Models
{
    public partial class Account
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Password { get; set; }
        public byte? Status { get; set; }
        public int? RoleId { get; set; }
        public string? Username { get; set; }
        public int? BrandId { get; set; }

        public virtual Brand? Brand { get; set; }
        public virtual Role? Role { get; set; }
    }
}
