using CounselVoting.Domain.Model;
using CounselVoting.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CounselVoting.Infrastructure.Repository
{
    public interface IRuleRepository
    {
        Task<IEnumerable<Rule>> GetAllAsync();
    }

    public class RuleRepository : IRuleRepository
    {
        private readonly CounselContext _context;

        public RuleRepository(CounselContext context) => _context = context;

        public async Task<IEnumerable<Rule>> GetAllAsync() => await _context.Rules.ToListAsync();
    }
}