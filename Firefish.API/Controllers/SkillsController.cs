using Firefish.Core.Contracts.Services;
using Firefish.Core.Models.Skill.Requests;
using Firefish.Core.Models.Skill.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Firefish.API.Controllers;

/// <summary>
///     Controller responsible for managing skill-related operations in the API.
/// </summary>
/// <remarks>
///     This controller provides endpoints for retrieving, adding, and deleting skills,
///     both for individual candidates and across the entire system.
/// </remarks>
[Route("api/[controller]")]
[ApiController]
public class SkillsController(ISkillService skillService) : ControllerBase
{
    /// <summary>
    ///     Asynchronously retrieves all skills from the system.
    /// </summary>
    /// <returns>
    ///     An ActionResult containing a SkillResponseModel with all skills.
    ///     Returns an Ok result (200 OK) with the list of all skills if successful.
    /// </returns>
    /// <response code="200">Returns the list of all skills</response>
    /// <response code="404">If no skills are found</response>
    /// <response code="500">If there was an internal server error during skill retrieval</response>
    [HttpGet]
    public async Task<ActionResult<SkillResponseModel>> GetAllSkillsAsync()
    {
        try
        {
            var skills = await skillService.GetAllSkillsAsync();
            return Ok(skills);
        }
        catch (KeyNotFoundException ex)
        {
            return Problem(
                title: "Skill Not Found",
                detail: ex.Message,
                statusCode: StatusCodes.Status404NotFound
            );
        }
        catch (Exception ex)
        {
            return Problem(
                title: "Internal Server Error",
                detail: $"An error occurred while getting skills: {ex.Message}",
                statusCode: StatusCodes.Status500InternalServerError
            );
        }
    }

    /// <summary>
    ///     Retrieves all skills for a specific candidate.
    /// </summary>
    /// <param name="candidateId">The unique identifier of the candidate.</param>
    /// <returns>
    ///     An ActionResult containing an IEnumerable of CandidateSkillResponseModel objects.
    ///     Returns an Ok result (200 OK) with the list of skills for the candidate if successful.
    /// </returns>
    /// <response code="200">Returns the list of skills for the candidate</response>
    /// <response code="404">If the candidate is not found</response>
    /// <response code="500">If there was an internal server error during skill retrieval</response>
    [HttpGet("candidate/{candidateId:int}")]
    public async Task<ActionResult<IEnumerable<CandidateSkillResponseModel>>> Get(int candidateId)
    {
        try
        {
            var skills = await skillService.GetSkillsByCandidateIdAsync(candidateId);
            return Ok(skills);
        }
        catch (KeyNotFoundException ex)
        {
            return Problem(
                title: "Candidate Not Found",
                detail: ex.Message,
                statusCode: StatusCodes.Status404NotFound
            );
        }
        catch (ArgumentException ex)
        {
            return Problem(
                title: "Invalid Input",
                detail: ex.Message,
                statusCode: StatusCodes.Status400BadRequest
            );
        }
        catch (Exception ex)
        {
            return Problem(
                title: "Internal Server Error",
                detail: $"An error occurred while getting skills: {ex.Message}",
                statusCode: StatusCodes.Status500InternalServerError
            );
        }
    }

    /// <summary>
    ///     Adds a new skill to a candidate.
    /// </summary>
    /// <param name="candidateSkillModel">The skill object containing the details of the skill to be added.</param>
    /// <returns>
    ///     An ActionResult containing the created CandidateSkillResponseModel object if successful.
    ///     Returns a CreatedAtAction result (201 Created) with the location of the newly created resource.
    /// </returns>
    /// <response code="201">Returns the newly added skill</response>
    /// <response code="400">If the skill data is null or invalid</response>
    /// <response code="404">If the candidate is not found</response>
    /// <response code="500">If there was an internal server error during skill creation</response>
    [HttpPost]
    public async Task<ActionResult<CandidateSkillResponseModel>> Post(
        [FromBody] CandidateSkillRequestModel candidateSkillModel
    )
    {
        try
        {
            var addedSkill = await skillService.AddSkillByCandidateIdAsync(candidateSkillModel);
            return CreatedAtAction(
                nameof(Get),
                new { candidateId = addedSkill.First().SkillId },
                addedSkill
            );
        }
        catch (KeyNotFoundException ex)
        {
            return Problem(
                title: "Candidate Not Found",
                detail: ex.Message,
                statusCode: StatusCodes.Status404NotFound
            );
        }
        catch (InvalidOperationException ex)
        {
            return Problem(
                title: "Invalid Input",
                detail: ex.Message,
                statusCode: StatusCodes.Status400BadRequest
            );
        }
        catch (Exception ex)
        {
            return Problem(
                title: "Internal Server Error",
                detail: $"An error occurred while adding skill: {ex.Message}",
                statusCode: StatusCodes.Status500InternalServerError
            );
        }
    }

    /// <summary>
    ///     Deletes a specific skill from a candidate.
    /// </summary>
    /// <param name="candidateSkillId">The unique identifier of the candidate skill to delete.</param>
    /// <returns>
    ///     An ActionResult indicating the result of the deletion operation.
    ///     Returns a NoContent result (204 No Content) if the deletion is successful.
    /// </returns>
    /// <response code="204">If the skill was successfully deleted</response>
    /// <response code="404">If the candidate skill is not found</response>
    /// <response code="500">If there was an internal server error during skill deletion</response>
    [HttpDelete("{candidateSkillId:int}")]
    public async Task<ActionResult<IEnumerable<CandidateSkillResponseModel>>> Delete(
        int candidateSkillId
    )
    {
        try
        {
            var updatedSkills = await skillService.RemoveSkillByIdAsync(candidateSkillId);
            return Ok(updatedSkills);
        }
        catch (KeyNotFoundException ex)
        {
            return Problem(
                title: "Candidate Skill Not Found",
                detail: ex.Message,
                statusCode: StatusCodes.Status404NotFound
            );
        }
        catch (Exception ex)
        {
            return Problem(
                title: "Internal Server Error",
                detail: $"An error occurred while deleting skill: {ex.Message}",
                statusCode: StatusCodes.Status500InternalServerError
            );
        }
    }
}
