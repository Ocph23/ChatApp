
using System.Collections.Generic;

namespace Shared
{
    public class Contact
    {
        private readonly string? userId;
        public string Name { get; private set; }
        public string? UserName { get; private set; }
        public ICollection<TemanDTO> Frients { get; set; } = new List<TemanDTO>();
        public ICollection<GroupDTO> Groups { get; set; } = new List<GroupDTO>();
        public string? Email { get; private set; }

        public Contact(string userId, string name, string? userName, string? email)
        {
            this.userId = userId;
            this.Name = name;
            this.UserName= userName;
            this.Email= email;
        }
    }


}
