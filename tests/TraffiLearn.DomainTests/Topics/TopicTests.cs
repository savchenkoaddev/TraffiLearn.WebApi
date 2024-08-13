using FluentAssertions;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.Primitives;
using TraffiLearn.Domain.Shared;
using TraffiLearn.Domain.ValueObjects.Topics;
using TraffiLearn.DomainTests.Factories;

namespace TraffiLearn.DomainTests.Topics
{
    public class TopicTests
    {
        [Fact]
        public void Create_NullParameters_ShouldThrowArgumentNullException()
        {
            Action[] actions = [
                () =>
                {
                    Topic.Create(
                    new TopicId(Guid.NewGuid()),
                    null,
                    null);
                },
                () =>
                {
                    Topic.Create(
                    new TopicId(Guid.NewGuid()),
                    null,
                    TopicTitle.Create("Value").Value);
                },
                () =>
                {
                    Topic.Create(
                    new TopicId(Guid.NewGuid()),
                    TopicNumber.Create(1).Value,
                    null);
                },
            ];

            actions.Should().AllSatisfy(action => action.Should().Throw<ArgumentNullException>());
        }

        [Fact]
        public void Create_Valid_ShouldBeSuccesful()
        {
            var validTopicTitle = TopicFixtureFactory.CreateValidTitle();
            var validTopicNumber = TopicFixtureFactory.CreateValidNumber();
            var validId = new TopicId(Guid.NewGuid());

            var result = Topic.Create(
                validId,
                validTopicNumber,
                validTopicTitle);

            result.IsSuccess.Should().BeTrue();

            var topic = result.Value;

            topic.Number.Should().Be(validTopicNumber);
            topic.Title.Should().Be(validTopicTitle);
            topic.Id.Should().Be(validId);

            topic.Questions.Should().BeEmpty();
        }

        [Fact]
        public void Topic_ShouldInheritFromEntity()
        {
            var type = typeof(Topic);

            var isValueObject = typeof(Entity<TopicId>).IsAssignableFrom(type);
            isValueObject.Should().BeTrue("Topic should inherit from Entity.");
        }

        [Fact]
        public void AddQuestion_NullParameter_ShouldThrowArgumentNullException()
        {
            var topic = TopicFixtureFactory.CreateValidTopic();

            Action action = () =>
            {
                topic.AddQuestion(null);
            };

            // Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void AddQuestion_IfAlreadyAddedSameQuestion_ShouldReturnError()
        {
            var topic = TopicFixtureFactory.CreateValidTopic();

            var question = QuestionFixtureFactory.CreateValidQuestion();

            topic.AddQuestion(question);

            var addSameQuestionResult = topic.AddQuestion(question);

            addSameQuestionResult.IsFailure.Should().BeTrue();
            addSameQuestionResult.Error.Should().NotBe(Error.None);
        }

        [Fact]
        public void AddQuestion_Valid_ShouldBeSuccesful()
        {
            var topic = TopicFixtureFactory.CreateValidTopic();

            var question = QuestionFixtureFactory.CreateValidQuestion();

            var addResult = topic.AddQuestion(question);

            addResult.IsSuccess.Should().BeTrue();
            topic.Questions.Should().Contain(question);
        }

        [Fact]
        public void RemoveQuestion_NullParameter_ShouldThrowArgumentNullException()
        {
            var topic = TopicFixtureFactory.CreateValidTopic();

            Action action = () =>
            {
                topic.RemoveQuestion(null);
            };

            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void RemoveQuestion_IfQuestionNotPresent_ShouldReturnError()
        {
            var topic = TopicFixtureFactory.CreateValidTopic();

            var question = QuestionFixtureFactory.CreateValidQuestion();

            var removeResult = topic.RemoveQuestion(question);

            removeResult.IsFailure.Should().BeTrue();
            removeResult.Error.Should().NotBe(Error.None);
        }

        [Fact]
        public void RemoveQuestion_Valid_ShouldBeSuccesful()
        {
            var topic = TopicFixtureFactory.CreateValidTopic();

            var question = QuestionFixtureFactory.CreateValidQuestion();

            topic.AddQuestion(question);

            var removeResult = topic.RemoveQuestion(question);

            removeResult.IsSuccess.Should().BeTrue();
            topic.Questions.Should().NotContain(question);
        }

        [Fact]
        public void Update_NullParameters_ShouldThrowArgumentNullException()
        {
            var validTopic = TopicFixtureFactory.CreateValidTopic();
            var validNumber = TopicFixtureFactory.CreateValidNumber();
            var validTitle = TopicFixtureFactory.CreateValidTitle();

            Action[] actions = [
                () => { validTopic.Update(null, null); },
                () => { validTopic.Update(null, validTitle); },
                () => { validTopic.Update(validNumber, null); },
            ];

            actions.Should().AllSatisfy(
                action => action.Should().Throw<ArgumentNullException>());
        }

        [Fact]
        public void Update_Valid_ShouldBeSuccesful()
        {
            var validTopic = TopicFixtureFactory.CreateValidTopic();

            var newNumber = TopicNumber.Create(validTopic.Number.Value + 1).Value;
            var newTitle = TopicTitle.Create(validTopic.Title.Value + '1').Value;

            var updateResult = validTopic.Update(
                newNumber,
                newTitle);

            updateResult.IsSuccess.Should().BeTrue();

            validTopic.Number.Should().Be(newNumber);
            validTopic.Title.Should().Be(newTitle);
        }
    }
}