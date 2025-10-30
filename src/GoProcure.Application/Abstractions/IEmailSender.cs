﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoProcure.Application.Abstraction
{
    public interface IEmailSender
    {
        Task SendAsync(string to, string subject, string body, CancellationToken ct = default);
    }
}
