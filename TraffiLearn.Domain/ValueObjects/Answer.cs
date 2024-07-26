namespace TraffiLearn.Domain.ValueObjects
{
    public sealed record Answer
    {
        private const int MaxTextLength = 300;

        private Answer(
            string text,
            bool isCorrect)
        {
            Text = text;
            IsCorrect = isCorrect;
        }

        public string Text { get; init; }

        public bool IsCorrect { get; init; }

        public static Answer Create(
            string? text,
            bool? isCorrect)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(text, nameof(text));
            ArgumentNullException.ThrowIfNull(isCorrect, nameof(isCorrect));

            if (text.Length > MaxTextLength)
            {
                throw new ArgumentException($"Provided text length exceeds {MaxTextLength} characters");
            }

            return new Answer(
                text,
                isCorrect.Value);
        }

        public override int GetHashCode()
        {
            return Text.GetHashCode();
        }
    };
}
