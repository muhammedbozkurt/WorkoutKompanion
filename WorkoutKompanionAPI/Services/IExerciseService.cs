using WorkoutKompanion.Core.DTOs;
using WorkoutKompanion.Core.Utilities.Results;
using CoreResult = WorkoutKompanion.Core.Utilities.Results.IResult;

namespace WorkoutKompanionAPI.Services
{
    public interface IExerciseService
    {
        Task<IDataResult<ExerciseDto>> GetExerciseByIdAsync(int id);
        Task<IDataResult<ExerciseDto>> AddExerciseAsync(ExerciseCreateDto exerciseDto);
        Task<CoreResult> UpdateExerciseAsync(int id, ExerciseUpdateDto exerciseDto);
        Task<CoreResult> DeleteExerciseAsync(int id);
    }
}
