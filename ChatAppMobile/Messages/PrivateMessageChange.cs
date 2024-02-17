using CommunityToolkit.Mvvm.Messaging.Messages;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatAppMobile.Messages
{

    public class RequestPublicKey : ValueChangedMessage<MessageRequestPublicKey>
    {
        public RequestPublicKey(MessageRequestPublicKey model):base(model)
        {
            
        }
    }

    internal class PrivateMessageChange :ValueChangedMessage<MessagePrivate>
    {
        public PrivateMessageChange(MessagePrivate model) : base(model)
        {

        }
    }


    internal class PrivateSendMessageChange : ValueChangedMessage<MessagePrivate>
    {
        public PrivateSendMessageChange(MessagePrivate model) : base(model)
        {

        }
    }




    internal class GroupMessageChange : ValueChangedMessage<MessageGroup>
    {
        public GroupMessageChange(MessageGroup model) : base(model)
        {

        }
    }


    internal class GroupSendMessageChange : ValueChangedMessage<MessageGroup>
    {
        public GroupSendMessageChange(MessageGroup model) : base(model)
        {

        }
    }
}
