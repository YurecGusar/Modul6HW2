using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modul6HW2.Services.Models
{
    public class RefreshTokenModel
    {
        public int Id { get; set; }

        public string Value { get; set; }

        public string UserId { get; set; }
        public UserModel User { get; set; }
    }
}
