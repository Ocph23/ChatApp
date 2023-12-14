using Shared;
using Shared.Contracts;

namespace ChatAppMobile.Mocks
{
    public class MessageServiceMock : IMessageService
    {

        private ICollection<MessageGroup> _messagesGroup = new List<MessageGroup>() {
            new MessageGroup(){ GroupId=1, MessageType= MessageType.File, PengirimId="1",
                Status= MessageStatus.Baru, Tanggal=DateTime.Now.ToUniversalTime(), Text="Pesan A", Id=1},
            new MessageGroup(){ GroupId=2, MessageType= MessageType.File, PengirimId="2",
                Status= MessageStatus.Baru, Tanggal=DateTime.Now.ToUniversalTime(), Text="Pesan B", Id=2},

        };
        private ICollection<MessagePrivate> _messagesPrivate = new List<MessagePrivate>() {
            new MessagePrivate{ MessageType= MessageType.Text, PenerimaId="1", PengirimId="2", Tanggal=DateTime.Now, Text="Pesan C",
                Status= MessageStatus.Baru, Id=1 },
            new MessagePrivate{ MessageType= MessageType.Text, PenerimaId="2", PengirimId="1", Tanggal=DateTime.Now, Text="Pesan D",
                Status= MessageStatus.Baru, Id=2 },
        };
        public Task<IEnumerable<MessageGroup>> GetGroupMessage(int groupId)
        {
            return Task.FromResult(_messagesGroup.AsEnumerable());
        }

        public Task<IEnumerable<MessagePrivate>> GetPrivateMessage(string? userid1, string userid2)
        {
            return Task.FromResult(_messagesPrivate.AsEnumerable());
        }

        public Task<MessageGroup> PostGroupMessage(MessageGroup message)
        {
            _messagesGroup.Add(message);
            return Task.FromResult(message);
        }

        public Task<MessagePrivate> PostPrivateMessage(MessagePrivate message)
        {
            _messagesPrivate.Add(message);
            return Task.FromResult(message);
        }

        public Task<bool> ReadMassage(string? temanId, string myId)
        {
            throw new NotImplementedException();
        }
    }
}
