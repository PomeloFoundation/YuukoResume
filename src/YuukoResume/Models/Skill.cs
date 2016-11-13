namespace YuukoResume.Models
{
    public enum SkillPerformance
    {
        Professional,
        Other,
        Language
    }

    public class Skill
    {
        public long Id { get; set; }

        public string Title { get; set; }

        public int Level { get; set; }

        public SkillPerformance Performance { get; set; }
    }
}
