using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STB.SmartCard.Application.DTOs
{
    public class ValidateOtpDto
    {
        public Guid TransactionPendingId { get; set; }
        public string OtpCode { get; set; }
    }

}
