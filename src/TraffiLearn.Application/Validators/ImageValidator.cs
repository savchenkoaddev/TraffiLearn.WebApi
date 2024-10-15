using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace TraffiLearn.Application.Validators
{
    internal sealed class ImageValidator : AbstractValidator<IFormFile?>
    {
        private const long MAX_SIZE_IN_BYTES = 500_000;

        public ImageValidator()
        {
            RuleFor(file => file)
                .Must(BeValidImage!)
                .WithMessage(
                    "The file must be a valid image type (jpg, jpeg, png, gif, bmp).")
                .When(file => file is not null);

            RuleFor(file => file)
                .Must(BeValidSize!)
                .WithMessage("The file size must be less than 500 Kb.")
                .When(file => file is not null);
        }

        private bool BeValidImage(IFormFile file)
        {
            HashSet<string> validExtensions = new() { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };

            var extension = Path.GetExtension(file.FileName);

            return validExtensions.Contains(extension.ToLower());
        }

        private bool BeValidSize(IFormFile file)
        {
            return file.Length <= MAX_SIZE_IN_BYTES;
        }
    }
}
