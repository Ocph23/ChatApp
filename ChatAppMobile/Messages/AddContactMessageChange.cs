using ChatAppMobile.Models;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatAppMobile.Messages
{
    public  class AddContactMessageChange  : ValueChangedMessage<TemanViewModel>
    {
        public AddContactMessageChange(TemanViewModel model):base(model)
        {
                
        }
    }



    public class AddGroupMessageChange : ValueChangedMessage<GroupDTO>
    {
        public AddGroupMessageChange(GroupDTO model) : base(model)
        {

        }
    }
}
