using Firefish.Core.Entities;

namespace Firefish.Infrastructure.Helpers;

/// <summary>
/// Provides constant string representations of field names for the Candidate entity.
/// </summary>
/// <remarks>
/// This static class contains string constants that match the property names of the Candidate class.
/// These constants can be used for consistent property name references across the application, and
/// to avoid "magic strings" in the code. 
/// </remarks>
public static class CandidateFieldNames
{
    public const string Id = nameof(Candidate.Id);
    public const string FirstName = nameof(Candidate.FirstName);
    public const string Surname = nameof(Candidate.Surname);
    public const string DateOfBirth = nameof(Candidate.DateOfBirth);
    public const string Address = nameof(Candidate.Address) + "1";
    public const string Town = nameof(Candidate.Town);
    public const string Country = nameof(Candidate.Country);
    public const string PostCode = nameof(Candidate.PostCode);
    public const string PhoneHome = nameof(Candidate.PhoneHome);
    public const string PhoneMobile = nameof(Candidate.PhoneMobile);
    public const string PhoneWork = nameof(Candidate.PhoneWork);
    public const string CreatedDate = nameof(Candidate.CreatedDate);
    public const string UpdatedDate = nameof(Candidate.UpdatedDate);
}