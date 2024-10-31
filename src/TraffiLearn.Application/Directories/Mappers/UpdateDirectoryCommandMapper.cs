using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Directories.Commands.Update;
using TraffiLearn.Application.Directories.DTO;
using TraffiLearn.Domain.Aggregates.Directories;
using TraffiLearn.Domain.Aggregates.Directories.DirectoryNames;
using TraffiLearn.Domain.Aggregates.Directories.DirectorySections;
using TraffiLearn.Domain.Aggregates.Directories.DirectorySections.SectionContents;
using TraffiLearn.Domain.Aggregates.Directories.DirectorySections.SectionNames;
using TraffiLearn.Domain.Shared;
using Directory = TraffiLearn.Domain.Aggregates.Directories.Directory;

namespace TraffiLearn.Application.Directories.Mappers
{
    internal sealed class UpdateDirectoryCommandMapper
        : Mapper<UpdateDirectoryCommand, Result<Directory>>
    {
        public override Result<Directory> Map(UpdateDirectoryCommand source)
        {
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
                new DirectoryId(Guid.NewGuid()),
                nameResult.Value,
                sections);
        }

        private Result<List<DirectorySection>> ParseSections(IEnumerable<DirectorySectionRequest?> requestSections)
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
