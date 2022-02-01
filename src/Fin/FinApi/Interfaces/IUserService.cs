using FinApi.Requests;
using FinApi.Responses;
using System.Threading.Tasks;

namespace FinApi.Interfaces
{
    public interface IUserService
    {
        Task<TokenResponse> LoginAsync(LoginRequest loginRequest);
        Task<SignupResponse> SignupAsync(SignupRequest signupRequest);
    }
}
