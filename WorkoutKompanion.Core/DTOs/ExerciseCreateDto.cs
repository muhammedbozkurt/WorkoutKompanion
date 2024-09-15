namespace WorkoutKompanion.Core.DTOs
{
    public class ExerciseCreateDto
    {
        public int WorkoutId { get; set; }
        public string Name { get; set; }
        public int Reps { get; set; }
        public int Sets { get; set; }
    }
}
