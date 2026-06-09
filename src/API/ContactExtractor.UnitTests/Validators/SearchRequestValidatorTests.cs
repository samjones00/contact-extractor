using ContactExtractor.Api.Models;
using ContactExtractor.Api.Validators;
using ContactExtractor.Core.Models;
using FluentAssertions;

namespace ContactExtractor.UnitTests.Validators
{
    public class SearchRequestValidatorTests
    {
        private SearchRequestValidator _validator;

        public SearchRequestValidatorTests()
        {
            var settings = new Settings
            {
                AllowedLocations = ["London", "Birmingham", "Leeds"]
            };

            _validator = new(settings);
        }

        [TestCase("London")]
        [TestCase("Birmingham")]
        [TestCase("Leeds")]
        public void GivenValidLocation_ShouldBeTrue(string value)
        {
            // Arrange
            var request = new SearchRequest { Location = value };

            // Act
            var result = _validator.Validate(request);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [TestCaseSource(nameof(NullOrEmptyStrings))]
        public void GivenEmptyOrNullLocation_ShouldBeFalse(string value)
        {
            // Arrange
            var request = new SearchRequest { Location = value };

            // Act
            var result = _validator.Validate(request);

            // Assert
            result.Errors.Should().Contain(e => e.PropertyName == nameof(SearchRequest.Location))
                .Which.ErrorMessage.Should().Be("'Location' must not be empty.");
        }

        [Test]
        public void GivenUnsupportedLocation_ShouldBeFalse()
        {
            // Arrange
            var request = new SearchRequest { Location = "Waterloo" };

            // Act
            var result = _validator.Validate(request);

            // Assert
            result.Errors.Should().Contain(e => e.PropertyName == nameof(SearchRequest.Location))
                .Which.ErrorMessage.Should().Be("'Location' not supported.");
        }

        public static IEnumerable<string> NullOrEmptyStrings = [null, "", " "];

    }
}
