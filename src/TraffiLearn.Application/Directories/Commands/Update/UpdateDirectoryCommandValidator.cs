using FluentValidation;
using TraffiLearn.Domain.Aggregates.Directories.DirectoryNames;
using TraffiLearn.Domain.Aggregates.Directories.DirectorySections.SectionContents;
using TraffiLearn.Domain.Aggregates.Directories.DirectorySections.SectionNames;

namespace TraffiLearn.Application.Directories.Commands.Update
{
    internal sealed class UpdateDirectoryCommandValidator
        : AbstractValidator<UpdateDirectoryCommand>
    {
        public UpdateDirectoryCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty();

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
