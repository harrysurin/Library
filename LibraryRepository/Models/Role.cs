using System;
using Microsoft.AspNetCore.Identity;

namespace LibraryRepository.Models
{
    public class Role: IdentityRole<Guid>
    {
        public Role()
            : base()
        {
            Id = Guid.NewGuid();
        }

        public Role(string roleName)
            : base(roleName)
        {
            Id = Guid.NewGuid();
        }
    }
}