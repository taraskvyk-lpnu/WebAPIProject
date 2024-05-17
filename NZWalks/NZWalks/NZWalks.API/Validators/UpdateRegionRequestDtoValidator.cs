using FluentValidation;
using NZWalks.API.Models.DTO;

namespace NZWalks.API;

public class UpdateRegionRequestDtoValidator : AbstractValidator<UpdateRegionRequestDto>
{
    public UpdateRegionRequestDtoValidator()
    {
        RuleFor(x => x.Code).NotEmpty().Length(3, 3);
        RuleFor(x => x.Name).NotEmpty().Length(3, 40);
    }
}