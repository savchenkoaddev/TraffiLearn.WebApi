using FluentAssertions;
using TraffiLearn.Domain.Regions;
using TraffiLearn.Domain.Regions.RegionNames;
using TraffiLearn.Testing.Shared.Factories;

namespace TraffiLearn.DomainTests.Regions
{
    public sealed class RegionTests
    {
        [Fact]
        public void Create_IfPassedNullArgs_ShouldThrowArgumentNullException()
        {
            Action action = () =>
            {
                Region.Create(
                    new RegionId(Guid.NewGuid()),
                    null!);
            };

            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Create_IfPassedValidArgs_ShouldBeSuccesful()
        {
            var validRegionName = RegionFixtureFactory.CreateRegionName();
            var validId = new RegionId(Guid.NewGuid());

            var createResult = Region.Create(
                validId,
                validRegionName);

            createResult.IsSuccess.Should().BeTrue();

            var region = createResult.Value;

            region.Id.Should().Be(validId);
            region.Name.Should().Be(validRegionName);
        }

        [Fact]
        public void Update_IfPassedNullArgs_ShouldThrowArgumentNullException()
        {
            var region = RegionFixtureFactory.CreateRegion();

            Action action = () =>
            {
                region.Update(null!);
            };

            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Update_IfPassedValidArgs_ShouldBeSuccesful()
        {
            var region = RegionFixtureFactory.CreateRegion();

            var newRegionName = RegionName.Create(RegionFixtureFactory.CreateRegionName().Value + '1').Value;

            var updateResult = region.Update(newRegionName);

            updateResult.IsSuccess.Should().BeTrue();
            region.Name.Should().Be(newRegionName);
        }
    }
}
