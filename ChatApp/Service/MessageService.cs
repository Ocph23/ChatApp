using ChatApp.Models;
using Microsoft.EntityFrameworkCore;
using Shared;
using Shared.Contracts;
using System.Text.RegularExpressions;

namespace ChatApp.Service
{


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
        public Task<bool> ReadMassage(string? temanId, string myId)
        {
            try
            {
                var message = dbcontext.PesanPrivat.Where(x => x.PengirimId == temanId && x.PenerimaId == myId)
                    .ExecuteUpdate(x => x.SetProperty(x => x.Status, MessageStatus.Baca));
                return Task.FromResult(true);
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

        public Task<bool> DeletePrivate(int id)
        {
            try
            {
                var data = dbcontext.PesanPrivat.SingleOrDefault(p => p.Id == id);
                if (data == null)
                    throw new SystemException("Data Tidak Ditemukan !");
                dbcontext.PesanPrivat.Remove(data);
                dbcontext.SaveChanges();
                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public Task<bool> DeleteGroup(int id)
        {
            try
            {
                var data = dbcontext.PesanGroup.SingleOrDefault(p => p.Id == id);
                if (data == null)
                    throw new SystemException("Data Tidak Ditemukan !");
                dbcontext.PesanGroup.Remove(data);
                dbcontext.SaveChanges();
                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                throw new SystemException(ex.Message);
            }
        }
    }
}
