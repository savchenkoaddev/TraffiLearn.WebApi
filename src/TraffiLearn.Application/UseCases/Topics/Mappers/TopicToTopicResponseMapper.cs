﻿using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.UseCases.Topics.DTO;
using TraffiLearn.Domain.Topics;

namespace TraffiLearn.Application.UseCases.Topics.Mappers
{
    internal sealed class TopicToTopicResponseMapper : Mapper<Topic, TopicResponse>
    {
        public override TopicResponse Map(Topic source)
        {
            return new TopicResponse(
                Id: source.Id.Value,
                TopicNumber: source.Number.Value,
                Title: source.Title.Value,
                ImageUri: source.ImageUri?.Value);
        }
    }
}
