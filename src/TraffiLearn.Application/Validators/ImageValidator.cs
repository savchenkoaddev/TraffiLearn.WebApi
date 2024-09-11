using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace TraffiLearn.Application.Validators
{
    internal sealed class ImageValidator : AbstractValidator<IFormFile?>
    {
        public ImageValidator()
        {
            RuleFor(file => file)
                .Must(BeValidImage!)
                .WithMessage(
                    "The file must be a valid image type (jpg, jpeg, png, gif, bmp).")
                .When(file => file is not null);
        }

        private bool BeValidImage(IFormFile file)
        {
            string[] validExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };

            var extension = Path.GetExtension(file.FileName);

            return validExtensions.Contains(extension.ToLower());
        }
    }
}
