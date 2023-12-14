using ChatAppMobile.Models;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatAppMobile
{
    public static class MobileExtention
    {
        public static TemanViewModel ToTemanViewModel(this TemanDTO teman)
        {
            return new TemanViewModel()
            {
                Email = teman.Email,
                Keanggotaan = teman.Keanggotaan,
                Messages = new System.Collections.ObjectModel.ObservableCollection<MessagePrivate>(teman.Messages),
                Nama = teman.Nama,
                Photo = teman.Photo,
                TemanId = teman.TemanId,
            };
        }


    }
}
