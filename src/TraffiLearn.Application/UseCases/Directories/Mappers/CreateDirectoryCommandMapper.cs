using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.UseCases.Directories.Commands.Create;
using TraffiLearn.Application.UseCases.Directories.DTO;
using TraffiLearn.Domain.Directories;
using TraffiLearn.Domain.Directories.DirectoryNames;
using TraffiLearn.Domain.Directories.DirectorySections;
using TraffiLearn.Domain.Directories.DirectorySections.SectionContents;
using TraffiLearn.Domain.Directories.DirectorySections.SectionNames;
using TraffiLearn.SharedKernel.Shared;
using Directory = TraffiLearn.Domain.Directories.Directory;

namespace TraffiLearn.Application.UseCases.Directories.Mappers
{
    internal sealed class CreateDirectoryCommandMapper
        : Mapper<CreateDirectoryCommand, Result<Directory>>
    {
        public override Result<Directory> Map(CreateDirectoryCommand source)
        {
            DirectoryId directoryId = new(Guid.NewGuid());

            var sectionsResult = ParseSections(source.Sections);

            if (sectionsResult.IsFailure)
            {
                return Result.Failure<Directory>(sectionsResult.Error);
            }

            var sections = sectionsResult.Value;

            Result<DirectoryName> nameResult = DirectoryName.Create(source.Name);

            if (nameResult.IsFailure)
            {
                return Result.Failure<Directory>(nameResult.Error);
            }

            return Directory.Create(
                directoryId,
                nameResult.Value,
                sections);
        }

        private Result<List<DirectorySection>> ParseSections(IEnumerable<DirectorySectionRequest> requestSections)
        {
            List<DirectorySection> sections = [];

            foreach (var section in requestSections)
            {
                var sectionNameCreateResult = SectionName.Create(section.Name);

                if (sectionNameCreateResult.IsFailure)
                {
                    return Result.Failure<List<DirectorySection>>(sectionNameCreateResult.Error);
                }

                var sectionName = sectionNameCreateResult.Value;

                var sectionContentCreateResult = SectionContent.Create(section.Content);

                if (sectionContentCreateResult.IsFailure)
                {
                    return Result.Failure<List<DirectorySection>>(sectionContentCreateResult.Error);
                }

                var sectionContent = sectionContentCreateResult.Value;

                var sectionCreateResult = DirectorySection.Create(
                    name: sectionName,
                    content: sectionContent);

                if (sectionCreateResult.IsFailure)
                {
                    return Result.Failure<List<DirectorySection>>(sectionCreateResult.Error);
                }

                sections.Add(sectionCreateResult.Value);
            }

            return Result.Success(sections);
        }
    }
}
