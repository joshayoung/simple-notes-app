using System.ComponentModel.DataAnnotations;
using FluentAssertions;
using Xunit;

namespace Converters.Tests
{
    public class InverseBooleanConverterTest
    {
        [Theory]
        [InlineData(true, false)]
        [InlineData(false, true)]
        public void Convert_Called_ReturnsCorrectString(bool input, bool output)
        {
            var converter = new InverseBooleanConverter();

            var result = converter.Convert(input, null, null, null);

            result.Should().Be(output);
        }
        
        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void ConvertBack_Called_ReturnsCorrectString(bool input)
        {
            var converter = new InverseBooleanConverter();

            var result = converter.ConvertBack(input, null, null, null);

            result.Should().Be(input);
        }
    }
}