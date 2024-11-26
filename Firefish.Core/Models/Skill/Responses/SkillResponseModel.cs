namespace Firefish.Core.Models.Skill.Responses;

public class SkillResponseModel
{
    public int CandidateSkillId { get; set; }
    public required int SkillId { get; set; }
    public required string Name { get; set; }
}
