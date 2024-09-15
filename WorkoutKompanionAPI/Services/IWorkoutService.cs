using WorkoutKompanion.Core.DTOs;
using WorkoutKompanion.Core.Entities;
using WorkoutKompanion.Core.Utilities.Results;
using CoreResult = WorkoutKompanion.Core.Utilities.Results.IResult;

namespace WorkoutKompanionAPI.Services
{
    public interface IWorkoutService
    {
        Task<IDataResult<WorkoutDto>> GetWorkoutByIdAsync(int id);
        Task<IDataResult<WorkoutDto>> AddWorkoutAsync(WorkoutCreateDto workoutDto);
        Task<CoreResult> UpdateWorkoutAsync(int id, WorkoutUpdateDto workoutDto);
        Task<CoreResult> DeleteWorkoutAsync(int id);
        Task<IDataResult<IEnumerable<WorkoutDto>>> GetFilteredWorkoutsWithPaginationAsync(int? duration, string difficulty, string region, int pageNumber, int pageSize);
        Task<IDataResult<(Workout, IEnumerable<Exercise>)>> GetWorkoutWithExercisesAsync(int workoutId, int pageNumber, int pageSize);
    }
}
