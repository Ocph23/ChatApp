using Shared;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shared.Contracts
{
    public interface IMessageService
    {
        Task<IEnumerable<MessagePrivate>> GetPrivateMessage(string? userid1, string userid2);
        Task<MessagePrivate> PostPrivateMessage(MessagePrivate message);
        Task<IEnumerable<MessageGroup>> GetGroupMessage(int groupId);
        Task<MessageGroup> PostGroupMessage(MessageGroup mesage);
        Task<bool> ReadMassage(string? temanId, string myId);
        Task<bool> DeletePrivate(int id);
        Task<bool> DeleteGroup(int id);
    }
}
