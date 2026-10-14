using NUnit.Framework;

namespace Tests.PrettyNumbersTests;

[TestFixture]
public class PrettyNumbersTests
{
    [Test]
    [TestCase(100f, "100")]
    [TestCase(-100f, "-100")]
    [TestCase(999f, "999")]
    [TestCase(1000f, "1K")]
    [TestCase(1200f, "1.2K")]
    [TestCase(-1200f, "-1.2K")]
    [TestCase(150000f, "150K")]
    [TestCase(999_949f, "999.9K")]
    [TestCase(999_999f, "1M")]
    [TestCase(-999_999f, "-1M")]
    [TestCase(1600000f, "1.6M")]
    [TestCase(-1600000f, "-1.6M")]
    public static void NumbersShouldFormatCorrectly(float number, string result)
    {
        Assert.That(PrettyNumbers.Format(number), Is.EqualTo(result));
    }

    [Test]
    [SetCulture("en-ZA")]
    public static void FormattingIgnoresSystemCulture()
    {
        Assert.That(PrettyNumbers.Format(1200f), Is.EqualTo("1.2K"));
    }
}
