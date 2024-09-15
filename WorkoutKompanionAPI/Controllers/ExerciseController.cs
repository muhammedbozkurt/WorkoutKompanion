using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkoutKompanion.Core.DTOs;
using WorkoutKompanionAPI.Services;

namespace WorkoutKompanionAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ExerciseController : ControllerBase
    {
        private readonly IExerciseService _exerciseService;

        public ExerciseController(IExerciseService exerciseService)
        {
            _exerciseService = exerciseService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetExerciseById(int id)
        {
            var result = await _exerciseService.GetExerciseByIdAsync(id);
            if (!result.Success)
                return NotFound(result.Message);

            return Ok(new { data = result.Data, message = result.Message, success = result.Success });
        }

        [HttpPost]
        public async Task<IActionResult> AddExercise(ExerciseCreateDto exerciseDto)
        {
            var result = await _exerciseService.AddExerciseAsync(exerciseDto);
            if (!result.Success)
                return BadRequest(result.Message);

            return CreatedAtAction(nameof(GetExerciseById), new { id = result.Data.Id }, new { data = result.Data, message = result.Message, success = result.Success });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateExercise(int id, ExerciseUpdateDto exerciseDto)
        {
            var result = await _exerciseService.UpdateExerciseAsync(id, exerciseDto);
            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(new { message = result.Message, success = result.Success });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExercise(int id)
        {
            var result = await _exerciseService.DeleteExerciseAsync(id);
            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(new { message = result.Message, success = result.Success });
        }
    }
}
