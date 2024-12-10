using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Helpers;
using TraffiLearn.Application.UseCases.Comments.DTO;
using TraffiLearn.Domain.Comments;
using TraffiLearn.Domain.Questions;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Questions.Queries.GetQuestionCommentsPaginated
{
    internal sealed class GetQuestionCommentsPaginatedQueryHandler
        : IRequestHandler<GetQuestionCommentsPaginatedQuery,
            Result<PaginatedCommentsResponse>>
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly Mapper<Comment, CommentResponse> _commentMapper;

        public GetQuestionCommentsPaginatedQueryHandler(
            ICommentRepository commentRepository,
            IQuestionRepository questionRepository,
            Mapper<Comment, CommentResponse> commentMapper)
        {
            _commentRepository = commentRepository;
            _questionRepository = questionRepository;
            _commentMapper = commentMapper;
        }

        public async Task<Result<PaginatedCommentsResponse>> Handle(
            GetQuestionCommentsPaginatedQuery request,
            CancellationToken cancellationToken)
        {
            QuestionId questionId = new(request.QuestionId.Value);

            var exists = await _questionRepository.ExistsAsync(
                questionId,
                cancellationToken);

            if (!exists)
            {
                return Result.Failure<PaginatedCommentsResponse>(
                   QuestionErrors.NotFound);
            }

            var comments = await _commentRepository
                .GetManyByQuestionIdWithRepliesAndCreatorsAsync(
                    questionId,
                    page: request.Page,
                    pageSize: request.PageSize,
                    cancellationToken);

            var commentsCount = await _commentRepository.CountWithQuestionIdAsync(
                questionId,
                cancellationToken);

            var totalPages = PaginationCalculator.CalculateTotalPages(
                pageSize: request.PageSize,
                itemsCount: commentsCount);

            var commentsResponse = _commentMapper.Map(comments);

            return Result.Success(new PaginatedCommentsResponse(
                Comments: commentsResponse,
                TotalPages: totalPages));
        }
    }
}
