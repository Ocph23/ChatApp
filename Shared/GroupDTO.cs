using System;
using System.Collections.Generic;
using System.Linq;

namespace Shared
{
    public class GroupDTO
    {
        public int Id { get; set; }
        public required string NameGroup { get; set; }
        public required string Description { get; set; }
        public DateTime Created { get; set; }
        public string? Owner { get; set; }
        public IEnumerable<TemanDTO> Anggota { get; set; } = Enumerable.Empty<TemanDTO>();
    }
}