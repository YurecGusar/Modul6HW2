using Microsoft.AspNetCore.Identity;
using System;

namespace Modul6Hw2.Data.Entities
{
    public class User : IdentityUser
    {
        public DateTime BirthDate { get; set; }
    }
}
