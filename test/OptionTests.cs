using FluentAssertions;
using LanguageExt;
using NUnit.Framework;
using static LanguageExt.Prelude;
using SpikeLanguageExt;

namespace RailwayTests
{
    [TestFixture]
    public class OptionTests
    {
        [Test]
        public void ShouldExtractOptionValue()
        {
            Option<int> value = Some(10);
            
            var contained = value.Match<int>(_ => _, () => 0);
            contained.Should().Be(10);
        }

        [Test]
        public void ShouldExtractOtherValue()
        {
            Option<int> value = None;
            
            var contained = value.Match<int>(_ => _, () => 0);
            contained.Should().Be(0);
        }

        [Test]
        public void ShouldExtractOptionValueOrElse()
        {
            Option<int> value = Some(10);
            
            var contained = value.OrElse(0);
            contained.Should().Be(10);
        }

        [Test]
        public void ShouldExtractOtherValueOrElse()
        {
            Option<int> value = None;
            
            var contained = value.OrElse(0);
            contained.Should().Be(0);
        }
    }
}