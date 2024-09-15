namespace WorkoutKompanion.Core.Entities
{
    public class Workout : BaseEntity
    {
        public string Name { get; set; }
        public int Duration { get; set; }
        public string Difficulty { get; set; }
        public string Region { get; set; }
    }
}
