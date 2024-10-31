﻿using TraffiLearn.SharedKernel.Primitives;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Domain.Comments.CommentContents
{
    public sealed class CommentContent : ValueObject
    {
        public const int MaxLength = 500;

        private CommentContent(string value)
        {
            Value = value;
        }

        public string Value { get; }

        public override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }

        public static Result<CommentContent> Create(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return Result.Failure<CommentContent>(CommentContentErrors.Empty);
            }

            if (value.Length > MaxLength)
            {
                return Result.Failure<CommentContent>(
                    CommentContentErrors.TooLong(maxLength: MaxLength));
            }

            return new CommentContent(value);
        }
    }
}
