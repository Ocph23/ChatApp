using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Shared
{
    public class TemanDTO
    {
        public string? Nama { get; set; }
        public string? TemanId { get; set; }
        public string? Email { get; set; }
        public string? Photo { get; set; }
        public KeanggotaanGroup Keanggotaan { get; set; }
        public ICollection<MessagePrivate> Messages { get; set; } = new List<MessagePrivate>();
        public int IsNewCount => Messages.Where(x => x.Status == MessageStatus.Baru).Count();
    }
}