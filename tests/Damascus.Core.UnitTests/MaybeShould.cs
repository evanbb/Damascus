using Xunit;
using FluentAssertions;
using System.Collections.Generic;
using System.Collections;
using System;

namespace Damascus.Core.UnitTests
{
    public delegate object CastMaybe();

    public class MaybeShould
    {
        [Theory]
        [MemberData(nameof(ConvertParams))]
        public void ConvertConvertibleTypes(CastMaybe action, object expectedOutput)
        {
            var output = action();

            output.Should().Be(expectedOutput);
        }

        public static IEnumerable<object[]> ConvertParams()
        {
            return new[]
            {
                new object[] { (CastMaybe)(() => 123.ToMaybe().Cast<long>()), 123L },
                new object[] { (CastMaybe)(() => 1.23m.ToMaybe().Cast<long>()), 1L },
                new object[] { (CastMaybe)(() => 1.23.ToMaybe().Cast<float>()), 1.23f },
                new object[] { (CastMaybe)(() => 1.23f.ToMaybe().Cast<decimal>()), Maybe.Nothing },

                new object[] { (CastMaybe)(() => Maybe.Nothing.Cast<decimal>()), Maybe.Nothing },
                new object[] { (CastMaybe)(() => Maybe.Nothing.Cast<long>()), Maybe.Nothing },
            };
        }

        [Theory]
        [MemberData(nameof(CastParams))]
        public void CastCastableTypes(CastMaybe action, object expectedOutput)
        {
            var output = action();

            output.Should().Be(expectedOutput);
        }

        public static IEnumerable<object[]> CastParams()
        {
            return new[]
            {
                new object[] { (CastMaybe)(() => 123.ToMaybe().Cast<long>()), 123L },
                new object[] { (CastMaybe)(() => 1.23m.ToMaybe().Cast<long>()), 1L },
                new object[] { (CastMaybe)(() => 1.23.ToMaybe().Cast<float>()), 1.23f },
                new object[] { (CastMaybe)(() => 1.23f.ToMaybe().Cast<decimal>()), Maybe.Nothing },

                new object[] { (CastMaybe)(() => Maybe.Nothing.Cast<decimal>()), Maybe.Nothing },
                new object[] { (CastMaybe)(() => Maybe.Nothing.Cast<long>()), Maybe.Nothing },
            };
        }
    }
}
