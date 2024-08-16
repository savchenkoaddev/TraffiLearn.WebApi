using FluentAssertions;
using TraffiLearn.Domain.Aggregates.Topics.ValueObjects;
using TraffiLearn.Domain.Primitives;
using TraffiLearn.Domain.Shared;
using TraffiLearn.DomainTests.Factories;

namespace TraffiLearn.DomainTests.Topics
{
    public class TopicTests
    {
        [Fact]
        public void Create_IfPassedNullArgs_ShouldThrowArgumentNullException()
        {
            Action[] actions = [
                () =>
                {
                    TopicId.Create(
                    new TopicId(Guid.NewGuid()),
                    null,
                    null);
                },
                () =>
                {
                    TopicId.Create(
                    new TopicId(Guid.NewGuid()),
                    null,
                    TopicTitle.Create("Value").Value);
                },
                () =>
                {
                    TopicId.Create(
                    new TopicId(Guid.NewGuid()),
                    TopicNumber.Create(1).Value,
                    null);
                },
            ];

            actions.Should().AllSatisfy(action => action.Should().Throw<ArgumentNullException>());
        }

        [Fact]
        public void Create_IfPassedValidArgs_ShouldBeSuccesful()
        {
            var validTopicTitle = TopicFixtureFactory.CreateTitle();
            var validTopicNumber = TopicFixtureFactory.CreateNumber();
            var validId = new Domain.Aggregates.Topics.ValueObjects.TopicId(Guid.NewGuid());

            var result = TopicId.Create(
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
        public void AddQuestion_IfPassedNullArgs_ShouldThrowArgumentNullException()
        {
            var topic = TopicFixtureFactory.CreateTopic();

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
            var topic = TopicFixtureFactory.CreateTopic();

            var question = QuestionFixtureFactory.CreateQuestion();

            topic.AddQuestion(question);

            var addSameQuestionResult = topic.AddQuestion(question);

            addSameQuestionResult.IsFailure.Should().BeTrue();
            addSameQuestionResult.Error.Should().NotBe(Error.None);
        }

        [Fact]
        public void AddQuestion_IfPassedValidArgs_ShouldBeSuccesful()
        {
            var topic = TopicFixtureFactory.CreateTopic();

            var question = QuestionFixtureFactory.CreateQuestion();

            var addResult = topic.AddQuestion(question);

            addResult.IsSuccess.Should().BeTrue();
            topic.Questions.Should().HaveCount(1);
            topic.Questions.Should().Contain(question);
        }

        [Fact]
        public void RemoveQuestion_IfPassedNullArgs_ShouldThrowArgumentNullException()
        {
            var topic = TopicFixtureFactory.CreateTopic();

            Action action = () =>
            {
                topic.RemoveQuestion(null);
            };

            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void RemoveQuestion_IfQuestionNotPresent_ShouldReturnError()
        {
            var topic = TopicFixtureFactory.CreateTopic();

            var question = QuestionFixtureFactory.CreateQuestion();

            var removeResult = topic.RemoveQuestion(question);

            removeResult.IsFailure.Should().BeTrue();
            removeResult.Error.Should().NotBe(Error.None);
        }

        [Fact]
        public void RemoveQuestion_IfPassedValidArgs_ShouldBeSuccesful()
        {
            var topic = TopicFixtureFactory.CreateTopic();

            var question = QuestionFixtureFactory.CreateQuestion();

            topic.AddQuestion(question);

            var removeResult = topic.RemoveQuestion(question);

            removeResult.IsSuccess.Should().BeTrue();
            topic.Questions.Should().BeEmpty();
        }

        [Fact]
        public void Update_IfPassedNullArgs_ShouldThrowArgumentNullException()
        {
            var validTopic = TopicFixtureFactory.CreateTopic();
            var validNumber = TopicFixtureFactory.CreateNumber();
            var validTitle = TopicFixtureFactory.CreateTitle();

            Action[] actions = [
                () => { validTopic.Update(null, null); },
                () => { validTopic.Update(null, validTitle); },
                () => { validTopic.Update(validNumber, null); },
            ];

            actions.Should().AllSatisfy(
                action => action.Should().Throw<ArgumentNullException>());
        }

        [Fact]
        public void Update_IfPassedValidArgs_ShouldBeSuccesful()
        {
            var validTopic = TopicFixtureFactory.CreateTopic();

            var newNumber = TopicNumber.Create(validTopic.Number.Value + 1).Value;
            var newTitle = TopicTitle.Create(validTopic.Title.Value + '1').Value;

            var updateResult = validTopic.Update(
                newNumber,
                newTitle);

            updateResult.IsSuccess.Should().BeTrue();

            validTopic.Number.Should().Be(newNumber);
            validTopic.Title.Should().Be(newTitle);
        }

        [Fact]
        public void Topic_ShouldInheritFromEntity()
        {
            var type = typeof(Domain.Aggregates.Topics.Topic);

            var isValueObject = typeof(Entity<Domain.Aggregates.Topics.ValueObjects.TopicId>).IsAssignableFrom(type);
            isValueObject.Should().BeTrue("Topic should inherit from Entity.");
        }
    }
}