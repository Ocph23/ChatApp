using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatAppMobile.Models
{
    public record ChatMessage (string? Text, DateTime? Tanggal, bool IsMe);
}
