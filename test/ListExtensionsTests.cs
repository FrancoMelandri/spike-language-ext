using FluentAssertions;
using NUnit.Framework;
using SpikeLanguageExt;
using System;

namespace SpikeLanguageExtTest
{
    public class ListExtensionsTests
    {
        private ListExtensionsSamples _sut;

        Func<int, int, int> sum = (x, y) => x + y;
        Func<int, int, int> mul = (x, y) => x * y;

        [SetUp]
        public void SetUp()
        {
            _sut = new ListExtensionsSamples();
        }

        [Test]
        public void Fold_ShouldSum_Array_Items()
        {
            var arr = new[] { 1, 2, 3 };
            
            var result = _sut.Compute(arr, 0, sum);

            result.Should().Be(6);
        }

        [Test]
        public void Fold_ShouldSum_Array_Items_FromInitValue()
        {
            var arr = new[] { 1, 2, 3 };
            
            var result = _sut.Compute(arr, 10, sum);

            result.Should().Be(16);
        }

        [Test]
        public void Fold_ShouldMul_Array_Items()
        {
            var arr = new[] { 1, 2, 3, 4 };

            var result = _sut.Compute(arr, 1, mul);

            result.Should().Be(24);
        }

        [Test]
        public void Fold_ShouldMul_Array_Items_FromInitValue()
        {
            var arr = new[] { 1, 2, 3, 4 };

            var result = _sut.Compute(arr, 10, mul);

            result.Should().Be(240);
        }

        [Test]
        public void FoldWhile_ShouldSum_ArrayItems_WhileSumLessThen10()
        {
            var arr = new[] { 1, 2, 3, 4, 5, 6, 7, 8 };

            var result = _sut.PartialSum(arr, 0, sum, s => s != 5);

            result.Should().Be(10);
        }
    }
}
