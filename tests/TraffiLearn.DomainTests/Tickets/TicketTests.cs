﻿using FluentAssertions;
using TraffiLearn.Domain.Tickets;
using TraffiLearn.Domain.Tickets.TicketNumbers;
using TraffiLearn.SharedKernel.Primitives;
using TraffiLearn.SharedKernel.Shared;
using TraffiLearn.Testing.Shared.Factories;

namespace TraffiLearn.DomainTests.Tickets
{
    public sealed class TicketTests
    {
#pragma warning disable CS8625

        [Fact]
        public void Create_IfPassedNullArgs_ShouldThrowArgumentNullException()
        {
            Action action = () =>
            {
                Ticket.Create(
                new TicketId(Guid.NewGuid()),
                null);
            };

            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Create_IfPassedValidArgs_ShouldBeSuccesful()
        {
            var validTicketNumber = TicketFixtureFactory.CreateNumber();
            var validId = new TicketId(Guid.NewGuid());

            var createResult = Ticket.Create(
                validId,
                validTicketNumber);

            createResult.IsSuccess.Should().BeTrue();

            var ticket = createResult.Value;

            ticket.Id.Should().Be(validId);
            ticket.TicketNumber.Should().Be(validTicketNumber);
            ticket.Questions.Should().BeEmpty();
        }

        [Fact]
        public void Update_IfPassedNullArgs_ShouldThrowArgumentNullException()
        {
            var ticket = TicketFixtureFactory.CreateTicket();

            Action action = () =>
            {
                ticket.Update(null!);
            };

            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Update_IfPassedValidArgs_ShouldBeSuccesful()
        {
            var ticket = TicketFixtureFactory.CreateTicket();

            var ticketNumber = TicketNumber.Create(TicketFixtureFactory.CreateNumber().Value + 1).Value;

            var updateResult = ticket.Update(ticketNumber);

            updateResult.IsSuccess.Should().BeTrue();
            ticket.TicketNumber.Should().Be(ticketNumber);
        }

        [Fact]
        public void AddQuestion_IfPassedNullArgs_ShouldThrowArgumentNullException()
        {
            var validTicket = TicketFixtureFactory.CreateTicket();

            Action action = () =>
            {
                validTicket.AddQuestion(null);
            };

            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void AddQuestion_IfSameQuestionAlreadyAdded_ShouldReturnError()
        {
            var validTicket = TicketFixtureFactory.CreateTicket();
            var question = QuestionFixtureFactory.CreateQuestion();

            validTicket.AddQuestion(question);

            var result = validTicket.AddQuestion(question);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().NotBe(Error.None);
        }

        [Fact]
        public void AddQuestion_IfValidCase_ShouldBeSuccesful()
        {
            var validTicket = TicketFixtureFactory.CreateTicket();
            var question = QuestionFixtureFactory.CreateQuestion();

            var result = validTicket.AddQuestion(question);

            result.IsSuccess.Should().BeTrue();

            validTicket.Questions.Should().HaveCount(1);
            validTicket.Questions.Should().Contain(question);
        }

        [Fact]
        public void RemoveQuestion_IfPassedNullArgs_ShouldThrowArgumentNullException()
        {
            var validTicket = TicketFixtureFactory.CreateTicket();

            Action action = () =>
            {
                validTicket.RemoveQuestion(null);
            };

            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void RemoveQuestion_IfQuestionNotPresent_ShouldReturnError()
        {
            var validTicket = TicketFixtureFactory.CreateTicket();
            var question = QuestionFixtureFactory.CreateQuestion();

            var result = validTicket.RemoveQuestion(question);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().NotBe(Error.None);
        }

        [Fact]
        public void RemoveQuestion_IfValidCase_ShouldBeSuccesful()
        {
            var validTicket = TicketFixtureFactory.CreateTicket();
            var question = QuestionFixtureFactory.CreateQuestion();

            validTicket.AddQuestion(question);

            var result = validTicket.RemoveQuestion(question);

            result.IsSuccess.Should().BeTrue();
            validTicket.Questions.Should().BeEmpty();
        }

        [Fact]
        public void Ticket_ShouldInheritFromEntity()
        {
            var type = typeof(Ticket);

            var isValueObject = typeof(Entity<TicketId>).IsAssignableFrom(type);
            isValueObject.Should().BeTrue("Ticket should inherit from Entity.");
        }

#pragma warning restore
    }
}
