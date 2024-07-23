using Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatAppMobile.Models
{

    public class MobileContact
    {
        public string? UserId { get; set; }
        public string? Name { get; set; }
        public string? UserName { get; set; }
        public string? Photo { get; set; }
        public ObservableCollection<TemanViewModel>? Friends { get; set; } = new ObservableCollection<TemanViewModel>();
        public ObservableCollection<GroupDTO>? Groups { get; set; } = new ObservableCollection<GroupDTO>();
        public string? Email { get; set; }

        public MobileContact()
        {

        }

        public MobileContact(string userId, string name, string? userName, string? email, string? photo = null)
        {
            this.UserId = userId;
            this.Name = name;
            this.UserName = userName;
            this.Email = email;
            this.Photo = photo;
        }
    }
}
