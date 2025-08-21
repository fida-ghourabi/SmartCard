using STB.SmartCard.Application.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STB.SmartCard.Application.UseCaseInterfaces
{
    public interface IAuthUseCase
    {
        Task<string> RegisterAsync(RegisterDto dto);
        Task<LoginResponseDto> LoginAsync(LoginDto dto);
    }
}
