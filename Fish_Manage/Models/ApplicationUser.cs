﻿using Microsoft.AspNetCore.Identity;

namespace Fish_Manage.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public ICollection<Order> Orders { get; set; }
    }

}
