using FluentValidation;

namespace TraffiLearn.Application.Directories.Queries.GetById
{
    internal sealed class GetDirectoryByIdQueryValidator
        : AbstractValidator<GetDirectoryByIdQuery>
    {
        public GetDirectoryByIdQueryValidator()
        {
            RuleFor(x => x.DirectoryId)
                .NotEmpty();
        }
    }
}
