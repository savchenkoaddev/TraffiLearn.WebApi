using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using TraffiLearn.Domain.Regions;
using TraffiLearn.Infrastructure.Persistence.Repositories;
using TraffiLearn.Testing.Shared.Factories;
using TraffiLearn.UnitTests.Abstractions;

namespace TraffiLearn.UnitTests.Repositories
{
    public sealed class RegionRepositoryTests : BaseRepositoryTest
    {
        private readonly RegionRepository _repository;

        public RegionRepositoryTests()
        {
            _repository = new RegionRepository(DbContext);
        }

        [Fact]
        public async Task InsertAsync_IfPassedNullArgs_ShouldThrowArgumentNullException()
        {
            Func<Task> action = async () =>
            {
                await _repository.InsertAsync(null!);
            };

            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task InsertAsync_IfPassedValidArgs_ShouldAddRegion()
        {
            // Arrange
            var region = RegionFixtureFactory.CreateRegion();

            // Act
            await _repository.InsertAsync(region);
            await DbContext.SaveChangesAsync();

            // Assert
            DbContext.Regions.Should().Contain(region);
        }

        [Fact]
        public async Task DeleteAsync_IfPassedNullArgs_ShouldThrowArgumentNullException()
        {
            Func<Task> action = async () =>
            {
                await _repository.DeleteAsync(null!);
            };

            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task DeleteAsync_IfPassedValidArgs_ShouldDeleteRegion()
        {
            // Arrange
            var region = RegionFixtureFactory.CreateRegion();

            await AddRangeAndSaveAsync(region);

            // Act
            await _repository.DeleteAsync(region);
            await DbContext.SaveChangesAsync();

            // Assert
            DbContext.Regions.Should().NotContain(region);
        }

        [Fact]
        public async Task ExistsAsync_IfRegionExists_ShouldReturnTrue()
        {
            // Arrange
            var region = RegionFixtureFactory.CreateRegion();

            await AddRangeAndSaveAsync(region);

            // Act
            var result = await _repository.ExistsAsync(region.Id);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task ExistsAsync_IfRegionDoesNotExist_ShouldReturnFalse()
        {
            // Arrange
            var regionId = new RegionId(Guid.NewGuid());

            // Act
            var result = await _repository.ExistsAsync(regionId);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task UpdateAsync_IfPassedNullArgs_ShouldThrowArgumentNullException()
        {
            Func<Task> action = async () =>
            {
                await _repository.UpdateAsync(null!);
            };

            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task UpdateAsync_IfPassedValidArgs_ShouldUpdateRegion()
        {
            // Arrange
            var region = RegionFixtureFactory.CreateRegion();

            var updatedRegion = Region.Create(
                region.Id,
                RegionFixtureFactory.CreateRegionName()).Value;

            await AddRangeAndSaveAsync(region);
            DbContext.Entry(region).State = EntityState.Detached;

            // Act
            await _repository.UpdateAsync(updatedRegion);
            await DbContext.SaveChangesAsync();

            // Assert
            DbContext.Regions.Should().Contain(updatedRegion);
            DbContext.Regions.First(t => t.Id == region.Id)
                .Should().BeEquivalentTo(updatedRegion);
        }

        [Fact]
        public async Task GetByIdAsync_IfRegionExists_ShouldReturnRegion()
        {
            // Arrange
            var region = RegionFixtureFactory.CreateRegion();

            await AddRangeAndSaveAsync(region);

            // Act
            var result = await _repository.GetByIdAsync(region.Id);

            // Assert
            result.Should().Be(region);
        }

        [Fact]
        public async Task GetByIdAsync_IfRegionDoesNotExist_ShouldReturnNull()
        {
            // Arrange
            var regionId = new RegionId(Guid.NewGuid());

            // Act
            var result = await _repository.GetByIdAsync(regionId);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetAllAsync_IfRegionsExist_ShouldReturnAllRegions()
        {
            // Arrange
            var region1 = RegionFixtureFactory.CreateRegion();

            var region2 = RegionFixtureFactory.CreateRegion();

            await AddRangeAndSaveAsync(region1, region2);

            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            result.Should().NotBeEmpty().And.HaveCount(2);
            result.Should()
                .Contain(region1)
                .And
                .Contain(region2);
        }

        [Fact]
        public async Task GetAllAsync_IfNoRegionsExist_ShouldReturnEmptyCollection()
        {
            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            result.Should().BeEmpty();
            result.Should().NotBeNull();
        }
    }
}
