using CounselVoting.Infrastructure.Context;
using CounselVoting.Infrastructure.Service;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CounselVoting.Api
{
    public class VotingEngineHostedService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMeasureCompletionEvaluator _measureCompletionEvaluator;

        public VotingEngineHostedService(
            IServiceScopeFactory scopeFactory, 
            IMeasureCompletionEvaluator measureCompletionEvaluator)
        {
            _scopeFactory = scopeFactory;

            using var scope = _scopeFactory.CreateScope();
            using var context = scope.ServiceProvider.GetRequiredService<CounselContext>();
            context.Database.EnsureCreated();

            _measureCompletionEvaluator = measureCompletionEvaluator;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await CheckMeasuresCompletion();

                await Task.Delay(5000, stoppingToken);
            }
        }

        private async Task CheckMeasuresCompletion()
        {
            using var scope = _scopeFactory.CreateScope();
            var measureService = scope.ServiceProvider.GetRequiredService<IMeasureService>();
            var measuresNotCompleted = await measureService.GetMeasuresNotCompletedAsync();

            foreach (var measure in measuresNotCompleted)
            {
                var evaluationResult = _measureCompletionEvaluator.Evaluate(measure);
                if (evaluationResult != null) // It's completed
                {
                    _ = await measureService.MarkAsCompleteAsync(measure.MeasureId, evaluationResult.Status.Value);
                    Console.WriteLine($"Marking measure {measure.MeasureId} as completed with status {evaluationResult.Status}");
                }
            }
        }
    }
}
