using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using TraffiLearn.Application.UseCases.Questions.DTO;
using TraffiLearn.Application.UseCases.Tickets.Commands.AddQuestionToTicket;
using TraffiLearn.Application.UseCases.Tickets.Commands.Create;
using TraffiLearn.Application.UseCases.Tickets.Commands.Delete;
using TraffiLearn.Application.UseCases.Tickets.Commands.RemoveQuestionFromTicket;
using TraffiLearn.Application.UseCases.Tickets.Commands.Update;
using TraffiLearn.Application.UseCases.Tickets.DTO;
using TraffiLearn.Application.UseCases.Tickets.Queries.GetAll;
using TraffiLearn.Application.UseCases.Tickets.Queries.GetById;
using TraffiLearn.Application.UseCases.Tickets.Queries.GetRandomTicketWithQuestions;
using TraffiLearn.Application.UseCases.Tickets.Queries.GetTicketQuestions;
using TraffiLearn.Infrastructure.Authentication;
using TraffiLearn.WebAPI.Extensions;
using TraffiLearn.WebAPI.Swagger;

namespace TraffiLearn.WebAPI.Controllers
{
    [HasPermission(Permission.AuthenticatedUser)]
    [Route("api/tickets")]
    [ApiController]
    public sealed class TicketsController : ControllerBase
    {
        private readonly ISender _sender;

        public TicketsController(ISender sender)
        {
            _sender = sender;
        }

        #region Queries


        /// <summary>
        /// Gets all tickets from the storage.
        /// </summary>
        /// <remarks>
        /// **Authentication Required:**<br />
        /// The user must be authenticated using a JWT token.
        /// </remarks>
        /// <response code="200">Successfully retrieved all tickets. Returns a list of tickets.</response>
        /// <response code="401">***Unauthorized.*** The user is not authenticated.</response>
        /// <response code="500">***Internal Server Error.*** An unexpected error occurred during the process.</response>
        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<TicketResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllTickets()
        {
            var queryResult = await _sender.Send(new GetAllTicketsQuery());

            return queryResult.IsSuccess ? Ok(queryResult.Value) : queryResult.ToProblemDetails();
        }

        /// <summary>
        /// Gets a random ticket with questions included.
        /// </summary>
        /// <remarks>
        /// **Authentication Required:**<br />
        /// The user must be authenticated using a JWT token.
        /// </remarks>
        /// <response code="200">Successfully retrieved ticket with questions. Returns a ticket.</response>
        /// <response code="401">***Unauthorized.*** The user is not authenticated.</response>
        /// <response code="500">***Internal Server Error.*** An unexpected error occurred during the process.</response>
        [HttpGet("random/with-questions")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(TicketResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetRandomTicketWithQuestions()
        {
            var queryResult = await _sender.Send(new GetRandomTicketWithQuestionsQuery());

            return queryResult.IsSuccess ? Ok(queryResult.Value) : queryResult.ToProblemDetails();
        }

        /// <summary>
        /// Gets a ticket with a specific ID.
        /// </summary>
        /// <remarks>
        /// **The request must include an ID of a ticket to get.**<br /><br /><br />
        /// ***Route parameters:***<br /><br />
        /// `TicketId` : Must be a valid GUID representing ID of a ticket.<br /><br /><br />
        /// **Authentication Required:**<br />
        /// The user must be authenticated using a JWT token.<br /><br />
        /// </remarks>
        /// <param name="ticketId">**The ID of a ticket to be retrieved**</param>
        /// <response code="200">Successfully retrieved the ticket with the provided ID. Returns the found ticket.</response>
        /// <response code="401">***Unauthorized.*** The user is not authenticated.</response>
        /// <response code="404">***Not found.*** No ticket exists with the provided ID.</response>
        /// <response code="500">***Internal Server Error.*** An unexpected error occurred during the process.</response>
        [HttpGet("{ticketId:guid}")]
        [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(TicketResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTicketById(
            [FromRoute] Guid ticketId)
        {
            var queryResult = await _sender.Send(new GetTicketByIdQuery(ticketId));

            return queryResult.IsSuccess ? Ok(queryResult.Value) : queryResult.ToProblemDetails();
        }

        /// <summary>
        /// Gets all questions associated with a ticket by a ticket ID.
        /// </summary>
        /// <remarks>
        /// **The request must include an ID of a ticket.**<br /><br /><br />
        /// ***Route parameters:***<br /><br />
        /// `TicketId` : Must be a valid GUID representing ID of a ticket.<br /><br /><br />
        /// **Authentication Required:**<br />
        /// The user must be authenticated using a JWT token.<br /><br />
        /// </remarks>
        /// <param name="ticketId">**The ID of a ticket used to find related questions.**</param>
        /// <response code="200">Successfully retrieved questions associated with the ticket with the provided ID. Returns a list of questions.</response>
        /// <response code="401">***Unauthorized.*** The user is not authenticated.</response>
        /// <response code="404">***Not found.*** No ticket exists with the provided ID.</response>
        /// <response code="500">***Internal Server Error.*** An unexpected error occurred during the process.</response>
        [HttpGet("{ticketId:guid}/questions")]
        [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(IEnumerable<QuestionResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTicketQuestions(
            [FromRoute] Guid ticketId)
        {
            var queryResult = await _sender.Send(new GetTicketQuestionsQuery(ticketId));

            return queryResult.IsSuccess ? Ok(queryResult.Value) : queryResult.ToProblemDetails();
        }


        #endregion

        #region Commands


        /// <summary>
        /// Creates a new ticket.
        /// </summary>
        /// <remarks>
        /// **If created a new ticket**, all the provided questions are going to contain the ticket (ID).<br /><br />
        /// ***Parameters:***<br /><br />
        /// `TicketNumber` : Number of the ticket. Must be greater than 0.<br /><br />
        /// `QuestionIds` : List of question IDs (represented in GUID) to be associated with the ticket. Must not be empty.<br /><br />
        /// **Authentication Required:**<br />
        /// The user must be authenticated using a JWT token. Only users with the `Owner` or `Admin` role can perform this action.<br /><br />
        /// </remarks>
        /// <param name="command">**The create ticket command.**</param>
        /// <response code="201">Successfully created a new ticket. Returns ID of a newly created ticket</response>
        /// <response code="400">***Bad request.*** The provided data is invalid or missing.</response>
        /// <response code="401">***Unauthorized.*** The user is not authenticated.</response>
        /// <response code="403">***Forbidden***. The user is not authorized to perform this action.</response>
        /// <response code="500">***Internal Server Error.*** An unexpected error occurred during the process.</response>
        [HttpPost]
        [HasPermission(Permission.ModifyApplicationData)]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateTicket(CreateTicketCommand command)
        {
            var commandResult = await _sender.Send(command);

            if (commandResult.IsSuccess)
            {
                return CreatedAtAction(
                    actionName: nameof(GetTicketById),
                    routeValues: new { ticketId = commandResult.Value },
                    value: commandResult.Value);
            }

            return commandResult.ToProblemDetails();
        }

        /// <summary>
        /// Updates an existing ticket.
        /// </summary>
        /// <remarks>
        /// **If updated a new ticket**, all the provided questions are going to contain the ticket (ID). All the questions which were removed from the ticket will not contain the ticket (ID).<br /><br />
        /// ***Parameters:***<br /><br />
        /// `TicketId` : ID of the ticket to be updated. Must be a valid GUID.<br /><br />
        /// `QuestionIds` : List of question IDs (represented in GUID) to be associated with the ticket. Must not be empty.<br /><br />
        /// **Authentication Required:**<br />
        /// The user must be authenticated using a JWT token. Only users with the `Owner` or `Admin` role can perform this action.<br /><br />
        /// </remarks>
        /// <param name="command">**The update ticket command.**</param>
        /// <response code="204">Successfully updated an existing ticket.</response>
        /// <response code="400">***Bad request.*** The provided data is invalid or missing.</response>
        /// <response code="401">***Unauthorized.*** The user is not authenticated.</response>
        /// <response code="403">***Forbidden***. The user is not authorized to perform this action.</response>
        /// <response code="404">***Not found.*** Ticket or questions with the id is not found.</response>
        /// <response code="500">***Internal Server Error.*** An unexpected error occurred during the process.</response>
        [HttpPut]
        [HasPermission(Permission.ModifyApplicationData)]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateTicket(UpdateTicketCommand command)
        {
            var commandResult = await _sender.Send(command);

            return commandResult.IsSuccess ? NoContent() : commandResult.ToProblemDetails();
        }

        /// <summary>
        /// Adds a question to a ticket.
        /// </summary>
        /// <remarks>
        /// **If question is added to the ticket**, the question will contain the ticket (ID).<br /><br />
        /// **The request must include the ID of the question and ticket.**<br /><br /><br />
        /// ***Route parameters:***<br /><br />
        /// `QuestionId` : Must be a valid GUID representing ID of the question.<br /><br />
        /// `TicketId` : Must be a valid GUID representing ID of the ticket.<br /><br />
        /// ***Authentication Required:***<br /><br />
        /// The user must be authenticated using a JWT token. Only users with the `Owner` or `Admin` role can perform this action.<br /><br />
        /// </remarks>
        /// <param name="questionId">**The ID of the question to be added to the ticket.**</param>
        /// <param name="ticketId">**The ID of the ticket to which the question will be added.**</param>
        /// <response code="204">Successfully added question to ticket.</response>
        /// <response code="400">***Bad request.*** The provided data is invalid or missing.</response>
        /// <response code="401">***Unauthorized.*** The user is not authenticated.</response>
        /// <response code="403">***Forbidden***. The user is not authorized to perform this action.</response>
        /// <response code="404">***Not found.*** Question or ticket with the provided id is not found.</response>
        /// <response code="500">***Internal Server Error.*** An unexpected error occurred during the process.</response>
        [HttpPut("{ticketId:guid}/add-question/{questionId:guid}")]
        [HasPermission(Permission.ModifyApplicationData)]
        [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddQuestionToTicket(
            [FromRoute] Guid questionId,
            [FromRoute] Guid ticketId)
        {
            var commandResult = await _sender.Send(new AddQuestionToTicketCommand(
                QuestionId: questionId,
                TicketId: ticketId));

            return commandResult.IsSuccess ? NoContent() : commandResult.ToProblemDetails();
        }

        /// <summary>
        /// Removes a question from a ticket.
        /// </summary>
        /// <remarks>
        /// **If question is removed from the ticket**, The ticket (ID) will be removed from question.<br /><br />
        /// **The request must include the ID of the question and ticket.**<br /><br /><br />
        /// ***Route parameters:***<br /><br />
        /// `QuestionId` : Must be a valid GUID representing ID of the question.<br /><br />
        /// `TicketId` : Must be a valid GUID representing ID of the ticket.<br /><br />
        /// ***Authentication Required:***<br /><br />
        /// The user must be authenticated using a JWT token. Only users with the `Owner` or `Admin` role can perform this action.<br /><br />
        /// </remarks>
        /// <param name="questionId">**The ID of the question to be removed from the ticket.**</param>
        /// <param name="ticketId">**The ID of the ticket from which the question will be removed.**</param>
        /// <response code="204">Successfully removed the question from the ticket.</response>
        /// <response code="400">***Bad request.*** The provided data is invalid or missing.</response>
        /// <response code="401">***Unauthorized.*** The user is not authenticated.</response>
        /// <response code="403">***Forbidden***. The user is not authorized to perform this action.</response>
        /// <response code="404">***Not found.*** Question or ticket with the provided id is not found.</response>
        /// <response code="500">***Internal Server Error.*** An unexpected error occurred during the process.</response>
        [HttpPut("{ticketId:guid}/remove-question/{questionId:guid}")]
        [HasPermission(Permission.ModifyApplicationData)]
        [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RemoveQuestionFromTicket(
            [FromRoute] Guid questionId,
            [FromRoute] Guid ticketId)
        {
            var commandResult = await _sender.Send(new RemoveQuestionFromTicketCommand(
                QuestionId: questionId,
                TicketId: ticketId));

            return commandResult.IsSuccess ? NoContent() : commandResult.ToProblemDetails();
        }

        /// <summary>
        /// Deletes a ticket using its ID.
        /// </summary>
        /// <remarks>
        /// **The request must include the ID of the ticket.**<br /><br /><br />
        /// ***Route parameters:***<br /><br />
        /// `TicketId` : Must be a valid GUID representing ID of the ticket.<br /><br /><br />
        /// **Authentication Required:**<br />
        /// The user must be authenticated using a JWT token. Only users with the `Owner` or `Admin` role can perform this action.<br /><br />
        /// </remarks>
        /// <param name="ticketId">**The ID of the ticket to be deleted.**</param>
        /// <response code="204">Successfully deleted the ticket.</response>
        /// <response code="401">***Unauthorized.*** The user is not authenticated.</response>
        /// <response code="403">***Forbidden***. The user is not authorized to perform this action.</response>
        /// <response code="404">***Not found.*** Question with the provided id is not found.</response>
        /// <response code="500">***Internal Server Error.*** An unexpected error occurred during the process.</response>
        [HttpDelete("{ticketId:guid}")]
        [HasPermission(Permission.ModifyApplicationData)]
        [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteTicket(
            [FromRoute] Guid ticketId)
        {
            var commandResult = await _sender.Send(new DeleteTicketCommand(ticketId));

            return commandResult.IsSuccess ? NoContent() : commandResult.ToProblemDetails();
        }


        #endregion
    }
}
