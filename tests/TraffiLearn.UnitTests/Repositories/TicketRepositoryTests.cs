using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using TraffiLearn.Domain.Questions;
using TraffiLearn.Domain.Tickets;
using TraffiLearn.Infrastructure.Persistence.Repositories;
using TraffiLearn.Testing.Shared.Factories;
using TraffiLearn.UnitTests.Abstractions;

namespace TraffiLearn.UnitTests.Repositories
{
    public sealed class TicketRepositoryTests : BaseRepositoryTest
    {
        private readonly TicketRepository _repository;

        public TicketRepositoryTests()
        {
            _repository = new TicketRepository(DbContext);
        }

        [Fact]
        public async Task InsertAsync_ShouldAddTicket()
        {
            // Arrange
            var ticket = TicketFixtureFactory.CreateTicket();

            // Act
            await _repository.InsertAsync(ticket);
            await DbContext.SaveChangesAsync();

            // Assert
            DbContext.Tickets.Should().Contain(ticket);
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteTicket()
        {
            // Arrange
            var ticket = TicketFixtureFactory.CreateTicket();

            await AddRangeAndSaveAsync(ticket);

            // Act
            await _repository.DeleteAsync(ticket);
            await DbContext.SaveChangesAsync();

            // Assert
            DbContext.Tickets.Should().NotContain(ticket);
        }

        [Fact]
        public async Task ExistsAsync_IfTicketExists_ShouldReturnTrue()
        {
            // Arrange
            var ticket = TicketFixtureFactory.CreateTicket();

            await AddRangeAndSaveAsync(ticket);

            // Act
            var result = await _repository.ExistsAsync(ticket.Id);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task ExistsAsync_IfTicketDoesNotExist_ShouldReturnFalse()
        {
            // Arrange
            var ticketId = new TicketId(Guid.NewGuid());

            // Act
            var result = await _repository.ExistsAsync(ticketId);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateTicket()
        {
            // Arrange
            var ticket = TicketFixtureFactory.CreateTicket();

            var updatedTicket = Ticket.Create(
                ticket.Id,
                TicketFixtureFactory.CreateNumber(2))
                .Value;

            await AddRangeAndSaveAsync(ticket);
            DbContext.Entry(ticket).State = EntityState.Detached;

            // Act
            await _repository.UpdateAsync(updatedTicket);
            await DbContext.SaveChangesAsync();

            // Assert
            DbContext.Tickets.Should().Contain(updatedTicket);
            DbContext.Tickets.First(t => t.Id == ticket.Id)
                .Should().BeEquivalentTo(updatedTicket);
        }

        [Fact]
        public async Task GetByIdAsync_IfTicketExists_ShouldReturnTicket()
        {
            // Arrange
            var ticket = TicketFixtureFactory.CreateTicket();

            await AddRangeAndSaveAsync(ticket);

            // Act
            var result = await _repository.GetByIdAsync(ticket.Id);

            // Assert
            result.Should().Be(ticket);
        }

        [Fact]
        public async Task GetByIdAsync_IfTicketDoesNotExist_ShouldReturnNull()
        {
            // Arrange
            var ticketId = new TicketId(Guid.NewGuid());

            // Act
            var result = await _repository.GetByIdAsync(ticketId);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetByIdWithQuestionsAsync_IfTicketExists_ShouldReturnTicket()
        {
            // Arrange
            var ticket = TicketFixtureFactory.CreateTicket();

            var question = QuestionFixtureFactory.CreateQuestion();

            ticket.AddQuestion(question);

            await AddRangeAndSaveAsync(ticket, question);

            // Act
            var result = await _repository.GetByIdWithQuestionsAsync(ticket.Id);

            // Assert
            result.Should().NotBeNull().And.Be(ticket);
            result!.Questions.Should().Contain(question);
        }

        [Fact]
        public async Task GetByIdWithQuestionsAsync_IfTicketDoesNotExist_ShouldReturnNull()
        {
            // Arrange
            var ticketId = new TicketId(Guid.NewGuid());

            // Act
            var result = await _repository.GetByIdWithQuestionsAsync(ticketId);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetManyByQuestionIdAsync_IfQuestionExists_ShouldReturnTickets()
        {
            // Arrange
            var ticket1 = TicketFixtureFactory.CreateTicket(number: 1);

            var ticket2 = TicketFixtureFactory.CreateTicket(number: 2);

            var question = QuestionFixtureFactory.CreateQuestion();

            ticket1.AddQuestion(question);
            ticket2.AddQuestion(question);

            await AddRangeAndSaveAsync(ticket1, ticket2, question);

            // Act
            var result = await _repository.GetManyByQuestionIdAsync(question.Id);

            // Assert
            result.Should().HaveCount(2);
            result.Should()
                .Contain(ticket1)
                .And
                .Contain(ticket2);
        }

        [Fact]
        public async Task GetManyByQuestionIdAsync_IfQuestionDoesNotExist_ShouldReturnEmptyCollection()
        {
            // Arrange
            var questionId = new QuestionId(Guid.NewGuid());

            // Act
            var result = await _repository.GetManyByQuestionIdAsync(questionId);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetManyByQuestionIdAsync_IfQuestionHasNoTickets_ShouldReturnEmptyCollection()
        {
            // Arrange
            var question = QuestionFixtureFactory.CreateQuestion();

            await AddRangeAndSaveAsync(question);

            // Act
            var result = await _repository.GetManyByQuestionIdAsync(question.Id);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAllAsync_IfTicketsExist_ShouldReturnAllTickets()
        {
            // Arrange
            var ticket1 = TicketFixtureFactory.CreateTicket(number: 1);

            var ticket2 = TicketFixtureFactory.CreateTicket(number: 2);

            await AddRangeAndSaveAsync(ticket1, ticket2);

            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            result.Should().NotBeEmpty().And.HaveCount(2);
            result.Should()
                .Contain(ticket1)
                .And
                .Contain(ticket2);
        }

        [Fact]
        public async Task GetAllAsync_IfTicketsDoNotExist_ShouldReturnEmptyCollection()
        {
            // Arrange

            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            result.Should().BeEmpty();
        }
    }
}
