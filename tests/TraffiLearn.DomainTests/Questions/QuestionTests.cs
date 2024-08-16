using FluentAssertions;
using TraffiLearn.Domain.Aggregates.Questions;
using TraffiLearn.Domain.Aggregates.Questions.ValueObjects;
using TraffiLearn.Domain.Primitives;
using TraffiLearn.Domain.Shared;
using TraffiLearn.DomainTests.Factories;

namespace TraffiLearn.DomainTests.Questions
{
    public sealed class QuestionTests
    {
        [Fact]
        public void Create_IfPassedNullArgs_ShouldThrowArgumentNullException()
        {
            Action[] actions = [
                () =>
                {
                    Question.Create(
                        new QuestionId(Guid.NewGuid()),
                        null,
                        QuestionFixtureFactory.CreateExplanation(),
                        QuestionFixtureFactory.CreateNumber(),
                        QuestionFixtureFactory.CreateAnswers(),
                        null);
                },
                () =>
                {
                    Question.Create(
                        new QuestionId(Guid.NewGuid()),
                        QuestionFixtureFactory.CreateContent(),
                        null,
                        QuestionFixtureFactory.CreateNumber(),
                        QuestionFixtureFactory.CreateAnswers(),
                        null);
                },
                () =>
                {
                    Question.Create(
                        new QuestionId(Guid.NewGuid()),
                        QuestionFixtureFactory.CreateContent(),
                        QuestionFixtureFactory.CreateExplanation(),
                        null,
                        QuestionFixtureFactory.CreateAnswers(),
                        null);
                },
                () =>
                {
                    Question.Create(
                        new QuestionId(Guid.NewGuid()),
                        QuestionFixtureFactory.CreateContent(),
                        QuestionFixtureFactory.CreateExplanation(),
                        QuestionFixtureFactory.CreateNumber(),
                        null,
                        null);
                },
            ];

            actions.Should().AllSatisfy(action =>
            {
                action.Should().Throw<ArgumentNullException>();
            });
        }

        [Fact]
        public void Create_IfPassedNullImageUriWhenOtherArgsValid_ShouldBeSuccesful()
        {
            var result = Question.Create(
                        new QuestionId(Guid.NewGuid()),
                        QuestionFixtureFactory.CreateContent(),
                        QuestionFixtureFactory.CreateExplanation(),
                        QuestionFixtureFactory.CreateNumber(),
                        QuestionFixtureFactory.CreateAnswers(),
                        null);

            result.IsSuccess.Should().BeTrue();
            result.Value.ImageUri.Should().BeNull();
        }

        [Fact]
        public void Create_IfPassedInvalidAnswers_ShouldReturnError()
        {
            List<Result<Question>> results =
            [
                Question.Create(
                    new QuestionId(Guid.NewGuid()),
                    QuestionFixtureFactory.CreateContent(),
                    QuestionFixtureFactory.CreateExplanation(),
                    QuestionFixtureFactory.CreateNumber(),
                    [],
                    null),

                Question.Create(
                    new QuestionId(Guid.NewGuid()),
                    QuestionFixtureFactory.CreateContent(),
                    QuestionFixtureFactory.CreateExplanation(),
                    QuestionFixtureFactory.CreateNumber(),
                    [
                        Answer.Create("1", false).Value,
                        Answer.Create("2", false).Value
                    ],
                    null),

                Question.Create(
                    new QuestionId(Guid.NewGuid()),
                    QuestionFixtureFactory.CreateContent(),
                    QuestionFixtureFactory.CreateExplanation(),
                    QuestionFixtureFactory.CreateNumber(),
                    [
                        Answer.Create("1", false).Value,
                        Answer.Create("1", true).Value
                    ],
                    null)
            ];

            results.Should().AllSatisfy(result =>
            {
                result.Error.Should().NotBe(Error.None);
            });
        }

        [Fact]
        public void Create_IfPassedValidArgs_ShouldBeSuccesful()
        {
            var id = new QuestionId(Guid.NewGuid());
            var content = QuestionFixtureFactory.CreateContent();
            var explanation = QuestionFixtureFactory.CreateExplanation();
            var number = QuestionFixtureFactory.CreateNumber();
            var answers = QuestionFixtureFactory.CreateAnswers();
            var imageUri = QuestionFixtureFactory.CreateImageUri();

            var nullUriResult = Question.Create(
                id,
                content,
                explanation,
                number,
                answers,
                null);

            nullUriResult.IsSuccess.Should().BeTrue();

            var notNullUriResult = Question.Create(
                id,
                content,
                explanation,
                number,
                answers,
                imageUri);

            notNullUriResult.IsSuccess.Should().BeTrue();

            nullUriResult.Value.Id.Should().Be(id);
            nullUriResult.Value.Content.Should().Be(content);
            nullUriResult.Value.Explanation.Should().Be(explanation);
            nullUriResult.Value.QuestionNumber.Should().Be(number);
            nullUriResult.Value.Answers.Should().BeEquivalentTo(answers);
            nullUriResult.Value.ImageUri.Should().BeNull();
            nullUriResult.Value.LikesCount.Should().Be(0);
            nullUriResult.Value.DislikesCount.Should().Be(0);
            nullUriResult.Value.LikedByUsersIds.Should().BeEmpty();
            nullUriResult.Value.DislikedByUsersIds.Should().BeEmpty();
            nullUriResult.Value.CommentIds.Should().BeEmpty();
            nullUriResult.Value.TicketIds.Should().BeEmpty();
            nullUriResult.Value.TopicIds.Should().BeEmpty();

            notNullUriResult.Value.ImageUri.Should().Be(imageUri);
        }

        [Fact]
        public void Update_IfPassedNullArgs_ShouldThrowArgumentNullException()
        {
            var question = QuestionFixtureFactory.CreateQuestion();

            Action[] actions = [
                () =>
                {
                    question.Update(
                        null,
                        QuestionFixtureFactory.CreateExplanation(),
                        QuestionFixtureFactory.CreateNumber(),
                        QuestionFixtureFactory.CreateAnswers(),
                        null);
                },
                () =>
                {
                    question.Update(
                        QuestionFixtureFactory.CreateContent(),
                        null,
                        QuestionFixtureFactory.CreateNumber(),
                        QuestionFixtureFactory.CreateAnswers(),
                        null);
                },
                () =>
                {
                    question.Update(
                        QuestionFixtureFactory.CreateContent(),
                        QuestionFixtureFactory.CreateExplanation(),
                        null,
                        QuestionFixtureFactory.CreateAnswers(),
                        null);
                },
                () =>
                {
                    question.Update(
                        QuestionFixtureFactory.CreateContent(),
                        QuestionFixtureFactory.CreateExplanation(),
                        QuestionFixtureFactory.CreateNumber(),
                        null,
                        null);
                },
            ];

            actions.Should().AllSatisfy(action =>
            {
                action.Should().Throw<ArgumentNullException>();
            });
        }

        [Fact]
        public void Update_IfPassedNullImageUriWhenOtherArgsValid_ShouldBeSuccesful()
        {
            var question = QuestionFixtureFactory.CreateQuestion();

            var result = question.Update(
                        QuestionFixtureFactory.CreateContent(),
                        QuestionFixtureFactory.CreateExplanation(),
                        QuestionFixtureFactory.CreateNumber(),
                        QuestionFixtureFactory.CreateAnswers(),
                        null);

            result.IsSuccess.Should().BeTrue();
            question.ImageUri.Should().BeNull();
        }

        [Fact]
        public void Update_IfPassedInvalidAnswersWhenOtherArgsValid_ShouldReturnError()
        {
            var question = QuestionFixtureFactory.CreateQuestion();

            List<List<Answer>> cases = [
                [],

                [
                    Answer.Create("value", false).Value,
                ],

                [
                    Answer.Create("value1", false).Value,
                    Answer.Create("value2", false).Value,
                ],

                [
                    Answer.Create("value1", true).Value,
                    Answer.Create("value1", false).Value,
                ],
            ];

            foreach (var @case in cases)
            {
                var updateResult = question.Update(
                    question.Content,
                    question.Explanation,
                    question.QuestionNumber,
                    @case,
                    question.ImageUri);

                updateResult.IsFailure.Should().BeTrue();
                updateResult.Error.Should().NotBe(Error.None);
            }
        }

        [Fact]
        public void Update_IfPassedValidArgs_ShouldBeSuccesful()
        {
            var question = QuestionFixtureFactory.CreateQuestion();

            var content = QuestionContent.Create(
                question.Content.Value + "1").Value;

            var explanation = QuestionExplanation.Create(
                question.Explanation.Value + "1").Value;

            var number = QuestionNumber.Create(
                question.QuestionNumber.Value + 1).Value;

            var answers = question.Answers.ToList();
            answers.Add(QuestionFixtureFactory.CreateAnswer());

            if (question.ImageUri is null)
            {
                question.SetImageUri(QuestionFixtureFactory.CreateImageUri());
            }

            var updateResult = question.Update(
                content,
                explanation,
                number,
                answers,
                null);

            updateResult.IsSuccess.Should().BeTrue();

            question.Content.Should().Be(content);
            question.Explanation.Should().Be(explanation);
            question.QuestionNumber.Should().Be(number);
            question.Answers.Should().BeEquivalentTo(answers);
            question.ImageUri.Should().BeNull();
        }

        [Fact]
        public void AddAnswer_IfPassedNullArgs_ShouldThrowArgumentNullException()
        {
            var question = QuestionFixtureFactory.CreateQuestion();

            Action action = () =>
            {
                question.AddAnswer(null);
            };

            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void AddAnswer_IfAnswerAlreadyAdded_ShouldReturnError()
        {
            var question = QuestionFixtureFactory.CreateQuestion();

            var answer = question.Answers.First();

            var result = question.AddAnswer(answer);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().NotBe(Error.None);
        }

        [Fact]
        public void AddAnswer_IfPassedValidArgs_ShouldBeSuccesful()
        {
            var question = QuestionFixtureFactory.CreateQuestion();

            var countBefore = question.Answers.Count();

            var answer = Answer.Create(Guid.NewGuid().ToString(), true).Value;

            var result = question.AddAnswer(answer);

            result.IsSuccess.Should().BeTrue();
            question.Answers.Should().HaveCount(countBefore + 1);
            question.Answers.Should().Contain(answer);
        }

        [Fact]
        public void RemoveAnswer_IfPassedNullArgs_ShouldThrowArgumentNullException()
        {
            var question = QuestionFixtureFactory.CreateQuestion();

            Action action = () =>
            {
                question.RemoveAnswer(null);
            };

            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void RemoveAnswer_IfPassedInvalidArgs_ShouldReturnError()
        {
            var question = QuestionFixtureFactory.CreateQuestion();

            var answer = Answer.Create($"{Guid.NewGuid()}", true).Value;

            var notPresentAnswerResult = question.RemoveAnswer(answer);

            notPresentAnswerResult.IsFailure.Should().BeTrue();
            notPresentAnswerResult.Error.Should().NotBe(Error.None);

            while (question.Answers.Count == 1)
            {
                question.RemoveAnswer(question.Answers.First(a => a.IsCorrect == false));
            }

            var removeResult = question.RemoveAnswer(question.Answers.First());

            removeResult.IsFailure.Should().BeTrue();
            removeResult.Error.Should().NotBe(Error.None);
        }

        [Fact]
        public void RemoveAnswer_IfPassedValidArgs_ShouldBeSuccesful()
        {
            var question = QuestionFixtureFactory.CreateQuestion();

            var answer = Answer.Create(
                text: Guid.NewGuid().ToString(),
                isCorrect: true).Value;

            question.AddAnswer(answer);

            var countBefore = question.Answers.Count();

            var result = question.RemoveAnswer(answer);

            result.IsSuccess.Should().BeTrue();
            question.Answers.Should().HaveCount(countBefore - 1);
            question.Answers.Should().NotContain(answer);
        }

        [Fact]
        public void AddTopic_IfPassedNullArgs_ShouldThrowArgumentNullException()
        {
            var question = QuestionFixtureFactory.CreateQuestion();

            Action action = () =>
            {
                question.AddTopic(null);
            };

            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void AddTopic_IfTopicAlreadyAdded_ShouldReturnError()
        {
            var question = QuestionFixtureFactory.CreateQuestion();

            var topic = TopicFixtureFactory.CreateTopic();

            question.AddTopic(topic);
            var result = question.AddTopic(topic);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().NotBe(Error.None);
        }

        [Fact]
        public void AddTopic_IfPassedValidArgs_ShouldBeSuccesful()
        {
            var question = QuestionFixtureFactory.CreateQuestion();

            var countBefore = question.TopicIds.Count();

            var topic = TopicFixtureFactory.CreateTopic();

            var result = question.AddTopic(topic);

            result.IsSuccess.Should().BeTrue();
            question.TopicIds.Should().HaveCount(countBefore + 1);
            question.TopicIds.Should().Contain(topic);
        }

        [Fact]
        public void RemoveTopic_IfPassedNullArgs_ShouldThrowArgumentNullException()
        {
            var question = QuestionFixtureFactory.CreateQuestion();

            Action action = () =>
            {
                question.RemoveTopic(null);
            };

            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void RemoveTopic_IfTopicNotPresent_ShouldReturnError()
        {
            var question = QuestionFixtureFactory.CreateQuestion();

            var topic = TopicFixtureFactory.CreateTopic();

            var result = question.RemoveTopic(topic);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().NotBe(Error.None);
        }

        [Fact]
        public void RemoveTopic_IfPassedValidArgs_ShouldBeSuccesful()
        {
            var question = QuestionFixtureFactory.CreateQuestion();

            var topic = TopicFixtureFactory.CreateTopic();

            question.AddTopic(topic);

            var countBefore = question.TopicIds.Count();

            var result = question.RemoveTopic(topic);

            result.IsSuccess.Should().BeTrue();
            question.TopicIds.Should().HaveCount(countBefore - 1);
            question.TopicIds.Should().NotContain(topic);
        }

        [Fact]
        public void AddTicket_IfPassedNullArgs_ShouldThrowArgumentNullException()
        {
            var question = QuestionFixtureFactory.CreateQuestion();

            Action action = () =>
            {
                question.AddTicket(null);
            };

            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void AddTicket_IfTicketAlreadyAdded_ShouldReturnError()
        {
            var question = QuestionFixtureFactory.CreateQuestion();

            var ticket = TicketFixtureFactory.CreateTicket();

            question.AddTicket(ticket);
            var result = question.AddTicket(ticket);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().NotBe(Error.None);
        }

        [Fact]
        public void AddTicket_IfPassedValidArgs_ShouldBeSuccesful()
        {
            var question = QuestionFixtureFactory.CreateQuestion();

            var countBefore = question.TicketIds.Count();

            var ticket = TicketFixtureFactory.CreateTicket();

            var result = question.AddTicket(ticket);

            result.IsSuccess.Should().BeTrue();
            question.TicketIds.Should().HaveCount(countBefore + 1);
            question.TicketIds.Should().Contain(ticket);
        }

        [Fact]
        public void RemoveTicket_IfPassedNullArgs_ShouldThrowArgumentNullException()
        {
            var question = QuestionFixtureFactory.CreateQuestion();

            Action action = () =>
            {
                question.RemoveTicket(null);
            };

            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void RemoveTicket_IfTicketNotPresent_ShouldReturnError()
        {
            var question = QuestionFixtureFactory.CreateQuestion();

            var ticket = TicketFixtureFactory.CreateTicket();

            var result = question.RemoveTicket(ticket);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().NotBe(Error.None);
        }

        [Fact]
        public void RemoveTicket_IfPassedValidArgs_ShouldBeSuccesful()
        {
            var question = QuestionFixtureFactory.CreateQuestion();

            var ticket = TicketFixtureFactory.CreateTicket();

            question.AddTicket(ticket);

            var countBefore = question.TicketIds.Count();

            var result = question.RemoveTicket(ticket);

            result.IsSuccess.Should().BeTrue();
            question.TicketIds.Should().HaveCount(countBefore - 1);
            question.TicketIds.Should().NotContain(ticket);
        }

        [Fact]
        public void AddLike_IfPassedNullArgs_ShouldThrowArgumentNullException()
        {
            var question = QuestionFixtureFactory.CreateQuestion();

            Action action = () =>
            {
                question.AddLike(null);
            };

            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void AddLike_IfUserAlreadyLiked_ShouldReturnError()
        {
            var question = QuestionFixtureFactory.CreateQuestion();

            var user = UserFixtureFactory.CreateUser();

            question.AddLike(user);
            var result = question.AddLike(user);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().NotBe(Error.None);
        }

        [Fact]
        public void AddLike_IfUserAlreadyDisliked_ShouldReturnError()
        {
            var question = QuestionFixtureFactory.CreateQuestion();

            var user = UserFixtureFactory.CreateUser();

            question.AddDislike(user);
            var result = question.AddLike(user);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().NotBe(Error.None);
        }

        [Fact]
        public void AddLike_IfPassedValidArgs_ShouldBeSuccesful()
        {
            var question = QuestionFixtureFactory.CreateQuestion();

            var countBefore = question.LikedByUsersIds.Count();
            var likesBefore = question.LikesCount;

            var user = UserFixtureFactory.CreateUser();

            var result = question.AddLike(user);

            result.IsSuccess.Should().BeTrue();
            question.LikedByUsersIds.Should().HaveCount(countBefore + 1);
            question.LikedByUsersIds.Should().Contain(user);
            question.LikesCount.Should().Be(likesBefore + 1);
        }

        [Fact]
        public void AddDislike_IfPassedNullArgs_ShouldThrowArgumentNullException()
        {
            var question = QuestionFixtureFactory.CreateQuestion();

            Action action = () =>
            {
                question.AddDislike(null);
            };

            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void AddDislike_IfUserAlreadyDisliked_ShouldReturnError()
        {
            var question = QuestionFixtureFactory.CreateQuestion();

            var user = UserFixtureFactory.CreateUser();

            question.AddDislike(user);
            var result = question.AddDislike(user);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().NotBe(Error.None);
        }

        [Fact]
        public void AddDislike_IfUserAlreadyLiked_ShouldReturnError()
        {
            var question = QuestionFixtureFactory.CreateQuestion();

            var user = UserFixtureFactory.CreateUser();

            question.AddLike(user);
            var result = question.AddDislike(user);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().NotBe(Error.None);
        }

        [Fact]
        public void AddDislike_IfPassedValidArgs_ShouldBeSuccesful()
        {
            var question = QuestionFixtureFactory.CreateQuestion();

            var countBefore = question.DislikedByUsersIds.Count();
            var dislikesBefore = question.DislikesCount;

            var user = UserFixtureFactory.CreateUser();

            var result = question.AddDislike(user);

            result.IsSuccess.Should().BeTrue();
            question.DislikedByUsersIds.Should().HaveCount(countBefore + 1);
            question.DislikedByUsersIds.Should().Contain(user);
            question.DislikesCount.Should().Be(dislikesBefore + 1);
        }

        [Fact]
        public void RemoveLike_IfPassedNullArgs_ShouldThrowArgumentNullException()
        {
            var question = QuestionFixtureFactory.CreateQuestion();

            Action action = () =>
            {
                question.RemoveLike(null);
            };

            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void RemoveLike_IfQuestionNotLiked_ShouldReturnError()
        {
            var question = QuestionFixtureFactory.CreateQuestion();

            var user = UserFixtureFactory.CreateUser();

            var result = question.RemoveLike(user);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().NotBe(Error.None);
        }

        [Fact]
        public void RemoveLike_IfPassedValidArgs_ShouldBeSuccesful()
        {
            var question = QuestionFixtureFactory.CreateQuestion();

            var user = UserFixtureFactory.CreateUser();

            question.AddLike(user);

            var countBefore = question.LikedByUsersIds.Count();
            var likesBefore = question.LikesCount;

            var result = question.RemoveLike(user);

            result.IsSuccess.Should().BeTrue();

            question.LikedByUsersIds.Should().HaveCount(countBefore - 1);
            question.LikedByUsersIds.Should().NotContain(user);

            question.LikesCount.Should().Be(likesBefore - 1);
        }

        [Fact]
        public void RemoveDislike_IfPassedNullArgs_ShouldThrowArgumentNullException()
        {
            var question = QuestionFixtureFactory.CreateQuestion();

            Action action = () =>
            {
                question.RemoveDislike(null);
            };

            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void RemoveDislike_IfQuestionNotDisliked_ShouldReturnError()
        {
            var question = QuestionFixtureFactory.CreateQuestion();

            var user = UserFixtureFactory.CreateUser();

            var result = question.RemoveDislike(user);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().NotBe(Error.None);
        }

        [Fact]
        public void RemoveDislike_IfPassedValidArgs_ShouldBeSuccesful()
        {
            var question = QuestionFixtureFactory.CreateQuestion();

            var user = UserFixtureFactory.CreateUser();

            question.AddDislike(user);

            var countBefore = question.DislikedByUsersIds.Count();
            var dislikesBefore = question.DislikesCount;

            var result = question.RemoveDislike(user);

            result.IsSuccess.Should().BeTrue();

            question.DislikedByUsersIds.Should().HaveCount(countBefore - 1);
            question.DislikedByUsersIds.Should().NotContain(user);

            question.DislikesCount.Should().Be(dislikesBefore - 1);
        }

        [Fact]
        public void AddComment_IfPassedNullArgs_ShouldThrowArgumentNullException()
        {
            var question = QuestionFixtureFactory.CreateQuestion();

            Action action = () =>
            {
                question.AddComment(null);
            };

            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void AddComment_IfCommentAlreadyAdded_ShouldReturnError()
        {
            var question = QuestionFixtureFactory.CreateQuestion();

            var comment = CommentFixtureFactory.CreateComment();

            question.AddComment(comment);
            var result = question.AddComment(comment);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().NotBe(Error.None);
        }

        [Fact]
        public void AddComment_IfPassedValidArgs_ShouldBeSuccesful()
        {
            var question = QuestionFixtureFactory.CreateQuestion();

            var countBefore = question.CommentIds.Count();

            var comment = CommentFixtureFactory.CreateComment();

            var result = question.AddComment(comment);

            result.IsSuccess.Should().BeTrue();
            question.CommentIds.Should().HaveCount(countBefore + 1);
            question.CommentIds.Should().Contain(comment);
        }

        [Fact]
        public void Question_ShouldInheritFromEntity()
        {
            var type = typeof(Question);

            var isValueObject = typeof(Entity<QuestionId>).IsAssignableFrom(type);
            isValueObject.Should().BeTrue("Question should inherit from Entity.");
        }
    }
}
