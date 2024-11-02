using TraffiLearn.Domain.Directories.DirectorySections.SectionContents;
using TraffiLearn.Domain.Directories.DirectorySections.SectionNames;
using TraffiLearn.SharedKernel.Primitives;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Domain.Directories.DirectorySections
{
    public sealed class DirectorySection : ValueObject
    {
        private DirectorySection(
            SectionName name,
            SectionContent content)
        {
            Name = name;
            Content = content;
        }

        public SectionName Name { get; }

        public SectionContent Content { get; }

        public static Result<DirectorySection> Create(
            SectionName name,
            SectionContent content)
        {
            ArgumentNullException.ThrowIfNull(name);
            ArgumentNullException.ThrowIfNull(content);

            return new DirectorySection(name, content);
        }

        public override IEnumerable<object> GetAtomicValues()
        {
            yield return Name;
            yield return Content;
        }
    }
}
