using Firefish.Core.Contracts.Services;
using Firefish.Core.Models.Candidate.Requests;
using Firefish.Core.Models.Candidate.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Firefish.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CandidatesController(ICandidateService candidateService) : ControllerBase
{
    /// <summary>
    ///     Retrieves all candidates.
    /// </summary>
    /// <returns>
    ///     An ActionResult containing an IEnumerable of CandidateListItemResponseModel objects.
    ///     Returns an Ok result (200 OK) with the list of all candidates if successful.
    /// </returns>
    /// <response code="200">Returns the list of all candidates</response>
    /// <response code="500">If there was an internal server error during candidate retrieval</response>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CandidateListItemResponseModel>>> Get()
    {
        try
        {
            var allCandidates = await candidateService.GetAllCandidatesAsync();
            return Ok(allCandidates);
        }
        catch (Exception ex)
        {
            return Problem(
                title: "Internal Server Error",
                detail: $"An error occurred while getting candidates: {ex.Message}",
                statusCode: StatusCodes.Status500InternalServerError
            );
        }
    }

    /// <summary>
    ///     Retrieves a specific candidate by their ID.
    /// </summary>
    /// <param name="id">The unique identifier of the candidate to retrieve.</param>
    /// <returns>
    ///     An ActionResult containing the requested CandidateDetailsResponseModel object if found.
    ///     Returns an Ok result (200 OK) with the candidate data if successful.
    /// </returns>
    /// <response code="200">Returns the requested candidate</response>
    /// <response code="404">If the candidate is not found</response>
    /// <response code="500">If there was an internal server error during candidate retrieval</response>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<CandidateDetailsResponseModel>> Get(int id)
    {
        try
        {
            CandidateDetailsResponseModel responseModel =
                await candidateService.GetCandidateByIdAsync(id);
            return Ok(responseModel);
        }
        catch (KeyNotFoundException ex)
        {
            return Problem(
                title: "Candidate Not Found",
                detail: ex.Message,
                statusCode: StatusCodes.Status404NotFound
            );
        }
        catch (Exception ex)
        {
            return Problem(
                title: "Internal Server Error",
                detail: $"An error occurred while getting candidate: {ex.Message}",
                statusCode: StatusCodes.Status500InternalServerError
            );
        }
    }

    /// <summary>
    ///     Creates a new candidate based on the provided candidate data.
    /// </summary>
    /// <param name="requestModel">The candidate object containing the details of the candidate to be created.</param>
    /// <returns>
    ///     An ActionResult containing the created CandidateDetailsResponseModel object if successful.
    ///     Returns a CreatedAtAction result (201 Created) with the location of the newly created resource.
    /// </returns>
    /// <response code="201">Returns the newly created candidate</response>
    /// <response code="400">If the candidate data is null or invalid</response>
    /// <response code="500">If there was an internal server error during candidate creation</response>
    [HttpPost]
    public async Task<ActionResult<CandidateDetailsResponseModel>> Post(
        [FromBody] CandidateModifyRequestModel requestModel
    )
    {
        try
        {
            CandidateDetailsResponseModel responseModel =
                await candidateService.CreateCandidateAsync(requestModel);
            return CreatedAtAction(nameof(Get), new { id = responseModel.Id }, responseModel);
        }
        catch (ArgumentNullException ex)
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
                detail: $"An error occurred while creating candidate: {ex.Message}",
                statusCode: StatusCodes.Status500InternalServerError
            );
        }
    }

    /// <summary>
    ///     Updates an existing candidate with the provided data.
    /// </summary>
    /// <param name="id">The unique identifier of the candidate to update.</param>
    /// <param name="requestModel">The updated candidate data.</param>
    /// <returns>
    ///     An ActionResult containing the updated CandidateDetailsResponseModel object if successful.
    ///     Returns an Ok result (200 OK) with the updated candidate data.
    /// </returns>
    /// <response code="200">Returns the updated candidate</response>
    /// <response code="400">If the candidate data is null or invalid</response>
    /// <response code="404">If the candidate with the specified ID is not found</response>
    /// <response code="500">If there was an internal server error during candidate update</response>
    [HttpPut("{id:int}")]
    public async Task<ActionResult<CandidateDetailsResponseModel>> Put(
        int id,
        [FromBody] CandidateModifyRequestModel requestModel
    )
    {
        try
        {
            CandidateDetailsResponseModel responseModel =
                await candidateService.UpdateExistingCandidateAsync(id, requestModel);
            return Ok(responseModel);
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
                detail: $"An error occurred while updating candidate: {ex.Message}",
                statusCode: StatusCodes.Status500InternalServerError
            );
        }
    }
}
