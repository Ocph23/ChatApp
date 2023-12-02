namespace ChatApp.Models
{
    public class Group
    {
        public int Id { get; set; }
        public string Nama { get; set; }
        public string Deskripsi { get; set; }
        public ApplicationUser Pembuat { get; set; }
        public List<AnggotaGroup> Anggota { get; }  =new List<AnggotaGroup>();
        public DateTime TanggalBuat { get; set; } = DateTime.Now;
    }


}
