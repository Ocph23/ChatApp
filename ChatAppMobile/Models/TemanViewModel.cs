using ChatAppMobile.ViewModels;
using Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatAppMobile.Models
{
    public class TemanViewModel : BaseNotify
    {
        public TemanViewModel()
        {
        }

        internal void AddMessage(MessagePrivate value)
        {
            Messages.Add(value);
            Count = Messages.Count(x => x.Status == MessageStatus.Baru);

            OnPropertyChanged(nameof(LastMessageDate));
        }

        private string? nama;

        public string? Nama
        {
            get { return nama; }
            set { SetProperty(ref nama, value); }
        }


        private string? temanId;

        public string? TemanId
        {
            get { return temanId; }
            set { SetProperty(ref temanId, value); }
        }


        private string? email;

        public string? Email
        {
            get { return email; }
            set { SetProperty(ref email, value); }
        }


        private string? photo;

        public string? Photo
        {
            get { return photo; }
            set { SetProperty(ref photo, value); }
        }


        private KeanggotaanGroup keanggotaan;

        public KeanggotaanGroup Keanggotaan
        {
            get { return keanggotaan; }
            set { SetProperty(ref keanggotaan, value); }
        }


        private int count;

        public int Count
        {
            get { return count; }
            set { SetProperty(ref count, value); }
        }

        private string? lastMessageDate=string.Empty;

        public string? LastMessageDate
        {
            get
            {
                if (Messages.Any())
                {
                    var data = Messages.Last();
                    lastMessageDate = data.Tanggal.Value.ToString("HH:ss");
                }
                return lastMessageDate;
            }
            set { SetProperty(ref lastMessageDate, value); }
        }

        public ObservableCollection<MessagePrivate> Messages { get; set; } = new ObservableCollection<MessagePrivate>();

    }
}
