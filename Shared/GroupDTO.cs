using System;
using System.Collections.Generic;

namespace Shared
{
    public class GroupDTO
    {
        public int Id { get; set; }
        public string NameGroup { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public string? Owner { get; set; }
        public IEnumerable<TemanDTO> Anggota { get; set; }
    }
}