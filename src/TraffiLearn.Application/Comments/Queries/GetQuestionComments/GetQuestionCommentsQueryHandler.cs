using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Comments.DTO;
using TraffiLearn.Domain.Aggregates.Comments;
using TraffiLearn.Domain.Aggregates.Questions;
using TraffiLearn.Domain.Aggregates.Questions.Errors;
using TraffiLearn.Domain.Aggregates.Questions.ValueObjects;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Comments.Queries.GetQuestionComments
{
    internal sealed class GetQuestionCommentsQueryHandler
        : IRequestHandler<GetQuestionCommentsQuery,
            Result<IEnumerable<CommentResponse>>>
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly Mapper<Comment, CommentResponse> _commentMapper;

        public GetQuestionCommentsQueryHandler(
            ICommentRepository commentRepository,
            IQuestionRepository questionRepository,
            Mapper<Comment, CommentResponse> commentMapper)
        {
            _commentRepository = commentRepository;
            _questionRepository = questionRepository;
            _commentMapper = commentMapper;
        }

        public async Task<Result<IEnumerable<CommentResponse>>> Handle(
            GetQuestionCommentsQuery request,
            CancellationToken cancellationToken)
        {
            QuestionId questionId = new(request.QuestionId.Value);

            var exists = await _questionRepository.ExistsAsync(
                questionId,
                cancellationToken);

            if (!exists)
            {
                return Result.Failure<IEnumerable<CommentResponse>>(
                   QuestionErrors.NotFound);
            }

            var comments = await _commentRepository
                .GetManyByQuestionIdWithRepliesAsync(
                    questionId,
                    cancellationToken);

            return Result.Success(_commentMapper.Map(comments));
        }
    }
}
