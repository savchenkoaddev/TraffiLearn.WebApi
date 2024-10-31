using FluentValidation;
using TraffiLearn.Domain.Aggregates.Directories.DirectoryNames;
using TraffiLearn.Domain.Aggregates.Directories.DirectorySections.SectionContents;
using TraffiLearn.Domain.Aggregates.Directories.DirectorySections.SectionNames;

namespace TraffiLearn.Application.Directories.Commands.Create
{
    internal sealed class CreateDirectoryCommandValidator
        : AbstractValidator<CreateDirectoryCommand>
    {
        public CreateDirectoryCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(DirectoryName.MaxLength);

            RuleFor(x => x.Sections)
                .NotEmpty();

            RuleForEach(x => x.Sections)
                .NotEmpty()
                .ChildRules(x =>
                {
                    x.RuleFor(x => x.Name)
                        .NotEmpty()
                        .MaximumLength(SectionName.MaxLength);

                    x.RuleFor(x => x.Content)
                        .NotEmpty()
                        .MaximumLength(SectionContent.MaxLength);
                })
                .When(x => x.Sections is not null);
        }
    }
}
