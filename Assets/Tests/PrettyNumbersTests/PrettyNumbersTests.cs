using NUnit.Framework;
using Project.Assets.Scripts.Utilities;

namespace Project.Assets.Tests.PrettyNumbersTests
{
    [TestFixture]
    public class PrettyNumbersTests
    {
        [Test]
        [TestCase(100f, "100")]
        [TestCase(-100f, "-100")]
        [TestCase(1200f, "1.2K")]
        [TestCase(-1200f, "-1.2K")]
        [TestCase(150000f, "150K")]
        [TestCase(1600000f, "1.6M")]
        [TestCase(-1600000f, "-1.6M")]
        public static void NumbersShouldFormatCorrectly(float number, string result)
        {
            var formattedNumber = PrettyNumbers.Format(number);
            Assert.That(formattedNumber == result);
        }
    }
}
