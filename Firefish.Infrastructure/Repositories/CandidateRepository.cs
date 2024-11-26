using Firefish.Core.Contracts.Repositories;
using Firefish.Core.Entities;
using Firefish.Infrastructure.Helpers;
using Microsoft.Data.SqlClient;

namespace Firefish.Infrastructure.Repositories;

public class CandidateRepository : ICandidateRepository
{
    // Base query for retrieving all candidates from the database - can be modified to include additional filters or sorting as needed.
    private const string AllCandidateBaseQuery = $"""
        SELECT [{CandidateFieldNames.Id}], [{CandidateFieldNames.FirstName}], [{CandidateFieldNames.Surname}], [{CandidateFieldNames.DateOfBirth}],
        [{CandidateFieldNames.Address}], [{CandidateFieldNames.Town}], [{CandidateFieldNames.Country}], [{CandidateFieldNames.PostCode}], 
        [{CandidateFieldNames.PhoneHome}], [{CandidateFieldNames.PhoneMobile}], [{CandidateFieldNames.PhoneWork}], [{CandidateFieldNames.CreatedDate}],
        [{CandidateFieldNames.UpdatedDate}] FROM [Candidate];
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
            connection.Open();

            await using var command = new SqlCommand(AllCandidateBaseQuery, connection);
            await using SqlDataReader? reader = await command.ExecuteReaderAsync();
            while (reader.Read())
            {
                candidates.Add(
                    new Candidate
                    {
                        Id = reader.GetInt32(reader.GetOrdinal(CandidateFieldNames.Id)),
                        FirstName = reader.IsDBNull(
                            reader.GetOrdinal(CandidateFieldNames.FirstName)
                        )
                            ? null
                            : reader.GetString(reader.GetOrdinal(CandidateFieldNames.FirstName)),
                        Surname = reader.IsDBNull(reader.GetOrdinal(CandidateFieldNames.Surname))
                            ? null
                            : reader.GetString(reader.GetOrdinal(CandidateFieldNames.Surname)),
                        DateOfBirth = reader.GetDateTime(
                            reader.GetOrdinal(CandidateFieldNames.DateOfBirth)
                        ),
                        Address = reader.IsDBNull(reader.GetOrdinal(CandidateFieldNames.Address))
                            ? null
                            : reader.GetString(reader.GetOrdinal(CandidateFieldNames.Address)),
                        Town = reader.IsDBNull(reader.GetOrdinal(CandidateFieldNames.Town))
                            ? null
                            : reader.GetString(reader.GetOrdinal(CandidateFieldNames.Town)),
                        Country = reader.IsDBNull(reader.GetOrdinal(CandidateFieldNames.Country))
                            ? null
                            : reader.GetString(reader.GetOrdinal(CandidateFieldNames.Country)),
                        PostCode = reader.IsDBNull(reader.GetOrdinal(CandidateFieldNames.PostCode))
                            ? null
                            : reader.GetString(reader.GetOrdinal(CandidateFieldNames.PostCode)),
                        PhoneHome = reader.IsDBNull(
                            reader.GetOrdinal(CandidateFieldNames.PhoneHome)
                        )
                            ? null
                            : reader.GetString(reader.GetOrdinal(CandidateFieldNames.PhoneHome)),
                        PhoneMobile = reader.IsDBNull(
                            reader.GetOrdinal(CandidateFieldNames.PhoneMobile)
                        )
                            ? null
                            : reader.GetString(reader.GetOrdinal(CandidateFieldNames.PhoneMobile)),
                        PhoneWork = reader.IsDBNull(
                            reader.GetOrdinal(CandidateFieldNames.PhoneWork)
                        )
                            ? null
                            : reader.GetString(reader.GetOrdinal(CandidateFieldNames.PhoneWork)),
                        CreatedDate = reader.GetDateTime(
                            reader.GetOrdinal(CandidateFieldNames.CreatedDate)
                        ),
                        UpdatedDate = reader.GetDateTime(
                            reader.GetOrdinal(CandidateFieldNames.UpdatedDate)
                        ),
                    }
                );
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
            connection.Open();

            await using var command = new SqlCommand(
                $"{AllCandidateBaseQuery} WHERE {CandidateFieldNames.Id} = @{CandidateFieldNames.Id}",
                connection
            );
            command.Parameters.AddWithValue("@Id", candidateId);

            await using SqlDataReader? reader = await command.ExecuteReaderAsync();
            if (reader.Read())
            {
                return new Candidate
                {
                    Id = reader.GetInt32(reader.GetOrdinal(CandidateFieldNames.Id)),
                    FirstName = reader.IsDBNull(reader.GetOrdinal(CandidateFieldNames.FirstName))
                        ? null
                        : reader.GetString(reader.GetOrdinal(CandidateFieldNames.FirstName)),
                    Surname = reader.IsDBNull(reader.GetOrdinal(CandidateFieldNames.Surname))
                        ? null
                        : reader.GetString(reader.GetOrdinal(CandidateFieldNames.Surname)),
                    DateOfBirth = reader.GetDateTime(
                        reader.GetOrdinal(CandidateFieldNames.DateOfBirth)
                    ),
                    Address = reader.IsDBNull(reader.GetOrdinal(CandidateFieldNames.Address))
                        ? null
                        : reader.GetString(reader.GetOrdinal(CandidateFieldNames.Address)),
                    Town = reader.IsDBNull(reader.GetOrdinal(CandidateFieldNames.Town))
                        ? null
                        : reader.GetString(reader.GetOrdinal(CandidateFieldNames.Town)),
                    Country = reader.IsDBNull(reader.GetOrdinal(CandidateFieldNames.Country))
                        ? null
                        : reader.GetString(reader.GetOrdinal(CandidateFieldNames.Country)),
                    PostCode = reader.IsDBNull(reader.GetOrdinal(CandidateFieldNames.PostCode))
                        ? null
                        : reader.GetString(reader.GetOrdinal(CandidateFieldNames.PostCode)),
                    PhoneHome = reader.IsDBNull(reader.GetOrdinal(CandidateFieldNames.PhoneHome))
                        ? null
                        : reader.GetString(reader.GetOrdinal(CandidateFieldNames.PhoneHome)),
                    PhoneMobile = reader.IsDBNull(
                        reader.GetOrdinal(CandidateFieldNames.PhoneMobile)
                    )
                        ? null
                        : reader.GetString(reader.GetOrdinal(CandidateFieldNames.PhoneMobile)),
                    PhoneWork = reader.IsDBNull(reader.GetOrdinal(CandidateFieldNames.PhoneWork))
                        ? null
                        : reader.GetString(reader.GetOrdinal(CandidateFieldNames.PhoneWork)),
                    CreatedDate = reader.GetDateTime(
                        reader.GetOrdinal(CandidateFieldNames.CreatedDate)
                    ),
                    UpdatedDate = reader.GetDateTime(
                        reader.GetOrdinal(CandidateFieldNames.UpdatedDate)
                    ),
                };
            }

            return null;
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
            connection.Open();

            await using var command = new SqlCommand(
                $"""
                INSERT INTO [Candidate] 
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

            candidate.Id = await GenerateIdentity();
            command.Parameters.AddWithValue($"@{CandidateFieldNames.Id}", candidate.Id);
            command.Parameters.AddWithValue($"@{CandidateFieldNames.CreatedDate}", DateTime.Now);
            ParameteriseValuesForCommand(command, candidate);

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
            connection.Open();

            await using var command = new SqlCommand(
                $"""
                            UPDATE [Candidate] 
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
    // Note: Id and CreatedDate are not included here as they vary across update and create operations
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

    // Private static method to generate Identity based on last highest ID in db
    private static async Task<int> GenerateIdentity()
    {
        try
        {
            await using var connection = new SqlConnection(SqlConnectionHelper.ConnectionString);
            connection.Open();

            await using var command = new SqlCommand(
                $"SELECT MAX({CandidateFieldNames.Id}) + 1 FROM [Candidate]",
                connection
            );

            int identity = (int)await command.ExecuteScalarAsync();
            return identity;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error generating identity: {ex.Message}", ex);
        }
    }
}
