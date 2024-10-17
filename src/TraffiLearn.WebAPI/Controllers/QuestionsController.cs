using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using TraffiLearn.Application.AI.DTO;
using TraffiLearn.Application.Comments.DTO;
using TraffiLearn.Application.Questions.Commands.AddCommentToQuestion;
using TraffiLearn.Application.Questions.Commands.Delete;
using TraffiLearn.Application.Questions.DTO;
using TraffiLearn.Application.Questions.Queries.GetAIQuestionExplanation;
using TraffiLearn.Application.Questions.Queries.GetById;
using TraffiLearn.Application.Questions.Queries.GetPaginated;
using TraffiLearn.Application.Questions.Queries.GetQuestionCommentsPaginated;
using TraffiLearn.Application.Questions.Queries.GetQuestionsForTheoryTest;
using TraffiLearn.Application.Questions.Queries.GetQuestionTickets;
using TraffiLearn.Application.Questions.Queries.GetQuestionTopics;
using TraffiLearn.Application.Questions.Queries.GetRandomQuestions;
using TraffiLearn.Application.Tickets.DTO;
using TraffiLearn.Application.Topics.DTO;
using TraffiLearn.Application.Users.Commands.DislikeQuestion;
using TraffiLearn.Application.Users.Commands.LikeQuestion;
using TraffiLearn.Application.Users.Commands.MarkQuestion;
using TraffiLearn.Application.Users.Commands.RemoveQuestionDislike;
using TraffiLearn.Application.Users.Commands.RemoveQuestionLike;
using TraffiLearn.Application.Users.Commands.UnmarkQuestion;
using TraffiLearn.Application.Users.Queries.GetCurrentUserDislikedQuestions;
using TraffiLearn.Application.Users.Queries.GetCurrentUserLikedQuestions;
using TraffiLearn.Application.Users.Queries.GetMarkedQuestions;
using TraffiLearn.Infrastructure.Authentication;
using TraffiLearn.WebAPI.CommandWrappers.CreateQuestion;
using TraffiLearn.WebAPI.CommandWrappers.UpdateQuestion;
using TraffiLearn.WebAPI.Extensions;
using TraffiLearn.WebAPI.Swagger;
using static System.Net.Mime.MediaTypeNames;

namespace TraffiLearn.WebAPI.Controllers
{
    [HasPermission(Permission.AccessData)]
    [Route("api/questions")]
    [ApiController]
    public sealed class QuestionsController : ControllerBase
    {
        private readonly ISender _sender;

        public QuestionsController(ISender sender)
        {
            _sender = sender;
        }

        #region Queries


        /// <summary>
        /// Gets paginated questions from the storage.
        /// </summary>
        /// <remarks>
        /// ***Query parameters:***<br /><br />
        /// `Page` : Must be a number greater or equal to 1. Not required field (the default value is: **1**).<br /><br />
        /// `PageSize` : Represents amount of questions in a page. Must be a number greater or equal to 1. Not required field (the default value is: **10**).<br /><br /><br />
        /// **Authentication Required:**<br />
        /// The user must be authenticated using a JWT token.
        /// </remarks>
        /// <param name="page">**Number of a page to get.**</param>
        /// <param name="pageSize">**Size of a page (amount of questions).**</param>
        /// <response code="200">Successfully retrieved paginated questions. Returns a list of questions along with the total available pages count.</response>
        /// <response code="400">***Bad request.*** The provided data is invalid.</response>
        /// <response code="401">***Unauthorized.*** The user is not authenticated.</response>
        /// <response code="500">***Internal Server Error.*** An unexpected error occurred during the process.</response>
        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(PaginatedQuestionsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPaginatedQuestions(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var queryResult = await _sender.Send(new GetPaginatedQuestionsQuery(
                Page: page,
                PageSize: pageSize));

            return queryResult.IsSuccess ? Ok(queryResult.Value) : queryResult.ToProblemDetails();
        }

        /// <summary>
        /// Gets a chosen amount of random questions.
        /// </summary>
        /// <remarks>
        /// **The request can include amount of random questions being retrieved.**<br /><br /><br />
        /// ***Query parameters:***<br /><br />
        /// `Amount` : Represents amount of random questions to get. Must be a number greater or equal to 1. The default value is: **1**<br /><br /><br />
        /// **Authentication Required:**<br />
        /// The user must be authenticated using a JWT token.<br /><br />
        /// </remarks>
        /// <param name="amount">**The amount of random questions to get**</param>
        /// <response code="200">Successfully the amount of random questions. Returns a list of questions.</response>
        /// <response code="400">***Bad request.*** The provided data is invalid.</response>
        /// <response code="401">***Unauthorized.*** The user is not authenticated.</response>
        /// <response code="500">***Internal Server Error.*** An unexpected error occurred during the process.</response>
        [HttpGet("random")]
        [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(IEnumerable<QuestionResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetRandomQuestions(
            [FromQuery] int amount = 1)
        {
            var queryResult = await _sender.Send(new GetRandomQuestionsQuery(amount));

            return queryResult.IsSuccess ? Ok(queryResult.Value) : queryResult.ToProblemDetails();
        }

        /// <summary>
        /// Gets a question with a specific ID.
        /// </summary>
        /// <remarks>
        /// **The request must include an ID of a question to get.**<br /><br /><br />
        /// ***Route parameters:***<br /><br />
        /// `QuestionId` : Must be a valid GUID representing ID of a question.<br /><br /><br />
        /// **Authentication Required:**<br />
        /// The user must be authenticated using a JWT token.<br /><br />
        /// </remarks>
        /// <param name="questionId">**The ID of a question to be retrieved**</param>
        /// <response code="200">Successfully retrieved the question with the provided ID. Returns the found question.</response>
        /// <response code="401">***Unauthorized.*** The user is not authenticated.</response>
        /// <response code="404">***Not found.*** No question exists with the provided ID.</response>
        /// <response code="500">***Internal Server Error.*** An unexpected error occurred during the process.</response>
        [HttpGet("{questionId:guid}")]
        [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(QuestionResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetQuestionById(
            [FromRoute] Guid questionId)
        {
            var queryResult = await _sender.Send(new GetQuestionByIdQuery(questionId));

            return queryResult.IsSuccess ? Ok(queryResult.Value) : queryResult.ToProblemDetails();
        }

        /// <summary>
        /// Gets all topics associated with a question by a question ID.
        /// </summary>
        /// <remarks>
        /// **The request must include an ID of a question.**<br /><br /><br />
        /// ***Route parameters:***<br /><br />
        /// `QuestionId` : Must be a valid GUID representing ID of a question.<br /><br /><br />
        /// **Authentication Required:**<br />
        /// The user must be authenticated using a JWT token.<br /><br />
        /// </remarks>
        /// <param name="questionId">**The ID of a question used to find related topics.**</param>
        /// <response code="200">Successfully retrieved topics associated with the question with the provided ID. Returns a list of topics.</response>
        /// <response code="401">***Unauthorized.*** The user is not authenticated.</response>
        /// <response code="404">***Not found.*** No question exists with the provided ID.</response>
        /// <response code="500">***Internal Server Error.*** An unexpected error occurred during the process.</response>
        [HttpGet("{questionId:guid}/topics")]
        [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(IEnumerable<TopicResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetQuestionTopics(
            [FromRoute] Guid questionId)
        {
            var queryResult = await _sender.Send(new GetQuestionTopicsQuery(questionId));

            return queryResult.IsSuccess ? Ok(queryResult.Value) : queryResult.ToProblemDetails();
        }

        /// <summary>
        /// Gets all tickets associated with a question by a question ID.
        /// </summary>
        /// <remarks>
        /// **The request must include an ID of a question.**<br /><br /><br />
        /// ***Route parameters:***<br /><br />
        /// `QuestionId` : Must be a valid GUID representing ID of a question.<br /><br /><br />
        /// **Authentication Required:**<br />
        /// The user must be authenticated using a JWT token.<br /><br />
        /// </remarks>
        /// <param name="questionId">**The ID of a question used to find related tickets.**</param>
        /// <response code="200">Successfully retrieved tickets associated with the question with the provided ID. Returns a list of tickets.</response>
        /// <response code="401">***Unauthorized.*** The user is not authenticated.</response>
        /// <response code="404">***Not found.*** No question exists with the provided ID.</response>
        /// <response code="500">***Internal Server Error.*** An unexpected error occurred during the process.</response>
        [HttpGet("{questionId:guid}/tickets")]
        [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(IEnumerable<TicketResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetQuestionTickets(
            [FromRoute] Guid questionId)
        {
            var queryResult = await _sender.Send(new GetQuestionTicketsQuery(questionId));

            return queryResult.IsSuccess ? Ok(queryResult.Value) : queryResult.ToProblemDetails();
        }

        /// <summary>
        /// Gets a configured amount of questions for a theory test.
        /// </summary>
        /// <remarks>
        /// The amount of a theory test's questions **can be configured** in app configuration (the default value is: **20**). <br /> <br />
        /// ***Contact the API owners for more details.***<br /><br />
        /// **Authentication Required:**<br />
        /// The user must be authenticated using a JWT token.<br /><br />
        /// </remarks>
        /// <response code="200">Successfully retrieved questions for a theory test. Returns a list of questions.</response>
        /// <response code="401">***Unauthorized.*** The user is not authenticated.</response>
        /// <response code="500">***Internal Server Error.*** An unexpected error occurred during the process.</response>
        [HttpGet("theory-test")]
        [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(IEnumerable<QuestionResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetQuestionsForTheoryTest()
        {
            var queryResult = await _sender.Send(new GetQuestionsForTheoryTestQuery());

            return queryResult.IsSuccess ? Ok(queryResult.Value) : queryResult.ToProblemDetails();
        }

        /// <summary>
        /// Gets paginated comments associated with a question by a question id.
        /// </summary>
        /// <remarks>
        /// **The request must include an ID of a question.**<br /><br /><br />
        /// ***Route parameters:***<br /><br />
        /// `QuestionId` : Must be a valid GUID representing ID of a question.<br /><br /><br />
        /// ***Query parameters:***<br /><br />
        /// `Page` : Must be a number greater or equal to 1. Not required field (the default value is: **1**).<br /><br />
        /// `PageSize` : Represents amount of comments in a page. Must be a number greater or equal to 1. Not required field (the default value is: **10**).<br /><br /><br />
        /// **Authentication Required:**<br />
        /// The user must be authenticated using a JWT token.<br /><br />
        /// </remarks>
        /// <param name="questionId">**The ID of a question used to find related comments.**</param>
        /// <param name="page">**Number of a page to get.**</param>
        /// <param name="pageSize">**Size of a page (amount of questions).**</param>
        /// <response code="200">Successfully retrieved comments associated with the question with the provided ID. Returns a list of comments.</response>
        /// <response code="400">***Bad request.*** The provided data is invalid or missing.</response>
        /// <response code="401">***Unauthorized.*** The user is not authenticated.</response>
        /// <response code="404">***Not found.*** No question exists with the provided ID.</response>
        /// <response code="500">***Internal Server Error.*** An unexpected error occurred during the process.</response>
        [HttpGet("{questionId:guid}/comments")]
        [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(PaginatedCommentsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetQuestionComments(
            [FromRoute] Guid questionId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var queryResult = await _sender.Send(
                new GetQuestionCommentsPaginatedQuery(
                    QuestionId: questionId,
                    Page: page,
                    PageSize: pageSize));

            return queryResult.IsSuccess ? Ok(queryResult.Value) : queryResult.ToProblemDetails();
        }

        /// <summary>
        /// Generates explanation for a specific question.
        /// </summary>
        /// <remarks>
        /// ***If you try to generate an explanation for a question with explanation or image, you're going to get 400 Error.***<br /><br /><br />
        /// **The request must include an ID of a question.**<br /><br /><br />
        /// ***Route parameters:***<br /><br />
        /// `QuestionId` : Must be a valid GUID representing ID of a question.<br /><br /><br />
        /// **Authentication Required:**<br />
        /// The user must be authenticated using a JWT token.<br /><br />
        /// </remarks>
        /// <param name="questionId">**The ID of a question used to generate explanation from.**</param>
        /// <response code="200">Successfully generated explanation associated with the question with the provided ID. Returns an explanation.</response>
        /// <response code="401">***Unauthorized.*** The user is not authenticated.</response>
        /// <response code="404">***Not found.*** No question exists with the provided ID.</response>
        /// <response code="500">***Internal Server Error.*** An unexpected error occurred during the process.</response>
        [HttpGet("{questionId:guid}/ai-explanation")]
        [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(AITextResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAIQuestionExplanation(
            [FromRoute] Guid questionId)
        {
            var queryResult = await _sender.Send(
                new GetAIQuestionExplanationQuery(questionId));

            return queryResult.IsSuccess ? Ok(queryResult.Value) : queryResult.ToProblemDetails();
        }

        /// <summary>
        /// Gets marked questions of the current (logged in) user.
        /// </summary>
        /// <remarks>
        /// **Authentication Required:**<br />
        /// The user must be authenticated using a JWT token.
        /// </remarks>
        /// <response code="200">Successfully retrieved all questions marked by the current user. Returns a list of questions.</response>
        /// <response code="401">***Unauthorized.*** The user is not authenticated.</response>
        /// <response code="500">***Internal Server Error.*** An unexpected error occurred during the process.</response>
        [HttpGet("marked")]
        [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(IEnumerable<QuestionResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMarkedQuestions()
        {
            var queryResult = await _sender.Send(new GetMarkedQuestionsQuery());

            return queryResult.IsSuccess ? Ok(queryResult.Value) : queryResult.ToProblemDetails();
        }

        /// <summary>
        /// Gets liked questions of the current (logged in) user.
        /// </summary>
        /// <remarks>
        /// **Authentication Required:**<br />
        /// The user must be authenticated using a JWT token.
        /// </remarks>
        /// <response code="200">Successfully retrieved all questions liked by the current user. Returns a list of questions.</response>
        /// <response code="401">***Unauthorized.*** The user is not authenticated.</response>
        /// <response code="500">***Internal Server Error.*** An unexpected error occurred during the process.</response>
        [HttpGet("liked")]
        [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(IEnumerable<QuestionResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetLikedQuestions()
        {
            var queryResult = await _sender.Send(new GetCurrentUserLikedQuestionsQuery());

            return queryResult.IsSuccess ? Ok(queryResult.Value) : queryResult.ToProblemDetails();
        }

        /// <summary>
        /// Gets disliked questions of the current (logged in) user.
        /// </summary>
        /// <remarks>
        /// **Authentication Required:**<br />
        /// The user must be authenticated using a JWT token.
        /// </remarks>
        /// <response code="200">Successfully retrieved all questions disliked by the current user. Returns a list of questions.</response>
        /// <response code="401">***Unauthorized.*** The user is not authenticated.</response>
        /// <response code="500">***Internal Server Error.*** An unexpected error occurred during the process.</response>
        [HttpGet("disliked")]
        [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(IEnumerable<QuestionResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetDislikedQuestions()
        {
            var queryResult = await _sender.Send(new GetCurrentUserDislikedQuestionsQuery());

            return queryResult.IsSuccess ? Ok(queryResult.Value) : queryResult.ToProblemDetails();
        }


        #endregion

        #region Commands

        /// <summary>
        /// Creates a new question.
        /// </summary>
        /// <remarks>
        /// **If created a new question**, all the provided topics are going to contain the question (ID). The newly created question is going to contain the mentioned topic IDs as well.<br /><br />
        /// **If provided an image**, the question will be assigned an **image uri**. You are able to view the image using the image uri.<br /><br />
        /// **If an image is not provided**, the image uri will be null.<br /><br />
        /// If succesfully created a new question, this endpoint returns ID of the newly created question.<br /><br />
        /// ***Parameters:***<br /><br />
        /// `Image` : Must be a valid image (possible extensions: ".jpg", ".jpeg", ".png", ".gif", ".bmp"). The size must be less than 500 Kb. Not required field.<br /><br />
        /// `Request` : Question represented as a JSON object.<br /><br /><br />
        /// **Request parameters:**<br /><br />
        /// `Content` : Content (text) of the question. Must not be empty or whitespace. Maximum length: 2000.<br /><br />
        /// `Explanation` : Explanation of the correct answer/question. Not required field. Maximum length: 2000.<br /><br />
        /// `QuestionNumber` : Number of the question. Must be greater than 0.<br /><br />
        /// `TopicIds` : Topics which are going to contain the question. Must not be empty. Must represent a list of valid GUIDs.<br /><br />
        /// `Answers` : Represents a list of answers to the question. Must not be empty. Must contain at least one correct answer. Maximum answer content length: 300.<br /><br /><br />
        /// **Authentication Required:**<br />
        /// The user must be authenticated using a JWT token. Only users with the `Owner` or `Admin` role can perform this action.<br /><br />
        /// </remarks>
        /// <param name="command">**The create question command.**</param>  
        /// <response code="201">Successfully created a new question. Returns ID of a newly created question</response>
        /// <response code="400">***Bad request.*** The provided data is invalid or missing.</response>
        /// <response code="401">***Unauthorized.*** The user is not authenticated.</response>
        /// <response code="403">***Forbidden***. The user is not authorized to perform this action.</response>
        /// <response code="404">***Not found.*** Topics with the provided topic IDs are not found.</response>
        /// <response code="500">***Internal Server Error.*** An unexpected error occurred during the process.</response>
        [HasPermission(Permission.ModifyData)]
        [HttpPost]
        [Consumes(Multipart.FormData)]
        [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateQuestion(
            [FromForm] CreateQuestionCommandWrapper command)
        {
            var commandResult = await _sender.Send(command.ToCommand());

            if (commandResult.IsSuccess)
            {
                return CreatedAtAction(
                    actionName: nameof(GetQuestionById),
                    routeValues: new { questionId = commandResult.Value },
                    value: commandResult.Value);
            }

            return commandResult.ToProblemDetails();
        }

        /// <summary>
        /// Updates an existing question.
        /// </summary>
        /// <remarks>
        /// **If provided new image**, the old image gets deleted, the provided image is inserted and the image uri of the question is updated accordingly.<br /><br />
        /// **If a new image is not provided and `RemoveOldImageIfNewMissing` is *true***, the old image gets deleted and the image uri of the question is updated to **null**.<br /><br />
        /// **If a new image is not provided and `RemoveOldImageIfNewMissing` is *false***, the old image does not get deleted and the image uri of the question remains **same**.<br /><br /><br />
        /// **If the list of the new topic IDs contains a topic ID which is *not present* within the question**, the topic ID will be added to the question's topic IDs.<br /><br />
        /// **If the list of the new topic IDs contains a topic ID which is *already* present within the question**, the topic ID won't be added to the question's topic IDs as it's already there.<br /><br />
        /// **If the list of the new topic IDs does not contain a topic ID which is *already* present within the question**, the question's topic ID gets deleted from the question.<br /><br /><br />
        /// ***Parameters:***<br /><br />
        /// `Image` : Must be a valid image (possible extensions: ".jpg", ".jpeg", ".png", ".gif", ".bmp"). The size must be less than 500 Kb. Not required field.<br /><br />
        /// `Request` : Question represented as a JSON object.<br /><br /><br />
        /// **Request parameters:**<br /><br />
        /// `QuestionId` : ID of the question to be updated. Must be a valid GUID.<br /><br />
        /// `Content` : Content (text) of the question. Must not be empty or whitespace. Maximum length: 2000.<br /><br />
        /// `Explanation` : Explanation of the correct answer/question. Not required field. Maximum length: 2000.<br /><br />
        /// `QuestionNumber` : Number of the question. Must be greater than 0.<br /><br />
        /// `TopicIds` : Topics which are going to be used to update the question's topic IDs. Must not be empty. Must represent a list of valid GUIDs.<br /><br />
        /// `Answers` : Represents a list of answers to the question. Must not be empty. Must contain at least one correct answer. Maximum answer content length: 300.<br /><br />
        /// `RemoveOldImageIfNewMissing` : Boolean value indicating whether to delete an existing image from a question if the new image is not provided. If you intend to update a question without changing its image, set this field to **false**. Not required field (the default value: **true**).<br /><br /><br />
        /// **Authentication Required:**<br />
        /// The user must be authenticated using a JWT token. Only users with the `Owner` or `Admin` role can perform this action.<br /><br />
        /// </remarks>
        /// <param name="command">**The update question command.**</param>  
        /// <response code="204">Successfully updated an existing question.</response>
        /// <response code="400">***Bad request.*** The provided data is invalid or missing.</response>
        /// <response code="401">***Unauthorized.*** The user is not authenticated.</response>
        /// <response code="403">***Forbidden***. The user is not authorized to perform this action.</response>
        /// <response code="404">***Not found.*** Question with the ID is not found or topics with the provided topic IDs are not found.</response>
        /// <response code="500">***Internal Server Error.*** An unexpected error occurred during the process.</response>
        [HasPermission(Permission.ModifyData)]
        [HttpPut]
        [Consumes(Multipart.FormData)]
        [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateQuestion(
            [FromForm] UpdateQuestionCommandWrapper command)
        {
            var commandResult = await _sender.Send(command.ToCommand());

            return commandResult.IsSuccess ? NoContent() : commandResult.ToProblemDetails();
        }

        /// <summary>
        /// Deletes a question using its ID.
        /// </summary>
        /// <remarks>
        /// **The request must include the ID of the question.**<br /><br /><br />
        /// ***Route parameters:***<br /><br />
        /// `QuestionId` : Must be a valid GUID representing ID of the question.<br /><br /><br />
        /// **Authentication Required:**<br />
        /// The user must be authenticated using a JWT token. Only users with the `Owner` or `Admin` role can perform this action.<br /><br />
        /// </remarks>
        /// <param name="questionId">**The ID of the question to be deleted.**</param>  
        /// <response code="204">Successfully deleted the question.</response>
        /// <response code="401">***Unauthorized.*** The user is not authenticated.</response>
        /// <response code="403">***Forbidden***. The user is not authorized to perform this action.</response>
        /// <response code="404">***Not found.*** Question with the provided id is not found.</response>
        /// <response code="500">***Internal Server Error.*** An unexpected error occurred during the process.</response>
        [HasPermission(Permission.ModifyData)]
        [HttpDelete("{questionId:guid}")]
        [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteQuestion(
            [FromRoute] Guid questionId)
        {
            var commandResult = await _sender.Send(new DeleteQuestionCommand(questionId));

            return commandResult.IsSuccess ? NoContent() : commandResult.ToProblemDetails();
        }

        /// <summary>
        /// Adds a comment to an existing question.
        /// </summary>
        /// <remarks>
        /// **The request must include the ID of the question and the comment's content.**<br /><br /><br />
        /// ***Body parameters:***<br /><br />
        /// `QuestionId` : Must be a valid GUID representing the question to which the comment is going to be added.<br /><br />
        /// `Content` : Represents the comment's content (text). Must not be empty or whitespace. Maximum length: **500**.<br /><br /><br />
        /// **Authentication Required:**<br />
        /// The user must be authenticated using a JWT token.<br /><br />
        /// </remarks>
        /// <param name="command">**The add comment command.**</param>  
        /// <response code="201">Successfully added the comment to the question.</response>
        /// <response code="400">***Bad request.*** The provided data is invalid or missing.</response>
        /// <response code="401">***Unauthorized.*** The user is not authenticated.</response>
        /// <response code="404">***Not found.*** Question with the provided id is not found.</response>
        /// <response code="500">***Internal Server Error.*** An unexpected error occurred during the process.</response>
        [HasPermission(Permission.ModifyNonSensitiveData)]
        [HttpPost("add-comment")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddComment(
            [FromBody] AddCommentToQuestionCommand command)
        {
            var commandResult = await _sender.Send(command);

            return commandResult.IsSuccess ? Created() : commandResult.ToProblemDetails();
        }

        /// <summary>
        /// Marks a question for the current (logged in) user.
        /// </summary>
        /// <remarks>
        /// If the question is **already marked** by the user, this endpoint returns 400 Error.<br /><br /><br />
        /// **The request must include the ID of the question to be marked.**<br /><br /><br />
        /// ***Route parameters:***<br /><br />
        /// `QuestionId` : Must be a valid GUID representing the question to be marked.<br /><br /><br />
        /// **Authentication Required:**<br />
        /// The user must be authenticated using a JWT token.<br /><br />
        /// </remarks>
        /// <param name="questionId">**The ID of the question to be marked.**</param>  
        /// <response code="204">Successfully marked the question.</response>
        /// <response code="400">***Bad request.*** The question is already marked by the user.</response>
        /// <response code="401">***Unauthorized.*** The user is not authenticated.</response>
        /// <response code="404">***Not found.*** Question with the provided id is not found.</response>
        /// <response code="500">***Internal Server Error.*** An unexpected error occurred during the process.</response>
        [HasPermission(Permission.ModifyNonSensitiveData)]
        [HttpPut("{questionId:guid}/mark")]
        [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> MarkQuestion(
            [FromRoute] Guid questionId)
        {
            var commandResult = await _sender.Send(new MarkQuestionCommand(questionId));

            return commandResult.IsSuccess ? NoContent() : commandResult.ToProblemDetails();
        }

        /// <summary>
        /// Unmarks a question for the current (logged in) user.
        /// </summary>
        /// <remarks>
        /// If the question is **not marked** by the user, this endpoint returns 400 Error.<br /><br /><br />
        /// **The request must include the ID of the question to be marked.**<br /><br /><br />
        /// ***Route parameters:***<br /><br />
        /// `QuestionId` : Must be a valid GUID representing the question to be unmarked.<br /><br /><br />
        /// **Authentication Required:**<br />
        /// The user must be authenticated using a JWT token.<br /><br />
        /// </remarks>
        /// <param name="questionId">**The ID of the question to be unmarked.**</param>  
        /// <response code="204">Successfully unmarked the question.</response>
        /// <response code="400">***Bad request.*** The question is not marked by the user.</response>
        /// <response code="401">***Unauthorized.*** The user is not authenticated.</response>
        /// <response code="404">***Not found.*** Question with the provided id is not found.</response>
        /// <response code="500">***Internal Server Error.*** An unexpected error occurred during the process.</response>
        [HasPermission(Permission.ModifyNonSensitiveData)]
        [HttpPut("{questionId:guid}/unmark")]
        [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UnmarkQuestion(
            [FromRoute] Guid questionId)
        {
            var commandResult = await _sender.Send(new UnmarkQuestionCommand(questionId));

            return commandResult.IsSuccess ? NoContent() : commandResult.ToProblemDetails();
        }

        /// <summary>
        /// Likes a question with the current (logged in) user.
        /// </summary>
        /// <remarks>
        /// If the question is **already liked** or **disliked** by the user, this endpoint returns 400 Error.<br /><br />
        /// **The request must include the ID of the question to be liked.**<br /><br /><br />
        /// ***Route parameters:***<br /><br />
        /// `QuestionId` : Must be a valid GUID representing the question to be liked.<br /><br /><br />
        /// **Authentication Required:**<br />
        /// The user must be authenticated using a JWT token.<br /><br />
        /// </remarks>
        /// <param name="questionId">**The ID of the question to be liked.**</param>  
        /// <response code="204">Successfully liked the question.</response>
        /// <response code="400">***Bad request.*** The question is already liked or disliked by the user.</response>
        /// <response code="401">***Unauthorized.*** The user is not authenticated.</response>
        /// <response code="404">***Not found.*** Question with the provided id is not found.</response>
        /// <response code="500">***Internal Server Error.*** An unexpected error occurred during the process.</response>
        [HasPermission(Permission.ModifyNonSensitiveData)]
        [HttpPut("{questionId:guid}/like")]
        [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> LikeQuestion(
            [FromRoute] Guid questionId)
        {
            var commandResult = await _sender.Send(new LikeQuestionCommand(questionId));

            return commandResult.IsSuccess ? NoContent() : commandResult.ToProblemDetails();
        }

        /// <summary>
        /// Dislikes a question with the current (logged in) user.
        /// </summary>
        /// <remarks>
        /// If the question is **already disliked** or **liked** by the user, this endpoint returns 400 Error.<br /><br />
        /// **The request must include the ID of the question to be disliked.**<br /><br /><br />
        /// ***Route parameters:***<br /><br />
        /// `QuestionId` : Must be a valid GUID representing the question to be disliked.<br /><br /><br />
        /// **Authentication Required:**<br />
        /// The user must be authenticated using a JWT token.<br /><br />
        /// </remarks>
        /// <param name="questionId">**The ID of the question to be disliked.**</param>  
        /// <response code="204">Successfully disliked the question.</response>
        /// <response code="400">***Bad request.*** The question is already disliked or liked by the user.</response>
        /// <response code="401">***Unauthorized.*** The user is not authenticated.</response>
        /// <response code="404">***Not found.*** Question with the provided id is not found.</response>
        /// <response code="500">***Internal Server Error.*** An unexpected error occurred during the process.</response>
        [HasPermission(Permission.ModifyNonSensitiveData)]
        [HttpPut("{questionId:guid}/dislike")]
        [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DislikeQuestion(
            [FromRoute] Guid questionId)
        {
            var commandResult = await _sender.Send(new DislikeQuestionCommand(questionId));

            return commandResult.IsSuccess ? NoContent() : commandResult.ToProblemDetails();
        }

        /// <summary>
        /// Removes a like for a question with the current (logged in) user.
        /// </summary>
        /// <remarks>
        /// If the question is **not liked** or **already disliked** by the user, this endpoint returns 400 Error.<br /><br />
        /// **The request must include the ID of the question.**<br /><br /><br />
        /// ***Route parameters:***<br /><br />
        /// `QuestionId` : Must be a valid GUID representing the question.<br /><br /><br />
        /// **Authentication Required:**<br />
        /// The user must be authenticated using a JWT token.<br /><br />
        /// </remarks>
        /// <param name="questionId">**The ID of the question.**</param>  
        /// <response code="204">Successfully removed a like from the question.</response>
        /// <response code="400">***Bad request.*** The question is not liked or already disliked by the user.</response>
        /// <response code="401">***Unauthorized.*** The user is not authenticated.</response>
        /// <response code="404">***Not found.*** Question with the provided id is not found.</response>
        /// <response code="500">***Internal Server Error.*** An unexpected error occurred during the process.</response>
        [HasPermission(Permission.ModifyNonSensitiveData)]
        [HttpPut("{questionId:guid}/remove-like")]
        [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RemoveQuestionLike(
            [FromRoute] Guid questionId)
        {
            var commandResult = await _sender.Send(new RemoveQuestionLikeCommand(questionId));

            return commandResult.IsSuccess ? NoContent() : commandResult.ToProblemDetails();
        }

        /// <summary>
        /// Removes a dislike for a question with the current (logged in) user.
        /// </summary>
        /// <remarks>
        /// If the question is **not disliked** or **already liked** by the user, this endpoint returns 400 Error.<br /><br />
        /// **The request must include the ID of the question.**<br /><br /><br />
        /// ***Route parameters:***<br /><br />
        /// `QuestionId` : Must be a valid GUID representing the question.<br /><br /><br />
        /// **Authentication Required:**<br />
        /// The user must be authenticated using a JWT token.<br /><br />
        /// </remarks>
        /// <param name="questionId">**The ID of the question.**</param>  
        /// <response code="204">Successfully removed a dislike from the question.</response>
        /// <response code="400">***Bad request.*** The question is not disliked or already liked by the user.</response>
        /// <response code="401">***Unauthorized.*** The user is not authenticated.</response>
        /// <response code="404">***Not found.*** Question with the provided id is not found.</response>
        /// <response code="500">***Internal Server Error.*** An unexpected error occurred during the process.</response>
        [HasPermission(Permission.ModifyNonSensitiveData)]
        [HttpPut("{questionId:guid}/remove-dislike")]
        [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RemoveQuestionDislike(
            [FromRoute] Guid questionId)
        {
            var commandResult = await _sender.Send(new RemoveQuestionDislikeCommand(questionId));

            return commandResult.IsSuccess ? NoContent() : commandResult.ToProblemDetails();
        }


        #endregion
    }
}
