using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkoutKompanion.Core.DTOs;
using WorkoutKompanionAPI.Services;

namespace WorkoutKompanionAPI.Controller
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class WorkoutController : ControllerBase
    {
        private readonly IWorkoutService _workoutService;

        public WorkoutController(IWorkoutService workoutService)
        {
            _workoutService = workoutService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetWorkoutById(int id)
        {
            var result = await _workoutService.GetWorkoutByIdAsync(id);

            if (!result.Success)
                return NotFound(new { message = result.Message, success = false });

            return Ok(new { data = result.Data, message = result.Message, success = true });
        }

        [HttpGet("{id}/details")]
        public async Task<IActionResult> GetWorkoutWithExercises(
            int id,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10
        )
        {
            var result = await _workoutService.GetWorkoutWithExercisesAsync(id, pageNumber, pageSize);

            if (!result.Success)
                return BadRequest(result.Message);

            if (result.Data.Item1 == null)
                return NotFound("Workout not found");

            if (!result.Data.Item2.Any())
                return Ok(new { workout = result.Data.Item1, message = "No exercises found", success = true });

            return Ok(new { workout = result.Data.Item1, exercises = result.Data.Item2, message = result.Message, success = result.Success });
        }


        [HttpPost]
        public async Task<IActionResult> AddWorkout(WorkoutCreateDto workoutDto)
        {
            var result = await _workoutService.AddWorkoutAsync(workoutDto);
            return Ok(new { data = result.Data, message = result.Message, success = result.Success });
        }

        [HttpGet("filter")]
        public async Task<IActionResult> GetFilteredWorkouts(
            [FromQuery] int? duration = null,
            [FromQuery] string difficulty = null,
            [FromQuery] string region = null,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10
        )
        {
            var result = await _workoutService.GetFilteredWorkoutsWithPaginationAsync(duration, difficulty, region, pageNumber, pageSize);

            if (!result.Success)
                return BadRequest(new { message = result.Message, success = false });

            return Ok(new { data = result.Data, message = result.Message, success = true });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWorkout(int id, WorkoutUpdateDto workoutDto)
        {
            var result = await _workoutService.UpdateWorkoutAsync(id, workoutDto);
            return Ok(new { message = result.Message, success = result.Success });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWorkout(int id)
        {
            var result = await _workoutService.DeleteWorkoutAsync(id);
            return Ok(new { message = result.Message, success = result.Success });
        }
    }
}
