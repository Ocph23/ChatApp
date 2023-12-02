using Shared;

namespace ChatApp.Models
{
    public class AnggotaGroup
    {
        public int Id { get; set; }
        public ApplicationUser Anggota { get; set; }
        public KeanggotaanGroup Keanggotaan { get; set; }
        public DateTime TanggalBergabung { get; set; } = DateTime.Now;

        public int GroupId { get; set; }
    }


}
