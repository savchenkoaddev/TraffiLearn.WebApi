﻿namespace TraffiLearn.Infrastructure.External.GroqAI.DTO
{
    public sealed record GroqApiResponse(
        List<GroqApiChoice> Choices);
}