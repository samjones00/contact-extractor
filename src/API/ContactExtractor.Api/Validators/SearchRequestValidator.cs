using ContactExtractor.Api.Models;
using ContactExtractor.Core.Models;
using FluentValidation;

namespace ContactExtractor.Api.Validators
{
    /// <summary>
    /// Validates <see cref="SearchRequest"/> instances.
    /// </summary>
    public class SearchRequestValidator : AbstractValidator<SearchRequest>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SearchRequestValidator"/> class.
        /// </summary>
        /// <param name="settings">The settings.</param>
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
