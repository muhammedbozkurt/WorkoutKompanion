using WorkoutKompanion.Core.DTOs;
using WorkoutKompanion.Core.Entities;
using WorkoutKompanion.Core.Interfaces;
using WorkoutKompanion.Core.Utilities.Helpers;
using WorkoutKompanion.Core.Utilities.Results;
using CoreResult = WorkoutKompanion.Core.Utilities.Results.IResult;

namespace WorkoutKompanionAPI.Services
{
    public class ExerciseService : IExerciseService
    {
        private readonly IGenericRepository<Exercise> _exerciseRepository;
        private readonly IUserHelper _userHelper;

        public ExerciseService(IGenericRepository<Exercise> exerciseRepository, IUserHelper userHelper)
        {
            _exerciseRepository = exerciseRepository;
            _userHelper = userHelper;
        }

        public async Task<IDataResult<ExerciseDto>> GetExerciseByIdAsync(int id)
        {
            var exercise = await _exerciseRepository.GetByIdAsync(id);
            if (exercise == null)
                return new ErrorDataResult<ExerciseDto>("Exercise not found.");

            var exerciseDto = new ExerciseDto
            {
                Id = exercise.Id,
                WorkoutId = exercise.WorkoutId,
                Name = exercise.Name,
                Reps = exercise.Reps,
                Sets = exercise.Sets,
                CreatedDate = exercise.CreatedDate,
                CreatedBy = exercise.CreatedBy,
                UpdatedDate = exercise.UpdatedDate,
                UpdatedBy = exercise.UpdatedBy
            };

            return new SuccessDataResult<ExerciseDto>(exerciseDto, "Exercise retrieved successfully.");
        }

        public async Task<IDataResult<ExerciseDto>> AddExerciseAsync(ExerciseCreateDto exerciseDto)
        {
            var exercise = new Exercise
            {
                WorkoutId = exerciseDto.WorkoutId,
                Name = exerciseDto.Name,
                Reps = exerciseDto.Reps,
                Sets = exerciseDto.Sets,
                CreatedBy = _userHelper.GetCurrentUserName(),
                CreatedDate = DateTime.Now
            };

            var exerciseId = await _exerciseRepository.AddAsync(exercise);
            if (exerciseId == 0)
                return new ErrorDataResult<ExerciseDto>("Failed to add exercise.");

            var addedExerciseDto = new ExerciseDto
            {
                Id = exerciseId,
                WorkoutId = exercise.WorkoutId,
                Name = exercise.Name,
                Reps = exercise.Reps,
                Sets = exercise.Sets,
                CreatedDate = exercise.CreatedDate,
                CreatedBy = exercise.CreatedBy
            };

            return new SuccessDataResult<ExerciseDto>(addedExerciseDto, "Exercise added successfully.");
        }

        public async Task<CoreResult> UpdateExerciseAsync(int id, ExerciseUpdateDto exerciseDto)
        {
            var existingExercise = await _exerciseRepository.GetByIdAsync(id);
            if (existingExercise == null)
                return new ErrorResult("Exercise not found.");

            existingExercise.Name = exerciseDto.Name;
            existingExercise.Reps = exerciseDto.Reps;
            existingExercise.Sets = exerciseDto.Sets;
            existingExercise.UpdatedBy = _userHelper.GetCurrentUserName();
            existingExercise.UpdatedDate = DateTime.Now;

            var updatedId = await _exerciseRepository.UpdateAsync(existingExercise);
            if (updatedId == 0)
                return new ErrorResult("Failed to update exercise.");

            return new SuccessResult("Exercise updated successfully.");
        }

        public async Task<CoreResult> DeleteExerciseAsync(int id)
        {
            var existingExercise = await _exerciseRepository.GetByIdAsync(id);
            if (existingExercise == null)
                return new ErrorResult("Exercise not found.");

            await _exerciseRepository.DeleteAsync(id);
            return new SuccessResult("Exercise deleted successfully.");
        }
    }
}
