using Dapper;
using MySql.Data.MySqlClient;
using System.Data;
using WorkoutKompanion.Core.Entities;
using WorkoutKompanion.Core.Interfaces;
using WorkoutKompanion.Core.Utilities.Helpers;

namespace WorkoutKompanion.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly MySqlConnection _connection;
        private readonly IUserHelper _userHelper;

        public GenericRepository(MySqlConnection connection, IUserHelper userHelper)
        {
            _connection = connection;
            _userHelper = userHelper;
        }

        public async Task<T> GetByIdAsync(int id)
        {
            string procedureName = $"sp_Get{typeof(T).Name}ById";
            var parameters = new DynamicParameters();
            parameters.Add("p_Id", id, DbType.Int32);

            return await _connection.QueryFirstOrDefaultAsync<T>(
                procedureName,
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<IEnumerable<Workout>> GetFilteredWorkoutsWithPaginationAsync(int? duration, string difficulty, string region, int pageNumber, int pageSize)
        {
            var parameters = new DynamicParameters();
            parameters.Add("p_Duration", duration, DbType.Int32);
            parameters.Add("p_Difficulty", difficulty, DbType.String);
            parameters.Add("p_Region", region, DbType.String);
            parameters.Add("p_PageNumber", pageNumber, DbType.Int32);
            parameters.Add("p_PageSize", pageSize, DbType.Int32);

            return await _connection.QueryAsync<Workout>(
                "sp_GetFilteredWorkoutsWithPagination",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<IEnumerable<Exercise>> GetExercisesWithPaginationAsync(int workoutId, int pageNumber, int pageSize)
        {
            var parameters = new DynamicParameters();
            parameters.Add("p_WorkoutId", workoutId, DbType.Int32);
            parameters.Add("p_PageNumber", pageNumber, DbType.Int32);
            parameters.Add("p_PageSize", pageSize, DbType.Int32);

            return await _connection.QueryAsync<Exercise>(
                "sp_GetExercisesWithPagination",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> AddAsync(T entity)
        {
            string procedureName = $"sp_Insert{typeof(T).Name}";
            var createdBy = _userHelper.GetCurrentUserName();

            var parameters = new DynamicParameters();

            if (entity is Workout workout)
            {
                parameters.Add("p_Name", workout.Name, DbType.String);
                parameters.Add("p_Duration", workout.Duration, DbType.Int32);
                parameters.Add("p_Difficulty", workout.Difficulty, DbType.String);
                parameters.Add("p_Region", workout.Region, DbType.String);
            }
            else if (entity is Exercise exercise)
            {
                parameters.Add("p_WorkoutId", exercise.WorkoutId, DbType.Int32);
                parameters.Add("p_Name", exercise.Name, DbType.String);
                parameters.Add("p_Reps", exercise.Reps, DbType.Int32);
                parameters.Add("p_Sets", exercise.Sets, DbType.Int32);
            }

            parameters.Add("p_CreatedBy", createdBy);

            var insertedId = await _connection.QuerySingleOrDefaultAsync<int>(
                procedureName,
                parameters,
                commandType: CommandType.StoredProcedure
            );

            if (insertedId == 0)
            {
                throw new Exception("Insert failed, no ID returned.");
            }

            return insertedId;
        }

        public async Task<int> UpdateAsync(T entity)
        {
            string procedureName = $"sp_Update{typeof(T).Name}";
            var updatedBy = _userHelper.GetCurrentUserName();

            var parameters = new DynamicParameters();

            if (entity is Workout workout)
            {
                parameters.Add("p_Id", workout.Id, DbType.Int32);
                parameters.Add("p_Name", workout.Name, DbType.String);
                parameters.Add("p_Duration", workout.Duration, DbType.Int32);
                parameters.Add("p_Difficulty", workout.Difficulty, DbType.String);
                parameters.Add("p_Region", workout.Region, DbType.String);
            }
            else if (entity is Exercise exercise)
            {
                parameters.Add("p_Id", exercise.Id, DbType.Int32);
                parameters.Add("p_Name", exercise.Name, DbType.String);
                parameters.Add("p_Reps", exercise.Reps, DbType.Int32);
                parameters.Add("p_Sets", exercise.Sets, DbType.Int32);
            }

            parameters.Add("p_UpdatedBy", updatedBy);

            var updatedId = await _connection.QuerySingleOrDefaultAsync<int>(
                procedureName,
                parameters,
                commandType: CommandType.StoredProcedure
            );

            if (updatedId == 0)
            {
                throw new Exception("Update failed, no ID returned.");
            }

            return updatedId;
        }

        public async Task DeleteAsync(int id)
        {
            string procedureName = $"sp_Delete{typeof(T).Name}";

            var parameters = new DynamicParameters();
            parameters.Add("p_Id", id, DbType.Int32);
            var updatedBy = _userHelper.GetCurrentUserName();
            parameters.Add("p_UpdatedBy", updatedBy, DbType.String);

            await _connection.ExecuteAsync(procedureName, parameters, commandType: CommandType.StoredProcedure);
        }
    }
}
