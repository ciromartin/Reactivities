using System.Threading.Tasks;
using Application.Common.Core;
using Application.Common.Interfaces;
using Domain;

namespace Persistence.Identity
{
    public class IdentityService : IIdentityService
    {
        public Task<Result<AppUser>> CreateUserAsync(AppUser user, string password)
        {
            throw new System.NotImplementedException();
        }

        public Task<Result<AppUser>> LoginUser(string userName, string password)
        {
            throw new System.NotImplementedException();
        }
    }
}