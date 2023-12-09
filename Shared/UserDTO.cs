using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class UserDTO
    {
        public string Id { get; set; }
        public string? Nama { get; set; }
        public string? Email{ get; set; }
        public string? Telepon { get; set; }
        public string? Photo { get; set; }
        public bool Active { get; set; }
    }
}
