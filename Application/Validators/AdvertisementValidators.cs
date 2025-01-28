using Application.DTOs;
using FluentValidation;

namespace Application.Validators
{
    public class AdvertisementCreateValidator : AbstractValidator<AdvertisementCreateDto>
    {
        public AdvertisementCreateValidator() 
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required")
                .MaximumLength(150).WithMessage("Title must be less than 150 characters");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required")
                .MaximumLength(1000).WithMessage("Description must be less than 1000 characters");

            RuleFor(x => x.Cost)
                .NotNull().WithMessage("Cost is required")
                .GreaterThanOrEqualTo(0).WithMessage("Cost must be greater than or equal to 0");
        }
    }

    public class AdvertisementUpdateValidator : AbstractValidator<AdvertisementUpdateDto>
    {
        public AdvertisementUpdateValidator()
        {
            RuleFor(x => x.Title)
                .MaximumLength(150).WithMessage("Title must be less than 150 characters");

            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage("Description must be less than 1000 characters");

            RuleFor(x => x.Cost)
                .GreaterThanOrEqualTo(0).WithMessage("Cost must be greater than or equal to 0");
        }
    }
}
