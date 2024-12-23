﻿using MediatR;
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
    [HasPermission(Permission.AccessData)]
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


        /// <summary>
        /// Gets all topics sorted by number from the storage.
        /// </summary>
        /// <remarks>
        /// **Authentication Required:**<br />
        /// The user must be authenticated using a JWT token.
        /// </remarks>
        /// <response code="200">Successfully retrieved all topics. Returns a list of topics.</response>
        /// <response code="401">***Unauthorized.*** The user is not authenticated.</response>
        /// <response code="500">***Internal Server Error.*** An unexpected error occurred during the process.</response>
        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<TopicResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllSortedTopicsByNumber()
        {
            var queryResult = await _sender.Send(new GetAllSortedTopicsByNumberQuery());

            return queryResult.IsSuccess ? Ok(queryResult.Value) : queryResult.ToProblemDetails();
        }

        /// <summary>
        /// Gets a random topic with questions included.
        /// </summary>
        /// <remarks>
        /// The user must be authenticated using a JWT token.
        /// </remarks>
        /// <response code="200">Successfully retrieved topic with questions. Returns a topic.</response>
        /// <response code="401">***Unauthorized.*** The user is not authenticated.</response>
        /// <response code="500">***Internal Server Error.*** An unexpected error occurred during the process.</response>
        [HttpGet("random/with-questions")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(TopicWithQuestionsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetRandomTopicWithQuestions()
        {
            var queryResult = await _sender.Send(new GetRandomTopicWithQuestionsQuery());

            return queryResult.IsSuccess ? Ok(queryResult.Value) : queryResult.ToProblemDetails();
        }

        /// <summary>
        /// Gets a topic with a specific ID.
        /// </summary>
        /// <remarks>
        /// **The request must include an ID of a topic to get.**<br /><br /><br />
        /// ***Route parameters:***<br /><br />
        /// `TopicId` : Must be a valid GUID representing ID of a topic.<br /><br /><br />
        /// **Authentication Required:**<br />
        /// The user must be authenticated using a JWT token.<br /><br />
        /// </remarks>
        /// <param name="topicId">**The ID of a topic to be retrieved**</param>
        /// <response code="200">Successfully retrieved the topic with the provided ID. Returns the found topic.</response>
        /// <response code="401">***Unauthorized.*** The user is not authenticated.</response>
        /// <response code="404">***Not found.*** No topic exists with the provided ID.</response>
        /// <response code="500">***Internal Server Error.*** An unexpected error occurred during the process.</response>
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

        /// <summary>
        /// Gets all questions associated with a topic by a topic ID.
        /// </summary>
        /// <remarks>
        /// **The request must include an ID of a topic.**<br /><br /><br />
        /// ***Route parameters:***<br /><br />
        /// `TopicId` : Must be a valid GUID representing ID of a topic.<br /><br /><br />
        /// **Authentication Required:**<br />
        /// The user must be authenticated using a JWT token.<br /><br />
        /// </remarks>
        /// <param name="topicId">**The ID of a topic used to find related questions.**</param>
        /// <response code="200">Successfully retrieved questions associated with the topic with the provided ID. Returns a list of questions.</response>
        /// <response code="401">***Unauthorized.*** The user is not authenticated.</response>
        /// <response code="404">***Not found.*** No topic exists with the provided ID.</response>
        /// <response code="500">***Internal Server Error.*** An unexpected error occurred during the process.</response>
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


        /// <summary>
        /// Creates a new topic.
        /// </summary>
        /// <remarks>
        /// **If provided an image**, the topic will be assigned an **image uri**. You are able to view the image using the image uri.<br /><br />
        /// **If an image is not provided**, the image uri will be null.<br /><br />
        /// ***Parameters:***<br /><br />
        /// `Image` : Must be a valid image (possible extensions: ".jpg", ".jpeg", ".png", ".gif", ".bmp"). The size must be less than 500 Kb. Not required field.<br /><br />
        /// `Request` : Question represented as a JSON object.<br /><br /><br />
        /// ***Request Parameters:***<br /><br />
        /// `TopicNumber` : Number of the topic. Must be greater than 0.<br /><br />
        /// `Title` : Title of the topic. Must be less than 300 characters long.<br /><br /><br />
        /// **Authentication Required:**<br />
        /// The user must be authenticated using a JWT token. Only users with the `Owner` or `Admin` role can perform this action.<br /><br />
        /// </remarks>
        /// <param name="command">**The create topic command.**</param>
        /// <response code="201">Successfully created a new topic. Returns ID of a newly created topic</response>
        /// <response code="400">***Bad request.*** The provided data is invalid or missing.</response>
        /// <response code="401">***Unauthorized.*** The user is not authenticated.</response>
        /// <response code="403">***Forbidden***. The user is not authorized to perform this action.</response>
        /// <response code="500">***Internal Server Error.*** An unexpected error occurred during the process.</response>
        [HasPermission(Permission.ModifyData)]
        [HttpPost]
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

        /// <summary>
        /// Updates an existing topic.
        /// </summary>
        /// <remarks>
        /// **If provided new image**, the old image gets deleted, the provided image is inserted and the image uri of the topic is updated accordingly.<br /><br />
        /// **If a new image is not provided and `RemoveOldImageIfNewMissing` is *true***, the old image gets deleted and the image uri of the topic is updated to **null**.<br /><br />
        /// **If a new image is not provided and `RemoveOldImageIfNewMissing` is *false***, the old image does not get deleted and the image uri of the topic remains **same**.<br /><br /><br />
        /// ***Parameters:***<br /><br />
        /// `Image` : Must be a valid image (possible extensions: ".jpg", ".jpeg", ".png", ".gif", ".bmp"). The size must be less than 500 Kb. Not required field.<br /><br />
        /// `Request` : Topic represented as a JSON object.<br /><br /><br />
        /// ***Request Parameters:***<br /><br />
        /// `TopicId` : ID of the topic to be updated. Must be a valid GUID.<br /><br />
        /// `TopicNumber` : Number of the topic. Must be greater than 0.<br /><br />
        /// `Title` : Title of the topic. Must be less than 300 characters long.<br /><br />
        /// `RemoveOldImageIfNewMissing` : Boolean value indicating whether to delete an existing image from a topic if the new image is not provided. If you intend to update a topic without changing its image, set this field to **false**. Not required field (the default value: **true**).<br /><br /><br />
        /// **Authentication Required:**<br />
        /// The user must be authenticated using a JWT token. Only users with the `Owner` or `Admin` role can perform this action.<br /><br />
        /// </remarks>
        /// <param name="command">**The update topic command.**</param>
        /// <response code="204">Successfully updated an existing topic.</response>
        /// <response code="400">***Bad request.*** The provided data is invalid or missing.</response>
        /// <response code="401">***Unauthorized.*** The user is not authenticated.</response>
        /// <response code="403">***Forbidden***. The user is not authorized to perform this action.</response>
        /// <response code="404">***Not found.*** Topic with the ID is not found.</response>
        /// <response code="500">***Internal Server Error.*** An unexpected error occurred during the process.</response>
        [HasPermission(Permission.ModifyData)]
        [HttpPut]
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

        /// <summary>
        /// Adds a question to a topic.
        /// </summary>
        /// <remarks>
        /// **The request must include the ID of the question and topic.**<br /><br /><br />
        /// ***Route parameters:***<br /><br />
        /// `QuestionId` : Must be a valid GUID representing ID of the question.<br /><br />
        /// `TopicId` : Must be a valid GUID representing ID of the topic.<br /><br />
        /// ***Authentication Required:***<br /><br />
        /// The user must be authenticated using a JWT token. Only users with the `Owner` or `Admin` role can perform this action.<br /><br />
        /// </remarks>
        /// <param name="questionId">**The ID of the question to be added to topic.**</param>
        /// <param name="topicId">**The ID of the topic to which the question will be added.**</param>
        /// <response code="204">Successfully added question to topic.</response>
        /// <response code="400">***Bad request.*** The provided data is invalid or missing.</response>
        /// <response code="401">***Unauthorized.*** The user is not authenticated.</response>
        /// <response code="403">***Forbidden***. The user is not authorized to perform this action.</response>
        /// <response code="404">***Not found.*** Question or topic with the provided id is not found.</response>
        /// <response code="500">***Internal Server Error.*** An unexpected error occurred during the process.</response>
        [HasPermission(Permission.ModifyData)]
        [HttpPut("{topicId:guid}/add-question/{questionId:guid}")]
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

        /// <summary>
        /// Removes a question from a topic.
        /// </summary>
        /// <remarks>
        /// **The request must include the ID of the question and topic.**<br /><br /><br />
        /// ***Route parameters:***<br /><br />
        /// `QuestionId` : Must be a valid GUID representing ID of the question.<br /><br />
        /// `TopicId` : Must be a valid GUID representing ID of the topic.<br /><br />
        /// ***Authentication Required:***<br /><br />
        /// The user must be authenticated using a JWT token. Only users with the `Owner` or `Admin` role can perform this action.<br /><br />
        /// </remarks>
        /// <param name="questionId">**The ID of the question to be removed from the topic.**</param>
        /// <param name="topicId">**The ID of the topic from which the question will be removed.**</param>
        /// <response code="204">Successfully removed the question from the topic.</response>
        /// <response code="400">***Bad request.*** The provided data is invalid or missing.</response>
        /// <response code="401">***Unauthorized.*** The user is not authenticated.</response>
        /// <response code="403">***Forbidden***. The user is not authorized to perform this action.</response>
        /// <response code="404">***Not found.*** Question or topic with the provided id is not found.</response>
        /// <response code="500">***Internal Server Error.*** An unexpected error occurred during the process.</response>
        [HasPermission(Permission.ModifyData)]
        [HttpPut("{topicId:guid}/remove-question/{questionId:guid}")]
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

        /// <summary>
        /// Deletes a topic using its ID.
        /// </summary>
        /// <remarks>
        /// **The request must include the ID of the topic.**<br /><br /><br />
        /// ***Route parameters:***<br /><br />
        /// `TopicId` : Must be a valid GUID representing ID of a topic.<br /><br /><br />
        /// **Authentication Required:**<br />
        /// The user must be authenticated using a JWT token. Only users with the `Owner` or `Admin` role can perform this action.<br /><br />
        /// </remarks>
        /// <param name="topicId">**The ID of the topic to be deleted.**</param>
        /// <response code="204">Successfully deleted the topic.</response>
        /// <response code="401">***Unauthorized.*** The user is not authenticated.</response>
        /// <response code="403">***Forbidden***. The user is not authorized to perform this action.</response>
        /// <response code="404">***Not found.*** Topic with the provided id is not found.</response>
        /// <response code="500">***Internal Server Error.*** An unexpected error occurred during the process.</response>
        [HasPermission(Permission.ModifyData)]
        [HttpDelete("{topicId:guid}")]
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
