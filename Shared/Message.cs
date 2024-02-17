using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared
{
    public class Message
    {
        public string? Text { get; set; }
        public string? UrlFile { get; set; }
        public MessageType MessageType { get; set; }
        public string? PengirimId { get; set; }
        public DateTime? Tanggal { get; set; }
        public MessageStatus Status { get; set; }

        [NotMapped]
        public bool IsMe { get; set; }

    }

    public enum MessageStatus
    {
        Baru, Baca, Hapus
    }

    public class MessagePrivate : Message
    {
        public int Id { get; set; }
        public string? PenerimaId { get; set; }
        
    }

    public class MessageGroup : Message
    {
        public int Id { get; set; }
        public int GroupId { get; set; }

    }


    public class MessageRequestPublicKey
    {
       public string PublicKey { get; set; }


    }
}
