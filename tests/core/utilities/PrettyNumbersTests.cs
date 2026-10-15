using Game.Core.Utilities;

namespace Game.Tests.Core.Utilities;

[TestFixture]
public class PrettyNumbersTests
{
    [Test]
    [TestCase(double.NaN, "0")]
    [TestCase(double.PositiveInfinity, "0")]
    [TestCase(-0.04, "0")]
    [TestCase(0.1 + 0.2, "0.3")]
    [TestCase(12.34, "12.3")]
    [TestCase(100, "100")]
    [TestCase(-100, "-100")]
    [TestCase(999, "999")]
    [TestCase(999.96, "1K")]
    [TestCase(1000, "1K")]
    [TestCase(1200, "1.2K")]
    [TestCase(-1200, "-1.2K")]
    [TestCase(150_000, "150K")]
    [TestCase(999_949, "999.9K")]
    [TestCase(999_999, "1M")]
    [TestCase(-999_999, "-1M")]
    [TestCase(1_600_000, "1.6M")]
    [TestCase(-1_600_000, "-1.6M")]
    [TestCase(999_999_999, "1B")]
    [TestCase(2_500_000_000, "2.5B")]
    [TestCase(1.5e12, "1500B")]
    public static void FormatsWithSuffix(double number, string result)
    {
        Assert.That(PrettyNumbers.Format(number), Is.EqualTo(result));
    }

    [Test]
    [SetCulture("en-ZA")]
    public static void FormattingIgnoresSystemCulture()
    {
        Assert.That(PrettyNumbers.Format(1200), Is.EqualTo("1.2K"));
    }
}
