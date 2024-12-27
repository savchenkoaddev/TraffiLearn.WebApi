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


        /// <include file='Documentation/TicketsControllerDocs.xml' path='doc/members/member[@name="M:GetAllTickets"]/*'/>
        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<TicketResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllTickets()
        {
            var queryResult = await _sender.Send(new GetAllTicketsQuery());

            return queryResult.IsSuccess ? Ok(queryResult.Value) : queryResult.ToProblemDetails();
        }

        /// <include file='Documentation/TicketsControllerDocs.xml' path='doc/members/member[@name="M:GetRandomTicketWithQuestions"]/*'/>
        [HttpGet("random/with-questions")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(TicketResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetRandomTicketWithQuestions()
        {
            var queryResult = await _sender.Send(new GetRandomTicketWithQuestionsQuery());

            return queryResult.IsSuccess ? Ok(queryResult.Value) : queryResult.ToProblemDetails();
        }

        /// <include file='Documentation/TicketsControllerDocs.xml' path='doc/members/member[@name="M:GetTicketById"]/*'/>
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

        /// <include file='Documentation/TicketsControllerDocs.xml' path='doc/members/member[@name="M:GetTicketQuestions"]/*'/>
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


        /// <include file='Documentation/TicketsControllerDocs.xml' path='doc/members/member[@name="M:CreateTicket"]/*'/>
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

        /// <include file='Documentation/TicketsControllerDocs.xml' path='doc/members/member[@name="M:UpdateTicket"]/*'/>
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

        /// <include file='Documentation/TicketsControllerDocs.xml' path='doc/members/member[@name="M:AddQuestionToTicket"]/*'/>
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

        /// <include file='Documentation/TicketsControllerDocs.xml' path='doc/members/member[@name="M:RemoveQuestionFromTicket"]/*'/>
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

        /// <include file='Documentation/TicketsControllerDocs.xml' path='doc/members/member[@name="M:DeleteTicket"]/*'/>
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
