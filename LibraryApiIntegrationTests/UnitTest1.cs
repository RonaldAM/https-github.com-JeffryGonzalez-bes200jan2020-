using System;
using Xunit;

namespace LibraryApiIntegrationTests
{
    public class UnitTest1
    {
        [Theory]
        [InlineData(2,2,4)]
        [InlineData(8,2, 10)]
        [InlineData(8,3, 11)]
        public void Test1(int a, int b, int expectedSum)
        {
            Assert.Equal(expectedSum, a + b);
        }
    }
}
