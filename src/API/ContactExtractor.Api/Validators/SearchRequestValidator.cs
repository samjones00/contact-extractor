using ContactExtractor.Api.Models;
using ContactExtractor.Core.Models;
using FluentValidation;

namespace ContactExtractor.Api.Validators
{
    public class SearchRequestValidator : AbstractValidator<SearchRequest>
    {
        public SearchRequestValidator(Settings settings)
        {
            RuleFor(x => x.Location)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                .NotEmpty()
                .Must(settings.AllowedLocations.Contains)
                .WithMessage("'Location' not supported.");
        }
    }
}
