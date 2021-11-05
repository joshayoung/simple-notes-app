using FluentAssertions;
using Xunit;

namespace Converters.Tests
{
    public class StringTruncationConverterTest
    {
        [Theory]
        [InlineData(3, "testing", "tes…")]
        [InlineData(4, "test", "test…")]
        [InlineData(10, "testing", "testing")]
        public void Convert_Called_ReturnsCorrectString(int length, string input, string expectation)
        {
            var converter = new StringTruncationConverter { Length = length, };

            var result = converter.Convert(input, null, null, null);

            result.Should().Be(expectation);
        }
        
        [Theory]
        [InlineData(3, "testing", "tes…")]
        [InlineData(4, "test", "test…")]
        [InlineData(4, "tes", "tes")]
        public void ConvertBack_Called_ReturnsCorrectString(int length, string original, string input)
        {
            var converter = new StringTruncationConverter { Length = length, };
        
            converter.Convert(original, null, null, null);
            var result = converter.ConvertBack(input, null, null, null);
        
            result.Should().Be(original);
        }
        
        [Theory]
        [InlineData(3, "test…")]
        [InlineData(4, "test…")]
        [InlineData(4, "tes")]
        public void ConvertBack_ConvertNotCalled_ReturnsCorrectString(int length, string input)
        {
            var converter = new StringTruncationConverter { Length = length, };
        
            var result = converter.ConvertBack(input, null, null, null);
        
            result.Should().Be(input);
        }
    }
}