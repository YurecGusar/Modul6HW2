using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modul6Hw2.Data.Entities
{
    public class RefreshToken
    {
        public int Id { get; set; }

        public string Value { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }
    }
}
