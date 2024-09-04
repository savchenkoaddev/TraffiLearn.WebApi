﻿using Microsoft.AspNetCore.Http;
using System.Net.Http.Json;
using TraffiLearn.Application.Questions.Commands.Create;
using TraffiLearn.Application.Questions.DTO;
using TraffiLearn.Application.Tickets.DTO;
using TraffiLearn.Application.Topics.DTO;
using TraffiLearn.Domain.Aggregates.Users.Enums;
using TraffiLearn.IntegrationTests.Helpers;
using TraffiLearn.IntegrationTests.Questions.CreateQuestion;
using TraffiLearn.IntegrationTests.Topics;

namespace TraffiLearn.IntegrationTests.Questions
{
    public sealed class ApiQuestionClient
    {
        private readonly RequestSender _requestSender;
        private readonly ApiTopicClient _topicClient;

        public ApiQuestionClient(
            RequestSender requestSender,
            ApiTopicClient apiTopicClient)
        {
            _requestSender = requestSender;
            _topicClient = apiTopicClient;
        }

        public async Task<Guid> CreateValidQuestionWithTopicAsync(
            IFormFile? image = null,
            Role? createdWithRole = null)
        {
            var topicId = await _topicClient.CreateTopicAsAuthorizedAsync();

            var command = CreateQuestionCommandFactory.CreateValidCommand(
                topicIds: [topicId],
                image);

            return await CreateValidQuestionAsync(
                topicIds: [topicId],
                image,
                createdWithRole);
        }

        public Task<Guid> CreateValidQuestionWithTopicAsAuthorizedAsync(
            IFormFile? image = null)
        {
            return CreateValidQuestionWithTopicAsync(
                image,
                createdWithRole: Role.Owner);
        }

        public async Task<Guid> CreateValidQuestionAsync(
            List<Guid> topicIds,
            IFormFile? image = null,
            Role? createdWithRole = null)
        {
            var request = CreateQuestionCommandFactory.CreateValidCommand(
                topicIds,
                image);

            var questionId = await CreateQuestionAsync(
                command: request,
                image,
                createdWithRole: createdWithRole);

            return questionId;
        }

        public async Task<Guid> CreateValidQuestionAsAuthorizedAsync(
            List<Guid> topicIds,
            IFormFile? image = null)
        {
            var request = CreateQuestionCommandFactory.CreateValidCommand(
                topicIds,
                image);

            var questionId = await CreateQuestionAsync(
                command: request,
                image,
                createdWithRole: Role.Owner);

            return questionId;
        }

        private async Task<Guid> CreateQuestionAsync(
            CreateQuestionCommand command, 
            IFormFile? image,
            Role? createdWithRole = null)
        {
            var response = await _requestSender.SendMultipartFormDataWithJsonAndFileRequest(
                method: HttpMethod.Post,
                requestUri: QuestionEndpointRoutes.CreateQuestionRoute,
                value: command,
                file: image,
                sentFromRole: createdWithRole);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<Guid>();
        }

        public Task<IEnumerable<QuestionResponse>> GetAllQuestionsAsync(
            Role? getWithRole = null)
        {
            return _requestSender.GetFromJsonAsync<IEnumerable<QuestionResponse>>(
                requestUri: QuestionEndpointRoutes.GetAllQuestionsRoute,
                getWithRole: getWithRole);
        }

        public Task<IEnumerable<TopicResponse>> GetQuestionTopicsAsync(
            Guid questionId,
            Role? getWithRole = null)
        {
            return _requestSender.GetFromJsonAsync<IEnumerable<TopicResponse>>(
                requestUri: QuestionEndpointRoutes.GetQuestionTopicsRoute(questionId),
                getWithRole: getWithRole);
        }

        public Task<IEnumerable<TopicResponse>> GetQuestionTopicsAsAuthorizedUserAsync(
            Guid questionId)
        {
            return _requestSender.GetFromJsonAsync<IEnumerable<TopicResponse>>(
                requestUri: QuestionEndpointRoutes.GetQuestionTopicsRoute(questionId),
                getWithRole: Role.Owner);
        }

        public Task<IEnumerable<TicketResponse>> GetQuestionTicketsAsync(
            Guid questionId,
            Role? getWithRole = null)
        {
            return _requestSender.GetFromJsonAsync<IEnumerable<TicketResponse>>(
                requestUri: QuestionEndpointRoutes.GetQuestionTicketsRoute(questionId),
                getWithRole);
        }

        public Task<IEnumerable<TicketResponse>> GetQuestionTicketsAsAuthorizedUserAsync(
            Guid questionId)
        {
            return GetQuestionTicketsAsync(
                questionId,
                getWithRole: Role.Owner);
        }
    }
}