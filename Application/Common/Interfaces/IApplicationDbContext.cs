using System.Threading;
using System.Threading.Tasks;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        public DbSet<Activity> Activities {get;set;} 
         Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}