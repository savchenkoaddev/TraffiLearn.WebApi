using FluentAssertions;
using TraffiLearn.Domain.Aggregates.Questions.QuestionContents;
using TraffiLearn.Domain.Aggregates.Regions.RegionNames;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.DomainTests.Regions
{
    public sealed class RegionNameTests
    {
        [Fact]
        public void Create_IfPassedInvalidArgs_ShouldReturnError()
        {
            Func<Result<RegionName>>[] actions = [
                () =>
                {
                    return RegionName.Create(null);
                },
                () =>
                {
                    return RegionName.Create(
                        new string('1', QuestionContent.MaxLength + 1));
                },
                () =>
                {
                    return RegionName.Create(" ");
                },
            ];

            actions.Should().AllSatisfy(action =>
            {
                action().IsFailure.Should().BeTrue();
            });
        }

        [Fact]
        public void Create_IfPassedValidArgs_ShouldBeSuccesful()
        {
            var value = "string";
            var result = RegionName.Create(value);

            result.IsSuccess.Should().BeTrue();

            var content = result.Value;

            content.Value.Should().Be(value);
        }
    }
}
