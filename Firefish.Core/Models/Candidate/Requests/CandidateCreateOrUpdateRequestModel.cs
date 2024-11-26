namespace Firefish.Core.Models.Candidate.Requests;

public class CandidateCreateOrUpdateRequestModel
{
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
}