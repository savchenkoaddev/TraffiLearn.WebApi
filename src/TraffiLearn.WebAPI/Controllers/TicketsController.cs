using MediatR;
using Microsoft.AspNetCore.Mvc;
using TraffiLearn.Application.Tickets.Commands.AddQuestionToTicket;
using TraffiLearn.Application.Tickets.Commands.Create;
using TraffiLearn.Application.Tickets.Commands.Delete;
using TraffiLearn.Application.Tickets.Commands.RemoveQuestionFromTicket;
using TraffiLearn.Application.Tickets.Commands.Update;
using TraffiLearn.Application.Tickets.Queries.GetAll;
using TraffiLearn.Application.Tickets.Queries.GetById;
using TraffiLearn.Application.Tickets.Queries.GetRandomTicketWithQuestions;
using TraffiLearn.Application.Tickets.Queries.GetTicketQuestions;
using TraffiLearn.Infrastructure.Authentication;
using TraffiLearn.WebAPI.Extensions;

namespace TraffiLearn.WebAPI.Controllers
{
    [HasPermission(Permission.AccessData)]
    [Route("api/tickets")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        private readonly ISender _sender;

        public TicketsController(ISender sender)
        {
            _sender = sender;
        }

        #region Queries


        [HttpGet]
        public async Task<IActionResult> GetAllTickets()
        {
            var queryResult = await _sender.Send(new GetAllTicketsQuery());

            return queryResult.IsSuccess ? Ok(queryResult.Value) : queryResult.ToProblemDetails();
        }

        [HttpGet("tickets/random/with-questions")]
        public async Task<IActionResult> GetRandomTicketWithQuestions()
        {
            var queryResult = await _sender.Send(new GetRandomTicketWithQuestionsQuery());

            return queryResult.IsSuccess ? Ok(queryResult.Value) : queryResult.ToProblemDetails();
        }

        [HttpGet("{ticketId:guid}")]
        public async Task<IActionResult> GetTicketById(
            [FromRoute] Guid ticketId)
        {
            var queryResult = await _sender.Send(new GetTicketByIdQuery(ticketId));

            return queryResult.IsSuccess ? Ok(queryResult.Value) : queryResult.ToProblemDetails();
        }

        [HttpGet("{ticketId:guid}/questions")]
        public async Task<IActionResult> GetTicketQuestions(
            [FromRoute] Guid ticketId)
        {
            var queryResult = await _sender.Send(new GetTicketQuestionsQuery(ticketId));

            return queryResult.IsSuccess ? Ok(queryResult.Value) : queryResult.ToProblemDetails();
        }


        #endregion

        #region Commands


        [HasPermission(Permission.ModifyData)]
        [HttpPost]
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

        [HasPermission(Permission.ModifyData)]
        [HttpPut]
        public async Task<IActionResult> UpdateTicket(UpdateTicketCommand command)
        {
            var commandResult = await _sender.Send(command);

            return commandResult.IsSuccess ? NoContent() : commandResult.ToProblemDetails();
        }

        [HasPermission(Permission.ModifyData)]
        [HttpPut("{ticketId:guid}/add-question/{questionId:guid}")]
        public async Task<IActionResult> AddQuestionToTicket(
            [FromRoute] Guid questionId,
            [FromRoute] Guid ticketId)
        {
            var commandResult = await _sender.Send(new AddQuestionToTicketCommand(
                QuestionId: questionId,
                TicketId: ticketId));

            return commandResult.IsSuccess ? NoContent() : commandResult.ToProblemDetails();
        }

        [HasPermission(Permission.ModifyData)]
        [HttpPut("{ticketId:guid}/remove-question/{questionId:guid}")]
        public async Task<IActionResult> RemoveQuestionFromTicket(
            [FromRoute] Guid questionId,
            [FromRoute] Guid ticketId)
        {
            var commandResult = await _sender.Send(new RemoveQuestionFromTicketCommand(
                QuestionId: questionId,
                TicketId: ticketId));

            return commandResult.IsSuccess ? NoContent() : commandResult.ToProblemDetails();
        }

        [HasPermission(Permission.ModifyData)]
        [HttpDelete("{ticketId:guid}")]
        public async Task<IActionResult> DeleteTicket(
            [FromRoute] Guid ticketId)
        {
            var commandResult = await _sender.Send(new DeleteTicketCommand(ticketId));

            return commandResult.IsSuccess ? NoContent() : commandResult.ToProblemDetails();
        }


        #endregion
    }
}
