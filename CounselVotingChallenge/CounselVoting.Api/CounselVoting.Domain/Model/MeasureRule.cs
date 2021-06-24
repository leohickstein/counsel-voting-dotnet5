namespace CounselVoting.Domain.Model
{
    public class MeasureRule
    {
        public string Value { get; set; }

        public int MeasureId { get; set; }
        public Measure Measure { get; set; }

        public int RuleId { get; set; }
        public Rule Rule { get; set; }
    }
}
