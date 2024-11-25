using Firefish.Core.Entities;
using Firefish.Infrastructure.Helpers;
using Microsoft.Data.SqlClient;

namespace Firefish.Infrastructure.Repositories;

public class CandidateRepository
{
    private const string AllCandidateBaseQuery =
        "SELECT [ID], [FirstName], [Surname], [DateOfBirth], [Address1], [Town], [Country], [PostCode], [PhoneHome], [PhoneMobile], [PhoneWork], [CreatedDate], [UpdatedDate] FROM [Web_API_Task].[dbo].[Candidate]";

    public Candidate GetCandidateById(int candidateId)
    {
        using (var connection = new SqlConnection(SqlConnectionHelper.ConnectionString))
        {
            connection.Open();
            using (
                var command = new SqlCommand(
                    $"{AllCandidateBaseQuery} WHERE {CandidateFieldNames.Id} = @{CandidateFieldNames.Id}",
                    connection
                )
            )
            {
                command.Parameters.AddWithValue("@Id", candidateId);
                using (SqlDataReader? reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Candidate
                        {
                            Id = reader.GetInt32(reader.GetOrdinal(CandidateFieldNames.Id)),
                            FirstName = reader.IsDBNull(
                                reader.GetOrdinal(CandidateFieldNames.FirstName)
                            )
                                ? null
                                : reader.GetString(
                                    reader.GetOrdinal(CandidateFieldNames.FirstName)
                                ),
                            Surname = reader.IsDBNull(
                                reader.GetOrdinal(CandidateFieldNames.Surname)
                            )
                                ? null
                                : reader.GetString(reader.GetOrdinal(CandidateFieldNames.Surname)),
                            DateOfBirth = reader.GetDateTime(
                                reader.GetOrdinal(CandidateFieldNames.DateOfBirth)
                            ),
                            Address = reader.IsDBNull(
                                reader.GetOrdinal(CandidateFieldNames.Address)
                            )
                                ? null
                                : reader.GetString(reader.GetOrdinal(CandidateFieldNames.Address)),
                            Town = reader.IsDBNull(reader.GetOrdinal(CandidateFieldNames.Town))
                                ? null
                                : reader.GetString(reader.GetOrdinal(CandidateFieldNames.Town)),
                            Country = reader.IsDBNull(
                                reader.GetOrdinal(CandidateFieldNames.Country)
                            )
                                ? null
                                : reader.GetString(reader.GetOrdinal(CandidateFieldNames.Country)),
                            PostCode = reader.IsDBNull(
                                reader.GetOrdinal(CandidateFieldNames.PostCode)
                            )
                                ? null
                                : reader.GetString(reader.GetOrdinal(CandidateFieldNames.PostCode)),
                            PhoneHome = reader.IsDBNull(
                                reader.GetOrdinal(CandidateFieldNames.PhoneHome)
                            )
                                ? null
                                : reader.GetString(
                                    reader.GetOrdinal(CandidateFieldNames.PhoneHome)
                                ),
                            PhoneMobile = reader.IsDBNull(
                                reader.GetOrdinal(CandidateFieldNames.PhoneMobile)
                            )
                                ? null
                                : reader.GetString(
                                    reader.GetOrdinal(CandidateFieldNames.PhoneMobile)
                                ),
                            PhoneWork = reader.IsDBNull(
                                reader.GetOrdinal(CandidateFieldNames.PhoneWork)
                            )
                                ? null
                                : reader.GetString(
                                    reader.GetOrdinal(CandidateFieldNames.PhoneWork)
                                ),
                            CreatedDate = reader.GetDateTime(
                                reader.GetOrdinal(CandidateFieldNames.CreatedDate)
                            ),
                            UpdatedDate = reader.GetDateTime(
                                reader.GetOrdinal(CandidateFieldNames.UpdatedDate)
                            ),
                        };
                    }
                }
            }
        }

        return null;
    }

    public List<Candidate> GetAllCandidates()
    {
        var candidates = new List<Candidate>();
        using (var connection = new SqlConnection(SqlConnectionHelper.ConnectionString))
        {
            connection.Open();
            using (var command = new SqlCommand(AllCandidateBaseQuery, connection))
            {
                using (SqlDataReader? reader = command.ExecuteReader())
                {
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
                                    : reader.GetString(
                                        reader.GetOrdinal(CandidateFieldNames.FirstName)
                                    ),
                                Surname = reader.IsDBNull(
                                    reader.GetOrdinal(CandidateFieldNames.Surname)
                                )
                                    ? null
                                    : reader.GetString(
                                        reader.GetOrdinal(CandidateFieldNames.Surname)
                                    ),
                                DateOfBirth = reader.GetDateTime(
                                    reader.GetOrdinal(CandidateFieldNames.DateOfBirth)
                                ),
                                Address = reader.IsDBNull(
                                    reader.GetOrdinal(CandidateFieldNames.Address)
                                )
                                    ? null
                                    : reader.GetString(
                                        reader.GetOrdinal(CandidateFieldNames.Address)
                                    ),
                                Town = reader.IsDBNull(reader.GetOrdinal(CandidateFieldNames.Town))
                                    ? null
                                    : reader.GetString(reader.GetOrdinal(CandidateFieldNames.Town)),
                                Country = reader.IsDBNull(
                                    reader.GetOrdinal(CandidateFieldNames.Country)
                                )
                                    ? null
                                    : reader.GetString(
                                        reader.GetOrdinal(CandidateFieldNames.Country)
                                    ),
                                PostCode = reader.IsDBNull(
                                    reader.GetOrdinal(CandidateFieldNames.PostCode)
                                )
                                    ? null
                                    : reader.GetString(
                                        reader.GetOrdinal(CandidateFieldNames.PostCode)
                                    ),
                                PhoneHome = reader.IsDBNull(
                                    reader.GetOrdinal(CandidateFieldNames.PhoneHome)
                                )
                                    ? null
                                    : reader.GetString(
                                        reader.GetOrdinal(CandidateFieldNames.PhoneHome)
                                    ),
                                PhoneMobile = reader.IsDBNull(
                                    reader.GetOrdinal(CandidateFieldNames.PhoneMobile)
                                )
                                    ? null
                                    : reader.GetString(
                                        reader.GetOrdinal(CandidateFieldNames.PhoneMobile)
                                    ),
                                PhoneWork = reader.IsDBNull(
                                    reader.GetOrdinal(CandidateFieldNames.PhoneWork)
                                )
                                    ? null
                                    : reader.GetString(
                                        reader.GetOrdinal(CandidateFieldNames.PhoneWork)
                                    ),
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
            }
        }

        return candidates;
    }

    /// <summary>
    /// Updates an existing candidate's information in the database.
    /// </summary>
    /// <param name="candidate">The Candidate object containing updated information. All properties of this object will be used to update the corresponding record in the database.</param>
    /// <remarks>
    /// This method opens a new SQL connection, constructs an UPDATE query using the provided candidate's information,
    /// and executes it against the database. The UpdatedDate is automatically set to the current date and time.
    /// </remarks>
    public void UpdateExistingCandidate(Candidate candidate)
    {
        using (var connection = new SqlConnection(SqlConnectionHelper.ConnectionString))
        {
            connection.Open();
            using (
                var command = new SqlCommand(
                    $@"
            UPDATE Candidates 
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
            WHERE {CandidateFieldNames.Id} = @{CandidateFieldNames.Id}",
                    connection
                )
            )
            {
                command.Parameters.AddWithValue($"@{CandidateFieldNames.Id}", candidate.Id);
                command.Parameters.AddWithNullableValue($"@{CandidateFieldNames.FirstName}", candidate.FirstName);
                command.Parameters.AddWithNullableValue($"@{CandidateFieldNames.Surname}", candidate.Surname);
                command.Parameters.AddWithValue($"@{CandidateFieldNames.DateOfBirth}", candidate.DateOfBirth);
                command.Parameters.AddWithNullableValue($"@{CandidateFieldNames.Address}", candidate.Address);
                command.Parameters.AddWithNullableValue($"@{CandidateFieldNames.Town}", candidate.Town);
                command.Parameters.AddWithNullableValue($"@{CandidateFieldNames.Country}", candidate.Country);
                command.Parameters.AddWithNullableValue($"@{CandidateFieldNames.PostCode}", candidate.PostCode);
                command.Parameters.AddWithNullableValue($"@{CandidateFieldNames.PhoneHome}", candidate.PhoneHome);
                command.Parameters.AddWithNullableValue($"@{CandidateFieldNames.PhoneMobile}", candidate.PhoneMobile);
                command.Parameters.AddWithNullableValue($"@{CandidateFieldNames.PhoneWork}", candidate.PhoneWork);
                command.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);
    
                command.ExecuteNonQuery();
            }
        }
    }
}
