using CounselVoting.Domain.Enum;
using CounselVoting.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CounselVoting.Infrastructure.Service
{
    public interface ICompletionRule
    {
        EvaluationResult EvaluateRule(Measure measure, string value);
    }

    public class MinimumVotesRequiredCompletionRule : ICompletionRule
    {
        public EvaluationResult EvaluateRule(Measure measure, string value)
        {
            var totalVotes = measure.Votes.Count;
            var totalYesVotes = measure.Votes.Count(v => v.VoteChoice == VoteChoice.Yes);
            var minimumVotesRequired = int.Parse(value);

            var isRuleSatisfied = totalVotes > 0 && totalVotes >= minimumVotesRequired;
            if (isRuleSatisfied == false)
            {
                return new EvaluationResult(false, null);
            }

            var votingResult = isRuleSatisfied && totalYesVotes >= totalVotes - totalYesVotes 
                ? MeasureStatus.Passed 
                : MeasureStatus.Failed;

            // Just for demo purposes
            Console.WriteLine($@"Checking measure {measure.MeasureId} for the rule {nameof(MinimumVotesRequiredCompletionRule)}...
        TotalVotes: {totalVotes}, MinimumVotesRequired: {minimumVotesRequired}. RESULT = {isRuleSatisfied}, STATUS = {votingResult}

        ");

            return new EvaluationResult(isRuleSatisfied, votingResult);
        }
    }

    public class MinimumPercentageYesVotesRequiredCompletionRule : ICompletionRule
    {
        public EvaluationResult EvaluateRule(Measure measure, string value)
        {
            var totalVotes = measure.Votes.Count;
            var totalYesVotes = measure.Votes.Count(v => v.VoteChoice == VoteChoice.Yes);

            var minPercentageYesVotesRequired = int.Parse(value);

            var isRuleSatisfied = totalVotes > 0 && Math.Round((double)totalYesVotes / (double)totalVotes * 100, 2) >= minPercentageYesVotesRequired;
            if (isRuleSatisfied == false)
            {
                return new EvaluationResult(false, null);
            }

            var votingResult = isRuleSatisfied ? MeasureStatus.Passed : MeasureStatus.Failed;

            Console.WriteLine($@"Checking measure {measure.MeasureId} for the rule {nameof(MinimumPercentageYesVotesRequiredCompletionRule)}...
        TotalVotes: {totalVotes}, TotalYesVotes: {totalYesVotes}, MinPercentageYesVotesRequired: {minPercentageYesVotesRequired}. RESULT = {votingResult}, STATUS = {votingResult}

        ");

            return new EvaluationResult(isRuleSatisfied, votingResult);
        }
    }

    public interface IMeasureCompletionEvaluator
    {
        public EvaluationResult Evaluate(Measure measure);
    }

    public class MeasureCompletionEvaluator : IMeasureCompletionEvaluator
    {
        private readonly Dictionary<string, ICompletionRule> _completionRuleStrategies;

        public MeasureCompletionEvaluator(Dictionary<string, ICompletionRule> completionRuleStrategies)
        {
            _completionRuleStrategies = completionRuleStrategies;
        }

        public EvaluationResult Evaluate(Measure measure)
        {
            // Check if there is any measure rule that evaluetes as complete, if yes return true, otherwise false
            foreach (var measureRule in measure.Rules)
            {
                var hasRuleSatisfiedMeasureCompletion = _completionRuleStrategies[measureRule.Rule.Identifier].EvaluateRule(measure, measureRule.Value);
                if (hasRuleSatisfiedMeasureCompletion.Result)
                {
                    return hasRuleSatisfiedMeasureCompletion;
                }
            }
            return null;
        }
    }

    public record EvaluationResult(bool Result, MeasureStatus? Status);
}
