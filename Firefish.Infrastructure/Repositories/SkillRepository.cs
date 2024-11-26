using Firefish.Core.Contracts.Repositories;
using Firefish.Core.Entities;
using Firefish.Infrastructure.Helpers;
using Microsoft.Data.SqlClient;

namespace Firefish.Infrastructure.Repositories;

public class SkillRepository : ISkillRepository
{
    private const string SkillTableName = nameof(Skill);
    private const string CandidateSkillTableName = nameof(CandidateSkill);

    /// <summary>
    ///     Retrieves all skills associated with a specific candidate.
    /// </summary>
    /// <param name="candidateId">The ID of the candidate.</param>
    /// <returns>A collection of skills associated with the candidate.</returns>
    public async Task<IEnumerable<CandidateSkill>> GetSkillsByCandidateIdAsync(int candidateId)
    {
        var skills = new List<CandidateSkill>();

        try
        {
            await using var connection = new SqlConnection(SqlConnectionHelper.ConnectionString);
            await connection.OpenAsync();

            const string query = $"""
                                SELECT {CandidateSkillTableName}.{CandidateSkillFieldNames.Id}, {SkillTableName}.{CandidateSkillFieldNames.Id}, {CandidateSkillFieldNames.SkillName}
                                FROM {SkillTableName}
                                INNER JOIN {CandidateSkillTableName} ON {SkillTableName}.{CandidateSkillFieldNames.Id} = {CandidateSkillTableName}.{CandidateSkillFieldNames.SkillId}
                                WHERE {CandidateSkillTableName}.{CandidateSkillFieldNames.CandidateId} = @{CandidateSkillFieldNames.CandidateId}
                """;

            await using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@CandidateId", candidateId);

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
    public async Task<IEnumerable<CandidateSkill>> AddSkillByCandidateIdAsync(
        int candidateId,
        int skillId
    )
    {
        if (await SkillExistsForCandidateAsync(candidateId, skillId))
        {
            throw new InvalidOperationException(
                $"Skill with ID {skillId} already exists for candidate with ID {candidateId}"
            );
        }

        try
        {
            await using var connection = new SqlConnection(SqlConnectionHelper.ConnectionString);
            await connection.OpenAsync();

            const string query = $"""
                                INSERT INTO {CandidateSkillTableName} ({CandidateSkillFieldNames.CandidateId}, {CandidateSkillFieldNames.SkillId})
                                VALUES (@{CandidateSkillFieldNames.CandidateId}, @{CandidateSkillFieldNames.SkillId})
                """;

            await using var command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue(
                $"@{CandidateSkillFieldNames.CandidateId}",
                candidateId
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
    /// <param name="candidateId">The ID of the candidate.</param>
    /// <param name="skillId">The ID of the skill to remove.</param>
    /// <returns>The updated collection of skills for the candidate.</returns>
    public async Task<IEnumerable<CandidateSkill>> RemoveSkillByCandidateIdAsync(
        int candidateId,
        int skillId
    )
    {
        try
        {
            await using var connection = new SqlConnection(SqlConnectionHelper.ConnectionString);
            await connection.OpenAsync();

            const string query = $"""

                                DELETE FROM {CandidateSkillTableName}
                                WHERE {CandidateSkillFieldNames.CandidateId} = @{CandidateSkillFieldNames.CandidateId}
                                AND {CandidateSkillFieldNames.SkillId} = @{CandidateSkillFieldNames.SkillId}
                """;

            await using var command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue(
                $"@{CandidateSkillFieldNames.CandidateId}",
                candidateId
            );
            command.Parameters.AddWithValue($"@{CandidateSkillFieldNames.SkillId}", skillId);

            await command.ExecuteNonQueryAsync();

            return await GetSkillsByCandidateIdAsync(candidateId);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error removing skill from candidate: {ex.Message}", ex);
        }
    }

    /// <summary>
    ///     Checks if a skill with the given ID exists in the database.
    /// </summary>
    /// <param name="skillId">The ID of the skill to check.</param>
    /// <returns>True if the skill exists, false otherwise.</returns>
    public async Task<bool> SkillExistsAsync(int skillId)
    {
        try
        {
            await using var connection = new SqlConnection(SqlConnectionHelper.ConnectionString);
            await connection.OpenAsync();

            const string query = $"""
                SELECT COUNT(1) FROM {SkillTableName}
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
    ///     Checks if a skill with the given ID exists in the database.
    /// </summary>
    /// <param name="skillId">The ID of the skill to check.</param>
    /// <param name="candidateId">The ID of the candidate to check.</param>
    /// <returns>True if the skill exists for candidate, false otherwise.</returns>
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
