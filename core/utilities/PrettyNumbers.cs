namespace Game.Core.Utilities;

public static class PrettyNumbers
{
    private const double Step = 1_000;
    private const int Decimals = 1;

    private static readonly string[] Suffixes = ["", "K", "M", "B"];

    public static string Format(double number)
    {
        if (!double.IsFinite(number))
            number = 0;

        var magnitude = Math.Abs(number);
        var tier = 0;
        var scaled = Math.Round(magnitude, Decimals);

        while (scaled >= Step && tier < Suffixes.Length - 1)
        {
            tier++;
            scaled = Math.Round(magnitude / Math.Pow(Step, tier), Decimals);
        }

        var value = scaled == 0 ? 0 : Math.CopySign(scaled, number);

        return string.Create(GameLocale.Culture, $"{value}{Suffixes[tier]}");
    }
}
