﻿using TraffiLearn.Domain.Primitives;

namespace TraffiLearn.Domain.Errors.Questions
{
    public static class QuestionContentErrors
    {
        public static readonly Error EmptyText = 
            Error.Validation(
                code: "QuestionContent.EmptyText", 
                description: "Question content cannot be empty.");

        public static Error TooLongText(int allowedLength) => 
            Error.Validation(
                code: "QuestionContent.TooLongText", 
                description: $"Question content text must not exceed {allowedLength} characters.");
    }
}
