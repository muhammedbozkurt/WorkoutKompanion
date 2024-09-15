using WorkoutKompanion.Core.DTOs;
using WorkoutKompanion.Core.Entities;
using WorkoutKompanion.Core.Interfaces;
using WorkoutKompanion.Core.Utilities.Helpers;
using WorkoutKompanion.Core.Utilities.Results;
using CoreResult = WorkoutKompanion.Core.Utilities.Results.IResult;

namespace WorkoutKompanionAPI.Services
{
    public class WorkoutService : IWorkoutService
    {
        private readonly IGenericRepository<Workout> _workoutRepository;
        private readonly IGenericRepository<Exercise> _exerciseRepository;
        private readonly IUserHelper _userHelper;

        public WorkoutService(IGenericRepository<Workout> workoutRepository, IGenericRepository<Exercise> exerciseRepository, IUserHelper userHelper)
        {
            _workoutRepository = workoutRepository;
            _exerciseRepository = exerciseRepository;
            _userHelper = userHelper;
        }

        public async Task<IDataResult<(Workout, IEnumerable<Exercise>)>> GetWorkoutWithExercisesAsync(int workoutId, int pageNumber, int pageSize)
        {
            var workout = await _workoutRepository.GetByIdAsync(workoutId);

            if (workout == null)
                return new ErrorDataResult<(Workout, IEnumerable<Exercise>)>("Workout not found");

            var exercises = await _exerciseRepository.GetExercisesWithPaginationAsync(workoutId, pageNumber, pageSize);

            return new SuccessDataResult<(Workout, IEnumerable<Exercise>)>((workout, exercises), "Workout and exercises with pagination retrieved successfully.");
        }

        public async Task<IDataResult<WorkoutDto>> GetWorkoutByIdAsync(int id)
        {
            var workout = await _workoutRepository.GetByIdAsync(id);
            if (workout == null)
                return new ErrorDataResult<WorkoutDto>("Workout not found");

            var workoutDto = new WorkoutDto
            {
                Id = workout.Id,
                Name = workout.Name,
                Duration = workout.Duration,
                Difficulty = workout.Difficulty,
                Region = workout.Region,
                CreatedDate = workout.CreatedDate,
                CreatedBy = workout.CreatedBy,
                UpdatedDate = workout.UpdatedDate,
                UpdatedBy = workout.UpdatedBy
            };

            return new SuccessDataResult<WorkoutDto>(workoutDto, "Workout retrieved successfully.");
        }

        public async Task<IDataResult<WorkoutDto>> AddWorkoutAsync(WorkoutCreateDto workoutDto)
        {
            var workout = new Workout
            {
                Name = workoutDto.Name,
                Duration = workoutDto.Duration,
                Difficulty = workoutDto.Difficulty,
                Region = workoutDto.Region,
                CreatedBy = _userHelper.GetCurrentUserName(),
                CreatedDate = DateTime.Now
            };

            var workoutId = await _workoutRepository.AddAsync(workout);

            if (workoutId == 0)
            {
                return new ErrorDataResult<WorkoutDto>("Failed to add workout.");
            }

            var addedWorkoutDto = new WorkoutDto
            {
                Id = workoutId,
                Name = workout.Name,
                Duration = workout.Duration,
                Difficulty = workout.Difficulty,
                Region = workout.Region,
                CreatedDate = workout.CreatedDate,
                CreatedBy = workout.CreatedBy
            };

            return new SuccessDataResult<WorkoutDto>(addedWorkoutDto, "Workout added successfully.");
        }

        public async Task<CoreResult> UpdateWorkoutAsync(int id, WorkoutUpdateDto workoutDto)
        {
            var existingWorkout = await _workoutRepository.GetByIdAsync(id);
            if (existingWorkout == null)
            {
                return new ErrorResult("Workout not found");
            }

            existingWorkout.Name = workoutDto.Name;
            existingWorkout.Duration = workoutDto.Duration;
            existingWorkout.Difficulty = workoutDto.Difficulty;
            existingWorkout.Region = workoutDto.Region;
            existingWorkout.UpdatedBy = _userHelper.GetCurrentUserName();
            existingWorkout.UpdatedDate = DateTime.Now;

            var updatedId = await _workoutRepository.UpdateAsync(existingWorkout);

            if (updatedId == 0)
            {
                return new ErrorResult("Failed to update workout.");
            }

            return new SuccessResult("Workout updated successfully.");
        }

        public async Task<CoreResult> DeleteWorkoutAsync(int id)
        {
            var existingWorkout = await _workoutRepository.GetByIdAsync(id);
            if (existingWorkout == null)
                return new ErrorResult("Workout not found");

            await _workoutRepository.DeleteAsync(id);
            return new SuccessResult("Workout deleted successfully.");
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
            {
                return new ErrorDataResult<ExerciseDto>("Failed to add exercise.");
            }

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

        public async Task<IDataResult<IEnumerable<WorkoutDto>>> GetFilteredWorkoutsWithPaginationAsync(int? duration, string difficulty, string region, int pageNumber, int pageSize)
        {
            var workouts = await _workoutRepository.GetFilteredWorkoutsWithPaginationAsync(duration, difficulty, region, pageNumber, pageSize);

            var workoutDtos = workouts.Select(w => new WorkoutDto
            {
                Id = w.Id,
                Name = w.Name,
                Duration = w.Duration,
                Difficulty = w.Difficulty,
                Region = w.Region,
                CreatedDate = w.CreatedDate,
                CreatedBy = w.CreatedBy,
                UpdatedDate = w.UpdatedDate,
                UpdatedBy = w.UpdatedBy
            }).ToList();

            return new SuccessDataResult<IEnumerable<WorkoutDto>>(workoutDtos, "Filtered workouts with pagination retrieved successfully.");
        }

        public async Task<CoreResult> UpdateExerciseAsync(int id, ExerciseUpdateDto exerciseDto)
        {
            var existingExercise = await _exerciseRepository.GetByIdAsync(id);
            if (existingExercise == null)
            {
                return new ErrorResult("Exercise not found");
            }

            existingExercise.Name = exerciseDto.Name;
            existingExercise.Reps = exerciseDto.Reps;
            existingExercise.Sets = exerciseDto.Sets;
            existingExercise.UpdatedBy = _userHelper.GetCurrentUserName();
            existingExercise.UpdatedDate = DateTime.Now;

            var updatedId = await _exerciseRepository.UpdateAsync(existingExercise);

            if (updatedId == 0)
            {
                return new ErrorResult("Failed to update exercise.");
            }

            return new SuccessResult("Exercise updated successfully.");
        }
    }
}
