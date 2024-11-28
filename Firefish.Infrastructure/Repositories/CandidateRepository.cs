using Firefish.Core.Contracts.Repositories;
using Firefish.Core.Entities;
using Firefish.Infrastructure.Helpers;
using Microsoft.Data.SqlClient;

namespace Firefish.Infrastructure.Repositories;

// TODO: Implement logging and custom error handling
// TODO: Implement retry in case of failure for resiliency and fault tolerance.
// TODO: Implement caching if appropriate for the application's needs.'
/// <summary>
///     Repository class for managing Candidate entities in the database.
/// </summary>
/// <remarks>
///     This class provides methods for creating, retrieving, updating, and checking the existence of Candidate records.
///     It implements the ICandidateRepository interface and uses SQL Server for data persistence.
/// </remarks>
public class CandidateRepository : ICandidateRepository
{
    private const string CandidateTableName = nameof(Candidate);

    // Base query for retrieving all candidates from the database - can be modified to include additional filters or sorting as needed.
    private const string AllCandidateBaseQuery = $"""
        SELECT [{CandidateFieldNames.Id}], [{CandidateFieldNames.FirstName}], [{CandidateFieldNames.Surname}], [{CandidateFieldNames.DateOfBirth}],
        [{CandidateFieldNames.Address}], [{CandidateFieldNames.Town}], [{CandidateFieldNames.Country}], [{CandidateFieldNames.PostCode}], 
        [{CandidateFieldNames.PhoneHome}], [{CandidateFieldNames.PhoneMobile}], [{CandidateFieldNames.PhoneWork}], [{CandidateFieldNames.CreatedDate}],
        [{CandidateFieldNames.UpdatedDate}] FROM [{CandidateTableName}]
        """;

    /// <summary>
    ///     Retrieves all candidates from the database.
    /// </summary>
    /// <returns>
    ///     A Task that represents the asynchronous operation. The task result contains:
    ///     - A List of Candidate objects representing all candidates in the database.
    ///     - An empty list if no candidates are found.
    /// </returns>
    /// <remarks>
    ///     This method establishes a database connection, executes a SELECT query to retrieve all candidates,
    ///     and constructs a List of Candidate objects from the retrieved data. It handles null values for optional fields.
    /// </remarks>
    public async Task<IEnumerable<Candidate>> GetAllCandidatesAsync()
    {
        var candidates = new List<Candidate>();

        try
        {
            await using var connection = new SqlConnection(SqlConnectionHelper.ConnectionString);
            await connection.OpenAsync();

            // Ordered by created date in descending order so newest candidates are at the top.
            await using var command = new SqlCommand(AllCandidateBaseQuery, connection);
            await using SqlDataReader? reader = await command.ExecuteReaderAsync();
            while (reader.Read())
            {
                candidates.Add(MapCandidateFromReader(reader));
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Error retrieving candidate: {ex.Message}", ex);
        }

        return candidates;
    }

    /// <summary>
    ///     Retrieves a candidate from the database by their unique identifier.
    /// </summary>
    /// <param name="candidateId">The unique identifier of the candidate to retrieve.</param>
    /// <returns>
    ///     A Task that represents the asynchronous operation. The task result contains:
    ///     - A Candidate object if a matching record is found.
    ///     - Null if no matching record is found.
    /// </returns>
    /// <remarks>
    ///     This method establishes a database connection, executes a SELECT query with the provided candidateId,
    ///     and constructs a Candidate object from the retrieved data. It handles null values for optional fields.
    /// </remarks>
    public async Task<Candidate?> GetCandidateByIdAsync(int candidateId)
    {
        try
        {
            await using var connection = new SqlConnection(SqlConnectionHelper.ConnectionString);
            await connection.OpenAsync();

            await using var command = new SqlCommand(
                $"{AllCandidateBaseQuery} WHERE {CandidateFieldNames.Id} = @{CandidateFieldNames.Id}",
                connection
            );
            command.Parameters.AddWithValue($"@{CandidateFieldNames.Id}", candidateId);

            await using SqlDataReader? reader = await command.ExecuteReaderAsync();
            return reader.Read() ? MapCandidateFromReader(reader) : null;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error retrieving candidate: {ex.Message}", ex);
        }
    }

    /// <summary>
    ///     Asynchronously creates a new candidate in the database.
    /// </summary>
    /// <param name="candidate">
    ///     The Candidate object containing the information to be inserted into the database. The Id
    ///     property will be updated with the newly generated identity value.
    /// </param>
    /// <returns>
    ///     A Task that represents the asynchronous operation. The task result contains:
    ///     - The Candidate object with its Id property updated to reflect the newly assigned database identity.
    /// </returns>
    /// <remarks>
    ///     This method establishes a database connection, constructs an INSERT query using the provided candidate's
    ///     information,
    ///     executes it against the database, and retrieves the newly assigned identity value.
    ///     The CreatedDate and UpdatedDate are automatically set to the current date and time.
    /// </remarks>
    /// <exception cref="Exception">Thrown when an error occurs while creating the candidate record.</exception>
    public async Task<Candidate> CreateCandidateAsync(Candidate candidate)
    {
        try
        {
            await using var connection = new SqlConnection(SqlConnectionHelper.ConnectionString);
            await connection.OpenAsync();

            await using var command = new SqlCommand(
                $"""
                INSERT INTO [{CandidateTableName}] 
                ([{CandidateFieldNames.Id}],
                 [{CandidateFieldNames.FirstName}], 
                 [{CandidateFieldNames.Surname}], 
                 [{CandidateFieldNames.DateOfBirth}], 
                 [{CandidateFieldNames.Address}], 
                 [{CandidateFieldNames.Town}], 
                 [{CandidateFieldNames.Country}], 
                 [{CandidateFieldNames.PostCode}], 
                 [{CandidateFieldNames.PhoneHome}], 
                 [{CandidateFieldNames.PhoneMobile}], 
                 [{CandidateFieldNames.PhoneWork}], 
                 [{CandidateFieldNames.CreatedDate}],
                 [{CandidateFieldNames.UpdatedDate}])
                VALUES
                (@{CandidateFieldNames.Id},
                 @{CandidateFieldNames.FirstName}, 
                 @{CandidateFieldNames.Surname}, 
                 @{CandidateFieldNames.DateOfBirth}, 
                 @{CandidateFieldNames.Address}, 
                 @{CandidateFieldNames.Town}, 
                 @{CandidateFieldNames.Country}, 
                 @{CandidateFieldNames.PostCode}, 
                 @{CandidateFieldNames.PhoneHome}, 
                 @{CandidateFieldNames.PhoneMobile}, 
                 @{CandidateFieldNames.PhoneWork}, 
                 @{CandidateFieldNames.CreatedDate},
                 @{CandidateFieldNames.UpdatedDate});
                """,
                connection
            );

            candidate.Id = await SqlIdentityHelper.GenerateIdentityAsync(CandidateTableName);
            command.Parameters.AddWithValue($"@{CandidateFieldNames.Id}", candidate.Id);
            command.Parameters.AddWithValue($"@{CandidateFieldNames.CreatedDate}", DateTime.Now);
            ParameteriseValuesForCommand(command, candidate);

            await command.ExecuteNonQueryAsync();

            return candidate;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error inserting candidate: {ex.Message}", ex);
        }
    }

    /// <summary>
    ///     Updates an existing candidate's information in the database.
    /// </summary>
    /// <param name="candidate">
    ///     The Candidate object containing updated information. All properties of this object will be used
    ///     to update the corresponding record in the database.
    /// </param>
    /// <remarks>
    ///     This method opens a new SQL connection, constructs an UPDATE query using the provided candidate's information,
    ///     and executes it against the database. The UpdatedDate is automatically set to the current date and time.
    /// </remarks>
    /// <exception cref="Exception">Thrown when an error occurs while retrieving the candidate information.</exception>
    public async Task<Candidate> UpdateExistingCandidateAsync(Candidate candidate)
    {
        try
        {
            await using var connection = new SqlConnection(SqlConnectionHelper.ConnectionString);
            await connection.OpenAsync();

            await using var command = new SqlCommand(
                $"""
                            UPDATE [{CandidateTableName}] 
                            SET {CandidateFieldNames.FirstName} = @{CandidateFieldNames.FirstName}, 
                                {CandidateFieldNames.Surname} = @{CandidateFieldNames.Surname}, 
                                {CandidateFieldNames.DateOfBirth} = @{CandidateFieldNames.DateOfBirth}, 
                                {CandidateFieldNames.Address} = @{CandidateFieldNames.Address}, 
                                {CandidateFieldNames.Town} = @{CandidateFieldNames.Town}, 
                                {CandidateFieldNames.Country} = @{CandidateFieldNames.Country}, 
                                {CandidateFieldNames.PostCode} = @{CandidateFieldNames.PostCode}, 
                                {CandidateFieldNames.PhoneHome} = @{CandidateFieldNames.PhoneHome}, 
                                {CandidateFieldNames.PhoneMobile} = @{CandidateFieldNames.PhoneMobile}, 
                                {CandidateFieldNames.PhoneWork} = @{CandidateFieldNames.PhoneWork}, 
                                {CandidateFieldNames.UpdatedDate} = @{CandidateFieldNames.UpdatedDate} 
                            WHERE {CandidateFieldNames.Id} = @{CandidateFieldNames.Id}
                """,
                connection
            );

            // Parameterise the command with candidate's values'
            command.Parameters.AddWithValue($"@{CandidateFieldNames.Id}", candidate.Id);
            ParameteriseValuesForCommand(command, candidate);

            await command.ExecuteNonQueryAsync();

            return candidate;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error retrieving candidate: {ex.Message}", ex);
        }
    }

    /// <summary>
    ///     Asynchronously checks if a candidate with the specified ID exists in the database.
    /// </summary>
    /// <param name="candidateId">The unique identifier of the candidate to check for existence.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains a boolean value:
    ///     - true if a candidate with the specified ID exists;
    ///     - false if no candidate is found with the given ID.
    /// </returns>
    /// <exception cref="Exception">Thrown when an error occurs while retrieving the candidate information.</exception>
    public async Task<bool> CandidateExistsAsync(int candidateId)
    {
        try
        {
            Candidate? candidate = await GetCandidateByIdAsync(candidateId);
            return candidate != null;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error retrieving candidate: {ex.Message}", ex);
        }
    }

    // Private static method to parameterise SQL command with candidate's value's - void as parameters collection will be modified on original reference object
    // Note: Id and CreatedDate are not included here as they vary across implementation of update and create operations
    private static void ParameteriseValuesForCommand(SqlCommand command, Candidate candidate)
    {
        command.Parameters.AddWithNullableValue(
            $"@{CandidateFieldNames.FirstName}",
            candidate.FirstName
        );
        command.Parameters.AddWithNullableValue(
            $"@{CandidateFieldNames.Surname}",
            candidate.Surname
        );
        command.Parameters.AddWithValue(
            $"@{CandidateFieldNames.DateOfBirth}",
            candidate.DateOfBirth
        );
        command.Parameters.AddWithNullableValue(
            $"@{CandidateFieldNames.Address}",
            candidate.Address
        );
        command.Parameters.AddWithNullableValue($"@{CandidateFieldNames.Town}", candidate.Town);
        command.Parameters.AddWithNullableValue(
            $"@{CandidateFieldNames.Country}",
            candidate.Country
        );
        command.Parameters.AddWithNullableValue(
            $"@{CandidateFieldNames.PostCode}",
            candidate.PostCode
        );
        command.Parameters.AddWithNullableValue(
            $"@{CandidateFieldNames.PhoneHome}",
            candidate.PhoneHome
        );
        command.Parameters.AddWithNullableValue(
            $"@{CandidateFieldNames.PhoneMobile}",
            candidate.PhoneMobile
        );
        command.Parameters.AddWithNullableValue(
            $"@{CandidateFieldNames.PhoneWork}",
            candidate.PhoneWork
        );
        command.Parameters.AddWithValue($"@{CandidateFieldNames.UpdatedDate}", DateTime.Now);
    }

    // Maps candidate from reader

    private Candidate MapCandidateFromReader(SqlDataReader reader)
    {
        return new Candidate
        {
            Id = reader.GetInt32(reader.GetOrdinal(CandidateFieldNames.Id)),
            FirstName = reader.GetNullableString(CandidateFieldNames.FirstName),
            Surname = reader.GetNullableString(CandidateFieldNames.Surname),
            DateOfBirth = reader.GetDateTime(reader.GetOrdinal(CandidateFieldNames.DateOfBirth)),
            Address = reader.GetNullableString(CandidateFieldNames.Address),
            Town = reader.GetNullableString(CandidateFieldNames.Town),
            Country = reader.GetNullableString(CandidateFieldNames.Country),
            PostCode = reader.GetNullableString(CandidateFieldNames.PostCode),
            PhoneHome = reader.GetNullableString(CandidateFieldNames.PhoneHome),
            PhoneMobile = reader.GetNullableString(CandidateFieldNames.PhoneMobile),
            PhoneWork = reader.GetNullableString(CandidateFieldNames.PhoneWork),
            CreatedDate = reader.GetDateTime(reader.GetOrdinal(CandidateFieldNames.CreatedDate)),
            UpdatedDate = reader.GetDateTime(reader.GetOrdinal(CandidateFieldNames.UpdatedDate)),
        };
    }
}
