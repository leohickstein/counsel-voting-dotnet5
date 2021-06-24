using CounselVoting.Domain.Enum;
using CounselVoting.Domain.Model;
using CounselVoting.Infrastructure.Context;
using CounselVoting.Infrastructure.Service;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CounselVoting.Infrastructure.Repository
{
    public interface IMeasureRepository
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

    public class MeasureRepository : IMeasureRepository, IDisposable
    {
        private readonly CounselContext _context;
        private readonly IDateTimeService _dateTime;

        public MeasureRepository(CounselContext context, IDateTimeService dateTimeService)
        {
            _context = context;
            _dateTime = dateTimeService;
        }

        public async Task<Measure> InsertAsync(Measure model)
        {
            await _context.Measures.AddAsync(model);
            await _context.SaveChangesAsync();

            return model;
        }

        public async Task<Measure> UpdateAsync(int id, Measure model)
        {
            var found = await _context.Measures.FindAsync(id);
            if (found != null)
            {
                found.Subject = model.Subject;
                found.Description = model.Description;
                found.CompletedDate = model.CompletedDate;
                found.IsComplete = model.IsComplete;

                _context.Measures.Update(found);
                await _context.SaveChangesAsync();

                return found;
            }

            return null;
        }

        public async Task<Measure> DeleteAsync(int id)
        {
            var found = await _context.Measures.FindAsync(id);
            if (found != null)
            {
                _context.Measures.Remove(found);
                await _context.SaveChangesAsync();

                return found;
            }

            return null;
        }

        public Task<Measure> GetAsync(int id)
        {
            return _context.Measures.Include(m => m.Rules)
                         .Include(m => m.Votes)
                         .FirstOrDefaultAsync(m => m.MeasureId == id);
        }

        public async Task<IEnumerable<Measure>> GetAllAsync()
        {
            return await _context.Measures.ToListAsync();
        }

        public async Task<bool> VoteAsync(MeasureVote model)
        {
            try
            {
                await _context.MeasureVotes.AddAsync(model);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<IEnumerable<Measure>> GetMeasuresNotCompletedAsync()
        {
            var result = await _context.Measures.Include(v => v.Votes)
                                          .Include(m => m.Rules)
                                          .ThenInclude(r => r.Rule)
                                          .Where(m => !m.IsComplete)
                                          .ToListAsync();
            return result;
        }

        public async Task<bool> MarkAsCompleteAsync(int id, MeasureStatus status)
        {
            var found = await _context.Measures.FindAsync(id);
            if (found != null)
            {
                found.IsComplete = true;
                found.CompletedDate = _dateTime.Now;
                found.Status = status;

                _context.Measures.Update(found);
                await _context.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}