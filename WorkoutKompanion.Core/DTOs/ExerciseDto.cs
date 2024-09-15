namespace WorkoutKompanion.Core.DTOs
{
    public class ExerciseDto
    {
        public int Id { get; set; }
        public int WorkoutId { get; set; }
        public string Name { get; set; }
        public int Reps { get; set; }
        public int Sets { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
    }
}
