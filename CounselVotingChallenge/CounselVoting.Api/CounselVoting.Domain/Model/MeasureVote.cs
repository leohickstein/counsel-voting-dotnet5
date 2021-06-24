using CounselVoting.Domain.Enum;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CounselVoting.Domain.Model
{
    public class MeasureVote
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VoteId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public VoteChoice VoteChoice { get; set; }

        [Required]
        [Column(TypeName = "datetime")]
        public DateTime? VoteDate { get; set; }

        public int MeasureId { get; set; }
        public Measure Measure { get; set; }
    }
}
