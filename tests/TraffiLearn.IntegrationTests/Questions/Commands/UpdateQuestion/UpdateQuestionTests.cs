using TraffiLearn.Domain.Aggregates.Users.Enums;
using TraffiLearn.IntegrationTests.Abstractions;
using TraffiLearn.IntegrationTests.Extensions;
using TraffiLearn.Testing.Shared.Factories;

namespace TraffiLearn.IntegrationTests.Questions.Commands.UpdateQuestion
{
    public sealed class UpdateQuestionTests : QuestionIntegrationTest
    {
        private readonly UpdateQuestionCommandFactory _commandFactory = new();

        public UpdateQuestionTests(
            WebApplicationFactory factory)
            : base(factory)
        { }

        [Fact]
        public async Task UpdateQuestion_IfUserIsNotAuthenticated_ShouldReturn401StatusCode()
        {
            var response = await ApiQuestionClient
                .SendValidUpdateQuestionRequestAsync(
                    questionId: Guid.NewGuid(),
                    topicIds: [Guid.NewGuid()],
                    sentFromRole: null);

            response.AssertUnauthorizedStatusCode();
        }

        [Fact]
        public async Task UpdateQuestion_IfUserIsNotAuthenticated_QuestionShouldNotBeUpdated()
        {
            var questionId = await ApiQuestionClient
                .CreateValidQuestionWithTopicAsAuthorizedAsync();

            var updatedExplanation = "updated-explanation";

            var command = _commandFactory.CreateValidCommand(
                questionId,
                topicIds: [Guid.NewGuid()]) with
            { Explanation = updatedExplanation };

            await ApiQuestionClient
                .SendUpdateQuestionRequestAsync(
                    request: command,
                    sentFromRole: null);

            var allQuestions = await ApiQuestionClient.GetAllQuestionsAsAuthorizedAsync();

            allQuestions.Single().Explanation.Should().NotBe(updatedExplanation);
        }


        [Fact]
        public async Task UpdateQuestion_IfUserIsNotAuthenticated_QuestionTopicsShouldNotBeUpdated()
        {
            var questionId = await ApiQuestionClient
                .CreateValidQuestionWithTopicAsAuthorizedAsync();

            var newTopicId = await ApiTopicClient
                .CreateValidTopicAsAuthorizedAsync();

            var command = _commandFactory.CreateValidCommand(
                questionId,
                topicIds: [newTopicId]);

            await ApiQuestionClient
                .SendUpdateQuestionRequestAsync(
                    request: command,
                    sentFromRole: null);

            var questionTopics = await ApiQuestionClient
                .GetQuestionTopicsAsAuthorizedUserAsync(questionId);

            questionTopics.Single().Id.Should().NotBe(newTopicId);
        }

        [Fact]
        public async Task UpdateQuestion_IfUserIsNotAuthenticated_TopicQuestionsShouldNotBeUpdated()
        {
            var questionId = await ApiQuestionClient
                .CreateValidQuestionWithTopicAsAuthorizedAsync();

            var newTopicId = await ApiTopicClient
                .CreateValidTopicAsAuthorizedAsync();

            var command = _commandFactory.CreateValidCommand(
                questionId,
                topicIds: [newTopicId]);

            await ApiQuestionClient
                .SendUpdateQuestionRequestAsync(
                    request: command,
                    sentFromRole: null);

            var topicQuestions = await ApiTopicClient
                .GetTopicQuestionsAsAuthorizedAsync(
                    topicId: newTopicId);

            topicQuestions.Should().BeEmpty();
        }

        [Fact]
        public async Task UpdateQuestion_IfUserIsNotAuthenticatedAndProvidedNewImageAndRemoveIntended_ImageUriShouldNotBeUpdated()
        {
            var questionId = await ApiQuestionClient
                .CreateValidQuestionWithTopicAsAuthorizedAsync();

            var oldImageUri = (await ApiQuestionClient.GetQuestionByIdAsAuthorizedAsync(questionId))
                .ImageUri;

            var newImage = ImageFixtureFactory.CreateImage(content: "New-image");

            var command = _commandFactory.CreateValidCommand(
                questionId,
                topicIds: [Guid.NewGuid()],
                image: newImage,
                removeOldImageIfNewImageMissing: true);

            await ApiQuestionClient
                .SendUpdateQuestionRequestAsync(
                    request: command,
                    sentFromRole: null);

            var newImageUri = (await ApiQuestionClient.GetQuestionByIdAsAuthorizedAsync(questionId))
                .ImageUri;

            newImageUri.Should().Be(oldImageUri);
        }

        [Fact]
        public async Task UpdateQuestion_IfUserIsNotAuthenticatedAndProvidedNewImageAndNoRemoveIntended_ImageUriShouldNotBeUpdated()
        {
            var questionId = await ApiQuestionClient
                .CreateValidQuestionWithTopicAsAuthorizedAsync();

            var oldImageUri = (await ApiQuestionClient.GetQuestionByIdAsAuthorizedAsync(questionId))
                .ImageUri;

            var newImage = ImageFixtureFactory.CreateImage(content: "New-image");

            var command = _commandFactory.CreateValidCommand(
                questionId,
                topicIds: [Guid.NewGuid()],
                image: newImage,
                removeOldImageIfNewImageMissing: false);

            await ApiQuestionClient
                .SendUpdateQuestionRequestAsync(
                    request: command,
                    sentFromRole: null);

            var newImageUri = (await ApiQuestionClient.GetQuestionByIdAsAuthorizedAsync(questionId))
                .ImageUri;

            newImageUri.Should().Be(oldImageUri);
        }

        [Fact]
        public async Task UpdateQuestion_IfUserIsNotAuthenticatedAndNotProvidedNewImageAndNoRemoveIntended_ImageUriShouldNotBeUpdated()
        {
            var questionId = await ApiQuestionClient
                .CreateValidQuestionWithTopicAsAuthorizedAsync();

            var oldImageUri = (await ApiQuestionClient.GetQuestionByIdAsAuthorizedAsync(questionId))
                .ImageUri;

            var command = _commandFactory.CreateValidCommand(
                questionId,
                topicIds: [Guid.NewGuid()],
                removeOldImageIfNewImageMissing: false);

            await ApiQuestionClient
                .SendUpdateQuestionRequestAsync(
                    request: command,
                    sentFromRole: null);

            var newImageUri = (await ApiQuestionClient.GetQuestionByIdAsAuthorizedAsync(questionId))
                .ImageUri;

            newImageUri.Should().Be(oldImageUri);
        }

        [Fact]
        public async Task UpdateQuestion_IfUserIsNotAuthenticatedAndNotProvidedNewImageAndRemoveIntended_ImageUriShouldNotBeUpdated()
        {
            var questionId = await ApiQuestionClient
                .CreateValidQuestionWithTopicAsAuthorizedAsync();

            var oldImageUri = (await ApiQuestionClient.GetQuestionByIdAsAuthorizedAsync(questionId))
                .ImageUri;

            var command = _commandFactory.CreateValidCommand(
                questionId,
                topicIds: [Guid.NewGuid()],
                removeOldImageIfNewImageMissing: true);

            await ApiQuestionClient
                .SendUpdateQuestionRequestAsync(
                    request: command,
                    sentFromRole: null);

            var newImageUri = (await ApiQuestionClient.GetQuestionByIdAsAuthorizedAsync(questionId))
                .ImageUri;

            newImageUri.Should().Be(oldImageUri);
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        public async Task UpdateQuestion_IfUserIsNotEligible_ShouldReturn403StatusCode(
            Role nonEligibleRole)
        {
            var response = await ApiQuestionClient
                .SendValidUpdateQuestionRequestAsync(
                    questionId: Guid.NewGuid(),
                    topicIds: [Guid.NewGuid()],
                    sentFromRole: nonEligibleRole);

            response.AssertForbiddenStatusCode();
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        public async Task UpdateQuestion_IfUserIsNotEligible_QuestionShouldNotBeUpdated(
            Role nonEligibleRole)
        {
            var questionId = await ApiQuestionClient
                .CreateValidQuestionWithTopicAsAuthorizedAsync();

            var updatedExplanation = "updated-explanation";

            var command = _commandFactory.CreateValidCommand(
                questionId,
                topicIds: [Guid.NewGuid()]) with
            { Explanation = updatedExplanation };

            await ApiQuestionClient
                .SendUpdateQuestionRequestAsync(
                    request: command,
                    sentFromRole: nonEligibleRole);

            var allQuestions = await ApiQuestionClient.GetAllQuestionsAsAuthorizedAsync();

            allQuestions.Single().Explanation.Should().NotBe(updatedExplanation);
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        public async Task UpdateQuestion_IfUserIsNotEligible_QuestionTopicsShouldNotBeUpdated(
            Role nonEligibleRole)
        {
            var questionId = await ApiQuestionClient
                .CreateValidQuestionWithTopicAsAuthorizedAsync();

            var newTopicId = await ApiTopicClient
                .CreateValidTopicAsAuthorizedAsync();

            var command = _commandFactory.CreateValidCommand(
                questionId,
                topicIds: [newTopicId]);

            await ApiQuestionClient
                .SendUpdateQuestionRequestAsync(
                    request: command,
                    sentFromRole: nonEligibleRole);

            var questionTopics = await ApiQuestionClient
                .GetQuestionTopicsAsAuthorizedUserAsync(questionId);

            questionTopics.Single().Id.Should().NotBe(newTopicId);
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        public async Task UpdateQuestion_IfUserIsNotEligible_TopicQuestionsShouldNotBeUpdated(
            Role nonEligibleRole)
        {
            var questionId = await ApiQuestionClient
                .CreateValidQuestionWithTopicAsAuthorizedAsync();

            var newTopicId = await ApiTopicClient
                .CreateValidTopicAsAuthorizedAsync();

            var command = _commandFactory.CreateValidCommand(
                questionId,
                topicIds: [newTopicId]);

            await ApiQuestionClient
                .SendUpdateQuestionRequestAsync(
                    request: command,
                    sentFromRole: nonEligibleRole);

            var topicQuestions = await ApiTopicClient
                .GetTopicQuestionsAsAuthorizedAsync(
                    topicId: newTopicId);

            topicQuestions.Should().BeEmpty();
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        public async Task UpdateQuestion_IfUserIsNotEligibleAndProvidedNewImageAndRemoveIntended_ImageUriShouldNotBeUpdated(
            Role nonEligibleRole)
        {
            var questionId = await ApiQuestionClient
                .CreateValidQuestionWithTopicAsAuthorizedAsync();

            var oldImageUri = (await ApiQuestionClient.GetQuestionByIdAsAuthorizedAsync(questionId))
                .ImageUri;

            var newImage = ImageFixtureFactory.CreateImage(content: "New-image");

            var command = _commandFactory.CreateValidCommand(
                questionId,
                topicIds: [Guid.NewGuid()],
                image: newImage,
                removeOldImageIfNewImageMissing: true);

            await ApiQuestionClient
                .SendUpdateQuestionRequestAsync(
                    request: command,
                    sentFromRole: nonEligibleRole);

            var newImageUri = (await ApiQuestionClient.GetQuestionByIdAsAuthorizedAsync(questionId))
                .ImageUri;

            newImageUri.Should().Be(oldImageUri);
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        public async Task UpdateQuestion_IfUserIsNotEligibleAndProvidedNewImageAndNoRemoveIntended_ImageUriShouldNotBeUpdated(
            Role nonEligibleRole)
        {
            var questionId = await ApiQuestionClient
                .CreateValidQuestionWithTopicAsAuthorizedAsync();

            var oldImageUri = (await ApiQuestionClient.GetQuestionByIdAsAuthorizedAsync(questionId))
                .ImageUri;

            var newImage = ImageFixtureFactory.CreateImage(content: "New-image");

            var command = _commandFactory.CreateValidCommand(
                questionId,
                topicIds: [Guid.NewGuid()],
                image: newImage,
                removeOldImageIfNewImageMissing: false);

            await ApiQuestionClient
                .SendUpdateQuestionRequestAsync(
                    request: command,
                    sentFromRole: nonEligibleRole);

            var newImageUri = (await ApiQuestionClient.GetQuestionByIdAsAuthorizedAsync(questionId))
                .ImageUri;

            newImageUri.Should().Be(oldImageUri);
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        public async Task UpdateQuestion_IfUserIsNotEligibleAndNotProvidedNewImageAndNoRemoveIntended_ImageUriShouldNotBeUpdated(
            Role nonEligibleRole)
        {
            var questionId = await ApiQuestionClient
                .CreateValidQuestionWithTopicAsAuthorizedAsync();

            var oldImageUri = (await ApiQuestionClient.GetQuestionByIdAsAuthorizedAsync(questionId))
                .ImageUri;

            var command = _commandFactory.CreateValidCommand(
                questionId,
                topicIds: [Guid.NewGuid()],
                removeOldImageIfNewImageMissing: false);

            await ApiQuestionClient
                .SendUpdateQuestionRequestAsync(
                    request: command,
                    sentFromRole: nonEligibleRole);

            var newImageUri = (await ApiQuestionClient.GetQuestionByIdAsAuthorizedAsync(questionId))
                .ImageUri;

            newImageUri.Should().Be(oldImageUri);
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        public async Task UpdateQuestion_IfUserIsNotEligibleAndNotProvidedNewImageAndRemoveIntended_ImageUriShouldNotBeUpdated(
            Role nonEligibleRole)
        {
            var questionId = await ApiQuestionClient
                .CreateValidQuestionWithTopicAsAuthorizedAsync();

            var oldImageUri = (await ApiQuestionClient.GetQuestionByIdAsAuthorizedAsync(questionId))
                .ImageUri;

            var command = _commandFactory.CreateValidCommand(
                questionId,
                topicIds: [Guid.NewGuid()],
                removeOldImageIfNewImageMissing: true);

            await ApiQuestionClient
                .SendUpdateQuestionRequestAsync(
                    request: command,
                    sentFromRole: nonEligibleRole);

            var newImageUri = (await ApiQuestionClient.GetQuestionByIdAsAuthorizedAsync(questionId))
                .ImageUri;

            newImageUri.Should().Be(oldImageUri);
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task UpdateQuestion_IfProvidedInvalidArgs_ShouldReturn400StatusCode(
            Role eligibleRole)
        {
            var questionId = await ApiQuestionClient
                .CreateValidQuestionWithTopicAsAuthorizedAsync();

            var invalidCommands = _commandFactory.CreateInvalidCommands(
                questionId,
                topicIds: [Guid.NewGuid()]);

            foreach (var command in invalidCommands)
            {
                var response = await ApiQuestionClient.SendUpdateQuestionRequestAsync(
                    request: command,
                    sentFromRole: eligibleRole);

                response.AssertBadRequestStatusCode();
            }
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task UpdateQuestion_IfProvidedInvalidArgs_QuestionTopicsShouldNotBeUpdated(
            Role eligibleRole)
        {
            var topicId = await ApiTopicClient.CreateValidTopicAsAuthorizedAsync();

            var questionId = await ApiQuestionClient
                .CreateValidQuestionWithTopicAsAuthorizedAsync();

            var questionTopicIdsBefore = (await ApiQuestionClient
                .GetQuestionTopicsAsAuthorizedUserAsync(questionId))
                .Select(t => t.Id);

            var invalidCommands = _commandFactory.CreateInvalidCommands(
                questionId,
                topicIds: [topicId]);

            foreach (var command in invalidCommands)
            {
                await ApiQuestionClient.SendUpdateQuestionRequestAsync(
                    request: command,
                    sentFromRole: eligibleRole);

                var questionTopics = await ApiQuestionClient.GetQuestionTopicsAsAuthorizedUserAsync(questionId);

                var questionTopicIds = questionTopics.Select(t => t.Id);

                questionTopicIds.Should().BeEquivalentTo(questionTopicIdsBefore);
            }
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task UpdateQuestion_IfProvidedInvalidArgs_TopicQuestionsShouldNotBeUpdated(
            Role eligibleRole)
        {
            var topicId = await ApiTopicClient.CreateValidTopicAsAuthorizedAsync();

            var questionId = await ApiQuestionClient
                .CreateValidQuestionWithTopicAsAuthorizedAsync();

            var topicQuestionIdsBefore = (await ApiTopicClient
                .GetTopicQuestionsAsAuthorizedAsync(topicId))
                .Select(t => t.Id);

            var invalidCommands = _commandFactory.CreateInvalidCommands(
                questionId,
                topicIds: [topicId]);

            foreach (var command in invalidCommands)
            {
                await ApiQuestionClient.SendUpdateQuestionRequestAsync(
                    request: command,
                    sentFromRole: eligibleRole);

                var topicQuestions = await ApiTopicClient
                    .GetTopicQuestionsAsAuthorizedAsync(topicId);

                var topicQuestionsIds = topicQuestions.Select(t => t.Id);

                topicQuestionsIds.Should().BeEquivalentTo(topicQuestionIdsBefore);
            }
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task UpdateQuestion_IfProvidedInvalidArgsAndProvidedNewImageAndRemoveIntended_ImageUriShouldNotBeUpdated(
            Role eligibleRole)
        {
            var topicId = await ApiTopicClient
                .CreateValidTopicAsAuthorizedAsync();

            var questionId = await ApiQuestionClient
                .CreateValidQuestionWithTopicAsAuthorizedAsync();

            var imageUriBefore = (await ApiQuestionClient
                .GetQuestionByIdAsAuthorizedAsync(questionId))
                .ImageUri;

            var image = ImageFixtureFactory.CreateImage(content: "updated-content");

            var invalidCommands = _commandFactory.CreateInvalidCommands(
                questionId,
                topicIds: [topicId],
                image: image,
                removeOldImageIfNewImageMissing: true);

            foreach (var command in invalidCommands)
            {
                await ApiQuestionClient.SendUpdateQuestionRequestAsync(
                    request: command,
                    sentFromRole: eligibleRole);

                var imageUriAfter = (await ApiQuestionClient
                   .GetQuestionByIdAsAuthorizedAsync(questionId))
                   .ImageUri;

                imageUriAfter.Should().Be(imageUriBefore);
            }
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task UpdateQuestion_IfProvidedInvalidArgsAndProvidedNewImageAndNoRemoveIntended_ImageUriShouldNotBeUpdated(
            Role eligibleRole)
        {
            var topicId = await ApiTopicClient
                .CreateValidTopicAsAuthorizedAsync();

            var questionId = await ApiQuestionClient
                .CreateValidQuestionWithTopicAsAuthorizedAsync();

            var imageUriBefore = (await ApiQuestionClient
                .GetQuestionByIdAsAuthorizedAsync(questionId))
                .ImageUri;

            var image = ImageFixtureFactory.CreateImage(content: "updated-content");

            var invalidCommands = _commandFactory.CreateInvalidCommands(
                questionId,
                topicIds: [topicId],
                image: image,
                removeOldImageIfNewImageMissing: false);

            foreach (var command in invalidCommands)
            {
                await ApiQuestionClient.SendUpdateQuestionRequestAsync(
                    request: command,
                    sentFromRole: eligibleRole);

                var imageUriAfter = (await ApiQuestionClient
                   .GetQuestionByIdAsAuthorizedAsync(questionId))
                   .ImageUri;

                imageUriAfter.Should().Be(imageUriBefore);
            }
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task UpdateQuestion_IfProvidedInvalidArgsAndNotProvidedNewImageAndNoRemoveIntended_ImageUriShouldNotBeUpdated(
            Role eligibleRole)
        {
            var topicId = await ApiTopicClient
                .CreateValidTopicAsAuthorizedAsync();

            var questionId = await ApiQuestionClient
                .CreateValidQuestionWithTopicAsAuthorizedAsync();

            var imageUriBefore = (await ApiQuestionClient
                .GetQuestionByIdAsAuthorizedAsync(questionId))
                .ImageUri;

            var invalidCommands = _commandFactory.CreateInvalidCommands(
                questionId,
                topicIds: [topicId],
                removeOldImageIfNewImageMissing: false);

            foreach (var command in invalidCommands)
            {
                await ApiQuestionClient.SendUpdateQuestionRequestAsync(
                    request: command,
                    sentFromRole: eligibleRole);

                var imageUriAfter = (await ApiQuestionClient
                   .GetQuestionByIdAsAuthorizedAsync(questionId))
                   .ImageUri;

                imageUriAfter.Should().Be(imageUriBefore);
            }
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task UpdateQuestion_IfProvidedInvalidArgsAndNotProvidedNewImageAndRemoveIntended_ImageUriShouldNotBeUpdated(
            Role eligibleRole)
        {
            var topicId = await ApiTopicClient
                .CreateValidTopicAsAuthorizedAsync();

            var questionId = await ApiQuestionClient
                .CreateValidQuestionWithTopicAsAuthorizedAsync();

            var imageUriBefore = (await ApiQuestionClient
                .GetQuestionByIdAsAuthorizedAsync(questionId))
                .ImageUri;

            var invalidCommands = _commandFactory.CreateInvalidCommands(
                questionId,
                topicIds: [topicId],
                removeOldImageIfNewImageMissing: true);

            foreach (var command in invalidCommands)
            {
                await ApiQuestionClient.SendUpdateQuestionRequestAsync(
                    request: command,
                    sentFromRole: eligibleRole);

                var imageUriAfter = (await ApiQuestionClient
                   .GetQuestionByIdAsAuthorizedAsync(questionId))
                   .ImageUri;

                imageUriAfter.Should().Be(imageUriBefore);
            }
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task UpdateQuestion_IfQuestionIsNotFound_ShouldReturn404StatusCode(
            Role eligibleRole)
        {
            var topicId = await ApiTopicClient
                .CreateValidTopicAsAuthorizedAsync();

            var command = _commandFactory.CreateValidCommand(
                questionId: Guid.NewGuid(),
                topicIds: [topicId]);

            var response = await ApiQuestionClient.SendUpdateQuestionRequestAsync(
                command, 
                sentFromRole: eligibleRole);

            response.AssertNotFoundStatusCode();
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task UpdateQuestion_IfTopicsAreNotFound_ShouldReturn404StatusCode(
            Role eligibleRole)
        {
            var questionId = await ApiQuestionClient
                .CreateValidQuestionWithTopicAsAuthorizedAsync();

            var command = _commandFactory.CreateValidCommand(
                questionId: questionId,
                topicIds: [Guid.NewGuid(), Guid.NewGuid()]);

            var response = await ApiQuestionClient.SendUpdateQuestionRequestAsync(
                command,
                sentFromRole: eligibleRole);

            response.AssertNotFoundStatusCode();
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task UpdateQuestion_IfTopicsAreNotFound_QuestionShouldNotBeUpdated(
            Role eligibleRole)
        {
            var questionId = await ApiQuestionClient
                .CreateValidQuestionWithTopicAsAuthorizedAsync();

            var updatedContent = "Updated-content";

            var command = _commandFactory.CreateValidCommand(
                questionId: questionId,
                topicIds: [Guid.NewGuid(), Guid.NewGuid()])
                with { Content = updatedContent };

            await ApiQuestionClient.SendUpdateQuestionRequestAsync(
                command,
                sentFromRole: eligibleRole);

            var question = await ApiQuestionClient.GetQuestionByIdAsAuthorizedAsync(
                questionId);
            
            question.Content.Should().NotBe(updatedContent);
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task UpdateQuestion_IfTopicsAreNotFound_QuestionTopicsShouldNotBeUpdated(
            Role eligibleRole)
        {
            var questionId = await ApiQuestionClient
                .CreateValidQuestionWithTopicAsAuthorizedAsync();

            var questionTopicsBefore = await ApiQuestionClient.GetQuestionTopicsAsAuthorizedUserAsync(
                questionId);

            var command = _commandFactory.CreateValidCommand(
                questionId: questionId,
                topicIds: [Guid.NewGuid(), Guid.NewGuid()]);

            await ApiQuestionClient.SendUpdateQuestionRequestAsync(
                command,
                sentFromRole: eligibleRole);

            var questionTopicsAfter = await ApiQuestionClient.GetQuestionTopicsAsAuthorizedUserAsync(
                questionId);

            questionTopicsAfter.Should().BeEquivalentTo(questionTopicsBefore);
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task UpdateQuestion_IfValidCase_ShouldReturn204StatusCode(
            Role eligibleRole)
        {
            var questionId = await ApiQuestionClient
                .CreateValidQuestionWithTopicAsAuthorizedAsync();

            var topicId = await ApiTopicClient.CreateValidTopicAsAuthorizedAsync();

            var command = _commandFactory.CreateValidCommand(
                questionId: questionId,
                topicIds: [topicId]);

            var response = await ApiQuestionClient.SendUpdateQuestionRequestAsync(
                command,
                sentFromRole: eligibleRole);

            response.AssertNoContentStatusCode();
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task UpdateQuestion_IfValidCase_QuestionShouldBeUpdated(
            Role eligibleRole)
        {
            var questionId = await ApiQuestionClient
                .CreateValidQuestionWithTopicAsAuthorizedAsync();

            var topicId = await ApiTopicClient.CreateValidTopicAsAuthorizedAsync();

            var newContent = "Updated-content!";

            var command = _commandFactory.CreateValidCommand(
                questionId: questionId,
                topicIds: [topicId]) 
                with { Content = newContent };

            await ApiQuestionClient.SendUpdateQuestionRequestAsync(
                request: command,
                sentFromRole: eligibleRole);

            var updatedQuestion = await ApiQuestionClient.GetQuestionByIdAsAuthorizedAsync(
                questionId);

            updatedQuestion.Content.Should().Be(newContent);
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task UpdateQuestion_IfValidCase_QuestionTopicsShouldBeUpdated(
            Role eligibleRole)
        {
            var questionId = await ApiQuestionClient
                .CreateValidQuestionWithTopicAsAuthorizedAsync();

            var topicId = await ApiTopicClient.CreateValidTopicAsAuthorizedAsync();

            var command = _commandFactory.CreateValidCommand(
                questionId: questionId,
                topicIds: [topicId]);

            await ApiQuestionClient.SendUpdateQuestionRequestAsync(
                request: command,
                sentFromRole: eligibleRole);

            var questionTopics = await ApiQuestionClient.GetQuestionTopicsAsAuthorizedUserAsync(
                questionId);

            questionTopics.Single().Id.Should().Be(topicId);
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task UpdateQuestion_IfValidCase_TopicQuestionsShouldBeUpdated(
            Role eligibleRole)
        {
            var questionId = await ApiQuestionClient
                .CreateValidQuestionWithTopicAsAuthorizedAsync();

            var topicId = await ApiTopicClient.CreateValidTopicAsAuthorizedAsync();

            var command = _commandFactory.CreateValidCommand(
                questionId: questionId,
                topicIds: [topicId]);

            await ApiQuestionClient.SendUpdateQuestionRequestAsync(
                request: command,
                sentFromRole: eligibleRole);

            var topicQuestions = await ApiTopicClient.GetTopicQuestionsAsAuthorizedAsync(
                topicId);

            topicQuestions.Single().Id.Should().Be(questionId);
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task UpdateQuestion_IfOldImageDoesNotExistAndNewImageIsProvidedAndRemoveIfNewOneMissingIntended_ImageUriShouldBeUpdated(
            Role eligibleRole)
        {
            var questionId = await ApiQuestionClient
                .CreateValidQuestionWithTopicAsAuthorizedAsync();

            var questionImageUriBefore = (await ApiQuestionClient.GetQuestionByIdAsAuthorizedAsync(
                questionId)).ImageUri;

            var topicId = await ApiTopicClient.CreateValidTopicAsAuthorizedAsync();

            var image = ImageFixtureFactory.CreateImage();

            var command = _commandFactory.CreateValidCommand(
                questionId: questionId,
                topicIds: [topicId],
                image,
                removeOldImageIfNewImageMissing: true);

            await ApiQuestionClient.SendUpdateQuestionRequestAsync(
                request: command,
                sentFromRole: eligibleRole);

            var question = await ApiQuestionClient.GetQuestionByIdAsAuthorizedAsync(
                questionId);

            question.ImageUri.Should().NotBe(questionImageUriBefore);
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task UpdateQuestion_IfOldImageExistsAndNewImageIsProvidedAndRemoveIfNewOneMissingIntended_ImageUriShouldBeUpdated(
            Role eligibleRole)
        {
            var questionId = await ApiQuestionClient
                .CreateValidQuestionWithTopicAsAuthorizedAsync(
                    image: ImageFixtureFactory.CreateImage());

            var questionImageUriBefore = (await ApiQuestionClient.GetQuestionByIdAsAuthorizedAsync(
                questionId)).ImageUri;

            var topicId = await ApiTopicClient.CreateValidTopicAsAuthorizedAsync();

            var image = ImageFixtureFactory.CreateImage(
                content: "updated-content!");

            var command = _commandFactory.CreateValidCommand(
                questionId: questionId,
                topicIds: [topicId],
                image,
                removeOldImageIfNewImageMissing: true);

            await ApiQuestionClient.SendUpdateQuestionRequestAsync(
                request: command,
                sentFromRole: eligibleRole);

            var question = await ApiQuestionClient.GetQuestionByIdAsAuthorizedAsync(
                questionId);

            question.ImageUri.Should().NotBe(questionImageUriBefore);
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task UpdateQuestion_IfOldImageDoesNotExistAndNewImageIsNotProvidedAndRemoveIfNewOneMissingIntended_ImageUriShouldBeNull(
            Role eligibleRole)
        {
            var questionId = await ApiQuestionClient
                .CreateValidQuestionWithTopicAsAuthorizedAsync();

            var topicId = await ApiTopicClient.CreateValidTopicAsAuthorizedAsync();

            var command = _commandFactory.CreateValidCommand(
                questionId: questionId,
                topicIds: [topicId],
                removeOldImageIfNewImageMissing: true);

            await ApiQuestionClient.SendUpdateQuestionRequestAsync(
                request: command,
                sentFromRole: eligibleRole);

            var question = await ApiQuestionClient.GetQuestionByIdAsAuthorizedAsync(
                questionId);

            question.ImageUri.Should().BeNull();
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task UpdateQuestion_IfOldImageExistsAndNewImageIsNotProvidedAndRemoveIfNewOneMissingIntended_ImageUriShouldBeNull(
            Role eligibleRole)
        {
            var questionId = await ApiQuestionClient
                .CreateValidQuestionWithTopicAsAuthorizedAsync(
                    image: ImageFixtureFactory.CreateImage());

            var questionImageUriBefore = (await ApiQuestionClient.GetQuestionByIdAsAuthorizedAsync(
                questionId)).ImageUri;

            var topicId = await ApiTopicClient.CreateValidTopicAsAuthorizedAsync();

            var command = _commandFactory.CreateValidCommand(
                questionId: questionId,
                topicIds: [topicId],
                removeOldImageIfNewImageMissing: true);

            await ApiQuestionClient.SendUpdateQuestionRequestAsync(
                request: command,
                sentFromRole: eligibleRole);

            var question = await ApiQuestionClient.GetQuestionByIdAsAuthorizedAsync(
                questionId);

            question.ImageUri.Should().BeNull();
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task UpdateQuestion_IfOldImageDoesNotExistAndNewImageIsNotProvidedAndRemoveIfNewOneMissingIsNotIntended_ImageUriShouldBeNull(
            Role eligibleRole)
        {
            var questionId = await ApiQuestionClient
                .CreateValidQuestionWithTopicAsAuthorizedAsync();

            var topicId = await ApiTopicClient.CreateValidTopicAsAuthorizedAsync();

            var command = _commandFactory.CreateValidCommand(
                questionId: questionId,
                topicIds: [topicId],
                removeOldImageIfNewImageMissing: false);

            await ApiQuestionClient.SendUpdateQuestionRequestAsync(
                request: command,
                sentFromRole: eligibleRole);

            var question = await ApiQuestionClient.GetQuestionByIdAsAuthorizedAsync(
                questionId);

            question.ImageUri.Should().BeNull();
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task UpdateQuestion_IfOldImageExistsAndNewImageIsNotProvidedAndRemoveIfNewOneMissingIsNotIntended_ImageUriShouldRemainSame(
            Role eligibleRole)
        {
            var questionId = await ApiQuestionClient
                .CreateValidQuestionWithTopicAsAuthorizedAsync(
                    image: ImageFixtureFactory.CreateImage());

            var questionImageUriBefore = (await ApiQuestionClient.GetQuestionByIdAsAuthorizedAsync(
                questionId)).ImageUri;

            var topicId = await ApiTopicClient.CreateValidTopicAsAuthorizedAsync();

            var command = _commandFactory.CreateValidCommand(
                questionId: questionId,
                topicIds: [topicId],
                removeOldImageIfNewImageMissing: false);

            await ApiQuestionClient.SendUpdateQuestionRequestAsync(
                request: command,
                sentFromRole: eligibleRole);

            var question = await ApiQuestionClient.GetQuestionByIdAsAuthorizedAsync(
                questionId);

            question.ImageUri.Should().Be(questionImageUriBefore);
        }
    }
}
