using TraffiLearn.Domain.Aggregates.Directories.Errors.Directories;
using TraffiLearn.Domain.Aggregates.Directories.ValueObjects.Directories;
using TraffiLearn.Domain.Aggregates.Directories.ValueObjects.Sections;
using TraffiLearn.Domain.Primitives;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Domain.Aggregates.Directories
{
    public sealed class Directory : AggregateRoot<DirectoryId>
    {
        private HashSet<DirectorySection> _sections = [];
        private DirectoryName _name;

        private Directory()
            : base(new(Guid.Empty))
        { }

        private Directory(
            DirectoryId id,
            DirectoryName name,
            List<DirectorySection> sections) : base(id)
        {
            Name = name;
            _sections = sections.ToHashSet();
        }

        public DirectoryName Name
        {
            get
            {
                return _name;
            }
            private set
            {
                ArgumentNullException.ThrowIfNull(nameof(value));

                _name = value;
            }
        }

        public IReadOnlyCollection<DirectorySection> Sections => _sections;

        public static Result<Directory> Create(
            DirectoryId id,
            DirectoryName name,
            List<DirectorySection> sections)
        {
            ArgumentNullException.ThrowIfNull(name);
            ArgumentNullException.ThrowIfNull(sections);

            if (sections.Count == 0)
            {
                return Result.Failure<Directory>(DirectoryErrors.EmptySections);
            }

            return new Directory(id, name, sections);
        }
    }
}
