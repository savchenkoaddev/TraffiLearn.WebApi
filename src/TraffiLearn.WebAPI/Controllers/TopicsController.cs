using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using TraffiLearn.Application.UseCases.Questions.DTO;
using TraffiLearn.Application.UseCases.Topics.Commands.AddQuestionToTopic;
using TraffiLearn.Application.UseCases.Topics.Commands.Delete;
using TraffiLearn.Application.UseCases.Topics.Commands.RemoveQuestionFromTopic;
using TraffiLearn.Application.UseCases.Topics.DTO;
using TraffiLearn.Application.UseCases.Topics.Queries.GetAllSortedByNumber;
using TraffiLearn.Application.UseCases.Topics.Queries.GetById;
using TraffiLearn.Application.UseCases.Topics.Queries.GetRandomTopicWithQuestions;
using TraffiLearn.Application.UseCases.Topics.Queries.GetTopicQuestions;
using TraffiLearn.Infrastructure.Authentication;
using TraffiLearn.WebAPI.CommandWrappers.CreateTopic;
using TraffiLearn.WebAPI.CommandWrappers.UpdateTopic;
using TraffiLearn.WebAPI.Extensions;
using TraffiLearn.WebAPI.Swagger;

namespace TraffiLearn.WebAPI.Controllers
{
    [HasPermission(Permission.AuthenticatedUser)]
    [Route("api/topics")]
    [ApiController]
    public sealed class TopicsController : ControllerBase
    {
        private readonly ISender _sender;

        public TopicsController(ISender sender)
        {
            _sender = sender;
        }

        #region Queries


        /// <include file='Documentation/TopicsControllerDocs.xml' path='doc/members/member[@name="M:GetAllSortedTopicsByNumber"]/*'/>
        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<TopicResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllSortedTopicsByNumber()
        {
            var queryResult = await _sender.Send(new GetAllSortedTopicsByNumberQuery());

            return queryResult.IsSuccess ? Ok(queryResult.Value) : queryResult.ToProblemDetails();
        }

        /// <include file='Documentation/TopicsControllerDocs.xml' path='doc/members/member[@name="M:GetRandomTopicWithQuestions"]/*'/>
        [HttpGet("random/with-questions")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(TopicWithQuestionsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetRandomTopicWithQuestions()
        {
            var queryResult = await _sender.Send(new GetRandomTopicWithQuestionsQuery());

            return queryResult.IsSuccess ? Ok(queryResult.Value) : queryResult.ToProblemDetails();
        }

        /// <include file='Documentation/TopicsControllerDocs.xml' path='doc/members/member[@name="M:GetTopicById"]/*'/>
        [HttpGet("{topicId:guid}")]
        [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(TopicResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTopicById(
            [FromRoute] Guid topicId)
        {
            var queryResult = await _sender.Send(new GetTopicByIdQuery(topicId));

            return queryResult.IsSuccess ? Ok(queryResult.Value) : queryResult.ToProblemDetails();
        }

        /// <include file='Documentation/TopicsControllerDocs.xml' path='doc/members/member[@name="M:GetTopicQuestions"]/*'/>
        [HttpGet("{topicId:guid}/questions")]
        [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(IEnumerable<QuestionResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTopicQuestions(
            [FromRoute] Guid topicId)
        {
            var queryResult = await _sender.Send(new GetTopicQuestionsQuery(topicId));

            return queryResult.IsSuccess ? Ok(queryResult.Value) : queryResult.ToProblemDetails();
        }


        #endregion

        #region Commands


        /// <include file='Documentation/TopicsControllerDocs.xml' path='doc/members/member[@name="M:CreateTopic"]/*'/>
        [HttpPost]
        [HasPermission(Permission.ModifyApplicationData)]
        [Consumes(MediaTypeNames.Multipart.FormData)]
        [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateTopic(
            [FromForm] CreateTopicCommandWrapper command)
        {
            var commandResult = await _sender.Send(command.ToCommand());

            if (commandResult.IsSuccess)
            {
                return CreatedAtAction(
                    actionName: nameof(GetTopicById),
                    routeValues: new { topicId = commandResult.Value },
                    value: commandResult.Value);
            }

            return commandResult.ToProblemDetails();
        }

        /// <include file='Documentation/TopicsControllerDocs.xml' path='doc/members/member[@name="M:UpdateTopic"]/*'/>
        [HttpPut]
        [HasPermission(Permission.ModifyApplicationData)]
        [Consumes(MediaTypeNames.Multipart.FormData)]
        [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateTopic(
            [FromForm] UpdateTopicCommandWrapper command)
        {
            var commandResult = await _sender.Send(command.ToCommand());

            return commandResult.IsSuccess ? NoContent() : commandResult.ToProblemDetails();
        }

        /// <include file='Documentation/TopicsControllerDocs.xml' path='doc/members/member[@name="M:AddQuestionToTopic"]/*'/>
        [HttpPut("{topicId:guid}/add-question/{questionId:guid}")]
        [HasPermission(Permission.ModifyApplicationData)]
        [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddQuestionToTopic(
            [FromRoute] Guid questionId,
            [FromRoute] Guid topicId)
        {
            var commandResult = await _sender.Send(new AddQuestionToTopicCommand(
                QuestionId: questionId,
                TopicId: topicId));

            return commandResult.IsSuccess ? NoContent() : commandResult.ToProblemDetails();
        }

        /// <include file='Documentation/TopicsControllerDocs.xml' path='doc/members/member[@name="M:RemoveQuestionFromTopic"]/*'/>
        [HttpPut("{topicId:guid}/remove-question/{questionId:guid}")]
        [HasPermission(Permission.ModifyApplicationData)]
        [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RemoveQuestionFromTopic(
            [FromRoute] Guid questionId,
            [FromRoute] Guid topicId)
        {
            var commandResult = await _sender.Send(new RemoveQuestionFromTopicCommand(
                QuestionId: questionId,
                TopicId: topicId));

            return commandResult.IsSuccess ? NoContent() : commandResult.ToProblemDetails();
        }

        /// <include file='Documentation/TopicsControllerDocs.xml' path='doc/members/member[@name="M:DeleteTopic"]/*'/>
        [HttpDelete("{topicId:guid}")]
        [HasPermission(Permission.ModifyApplicationData)]
        [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteTopic(
            [FromRoute] Guid topicId)
        {
            var commandResult = await _sender.Send(new DeleteTopicCommand(topicId));

            return commandResult.IsSuccess ? NoContent() : commandResult.ToProblemDetails();
        }


        #endregion
    }
}
