namespace WorkoutKompanion.Core.Entities
{
    public class Exercise : BaseEntity
    {
        public int WorkoutId { get; set; }
        public string Name { get; set; }
        public int Reps { get; set; }
        public int Sets { get; set; }
    }
}
