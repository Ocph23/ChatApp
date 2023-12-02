using ChatApp.Models;
using Microsoft.EntityFrameworkCore;
using Shared;
using System.Text.RegularExpressions;

namespace ChatApp.Service
{
    public interface IMessageService
    {
        Task<IEnumerable<MessagePrivate>> GetPrivateMessage(string? userid1, string userid2);
        Task<MessagePrivate> PostPrivateMessage(MessagePrivate message);
        Task<IEnumerable<MessageGroup>> GetGroupMessage(int groupId);
        Task<MessageGroup> PostGroupMessage(MessageGroup mesage);
    }


    public class MessageService : IMessageService
    {
        private readonly ApplicationDbContext dbcontext;

        public MessageService(ApplicationDbContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }

        public Task<IEnumerable<MessageGroup>> GetGroupMessage(int groupId)
        {
            try
            {
                var message = dbcontext.PesanGroup.Where(x => x.GroupId == groupId);
                return Task.FromResult(message.AsEnumerable());
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Task<IEnumerable<MessagePrivate>> GetPrivateMessage(string? userid1, string userid2)
        {
            try
            {
                var message = dbcontext.PesanPrivat.Where(x => (x.PengirimId == userid1 && x.PenerimaId == userid2) ||
                (x.PengirimId == userid2 && x.PenerimaId == userid1));
                return Task.FromResult(message.AsEnumerable());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<MessageGroup> PostGroupMessage(MessageGroup message)
        {
            try
            {
                message.Tanggal = DateTime.Now.ToUniversalTime();
                dbcontext.PesanGroup.Add(message);
                var result = dbcontext.SaveChanges();
                return Task.FromResult(message);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<MessagePrivate> PostPrivateMessage(MessagePrivate message)
        {
            try
            {
                message.Tanggal = DateTime.Now.ToUniversalTime();
                dbcontext.PesanPrivat.Add(message);
                var result = dbcontext.SaveChanges();
                return Task.FromResult(message);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
