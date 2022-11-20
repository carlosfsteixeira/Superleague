using Superleague.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Superleague.Data
{
    public interface IMatchRepository : IRepository<Match>
    {
        Task<List<Match>> GetAllMatchesAsync();
        Task VerifyIfMatchPlayedAsync(List<Match> matches);
    }
}