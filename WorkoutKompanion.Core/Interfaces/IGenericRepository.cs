using WorkoutKompanion.Core.Entities;

namespace WorkoutKompanion.Core.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int id);
        Task<int> AddAsync(T entity);
        Task<int> UpdateAsync(T entity);
        Task DeleteAsync(int id);
        Task<IEnumerable<Workout>> GetFilteredWorkoutsWithPaginationAsync(int? duration, string difficulty, string region, int pageNumber, int pageSize);
        Task<IEnumerable<Exercise>> GetExercisesWithPaginationAsync(int workoutId, int pageNumber, int pageSize);
    }
}
