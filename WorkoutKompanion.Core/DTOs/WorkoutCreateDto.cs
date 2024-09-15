namespace WorkoutKompanion.Core.DTOs
{
    public class WorkoutCreateDto
    {
        public string Name { get; set; }
        public int Duration { get; set; }
        public string Difficulty { get; set; }
        public string Region { get; set; }
    }
}
