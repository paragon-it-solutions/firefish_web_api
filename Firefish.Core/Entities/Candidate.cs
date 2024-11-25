namespace Firefish.Core.Entities;

/// <summary>
///     Candidate class representing a candidate entity from the database.
/// </summary>
/// <remarks>
///     All non-nullable properties are marked with the "required" attribute and
///     all nullable properties are marked as nullable reference types with "?".
/// </remarks>
public class Candidate
{
    public required int Id { get; set; }
    public string? FirstName { get; set; }
    public string? Surname { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string? Address { get; set; }
    public string? Town { get; set; }
    public string? Country { get; set; }
    public string? PostCode { get; set; }
    public string? PhoneHome { get; set; }
    public string? PhoneMobile { get; set; }
    public string? PhoneWork { get; set; }
    public required DateTime CreatedDate { get; set; }
    public required DateTime UpdatedDate { get; set; }
}