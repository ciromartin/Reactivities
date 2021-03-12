using System.Threading.Tasks;
using Application.Common.Core;
using Domain;

namespace Application.Common.Interfaces
{
    public interface IIdentityService
    {
        Task<Result<AppUser>> LoginUser(string userName, string password);

        Task<Result<AppUser>> CreateUserAsync(AppUser user, string password);
    }
}