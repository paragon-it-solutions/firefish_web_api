namespace Firefish.Core.Models.Skill.Responses;

public class CandidateSkillResponseModel
{
    public int CandidateSkillId { get; set; }
    public required int SkillId { get; set; }
    public required string Name { get; set; }
}
