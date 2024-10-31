using TraffiLearn.Domain.Directories.DirectoryNames;
using TraffiLearn.Domain.Directories.DirectorySections;
using TraffiLearn.SharedKernel.Primitives;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Domain.Directories
{
    public sealed class Directory : AggregateRoot<DirectoryId>
    {
        public const int MaxSectionsCount = 100;

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

            if (sections.Count > MaxSectionsCount)
            {
                return Result.Failure<Directory>(DirectoryErrors.TooManySections(MaxSectionsCount));
            }

            return new Directory(id, name, sections);
        }

        public Result Update(
            DirectoryName name,
            List<DirectorySection>? sections)
        {
            ArgumentNullException.ThrowIfNull(sections);

            if (sections.Count == 0)
            {
                return Result.Failure(DirectoryErrors.EmptySections);
            }

            if (sections.Count > MaxSectionsCount)
            {
                return Result.Failure(DirectoryErrors.TooManySections(MaxSectionsCount));
            }

            _sections = sections.ToHashSet();
            Name = name;

            return Result.Success();
        }
    }
}
