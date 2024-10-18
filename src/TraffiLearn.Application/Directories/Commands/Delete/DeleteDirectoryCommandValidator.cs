using FluentValidation;

namespace TraffiLearn.Application.Directories.Commands.Delete
{
    internal sealed class DeleteDirectoryCommandValidator
        : AbstractValidator<DeleteDirectoryCommand>
    {
        public DeleteDirectoryCommandValidator()
        {
            RuleFor(x => x.DirectoryId)
                .NotEmpty();
        }
    }
}
