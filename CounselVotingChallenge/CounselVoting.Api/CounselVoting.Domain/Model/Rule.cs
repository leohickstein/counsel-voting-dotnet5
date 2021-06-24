using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CounselVoting.Domain.Model
{
    public abstract class Rule
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RuleId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Identifier { get; set; }

        public List<MeasureRule> MeasureRules { get; set; }
    }

    // Table-per-hierarchy (TPH) - allows storing hierarchically related entities in a single table.
    public class VotingRule : Rule
    {

    }

    public class CompletionRule : Rule
    {

    }
}
