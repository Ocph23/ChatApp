﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcphApiAuth
{
    public record AuthenticateResponse(string UserName, string Email, string Token, string PublicKey);
}
