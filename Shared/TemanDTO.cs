namespace Shared
{
    public class TemanDTO
    {
        public string? Nama { get; set; }
        public string UserId { get; set; }
        public string? Email { get; set; }
        public string? Photo { get; set; }

        public KeanggotaanGroup Keanggotaan { get; set; }
    }
}