using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.UseCases.Directories.DTO;
using Directory = TraffiLearn.Domain.Directories.Directory;

namespace TraffiLearn.Application.UseCases.Directories.Mappers
{
    public sealed class DirectoryToDirectoryResponseMapper
        : Mapper<Directory, DirectoryResponse>
    {
        public override DirectoryResponse Map(Directory source)
        {
            return new DirectoryResponse(
                Id: source.Id.Value,
                Name: source.Name.Value,
                Sections: source.Sections.Select(
                    x => new DirectorySectionResponse(
                        x.Name.Value,
                        x.Content.Value)).ToList());
        }
    }
}
