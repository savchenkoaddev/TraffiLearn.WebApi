﻿using TraffiLearn.Domain.ValueObjects.Questions;

namespace TraffiLearn.Application.DTO.Questions
{
    public sealed record QuestionResponse(
        Guid Id,
        string Content,
        string Explanation,
        string? ImageUri,
        int LikesCount,
        int DislikesCount,
        int QuestionNumber,
        List<Answer> Answers);
}
