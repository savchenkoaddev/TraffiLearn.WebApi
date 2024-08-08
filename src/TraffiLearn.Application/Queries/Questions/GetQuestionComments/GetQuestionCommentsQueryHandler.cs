using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.DTO.Comments;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.Errors.Questions;
using TraffiLearn.Domain.RepositoryContracts;
using TraffiLearn.Domain.Shared;
using TraffiLearn.Domain.ValueObjects.Questions;

namespace TraffiLearn.Application.Queries.Questions.GetQuestionComments
{
    internal sealed class GetQuestionCommentsQueryHandler
        : IRequestHandler<GetQuestionCommentsQuery,
            Result<IEnumerable<CommentResponse>>>
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly Mapper<Comment, CommentResponse> _commentMapper;

        public GetQuestionCommentsQueryHandler(
            IQuestionRepository questionRepository,
            Mapper<Comment, CommentResponse> commentMapper)
        {
            _questionRepository = questionRepository;
            _commentMapper = commentMapper;
        }

        public async Task<Result<IEnumerable<CommentResponse>>> Handle(
            GetQuestionCommentsQuery request,
            CancellationToken cancellationToken)
        {
            QuestionId questionId = new(request.QuestionId.Value);

            var questionExists = await _questionRepository.ExistsAsync(
                questionId,
                cancellationToken);

            if (!questionExists) 
            {
                return Result.Failure<IEnumerable<CommentResponse>>(
                   QuestionErrors.NotFound);
            }

            var comments = await _questionRepository
                .GetQuestionCommentsWithRepliesAsync(
                    questionId,
                    cancellationToken);

            return Result.Success(_commentMapper.Map(comments));
        }
    }
}
