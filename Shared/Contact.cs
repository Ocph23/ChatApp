
using System.Collections.Generic;

namespace Shared
{
    public class Contact
    {
        public string? UserId { get; set; }
        public string? Name { get;  set; }
        public string? UserName { get; set; }
        public string? Photo { get; set; }
        public ICollection<TemanDTO>? Friends { get; set; } = new List<TemanDTO>();
        public ICollection<GroupDTO>? Groups { get; set; } = new List<GroupDTO>();
        public string? Email { get; set; }

        public Contact()
        {
            
        }

        public Contact(string userId, string name, string? userName, string? email, string? photo=null)
        {
            this.UserId = userId;
            this.Name = name;
            this.UserName= userName;
            this.Email= email;
            this.Photo = photo;
        }
    }


}
