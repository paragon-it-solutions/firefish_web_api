using Firefish.Core.Contracts.Repositories;
using Firefish.Core.Entities;
using Firefish.Infrastructure.Helpers;
using Microsoft.Data.SqlClient;

namespace Firefish.Infrastructure.Repositories;

/// <summary>
///     Repository class for managing skills and candidate-skill associations.
/// </summary>
public class SkillRepository : ISkillRepository
{
    private const string SkillTableName = nameof(Skill);
    private const string CandidateSkillTableName = nameof(CandidateSkill);

    /// <summary>
    ///     Retrieves all skills associated with a specific candidate.
    /// </summary>
    /// <param name="candidateId">The ID of the candidate.</param>
    /// <returns>A collection of skills associated with the candidate.</returns>
    /// <exception cref="Exception">Thrown when an error occurs while retrieving skills.</exception>
    public async Task<IEnumerable<CandidateSkill>> GetSkillsByCandidateIdAsync(int candidateId)
    {
        var skills = new List<CandidateSkill>();

        try
        {
            await using var connection = new SqlConnection(SqlConnectionHelper.ConnectionString);
            await connection.OpenAsync();

            // Query to join the skill and candidate-skill tables based on the candidate ID - note alias SkillName given to Skill.Name
            const string query = $"""
                                SELECT {CandidateSkillTableName}.{CandidateSkillFieldNames.Id}, {CandidateSkillTableName}.{CandidateSkillFieldNames.SkillId}, {SkillTableName}.Name as {CandidateSkillFieldNames.SkillName}
                                FROM {SkillTableName}
                                INNER JOIN {CandidateSkillTableName} ON {SkillTableName}.{CandidateSkillFieldNames.Id} = {CandidateSkillTableName}.{CandidateSkillFieldNames.SkillId}
                                WHERE {CandidateSkillTableName}.{CandidateSkillFieldNames.CandidateId} = @{CandidateSkillFieldNames.CandidateId}
                """;

            await using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue(
                $"@{CandidateSkillFieldNames.CandidateId}",
                candidateId
            );

            await using SqlDataReader? reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                skills.Add(
                    new CandidateSkill
                    {
                        Id = reader.GetInt32(reader.GetOrdinal(CandidateSkillFieldNames.Id)),
                        CandidateId = candidateId,
                        SkillId = reader.GetInt32(
                            reader.GetOrdinal(CandidateSkillFieldNames.SkillId)
                        ),
                        SkillName = reader.GetString(
                            reader.GetOrdinal(CandidateSkillFieldNames.SkillName)
                        ),
                    }
                );
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Error retrieving skills for candidate: {ex.Message}", ex);
        }

        return skills;
    }

    /// <summary>
    ///     Adds a skill to a candidate's profile.
    /// </summary>
    /// <param name="candidateId">The ID of the candidate.</param>
    /// <param name="skillId">The ID of the skill to add.</param>
    /// <returns>The updated collection of skills for the candidate.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the skill already exists for the candidate.</exception>
    /// <exception cref="Exception">Thrown when an error occurs while adding the skill.</exception>
    public async Task<IEnumerable<CandidateSkill>> AddSkillByCandidateIdAsync(
        int candidateId,
        int skillId
    )
    {
        try
        {
            if (await SkillExistsForCandidateAsync(skillId, candidateId))
            {
                throw new InvalidOperationException(
                    $"Skill with ID {skillId} already exists for candidate with ID {candidateId}"
                );
            }
            await using var connection = new SqlConnection(SqlConnectionHelper.ConnectionString);
            await connection.OpenAsync();

            const string query = $"""
                                INSERT INTO {CandidateSkillTableName} ({CandidateSkillFieldNames.Id}, {CandidateSkillFieldNames.CandidateId}, 
                                {CandidateSkillFieldNames.CreatedDate}, {CandidateSkillFieldNames.UpdatedDate}, {CandidateSkillFieldNames.SkillId})
                                VALUES (@{CandidateSkillFieldNames.Id}, @{CandidateSkillFieldNames.CandidateId}, 
                                @{CandidateSkillFieldNames.CreatedDate}, @{CandidateSkillFieldNames.UpdatedDate}, @{CandidateSkillFieldNames.SkillId})
                """;

            await using var command = new SqlCommand(query, connection);

            int candidateSkillId = await SqlIdentityHelper.GenerateIdentityAsync(
                CandidateSkillTableName
            );
            command.Parameters.AddWithValue($"@{CandidateSkillFieldNames.Id}", candidateSkillId);
            command.Parameters.AddWithValue(
                $"@{CandidateSkillFieldNames.CandidateId}",
                candidateId
            );
            command.Parameters.AddWithValue(
                $"@{CandidateSkillFieldNames.CreatedDate}",
                DateTime.Now
            );
            command.Parameters.AddWithValue(
                $"@{CandidateSkillFieldNames.UpdatedDate}",
                DateTime.Now
            );

            command.Parameters.AddWithValue($"@{CandidateSkillFieldNames.SkillId}", skillId);

            await command.ExecuteNonQueryAsync();

            return await GetSkillsByCandidateIdAsync(candidateId);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error adding skill to candidate: {ex.Message}", ex);
        }
    }

    /// <summary>
    ///     Removes a skill from a candidate's profile.
    /// </summary>
    /// <param name="candidateSkillId">The ID of the candidate-skill association to remove.</param>
    /// <returns>The updated collection of skills for the candidate.</returns>
    /// <exception cref="Exception">Thrown when an error occurs while removing the skill.</exception>
    public async Task<IEnumerable<CandidateSkill>> RemoveSkillByIdAsync(int candidateSkillId)
    {
        try
        {
            if (candidateSkillId < 0)
            {
                throw new ArgumentException("CandidateSkill ID must be a non-negative integer");
            }
            if (!await CandidateSkillExists(candidateSkillId))
            {
                throw new KeyNotFoundException(
                    $"CandidateSKill with ID {candidateSkillId} not found"
                );
            }

            await using var connection = new SqlConnection(SqlConnectionHelper.ConnectionString);
            await connection.OpenAsync();

            // Query deletes record but also outputs the deleted candidate ID for use in getting updated skills list
            const string query = $"""
                DECLARE @DeletedCandidateId TABLE (CandidateId INT);

                DELETE FROM {CandidateSkillTableName}
                OUTPUT DELETED.{CandidateSkillFieldNames.CandidateId} INTO @DeletedCandidateId
                WHERE {CandidateSkillFieldNames.Id} = @{CandidateSkillFieldNames.Id};

                SELECT CandidateId FROM @DeletedCandidateId;
                """;

            await using var command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue($"@{CandidateSkillFieldNames.Id}", candidateSkillId);

            int candidateId = (int)await command.ExecuteScalarAsync();

            return await GetSkillsByCandidateIdAsync(candidateId);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error removing skill from candidate: {ex.Message}", ex);
        }
    }

    /// <summary>
    ///     Checks if a CandidateSkill with the given ID exists in the database.
    /// </summary>
    /// <param name="skillId">The ID of the skill to check.</param>
    /// <returns>True if the CandidateSkill exists, false otherwise.</returns>
    /// <exception cref="Exception">Thrown when an error occurs while checking skill existence.</exception>
    public async Task<bool> CandidateSkillExists(int skillId)
    {
        try
        {
            await using var connection = new SqlConnection(SqlConnectionHelper.ConnectionString);
            await connection.OpenAsync();

            const string query = $"""
                SELECT COUNT(1) FROM {CandidateSkillTableName}
                WHERE {CandidateSkillFieldNames.Id} = @{CandidateSkillFieldNames.Id}
                """;

            await using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue($"@{CandidateSkillFieldNames.Id}", skillId);

            object? result = await command.ExecuteScalarAsync();
            return Convert.ToInt32(result) > 0;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error checking skill existence: {ex.Message}", ex);
        }
    }

    /// <summary>
    ///     Checks if a skill with the given ID exists for a specific candidate.
    /// </summary>
    /// <param name="skillId">The ID of the skill to check.</param>
    /// <param name="candidateId">The ID of the candidate to check.</param>
    /// <returns>True if the skill exists for the candidate, false otherwise.</returns>
    /// <exception cref="Exception">Thrown when an error occurs while checking skill existence for the candidate.</exception>
    public async Task<bool> SkillExistsForCandidateAsync(int skillId, int candidateId)
    {
        try
        {
            await using var connection = new SqlConnection(SqlConnectionHelper.ConnectionString);
            await connection.OpenAsync();

            const string query = $"""
                                SELECT COUNT(1) FROM {CandidateSkillTableName}
                                WHERE {CandidateSkillFieldNames.SkillId} = @{CandidateSkillFieldNames.SkillId}
                                AND {CandidateSkillFieldNames.CandidateId} = @{CandidateSkillFieldNames.CandidateId}
                """;

            await using var command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue($"@{CandidateSkillFieldNames.SkillId}", skillId);
            command.Parameters.AddWithValue(
                $"@{CandidateSkillFieldNames.CandidateId}",
                candidateId
            );

            object? result = await command.ExecuteScalarAsync();
            return Convert.ToInt32(result) > 0;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error checking skill for candidate: {ex.Message}", ex);
        }
    }
}
