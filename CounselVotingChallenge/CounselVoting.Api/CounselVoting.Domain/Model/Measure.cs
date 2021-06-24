using CounselVoting.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CounselVoting.Domain.Model
{
    public class Measure
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MeasureId { get; set; }

        [Required]
        [Column(TypeName = "varchar(100)")]
        [StringLength(100)]
        public string Subject { get; set; }

        [Column(TypeName = "varchar(255)")]
        [StringLength(255)]
        public string Description { get; set; }

        public MeasureStatus Status { get; set; }

        public bool IsComplete { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? CompletedDate { get; set; }

        public List<MeasureRule> Rules { get; set; }
        public List<MeasureVote> Votes { get; set; }
    }
}
