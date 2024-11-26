using Firefish.Core.Contracts.Repositories;
using Firefish.Core.Entities;
using Firefish.Infrastructure.Helpers;
using Microsoft.Data.SqlClient;

namespace Firefish.Infrastructure.Repositories;

public class CandidateRepository : ICandidateRepository
{
    // Base query for retrieving all candidates from the database - can be modified to include additional filters or sorting as needed.
    private const string AllCandidateBaseQuery =
        "SELECT [ID], [FirstName], [Surname], [DateOfBirth], [Address1], [Town], [Country], [PostCode], [PhoneHome], [PhoneMobile], [PhoneWork], [CreatedDate], [UpdatedDate] FROM [Candidate]";

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
                }
            );
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
                PhoneMobile = reader.IsDBNull(reader.GetOrdinal(CandidateFieldNames.PhoneMobile))
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

    public async Task<Candidate> CreateCandidateAsync(Candidate candidate)
    {
        await using var connection = new SqlConnection(SqlConnectionHelper.ConnectionString);
        connection.Open();

        // "SELECT SCOPE_IDENTITY()" returns the identity value of the inserted row.
        await using var command = new SqlCommand(
            """
                        INSERT INTO [Candidate] 
                        ([{CandidateFieldNames.FirstName}], 
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
                        (@{CandidateFieldNames.FirstName}, 
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
                        SELECT CAST(SCOPE_IDENTITY() AS INT);
            """,
            connection
        );
        
        command.Parameters.AddWithValue($"@{CandidateFieldNames.CreatedDate}", DateTime.Now);
        ParameteriseValuesForCommand(command, candidate);
        ParameteriseValuesForCommand(command, candidate);

        // Sets ID to db identity column
        candidate.Id = (int)await command.ExecuteScalarAsync();

        return candidate;
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
    public async Task<Candidate> UpdateExistingCandidateAsync(Candidate candidate)
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

    public async Task<bool> CandidateExistsAsync(int candidateId)
    {
        Candidate? candidate = await GetCandidateByIdAsync(candidateId);
        return candidate != null;
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
}
