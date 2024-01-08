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
    public  class AddMemberMessageChange : ValueChangedMessage<GroupDTO>
    {
        public AddMemberMessageChange(GroupDTO model):base(model)
        {
                
        }
    }

}
