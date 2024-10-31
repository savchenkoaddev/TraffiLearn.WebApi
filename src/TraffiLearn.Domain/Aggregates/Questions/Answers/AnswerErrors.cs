﻿using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Domain.Aggregates.Questions.Answers
{
    public static class AnswerErrors
    {
        public static readonly Error EmptyText =
            Error.Validation(
                code: "Answer.EmptyText",
                description: "Answer text cannot be empty.");

        public static Error TooLongText(int allowedLength) =>
            Error.Validation(
                code: "Answer.TooLongText",
                description: $"Answer text must not exceed {allowedLength} characters.");
    }
}
