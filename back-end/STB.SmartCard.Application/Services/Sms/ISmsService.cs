using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STB.SmartCard.Application.Services.Sms
{
    public interface ISmsService
    {
        Task SendOtpSmsAsync(string mobile, string otpCode);
    }
}
