namespace ChatApp.Models
{
    public class Pertemanan
    {
        public int Id { get; set; }
        public string TemanId { get; set; }
        public ApplicationUser Teman { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public DateTime TanggalBerteman { get; set; }
    }
}
