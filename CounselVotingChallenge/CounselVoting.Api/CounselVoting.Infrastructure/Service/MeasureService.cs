using CounselVoting.Domain.Enum;
using CounselVoting.Domain.Model;
using CounselVoting.Infrastructure.Repository;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CounselVoting.Infrastructure.Service
{
    public interface IMeasureService
    {
        Task<Measure> InsertAsync(Measure model);
        Task<Measure> UpdateAsync(int id, Measure model);
        Task<Measure> DeleteAsync(int id);
        Task<Measure> GetAsync(int id);
        Task<IEnumerable<Measure>> GetAllAsync();
        Task<IEnumerable<Measure>> GetMeasuresNotCompletedAsync();
        Task<bool> VoteAsync(MeasureVote model);
        Task<bool> MarkAsCompleteAsync(int id, MeasureStatus status);
        void Dispose();
    }

    public class MeasureService : IMeasureService, IDisposable
    {
        private readonly ILogger<MeasureService> _logger;
        private readonly IMeasureRepository _repository;

        public MeasureService(
            ILogger<MeasureService> logger,
            IMeasureRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public Task<Measure> InsertAsync(Measure model) => _repository.InsertAsync(model);

        public Task<Measure> UpdateAsync(int id, Measure model) => _repository.UpdateAsync(id, model);

        public Task<Measure> DeleteAsync(int id) => _repository.DeleteAsync(id);

        public Task<Measure> GetAsync(int id) => _repository.GetAsync(id);

        public Task<IEnumerable<Measure>> GetAllAsync() => _repository.GetAllAsync();

        public Task<bool> VoteAsync(MeasureVote model) => _repository.VoteAsync(model);

        public Task<IEnumerable<Measure>> GetMeasuresNotCompletedAsync() => _repository.GetMeasuresNotCompletedAsync();

        public Task<bool> MarkAsCompleteAsync(int id, MeasureStatus status) => _repository.MarkAsCompleteAsync(id, status);

        public void Dispose() => _repository.Dispose();
    }
}