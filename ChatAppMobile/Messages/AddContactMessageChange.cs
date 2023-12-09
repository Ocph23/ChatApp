using CommunityToolkit.Mvvm.Messaging.Messages;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatAppMobile.Messages
{
    public  class AddContactMessageChange  : ValueChangedMessage<TemanDTO>
    {
        public AddContactMessageChange(TemanDTO model):base(model)
        {
                
        }
    }
}
