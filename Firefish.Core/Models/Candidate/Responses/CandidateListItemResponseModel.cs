namespace Firefish.Core.Models.Candidate.Responses;

public class CandidateListItemResponseModel
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string? Town { get; set; }
    public string? Phone { get; set; }
}