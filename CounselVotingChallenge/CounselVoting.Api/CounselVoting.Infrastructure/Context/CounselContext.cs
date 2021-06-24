using CounselVoting.Domain.Enum;
using CounselVoting.Domain.Model;
using CounselVoting.Infrastructure.Helper;
using CounselVoting.Infrastructure.Service;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CounselVoting.Infrastructure.Context
{
    public class CounselContext : DbContext
    {
        public CounselContext(DbContextOptions<CounselContext> options)
            : base(options)
        {
        }

        public DbSet<Measure> Measures { get; set; }
        public DbSet<MeasureRule> MeasureRules { get; set; }
        public DbSet<MeasureVote> MeasureVotes { get; set; }
        public DbSet<Rule> Rules { get; set; }
        public DbSet<VotingRule> VotingRules { get; set; }
        public DbSet<CompletionRule> CompletionRules { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MeasureRule>()
                .HasKey(t => new { t.MeasureId, t.RuleId });

            modelBuilder.Entity<MeasureRule>()
                .HasOne(pt => pt.Measure)
                .WithMany(p => p.Rules)
                .HasForeignKey(pt => pt.MeasureId);

            modelBuilder.Entity<MeasureRule>()
                .HasOne(pt => pt.Rule)
                .WithMany(t => t.MeasureRules)
                .HasForeignKey(pt => pt.RuleId);

            modelBuilder.Entity<CompletionRule>()
                .HasData(GetRulesFromCompletionRuleStrategies());

            modelBuilder.Entity<Measure>()
                .HasData(
                    new Measure { MeasureId = 1, Subject = "Install a front gate", Description = "Description for installing a front gate", Status = MeasureStatus.Open },
                    new Measure { MeasureId = 2, Subject = "Fix the current generator", Description = "Description for fixing the current generator", Status = MeasureStatus.Open },
                    new Measure { MeasureId = 3, Subject = "Increase the number of strata counsels", Description = "Description for incresing the number of strata counsels", Status = MeasureStatus.Open }
                );

            modelBuilder.Entity<MeasureRule>()
                .HasData(
                    new MeasureRule { MeasureId = 1, RuleId = 1, Value = "5" }, // Minimum Votes Required Completion Rule
                    new MeasureRule { MeasureId = 2, RuleId = 2, Value = "80" }, // Minimum Percentage Yes Votes Required Completion Rule
                    new MeasureRule { MeasureId = 3, RuleId = 1, Value = "10" }, // Minimum Votes Required Completion Rule
                    new MeasureRule { MeasureId = 3, RuleId = 2, Value = "50" } // Minimum Percentage Yes Votes Required Completion Rule
                );
        }

        public static Rule[] GetRulesFromCompletionRuleStrategies()
        {
            var types = AssemblyHelper.GetAllTypesThatImplementInterface<ICompletionRule>();
            var result = types.Select((type, index) => new CompletionRule
            {
                RuleId = index + 1,
                Name = FormatRuleName(type.Name),
                Description = $"Description for {FormatRuleName(type.Name)}",
                Identifier = type.Name
            }).ToArray();

            return result;

            static string FormatRuleName(string name) => 
                string.Concat(name.Select(x => char.IsUpper(x) ? " " + x : x.ToString())).TrimStart(' ');
        }
    }
}