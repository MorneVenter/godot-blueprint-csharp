namespace Game.Core.Utilities;

public static class PrettyNumbers
{
    private const double Thousand = 1_000;
    private const double Million = 1_000_000;
    private const int Decimals = 1;
    private const string ThousandSuffix = "K";
    private const string MillionSuffix = "M";

    public static string Format(double number)
    {
        var magnitude = Math.Abs(number);

        if (magnitude < Thousand)
            return number.ToString(GameLocale.Culture);

        var thousands = Math.Round(magnitude / Thousand, Decimals);

        return thousands < Thousand
            ? WithSuffix(Math.CopySign(thousands, number), ThousandSuffix)
            : WithSuffix(
                Math.CopySign(Math.Round(magnitude / Million, Decimals), number),
                MillionSuffix
            );
    }

    private static string WithSuffix(double value, string suffix) =>
        string.Create(GameLocale.Culture, $"{value}{suffix}");
}
