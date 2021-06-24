using CounselVoting.Domain.Model;
using CounselVoting.Infrastructure.Repository;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CounselVoting.Infrastructure.Service
{
    public interface IRuleService
    {
        Task<IEnumerable<Rule>> GetAllAsync();
    }

    public class RuleService : IRuleService
    {
        private readonly ILogger<RuleService> _logger;
        private readonly IRuleRepository _repository;

        public RuleService(
            ILogger<RuleService> logger,
            IRuleRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public Task<IEnumerable<Rule>> GetAllAsync() => _repository.GetAllAsync();
    }
}