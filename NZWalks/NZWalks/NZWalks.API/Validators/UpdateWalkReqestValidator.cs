﻿using FluentValidation;
using NZWalks.API.Models.DTO;

namespace NZWalks.API;

public class UpdateWalkReqestValidator : AbstractValidator<UpdateWalkRequest>
{
    public UpdateWalkReqestValidator()
    {
        RuleFor(x => x.Description).Length(5, 200);
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.LengthInKm).GreaterThanOrEqualTo(0); 
    }
}