using Shared;

namespace Client.Models
{
    public class ChatRoom
    {
        private TemanDTO? contact;

        public string ReciveId => contact.TemanId;

        public bool IsPrivate = true;
        public ICollection<Message> Messages { get; set; } = new List<Message>();

        public ChatRoom(TemanDTO? contact)
        {
            this.contact = contact;
            IsPrivate = true;

            foreach (var item in contact.Messages)
            {
                Messages.Add(new Message
                {
                    MessageType = item.MessageType,
                    Status = item.Status,
                    Tanggal = item.Tanggal,
                    Text = item.Text,
                    PengirimId = item.PengirimId
                });
            }

        }

        internal Task AddMessage(MessagePrivate message)
        {
            Messages.Add(message);
            return Task.CompletedTask;
        }


        internal Task AddMessage(MessageGroup message)
        {
            Messages.Add(message);
            return Task.CompletedTask;
        }
    }
}
